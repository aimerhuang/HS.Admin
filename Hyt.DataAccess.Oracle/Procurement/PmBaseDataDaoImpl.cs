using Hyt.DataAccess.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Procurement
{
    /// <summary>
    /// 采购基础数据定义
    /// </summary>
    /// <remarks>杨云奕 添加</remarks>
    public class PmBaseDataDaoImpl : IPmBaseDataDao
    {
        /// <summary>
        /// 添加集装箱信息
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertPmContainer(Model.Procurement.PmContainer mod)
        {
           return  Context.Insert<Model.Procurement.PmContainer>("PmContainer", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 添加生产厂家
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertPmManufacturer(Model.Procurement.PmManufacturer mod)
        {
            return Context.Insert<Model.Procurement.PmManufacturer>("PmManufacturer", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 添加物流公司
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertPmLogisticsCompany(Model.Procurement.PmLogisticsCompany mod)
        {
            return Context.Insert<Model.Procurement.PmLogisticsCompany>("PmLogisticsCompany", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新集装箱信息
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdatePmContainer(Model.Procurement.PmContainer mod)
        {
             Context.Update<Model.Procurement.PmContainer>("PmContainer", mod).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }
        /// <summary>
        /// 更新生产厂家
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdatePmManufacturer(Model.Procurement.PmManufacturer mod)
        {
            Context.Update<Model.Procurement.PmManufacturer>("PmManufacturer", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 更新国际物流信息
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdatePmLogisticsCompany(Model.Procurement.PmLogisticsCompany mod)
        {
            Context.Update<Model.Procurement.PmLogisticsCompany>("PmLogisticsCompany", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除集装箱信息
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeletePmContainer(int SysNo)
        {
            string sql = " delete from PmContainer where SysNo='"+SysNo+"' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 删除生产厂家
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeletePmManufacturer(int SysNo)
        {
            string sql = " delete from PmManufacturer where SysNo='" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 删除物流公司信息
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeletePmLogisticsCompany(int SysNo)
        {
            string sql = " delete from PmLogisticsCompany where SysNo='" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 获取集装箱信息列表
        /// </summary>
        /// <returns></returns>
        public override List<Model.Procurement.PmContainer> GetPmContainerList()
        {
            string sql = " select * from PmContainer  ";
            return Context.Sql(sql).QueryMany<Model.Procurement.PmContainer>();
        }
        /// <summary>
        /// 获取生产厂家信息列表
        /// </summary>
        /// <returns></returns>
        public override List<Model.Procurement.PmManufacturer> GetPmManufacturert()
        {
            string sql = " select * from PmManufacturer  ";
            return Context.Sql(sql).QueryMany<Model.Procurement.PmManufacturer>();
        }
        /// <summary>
        /// 获取物流公司列表
        /// </summary>
        /// <returns></returns>
        public override List<Model.Procurement.PmLogisticsCompany> GetPmLogisticsCompanyList()
        {
            string sql = " select * from PmLogisticsCompany  ";
            return Context.Sql(sql).QueryMany<Model.Procurement.PmLogisticsCompany>();
        }
        /// <summary>
        /// 通过编号获取集装箱信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Procurement.PmContainer GetPmContainerBySysNo(int SysNo)
        {
            string sql = " select * from PmContainer where SysNo='"+SysNo+"'  ";
            return Context.Sql(sql).QuerySingle<Model.Procurement.PmContainer>();
        }
        /// <summary>
        /// 通过编号获取生产厂家信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Procurement.PmManufacturer GetPmManufacturerBySysNo(int SysNo)
        {
            string sql = " select * from PmManufacturer where SysNo='" + SysNo + "'    ";
            return Context.Sql(sql).QuerySingle<Model.Procurement.PmManufacturer>();
        }
        /// <summary>
        /// 通过编号获取物流公司信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Procurement.PmLogisticsCompany GetPmLogisticsCompanyBySysNo(int SysNo)
        {
            string sql = " select * from PmLogisticsCompany  where SysNo='" + SysNo + "'  ";
            return Context.Sql(sql).QuerySingle<Model.Procurement.PmLogisticsCompany>();
        }

        /// <summary>
        /// 获得供应商编码
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-1-6 杨浩 创建</remarks>
        public override string GetManufacturerCode(int sysno)
        {
            return Context.Sql("select top 1 ManufacturerCode from  PmManufacturer where sysno=" + sysno)
                .QuerySingle<string>();
        }
    }
}
