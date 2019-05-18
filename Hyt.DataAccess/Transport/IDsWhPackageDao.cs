using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    /// <summary>
    /// 包裹打包
    /// </summary>
    /// <remarks>
    /// 2016-5-18 杨云奕 添加
    /// </remarks>
    public abstract class IDsWhPackageDao : DaoBase<IDsWhPackageDao>
    {
        /// <summary>
        /// 添加包裹打包
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertMod(DsWhPackage mod);
        /// <summary>
        /// 修改包裹打包
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract void UpdateMod(DsWhPackage mod);
        /// <summary>
        /// 删除包裹打包数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract void DeleteBySysNo(int SysNo);
        /// <summary>
        /// 获取包裹打包数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract DsWhPackage GetDsWhPackageBySysNo(int SysNo);
        /// <summary>
        /// 获取包裹打包数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract CBDsWhPackage GetCBDsWhPackageBySysNo(int SysNo);

        /// <summary>
        /// 添加包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertModList(DsWhPackageList mod);
        /// <summary>
        /// 修改包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract void UpdateModList(DsWhPackageList mod);
        /// <summary>
        /// 删除包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract void DeleteModListBySysNo(int SysNo);
        /// <summary>
        /// 删除包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract void DeleteModListByPSysNo(int PSysNo);
        /// <summary>
        /// 获取包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract List<DsWhPackageList> GetWhPackageListByPSysNo(int pSysNo);

        public abstract void DoDsWhPackageQuery(Model.Pager<DsWhPackage> pageCusList);

        public abstract List<DsWhPackageList> GetDsWhPackageListByCourierNumbers(List<string> packNumList);

        public abstract List<CBDsWhPackageList> GetCBWhPackageListByPSysNo(int pSysNo);

        public abstract List<DsWhPackage> GetModByServiceTypeAndCompletePackage(string ServiceType);

        public abstract List<DsWhPackage> GetDsWhPackageByCourierNumbers(List<string> packNumList);

        public abstract CBDsWhPackage GetPackageOnAirLineData(int SysNo);
    }
}
