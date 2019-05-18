using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    /// <summary>
    /// 货物出库单操作实体
    /// </summary>
    /// <remarks>
    /// 2016-05-17 杨云奕 添加
    /// </remarks>
    public abstract class IDsWhOutStockDao : DaoBase<IDsWhOutStockDao>
    {
        #region 出库单操作
        /// <summary>
        /// 添加出库实体
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertMod(DsWhOutStock mod);
        /// <summary>
        /// 修改出库实体
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdateMod(DsWhOutStock mod);
        /// <summary>
        /// 通过自动编号删除实体数据
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteBySysNo(int SysNo);
        /// <summary>
        /// 获取出库单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract DsWhOutStock GetOutStockBySysNo(int SysNo);
        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract CBDsWhOutStock GetExtendsOutStockBySysNo(int SysNo);
        #endregion

        #region 出库单明细
        /// <summary>
        /// 添加出库单明细记录
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertModList(DsWhOutStockList mod);
        /// <summary>
        /// 更新出库单明细记录
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int UpdateModList(DsWhOutStockList mod);
        /// <summary>
        /// 删除出库单明细记录
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteModListBySysNo(int SysNo);
        /// <summary>
        /// 通过自动编号获取单独的出库单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract DsWhOutStockList GetOutStockListBySysNo(int SysNo);
        /// <summary>
        /// 通过父ID获取出库单明细
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public abstract List<DsWhOutStockList> GetOutStockListByPSysNo(int PSysNo);

        #endregion


        public abstract void DsWhOutStockPager(ref Model.Pager<CBDsWhOutStock> pageCusList);

        public abstract List<CBDsWhOutStockList> GetDsWhOutStockByBitNumber(string bitNumber);
    }
}
