using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// Dao结算单明细
    /// </summary>
    /// <remarks>2013-06-15 黄伟 创建</remarks>
    public abstract class ILgSettlementItemDao : DaoBase<ILgSettlementItemDao>
    {
        /// <summary>
        /// 创建结算单明细
        /// </summary>
        /// <param name="mode">结算单明细实体.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-12 何方 创建
        /// </remarks>
        public abstract int Create(LgSettlementItem mode);

        /// <summary>
        /// 获取结算单明细
        /// </summary>
        /// <param name="settlementSysNo">结算单主表系统编号</param>
        /// <returns>结算单明细列表</returns>
        /// <remarks>2013-06-28 黄伟 创建</remarks>
        public abstract List<LgSettlementItem> GetLgSettlementItems(int settlementSysNo);

        /// <summary>
        /// 获取结算单明细
        /// </summary>
        /// <param name="sysNo">结算单明细系统编号</param>
        /// <returns>
        /// 结算单明细
        /// </returns>
        /// <remarks>
        /// 2013-08-20 何方 创建
        /// </remarks>
        public abstract LgSettlementItem GetLgSettlementItem(int sysNo);

        /// <summary>
        /// 根据出库单号获取结算单明细
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <returns>
        /// 结算单明细
        /// </returns>
        /// <remarks>
        /// 2013-08-22 黄志勇 创建
        /// </remarks>
        public abstract LgSettlementItem GetLgSettlementItemByStockOut(int stockOutSysNo);

        /// <summary>
        /// 获取结算单主表及其明细
        /// </summary>
        /// <param name="settlementSysNo">结算单主表系统编号</param>
        /// <returns>结算单明细列表</returns>
        /// <remarks>2013-07-04 黄伟 创建</remarks>
        public abstract LgSettlement GetLgSettlementWithItems(int settlementSysNo);

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="sysNo">结算单明细系统编号</param>
        /// <returns>出库单明细</returns>
        /// <remarks>2013-07-05 黄伟 创建</remarks>
        public abstract List<WhStockOutItem> GetWhStockOutDetails(int sysNo);

        /// <summary>
        /// 更新结算单明细
        /// </summary>
        /// <param name="item">结算单明细</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2013-8-21 黄伟 创建</remarks>
        public abstract int Update(LgSettlementItem item);
    }
}
