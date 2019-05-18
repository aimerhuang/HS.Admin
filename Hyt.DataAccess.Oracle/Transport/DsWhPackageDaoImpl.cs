using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    /// <summary>
    /// 包裹打包
    /// </summary>
    /// <remarks>
    /// 2016-5-18 杨云奕 添加
    /// </remarks>
    public class DsWhPackageDaoImpl : IDsWhPackageDao
    {
        /// <summary>
        /// 添加包裹打包
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertMod(Model.Transport.DsWhPackage mod)
        {
            return Context.Insert("DsWhPackage", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改包裹打包
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override void UpdateMod(Model.Transport.DsWhPackage mod)
        {
            Context.Update("DsWhPackage", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除包裹打包数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override void DeleteBySysNo(int SysNo)
        {
            string sql = "delete from DsWhPackage where SysNo = '" + SysNo + "'";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 获取包裹打包数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override Model.Transport.DsWhPackage GetDsWhPackageBySysNo(int SysNo)
        {
            string sql = " select * from  DsWhPackage where sysNo = '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<DsWhPackage>();
        }
        /// <summary>
        /// 获取包裹打包数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override Model.Transport.CBDsWhPackage GetCBDsWhPackageBySysNo(int SysNo)
        {
            string sql = " select * from  DsWhPackage where sysNo = '" + SysNo + "' ";
            CBDsWhPackage mod =  Context.Sql(sql).QuerySingle<CBDsWhPackage>();
            mod.ModList = GetWhPackageListByPSysNo(SysNo);
            return mod;
        }

        /// <summary>
        /// 添加包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertModList(Model.Transport.DsWhPackageList mod)
        {
            return Context.Insert("DsWhPackageList", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override void UpdateModList(Model.Transport.DsWhPackageList mod)
        {
            Context.Update("DsWhPackage", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override void DeleteModListBySysNo(int SysNo)
        {
            string sql = "delete from DsWhPackageList where SysNo = '" + SysNo + "'";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 删除包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override void DeleteModListByPSysNo(int PSysNo)
        {
            string sql = "delete from DsWhPackageList where PSysNo = '" + PSysNo + "'";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 获取包裹打包明细
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override List<Model.Transport.DsWhPackageList> GetWhPackageListByPSysNo(int pSysNo)
        {
            string sql = " select * from  DsWhPackageList where PSysNo = '" + pSysNo + "' ";
            return Context.Sql(sql).QueryMany<DsWhPackageList>();
        }

        public override void DoDsWhPackageQuery(Model.Pager<DsWhPackage> pageCusList)
        {
            string table = " DsWhPackage ";

            #region sql条件
            string sqlWhere = @" 1=-1 ";
            #endregion
            string typeList = pageCusList.PageFilter.CusCode;
            switch (typeList.Split('_')[0])
            {
                case "ALL":
                    sqlWhere = " 1=1 ";
                    break;
                case "Dealer":
                    sqlWhere = " DsWhPackage.CusCode = '" + typeList.Split('_')[1] + "'  or  DsWhCustomer.DsSysNo = '" + typeList.Split('_')[2] + "' ";
                    table = "  DsWhPackage left join DsWhCustomer on DsWhPackage.CusCode = DsWhCustomer.CusCode ";
                    break;
                case "Customer":
                    sqlWhere = " DsWhPackage.CusCode = '" + typeList.Split('_')[1] + "' ";

                    break;
            }
            if (!string.IsNullOrEmpty(pageCusList.PageFilter.StatusCode))
            {
                if(!string.IsNullOrEmpty(sqlWhere))
                {
                    sqlWhere += " and ";
                }
                sqlWhere += " DsWhPackage.StatusCode= '" + pageCusList.PageFilter.StatusCode + "' ";
            }
            using (var context = Context.UseSharedConnection(true))
            {
                pageCusList.Rows = context.Select<DsWhPackage>("  *  ")
                           .From(table)
                           .Where(sqlWhere)
                           .Paging(pageCusList.CurrentPage, pageCusList.PageSize)
                           .OrderBy("DsWhPackage.SysNo desc")
                           .QueryMany();
                pageCusList.TotalRows = context.Select<int>("count(1)")
                           .From(table)
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }

        public override List<DsWhPackageList> GetDsWhPackageListByCourierNumbers(List<string> packNumList)
        {
            string sql = " select * from DsWhPackageList where CourierNumber in ('" + string.Join("','", packNumList.ToArray()) + "') ";
            return Context.Sql(sql).QueryMany<DsWhPackageList>();
        }

        public override List<CBDsWhPackageList> GetCBWhPackageListByPSysNo(int pSysNo)
        {
            string sql = " select DsWhPackageList.*,DsWhGoodsManagement.CustomerCode,SyUser.UserName as ScannedName,DsWhPackageList.ScannedDatetime as ScanDatetime,DsWhGoodsManagement.ReceiptCity as CityInfo from " +
                " DsWhPackageList inner join SyUser on DsWhPackageList.ScannedBy=SyUser.SysNo " +
                " inner join DsWhGoodsManagement on DsWhGoodsManagement.CourierNumber=DsWhPackageList.CourierNumber ";
            sql += " where DsWhPackageList.PSysNo = '" + pSysNo + "'  ";
            return Context.Sql(sql).QueryMany<CBDsWhPackageList>();
        }

        public override List<DsWhPackage> GetModByServiceTypeAndCompletePackage(string ServiceType)
        {
            string sql = " select * from DsWhPackage where ServiceType = '" + ServiceType 
                + "' and StatusCode = '" + (int)Hyt.Model.WorkflowStatus.DsWhStatus.包裹状态.完成打包 + "' ";
            return Context.Sql(sql).QueryMany<DsWhPackage>();
        }

        public override List<DsWhPackage> GetDsWhPackageByCourierNumbers(List<string> packNumList)
        {
            string sql = " select * from DsWhPackage where PackageNumber in ('" + string.Join("','", packNumList.ToArray()) + "') ";
            return Context.Sql(sql).QueryMany<DsWhPackage>();
        }

        /// <summary>
        /// 航空包裹运单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override CBDsWhPackage GetPackageOnAirLineData(int SysNo)
        {

            string sql = @" select DsWhPackage.*,DsWhTotalWaybill.MawbNumber,DsWhTotalWaybill.Dest,DsWhTotalWaybill.FlightNumber,
                            Count=(select count(1) from DsWhTotalWaybill a where DsWhTotalWaybill.MawbNumber=a.MawbNumber ) " +
                        @" from DsWhTotalWaybill inner join  DsWhTotalWaybillList on DsWhTotalWaybill.SysNo=DsWhTotalWaybillList.PSysNo 
                           inner join DsWhPackage on DsWhPackage.PackageNumber=DsWhTotalWaybillList.PackageNumber
                           where DsWhPackage.SysNo='" + SysNo + "'  ";
            return Context.Sql(sql).QuerySingle<CBDsWhPackage>();
        }
    }
}
