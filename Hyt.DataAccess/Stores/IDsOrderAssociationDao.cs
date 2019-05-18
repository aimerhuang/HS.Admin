using Hyt.DataAccess.Base;
using Hyt.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Stores
{
    /// <summary>
    /// 经销商订单与系统订单关联
    /// </summary>
    /// <remarks>2016-9-7 杨浩 创建</remarks>
    public abstract class IDsOrderAssociationDao : DaoBase<IDsOrderAssociationDao>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract bool Add(DsOrderAssociation model);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int Update(DsOrderAssociation model);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="dealerSysNo">系统订单编号</param>
        /// <param name="dealerOrderNo">经销商系统编号</param>
        /// <returns></returns>
        public abstract DsOrderAssociation GetOrderAssociationInfo(int dealerSysNo, string dealerOrderNo);
    }
}
