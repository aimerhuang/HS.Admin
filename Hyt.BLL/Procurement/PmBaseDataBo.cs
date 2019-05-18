using Hyt.DataAccess.Procurement;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Procurement
{
    public class PmBaseDataBo : BOBase<PmBaseDataBo>
    {
        /// <summary>
        /// 创建集装箱
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public int InsertPmContainer(PmContainer mod)
        {
            return IPmBaseDataDao.Instance.InsertPmContainer(mod);
        }
        /// <summary>
        /// 创建生产厂家
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int InsertPmManufacturer(PmManufacturer mod)
        {
            return IPmBaseDataDao.Instance.InsertPmManufacturer(mod);
        }
        /// <summary>
        /// 创建物流公司
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public  int InsertPmLogisticsCompany(PmLogisticsCompany mod)
        {
            return IPmBaseDataDao.Instance.InsertPmLogisticsCompany(mod);
        }
        /// <summary>
        /// 更新集装箱
        /// </summary>
        /// <param name="mod"></param>
        public  void UpdatePmContainer(PmContainer mod)
        {
            IPmBaseDataDao.Instance.UpdatePmContainer(mod);
        }
        /// <summary>
        /// 更新生产厂家
        /// </summary>
        /// <param name="mod"></param>
        public  void UpdatePmManufacturer(PmManufacturer mod)
        {
            IPmBaseDataDao.Instance.UpdatePmManufacturer(mod);
        }
        /// <summary>
        /// 更新物流公司
        /// </summary>
        /// <param name="mod"></param>
        public  void UpdatePmLogisticsCompany(PmLogisticsCompany mod)
        {
            IPmBaseDataDao.Instance.UpdatePmLogisticsCompany(mod);
        }
        /// <summary>
        /// 删除集装箱
        /// </summary>
        /// <param name="SysNo"></param>
        public  void DeletePmContainer(int SysNo)
        {
            IPmBaseDataDao.Instance.DeletePmContainer(SysNo);
        }
        /// <summary>
        /// 删除生产厂家
        /// </summary>
        /// <param name="SysNo"></param>
        public  void DeletePmManufacturer(int SysNo)
        {
            IPmBaseDataDao.Instance.DeletePmManufacturer(SysNo);
        }
        /// <summary>
        /// 伤处物流公司
        /// </summary>
        /// <param name="SysNo"></param>
        public  void DeletePmLogisticsCompany(int SysNo)
        {
            IPmBaseDataDao.Instance.DeletePmLogisticsCompany(SysNo);
        }
        /// <summary>
        /// 获取集装箱列表
        /// </summary>
        /// <returns></returns>
        public  List<PmContainer> GetPmContainerList()
        {
            return IPmBaseDataDao.Instance.GetPmContainerList();
        }
        /// <summary>
        /// 获取生产厂家列表
        /// </summary>
        /// <returns></returns>
        public  List<PmManufacturer> GetPmManufacturert()
        {
            return IPmBaseDataDao.Instance.GetPmManufacturert();
        }
        /// <summary>
        /// 获得物流公司列表
        /// </summary>
        /// <returns></returns>
        public  List<PmLogisticsCompany> GetPmLogisticsCompanyList()
        {
            return IPmBaseDataDao.Instance.GetPmLogisticsCompanyList();
        }
        /// <summary>
        /// 获取集装箱数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public PmContainer GetPmContainerBySysNo(int SysNo)
        {
            return IPmBaseDataDao.Instance.GetPmContainerBySysNo(SysNo);
        }
        /// <summary>
        /// 获取生产厂家的数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public PmManufacturer GetPmManufacturerBySysNo(int SysNo)
        {
            return IPmBaseDataDao.Instance.GetPmManufacturerBySysNo(SysNo);
        }
        /// <summary>
        /// 获取当前物流公司数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public PmLogisticsCompany GetPmLogisticsCompanyBySysNo(int SysNo)
        {
            return IPmBaseDataDao.Instance.GetPmLogisticsCompanyBySysNo(SysNo);
        }
    }
}
