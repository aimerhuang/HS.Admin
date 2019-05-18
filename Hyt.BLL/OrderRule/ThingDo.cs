using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.BLL.Sys;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Hyt.BLL.OrderRule
{
    /// <summary>
    /// 处理
    /// </summary>
    /// <remarks>2016-10-08  杨浩 创建</remarks>
    public class ThingDo : BOBase<ThingDo>
    {
        /// <summary>
        /// 将订单加入任务池
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <remarks>2016-10-08  杨浩 创建</remarks>
        public  void AddToJobPool(OrderData orderData)
        {
            if (orderData == null || orderData.Order == null || orderData.OrderItems == null) return;
            if (orderData.Order.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核)//待审核才加入任务池
            {
                if (orderData.AssignTo.HasValue) //手动审单客服编号
                {
                    SyJobPoolPublishBo.Instance.OrderAuditBySysNo(orderData.Order.SysNo, orderData.AssignTo.Value);
                    SyJobDispatcherBo.Instance.WriteJobLog(string.Format("创建订单审核任务，销售单编号:{0}", orderData.Order.SysNo), orderData.Order.SysNo, null, orderData.AssignTo.Value);
                }
                else
                {
                    SyJobPoolPublishBo.Instance.OrderAuditBySysNo(orderData.Order.SysNo);
                    SyJobDispatcherBo.Instance.WriteJobLog(string.Format("创建订单审核任务，销售单编号:{0}", orderData.Order.SysNo), orderData.Order.SysNo, null, 0);
                }
            }

            if (orderData.JobMessage != null)
            {
                //删除关联的任务消息
                SyJobMessageBo.Instance.Delete(orderData.JobMessage.SysNo);//删除关联任务消息 2015-01-21 朱成果
            }
           
        }
        /// <summary>
        /// 自动审核升舱订单
        /// </summary>
        /// <param name="orderData">订单数据</param>
        /// <remarks>2016-10-08  杨浩 创建</remarks>
        public void Audit(OrderData orderData)
        {
            if (orderData == null || orderData.Order == null || orderData.OrderItems == null) return;
            try
            {
                var user = SyUserBo.Instance.GetSyUser(1);//系统用户
                using (var tran = new TransactionScope())
                {
                    if (orderData.Order.DefaultWarehouseSysNo > 0 &&
                        orderData.Order.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核) //选择了仓库
                    {
                        bool flg = SoOrderBo.Instance.AuditSoOrder(orderData.Order.SysNo, user.SysNo, false); //审核订单
                        if (flg)
                        {
                            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "自动审核订单", LogStatus.系统日志目标类型.订单,
                                orderData.Order.SysNo, user.SysNo);
                            foreach (var pitem in orderData.OrderItems)
                            {
                                pitem.RealStockOutQuantity = pitem.Quantity; //如果不设置分摊会出错
                            }
                            var outStock = SoOrderBo.Instance.CreateOutStock(orderData.OrderItems,orderData.Order.DefaultWarehouseSysNo, user,false,orderData.Order.DeliveryTypeSysNo); //订单出库
                            if (outStock != null)
                            {
                                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "自动创建出库单", LogStatus.系统日志目标类型.出库单,
                                    outStock.SysNo, user.SysNo);
                            }
                        }
                        if (orderData.JobMessage != null)
                        {
                            //删除关联的任务消息
                            SyJobMessageBo.Instance.Delete(orderData.JobMessage.SysNo);//删除关联任务消息 2016-10-08 杨浩
                        }
                    }
                    else
                    {
                        AddToJobPool(orderData);
                    }
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                AddToJobPool(orderData);
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "自动审核订单", LogStatus.系统日志目标类型.订单, orderData.Order.SysNo, ex, 0);
            }          
        }
        
    }
}
