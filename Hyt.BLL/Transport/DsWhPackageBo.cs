using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Transport
{
    /// <summary>
    /// 包裹打包
    /// </summary>
    /// <remarks>
    /// 2016-5-18 杨云奕 添加
    /// </remarks>
    public class DsWhPackageBo : BOBase<DsWhPackageBo>
    {
        /// <summary>
        /// 添加包裹打包
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int InsertMod(Model.Transport.DsWhPackage mod)
        {
            return IDsWhPackageDao.Instance.InsertMod(mod);
        }
        /// <summary>
        /// 修改包裹打包
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  void UpdateMod(Model.Transport.DsWhPackage mod)
        {
            IDsWhPackageDao.Instance.UpdateMod(mod);
        }
        /// <summary>
        /// 删除包裹打包数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  void DeleteBySysNo(int SysNo)
        {
            IDsWhPackageDao.Instance.DeleteBySysNo(SysNo);
        }
        // <summary>
        /// 获取包裹打包数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  Model.Transport.DsWhPackage GetDsWhPackageBySysNo(int SysNo)
        {
            return IDsWhPackageDao.Instance.GetDsWhPackageBySysNo(SysNo);
        }
        /// <summary>
        /// 获取包裹打包数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  Model.Transport.CBDsWhPackage GetCBDsWhPackageBySysNo(int SysNo)
        {
            return IDsWhPackageDao.Instance.GetCBDsWhPackageBySysNo(SysNo);
        }
        /// <summary>
        /// 添加包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int InsertModList(Model.Transport.DsWhPackageList mod)
        {
            return IDsWhPackageDao.Instance.InsertModList(mod);
        }
        /// <summary>
        /// 修改包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  void UpdateModList(Model.Transport.DsWhPackageList mod)
        {
            IDsWhPackageDao.Instance.UpdateModList(mod);
        }
        /// <summary>
        /// 删除包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  void DeleteModListBySysNo(int SysNo)
        {
            IDsWhPackageDao.Instance.DeleteModListBySysNo(SysNo);
        }
        /// <summary>
        /// 删除包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  void DeleteModListByPSysNo(int PSysNo)
        {
            IDsWhPackageDao.Instance.DeleteModListByPSysNo(PSysNo);
        }
        /// <summary>
        /// 获取包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  List<Model.Transport.DsWhPackageList> GetWhPackageListByPSysNo(int pSysNo)
        {
            return IDsWhPackageDao.Instance.GetWhPackageListByPSysNo(pSysNo);
        }

        public void DoDsWhPackageQuery(ref Model.Pager<Model.Transport.DsWhPackage> pageCusList)
        {
             IDsWhPackageDao.Instance.DoDsWhPackageQuery(pageCusList);
        }

        public List<DsWhPackage> GetDsWhPackageByCourierNumbers(List<string> packNumList)
        {
            return IDsWhPackageDao.Instance.GetDsWhPackageByCourierNumbers(packNumList);
        }

        public List<Model.Transport.DsWhPackageList> GetDsWhPackageListByCourierNumbers(List<string> packNumList)
        {
            return IDsWhPackageDao.Instance.GetDsWhPackageListByCourierNumbers(packNumList);
        }

        public List<Model.Transport.CBDsWhPackageList> GetCBWhPackageListByPSysNo(int pSysNo)
        {
            return IDsWhPackageDao.Instance.GetCBWhPackageListByPSysNo(pSysNo);
        }

        public List<Model.Transport.DsWhPackage> GetModByServiceTypeAndCompletePackage(string ServiceType)
        {
            return IDsWhPackageDao.Instance.GetModByServiceTypeAndCompletePackage(ServiceType);
        }
        /// <summary>
        /// 获取包裹航空单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public CBDsWhPackage GetPackageOnAirLineData(int SysNo)
        {
            return IDsWhPackageDao.Instance.GetPackageOnAirLineData(SysNo);
        }
    }
}
