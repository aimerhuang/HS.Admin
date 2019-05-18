using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.CRM;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 站内信业务逻辑类
    /// </summary>
    /// <remarks>2013-08-07 杨晗 创建</remarks>
    public class CrSiteMessageBo : BOBase<CrSiteMessageBo>
    {

        /// <summary>
        /// 获取站内信分页方法
        /// </summary>
        /// <param name="customersysNo">用户系统编号</param>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="ids">站内信状态集合</param>
        /// <returns>站内信列表</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public PagedList<CrSiteMessage> GetPage(int customersysNo, int? pageIndex, CustomerStatus.站内信状态[] ids)
        {
            pageIndex = pageIndex ?? 1;
            var model = new PagedList<CrSiteMessage>();
            int count;
            var list = ICrSiteMessageDao.Instance.GetPage(customersysNo, (int)pageIndex, model.PageSize, ids, out count);

            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int) pageIndex;
            model.Style = PagedList.StyleEnum.WebSmall; 
            return model;
        }

        /// <summary>
        /// 根据接收人获取所有未删除站内信数量
        /// </summary>
        /// <param name="customerSysNo">接收人id</param>
        /// <returns>所有未删除的站内信数量</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public Dictionary<int, int> GetCount(int customerSysNo)
        {
            return ICrSiteMessageDao.Instance.GetCount(customerSysNo);
        }

        /// <summary>
        /// 获取站内信单条数据
        /// </summary>
        /// <param name="sysNo">站内信系统编号</param>
        /// <returns>站内信单条数据</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public CrSiteMessage GetSiteMessage(int sysNo)
        {
            return ICrSiteMessageDao.Instance.GetSiteMessage(sysNo);
        }

        /// <summary>
        /// 根据站内消息系统编号把消息设置为已读或删除
        /// </summary>
        /// <param name="sysNo">站内消息系统编号</param>
        /// <param name="status">站内消息状态</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public int SetMessageStatus(int sysNo, int status)
        {
            return ICrSiteMessageDao.Instance.SetMessageStatus(sysNo, status);
        }

        /// <summary>
        /// 删除站内信
        /// </summary>
        /// <param name="sysNo">站内信系统编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public int Delete(int sysNo)
        {
            return ICrSiteMessageDao.Instance.Delete(sysNo);
        }
    }
}
