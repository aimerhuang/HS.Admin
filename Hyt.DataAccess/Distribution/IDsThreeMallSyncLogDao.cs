using Hyt.DataAccess.Base;
using Hyt.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 同步三方商城
    /// </summary>
    /// <remarks>2016-7-8 杨浩 创建</remarks>
    public abstract class IDsThreeMallSyncLogDao : DaoBase<IDsThreeMallSyncLogDao>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public abstract int Add(DsThreeMallSyncLog model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public abstract int Update(DsThreeMallSyncLog model);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <param name="mallSysNo">商城编号</param>
        /// <param name="syncType">同步类型(订单:10 退货货单：20)</param>
        public abstract DsThreeMallSyncLog GetThreeMallSyncLogInfo(int dealerSysNo, int mallTypeSysNo,int syncType);
    }
}
