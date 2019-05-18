using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Notification
{
    /// <summary>
    /// 邮件
    /// </summary>
    /// <remarks>2013-10-8 陶辉 创建 </remarks>
    public abstract class INcEmailDao : DaoBase<INcEmailDao>
    {
        #region 操作

        /// <summary>
        /// 创建邮件
        /// </summary>
        /// <param name="model">邮件实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public abstract int Create(NcEmail model);

        /// <summary>
        /// 更新邮件
        /// </summary>
        /// <param name="model">邮件实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public abstract int Update(NcEmail model);

        #endregion

        #region 查询

        /// <summary>
        /// 获取邮件信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>邮件实体</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public abstract NcEmail GetNcEmail(int sysNo);

        /// <summary>
        /// 根据邮件发送状态获取邮件列表
        /// </summary>
        /// <param name="status">邮件发送状态</param>
        /// <returns>邮件列表</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public abstract List<NcEmail> GetNcEmailListByStatus(NotificationStatus.邮件发送状态 status);

        #endregion
    }
}
