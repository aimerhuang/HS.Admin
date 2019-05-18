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
    /// 短信
    /// </summary>
    /// <remarks>2013-10-8 陶辉 创建 </remarks>
    public abstract class INcSmsDao : DaoBase<INcSmsDao>
    {
        #region 操作

        /// <summary>
        /// 创建短信
        /// </summary>
        /// <param name="model">短信实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public abstract int Create(NcSms model);

        /// <summary>
        /// 更新短信
        /// </summary>
        /// <param name="model">短信实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public abstract int Update(NcSms model);
        
        #endregion

        #region 查询

        /// <summary>
        /// 获取短信信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>短信实体</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public abstract NcSms GetNcSms(int sysNo);

        /// <summary>
        /// 根据短信发送状态获取短信列表
        /// </summary>
        /// <param name="status">短信发送状态</param>
        /// <returns>短信列表</returns>
        /// <remarks>2013-10-8 陶辉 创建</remarks>
        public abstract List<NcSms> GetNcSmsListByStatus(NotificationStatus.短信发送状态 status);

        #endregion

    }
}
