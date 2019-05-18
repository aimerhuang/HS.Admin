using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 站内信接口类
    /// </summary>
    /// <remarks>2013-08-07 杨晗 创建</remarks>
    public abstract class ICrSiteMessageDao : DaoBase<ICrSiteMessageDao>
    {
        /// <summary>
        /// 获取站内信分页方法
        /// </summary>
        /// <param name="customersysNo">用户系统编号</param>
        /// <param name="pageIndex">起始页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="ids">站内信状态集合</param>
        /// <param name="messageCount">抛出总数</param>
        /// <returns>站内信列表</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public abstract IList<CrSiteMessage> GetPage(int customersysNo, int pageIndex, int pageSize, CustomerStatus.站内信状态[] ids,
                                                     out int messageCount);

        /// <summary>
        /// 根据接收人获取所有未删除站内信数量
        /// </summary>
        /// <param name="customerSysNo">接收人id</param>
        /// <returns>所有未删除的站内信数量</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public abstract Dictionary<int, int> GetCount(int customerSysNo);

        /// <summary>
        /// 获取站内信单条数据
        /// </summary>
        /// <param name="sysNo">站内信系统编号</param>
        /// <returns>站内信单条数据</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public abstract CrSiteMessage GetSiteMessage(int sysNo);

        /// <summary>
        /// 根据站内消息系统编号把消息设置为已读或删除
        /// </summary>
        /// <param name="sysNo">站内消息系统编号</param>
        /// <param name="status">站内消息状态</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public abstract int SetMessageStatus(int sysNo, int status);

        /// <summary>
        /// 删除站内信
        /// </summary>
        /// <param name="sysNo">站内信系统编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-07 杨晗 创建</remarks>
        public abstract int Delete(int sysNo);
    }
}
