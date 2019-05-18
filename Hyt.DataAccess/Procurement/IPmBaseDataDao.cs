using Hyt.DataAccess.Base;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Procurement
{
    /// <summary>
    /// 采购基础数据定义
    /// </summary>
    /// <remarks>杨云奕 添加</remarks>
    public abstract class IPmBaseDataDao : DaoBase<IPmBaseDataDao>
    {
        /// <summary>
        /// 添加集装箱信息
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertPmContainer(PmContainer mod);
        /// <summary>
        /// 添加生产厂家
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertPmManufacturer(PmManufacturer mod);
        /// <summary>
        /// 添加物流公司
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertPmLogisticsCompany(PmLogisticsCompany mod);
        /// <summary>
        /// 更新集装箱信息
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdatePmContainer(PmContainer mod);
        /// <summary>
        /// 更新生产厂家
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdatePmManufacturer(PmManufacturer mod);
        /// <summary>
        /// 更新国际物流信息
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdatePmLogisticsCompany(PmLogisticsCompany mod);

        /// <summary>
        /// 删除集装箱信息
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeletePmContainer(int SysNo);
        /// <summary>
        /// 删除生产厂家
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeletePmManufacturer(int SysNo);
        /// <summary>
        /// 删除物流公司信息
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeletePmLogisticsCompany(int SysNo);
        /// <summary>
        /// 获取集装箱信息列表
        /// </summary>
        /// <returns></returns>
        public abstract List<PmContainer> GetPmContainerList();
        /// <summary>
        /// 获取生产厂家信息列表
        /// </summary>
        /// <returns></returns>
        public abstract List<PmManufacturer> GetPmManufacturert();
        /// <summary>
        /// 获取物流公司列表
        /// </summary>
        /// <returns></returns>
        public abstract List<PmLogisticsCompany> GetPmLogisticsCompanyList();
        /// <summary>
        /// 通过编号获取集装箱信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract PmContainer GetPmContainerBySysNo(int SysNo);
        /// <summary>
        /// 通过编号获取生产厂家信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract PmManufacturer GetPmManufacturerBySysNo(int SysNo);
        /// <summary>
        /// 通过编号获取物流公司信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract PmLogisticsCompany GetPmLogisticsCompanyBySysNo(int SysNo);


        /// <summary>
        /// 获得供应商编码
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-1-6 杨浩 创建</remarks>
        public abstract string GetManufacturerCode(int sysno);
    }
}
