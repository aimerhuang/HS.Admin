using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    /// <summary>
    /// 总航运控制
    /// </summary>
    /// <remarks>
    /// 2016-5-18 杨云奕 添加
    /// </remarks>
    public abstract class IDsWhTotalWaybillDao : DaoBase<IDsWhTotalWaybillDao>
    {
        /// <summary>
        /// 添加总运单
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertMod(DsWhTotalWaybill mod);
        /// <summary>
        /// 修改总运单数据
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdateMod(DsWhTotalWaybill mod);
        /// <summary>
        /// 删除总运单数据
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteBySysNo(int SysNo);
        /// <summary>
        /// 通过编号获取总运单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract DsWhTotalWaybill GetDsWhTotalWaybillBySysNo(int SysNo);
        /// <summary>
        /// 通过编号获取总运单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract CBDsWhTotalWaybill GetCBDsWhTotalWaybillBySysNo(int SysNo);
        /// <summary>
        /// 添加总运单包裹数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertModList(DsWhTotalWaybillList mod);
        /// <summary>
        /// 修改总运单包裹数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int UpdateModList(DsWhTotalWaybillList mod);
        /// <summary>
        /// 通过父id删除总运单包裹明细
        /// </summary>
        /// <param name="PSysNo"></param>
        public abstract void DeleteModListByPSysNo(int PSysNo);
        /// <summary>
        /// 通过父id获取总运单中包裹数据
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public abstract List<DsWhTotalWaybillList> GetDsWhTotalWaybillListByPSysNo(int PSysNo);

        public abstract void DeleteModListBySysNo(int SysNo);

        public abstract void DsWhTotalWaybillPager(ref Model.Pager<CBDsWhTotalWaybill> pageCusList);


        public abstract List<DsWhTotalWaybill> GetTotalWayBillByCourierNumber(string CourierNumber);
    }
}
