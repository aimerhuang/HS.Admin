using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.CRM;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Pager;

namespace Hyt.BLL.CRM
{
    /// <summary>
    ///会员投诉业务逻辑
    /// </summary>
    /// <remarks>2013－07-09 苟治国 创建</remarks>
    public class CrComplaintBo : BOBase<CrComplaintBo>
    {
        /// <summary>
        /// 查看会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>会员投诉</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public Model.CrComplaint GetModelSingle(int sysNo)
        {
            return ICrComplaintDao.Instance.GetModelSingle(sysNo);
        }

        /// <summary>
        /// 查看会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>会员投诉</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public Model.CBCrComplaint GetModel(int sysNo)
        {
            return ICrComplaintDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据条件获取会员投诉的列表
        /// </summary>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="status">会员投诉状态</param>
        /// <param name="complainType">投诉类型</param>
        /// <param name="replyerType">回复类型</param>
        /// <param name="orderSysNo">会员投诉订单编号</param>
        /// <param name="customerSysNo">会员投诉订单编号</param>
        /// <param name="mobilePhoneNumber">会员手机号</param>
        /// <returns>会员投诉列表</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public PagedList<CBCrComplaint> Seach(int pageIndex, int? status, int? complainType, int? replyerType, int? orderSysNo,int? customerSysNo, string mobilePhoneNumber = null)
        {
            var list = new PagedList<CBCrComplaint>();
            var pager = new Pager<CBCrComplaint>();

            if (status == null)
            {
                status = (int)CustomerStatus.会员投诉状态.待处理;
            }
            else
            {
                status = status ?? -1;
            }
            if (complainType == null)
            {
                complainType = -1;
            }
            if (replyerType == null)
            {
                replyerType = -1;
            }
            if (orderSysNo == null)
            {
                orderSysNo = 0;
            }
            if (customerSysNo == null){
                customerSysNo =-1;
            }

            pager.CurrentPage = pageIndex;
            pager.PageFilter = new CBCrComplaint
            {
                Status = (int)status,
                ComplainType = (int)complainType,
                ReplyerType = -1,
                OrderSysNo = (int)orderSysNo,
                MobilePhoneNumber = mobilePhoneNumber,
                CustomerSysNo = (int)customerSysNo
                
            };
            pager.PageSize = list.PageSize;
            pager = ICrComplaintDao.Instance.Seach(pager);
            list = new PagedList<CBCrComplaint>
            {
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return list;
        }

        /// <summary>
        /// 插入会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public int Insert(Model.CrComplaint model)
        {
            int result = ICrComplaintDao.Instance.Insert(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建会员投诉", LogStatus.系统日志目标类型.客户投诉, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, result);
            return result;
        }

        /// <summary>
        /// 更新会员投诉
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public int Update(Model.CrComplaint model)
        {
            int result = ICrComplaintDao.Instance.Update(model);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("回复投诉内容{0}", model.SysNo), LogStatus.系统日志目标类型.客户投诉, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, result);
            return result;
        }

        /// <summary>
        /// 更新投诉状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="strWhere">条件</param>
        /// <returns>返回受影响的行</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public int UpdateStatus(string status, string strWhere)
        {
            int result = ICrComplaintDao.Instance.UpdateStatus(status, strWhere);
            if (result > 0)
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改投诉状态", LogStatus.系统日志目标类型.客户投诉, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, result);
            return result;
        }

        /// <summary>
        /// 删除会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        public bool Delete(int sysNo)
        {
            return ICrComplaintDao.Instance.Delete(sysNo);
        }
    }
}
