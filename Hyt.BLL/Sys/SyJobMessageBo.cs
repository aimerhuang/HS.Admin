using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.BLL.OrderRule;
using Hyt.DataAccess.Sys;
using Hyt.Model;
using Hyt.Model.JobMessageContent;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Sys
{
    /// <summary>
    /// 任务消息BO
    /// </summary>
    /// <remarks>2015-01-21  杨浩 创建</remarks>
    public class SyJobMessageBo : BOBase<SyJobMessageBo>
    {
        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public int Insert(SyJobMessage entity)
        {
            return ISyJobMessageDao.Instance.Insert(entity);
        }

        /// <summary>
        /// 添加订单创建任务消息
        /// </summary>
        /// <param name="ordersysno">订单编号</param>
      /// <remarks>2015-01-21  杨浩 创建</remarks>
        public int InsertOrderMessage(int ordersysno)
        {
            return InsertOrderMessage(new OrderJobMessage() { OrderSysNo = ordersysno });
        }

        /// <summary>
        /// 添加订单创建任务消息
        /// </summary>
        /// <param name="ordersysno">订单编号</param>
        ///<param name="assignTo">任务池执行人</param>
       /// <remarks>2015-01-21  杨浩 创建</remarks>
        public int InsertOrderMessage(int ordersysno, int assignTo)
        {
            return InsertOrderMessage(new OrderJobMessage() { OrderSysNo = ordersysno, AssignTo = assignTo });
        }
        
        /// <summary>
        /// 添加订单创建消息
        /// </summary>
        /// <param name="entity">消息内容</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public int InsertOrderMessage(OrderJobMessage entity)
        {
            SyJobMessage message = new SyJobMessage();
            message.CreateTime = DateTime.Now;
            message.MessageType = (int)SystemStatus.任务消息类型.订单创建消息;
            message.Content = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
            return Insert(message);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public void Update(SyJobMessage entity)
        {
            ISyJobMessageDao.Instance.Update(entity);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public SyJobMessage GetEntity(int sysNo)
        {
            return ISyJobMessageDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public void Delete(int sysNo)
        {
            ISyJobMessageDao.Instance.Delete(sysNo);
        }
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="messageType">类型编号</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public List<SyJobMessage> GetListByMessageType(int? messageType)
        {
            return ISyJobMessageDao.Instance.GetListByMessageType(messageType);

        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="message"></param>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public void DealWithMessage(SyJobMessage message)
        {
            bool isdeal = false;
            if(message!=null&&!string.IsNullOrEmpty(message.Content))
            {
               // SysLog.Instance.Info(LogStatus.系统日志来源.后台, Newtonsoft.Json.JsonConvert.SerializeObject(message), LogStatus.系统日志目标类型.任务消息, message.SysNo,0);//记录日志
                if (message.MessageType == (int)SystemStatus.任务消息类型.订单创建消息)
                { 
                    #region 订单创建消息
                    var model = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderJobMessage>(message.Content);
                    if(model!=null)
                    {
                        OrderData data = new OrderData();
                        data.Order = SoOrderBo.Instance.GetEntity(model.OrderSysNo);
                        data.OrderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(model.OrderSysNo);
                        data.AssignTo = model.AssignTo;//手动审单客服编号
                        data.JobMessage = message;
                        if (data.Order != null && data.OrderItems != null && data.Order.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核)
                        {
                            Hyt.BLL.OrderRule.OrderEngine.Instance.HandlerUpGradesOrder(data);
                            isdeal = true;
                        }
                    }
                    #endregion
                    if(!isdeal)
                    {
                        Delete(message.SysNo);//删除消息
                    }
                }  
            }
        }
        #endregion

        #region 任务执行
        /// <summary>
        /// 处理任务消息
        /// </summary>
        /// <param name="aftermin">消息创建后几分钟处理</param>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public void AutoTask(int aftermin=0)
        {
            var lst = GetListByMessageType(null);
            if(lst!=null)
            {
                foreach(var item in lst)
                {
                    if (item.CreateTime.AddMinutes(aftermin) <= DateTime.Now)
                    {
                        DealWithMessage(item);
                    }
                }
            }
        }
        #endregion

        #region 后台是否自动审单
        /// <summary>
        /// 添加审单消息或者任务
        /// </summary>
        /// <param name="ordersysno">订单编号</param>
        /// <param name="assignTo">审单客服</param>
        /// <param name="orderCreatorSysNo">订单创建人员</param>
        /// <param name="message">任务池日志</param>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public void InsertJobPoolOrMessage(int orderSysNo, int? assignTo, int orderCreatorSysNo, string message)
        {
            if(System.Configuration.ConfigurationManager.AppSettings["OrderJobMessageState"]=="Open")//任务消息
            {
                InsertOrderMessage(
                  new OrderJobMessage()
                  {
                      AssignTo = assignTo,
                      OrderSysNo = orderSysNo
                  }
                );
            }
            else
            {
                SyJobPoolPublishBo.Instance.OrderAuditBySysNo(orderSysNo, assignTo);
                SyJobDispatcherBo.Instance.WriteJobLog(message, orderSysNo, null, orderCreatorSysNo);
            }
        }
        #endregion
    }
}
