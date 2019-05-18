using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    /// <summary>
    /// 货物档案数据列表
    /// </summary>
    /// <remarks>
    /// 2015-5-17 杨云奕 添加
    /// </remarks>
    public abstract class IDsWhGoodsManagementDao : DaoBase<IDsWhGoodsManagementDao>
    {
        #region 货物档案主表     
        /// <summary>
        /// 添加主表数据
        /// </summary>
        /// <param name="mod">实体</param>
        /// <returns></returns>
        public abstract int InsertMod(DsWhGoodsManagement mod);
        /// <summary>
        /// 更新主表数据
        /// </summary>
        /// <param name="mod">实体</param>
        public abstract void UpdateMod(DsWhGoodsManagement mod);
        /// <summary>
        /// 删除主表数据
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        public abstract void DeleteMod(int SysNo);
        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract DsWhGoodsManagement GetModBySysNo(int SysNo);
        /// <summary>
        /// 获取实体数据
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <returns></returns>
        public abstract CBDsWhGoodsManagement GetExtendsModBySysNo(int SysNo);
        #endregion

        #region 获取货物数据明细
        /// <summary>
        /// 添加货物档案明细
        /// </summary>
        /// <param name="mod">货物档案明细</param>
        /// <returns></returns>
        public abstract int InsertModList(DsWhGoodsManagementList mod);
        /// <summary>
        /// 更新货物档案明细
        /// </summary>
        /// <param name="mod">货物档案明细</param>
        public abstract void UpdateModList(DsWhGoodsManagementList mod);
        /// <summary>
        /// 删除货物档案明细
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        public abstract void DeleteModList(int SysNo);
        /// <summary>
        /// 获取某一数据实体
        /// </summary>
        /// <param name="SysNo">自动编号</param>
        /// <returns></returns>
        public abstract DsWhGoodsManagementList GetModListBySysNo(int SysNo);
        /// <summary>
        /// 通过父编号获取商品明细集合
        /// </summary>
        /// <param name="PSysNo">父id编号</param>
        /// <returns></returns>
        public abstract List<DsWhGoodsManagementList> GetModListByPSysNo(int PSysNo);
        #endregion

        public abstract List<DsWhGoodsManagement> GetModListByBatchNumber(string batchNumber, 
            bool IsBindAllDealer, bool IsBindDealer, bool IsCustomer, 
            int DsSysNo, string CusCode);

        public abstract void GoodsManagePager(ref Model.Pager<CBDsWhGoodsManagement> pageCusList, bool IsBindAllDealer, bool IsBindDealer, bool IsCustomer,
            int DsSysNo, string CusCode, string OrderByKey, string OrderbyType);

        public abstract DsWhGoodsManagement GetModListByCourierNumber(string CourierNumber);

        public abstract List<DsWhGoodsManagement> GetDsWhGoodsManagementByCourierNumbers(List<string> packNumList);

        public abstract void UpdateModByWaybill(string MawbNumber,string SatausCode, List<string> waybillnumberList);

        public abstract List<CBDsWhGoodsManagement> GetAllGoodsManageBySearch(CBDsWhGoodsManagement cbGoodsMan);

        public abstract List<CBDsWhGoodsManagement> GetDsWhGoodsManagementByTotalWaybillNum(string WayWillNum);

        public abstract void OrderManagerGroupPager(Model.Pager<WhGoodsManagementGroup> pageCusList);

        public abstract List<CBDsWhGoodsManagement> GetAllGoodsManageByCreateTime(string crateTime);

        public abstract List<CBDsWhGoodsManagement> GetExtendsModBySysNos(string gmSysNos);

        public abstract List<CBDsWhGoodsManagement> GetGoodsManageBySearchText(bool cb_Select, string ipt_Batch, string ipt_StartOrder, string ipt_EndOrder, string ipt_OutStockDate, string sel_Stautus);

        public abstract DsWhGMReport GetDsWhReportData();
    }
}
