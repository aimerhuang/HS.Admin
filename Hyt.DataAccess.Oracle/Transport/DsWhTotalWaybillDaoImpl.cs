using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    /// <summary>
    /// 总航运控制
    /// </summary>
    /// <remarks>
    /// 2016-5-18 杨云奕 添加
    /// </remarks>
    public class DsWhTotalWaybillDaoImpl : IDsWhTotalWaybillDao
    {
        /// <summary>
        /// 添加总运单
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertMod(Model.Transport.DsWhTotalWaybill mod)
        {
            return Context.Insert("DsWhTotalWaybill", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改总运单数据
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdateMod(Model.Transport.DsWhTotalWaybill mod)
        {
            Context.Update("DsWhTotalWaybill", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除总运单数据
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeleteBySysNo(int SysNo)
        {
            string sql = "delete from DsWhTotalWaybill where SysNo = '"+SysNo+"'";
            Context.Sql(sql).Execute();
            DeleteModListByPSysNo(SysNo);
        }
        /// <summary>
        /// 通过编号获取总运单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Transport.DsWhTotalWaybill GetDsWhTotalWaybillBySysNo(int SysNo)
        {
            string sql = " select * from DsWhTotalWaybill where SysNo = '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<Hyt.Model.Transport.DsWhTotalWaybill>();
        }
        /// <summary>
        /// 通过编号获取总运单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Transport.CBDsWhTotalWaybill GetCBDsWhTotalWaybillBySysNo(int SysNo)
        {
            string sql = " select * from DsWhTotalWaybill where SysNo = '" + SysNo + "' ";
            CBDsWhTotalWaybill mod=  Context.Sql(sql).QuerySingle<Hyt.Model.Transport.CBDsWhTotalWaybill>();
            mod.ModList = GetDsWhTotalWaybillListByPSysNo(SysNo);
            return mod;
        }
        /// <summary>
        /// 添加总运单包裹数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertModList(Model.Transport.DsWhTotalWaybillList mod)
        {
            return Context.Insert("DsWhTotalWaybillList", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改总运单包裹数据
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int UpdateModList(Model.Transport.DsWhTotalWaybillList mod)
        {
            return Context.Update("DsWhTotalWaybillList", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 通过父id删除总运单包裹明细
        /// </summary>
        /// <param name="PSysNo"></param>
        public override void DeleteModListByPSysNo(int PSysNo)
        {
            string sql = "delete from DsWhTotalWaybillList where PSysNo = '" + PSysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 通过父id获取总运单中包裹数据
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public override List<Model.Transport.DsWhTotalWaybillList> GetDsWhTotalWaybillListByPSysNo(int PSysNo)
        {
            string sql = " select * from  DsWhTotalWaybillList where PSysNo = '" + PSysNo + "' ";
            return Context.Sql(sql).QueryMany<DsWhTotalWaybillList>();
        }

        public override void DeleteModListBySysNo(int SysNo)
        {
            string sql = "delete from DsWhTotalWaybillList where SysNo = '" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }

        public override void DsWhTotalWaybillPager(ref Model.Pager<CBDsWhTotalWaybill> pageCusList)
        {
            string table = @" DsWhTotalWaybill inner join LgDeliveryType on DsWhTotalWaybill.ServiceType=LgDeliveryType.OverseaCarrier  ";
            string sqlWhere = " 1=1 ";
            string typeList = pageCusList.PageFilter.CusCode;
            switch (typeList.Split('_')[0])
            {
                case "ALL":
                    sqlWhere = " 1=1 ";
                    break;
                case "Dealer":
                    sqlWhere = " ( DsWhTotalWaybill.CusCode = '" + typeList.Split('_')[1] + "' or  DsWhCustomer.DsSysNo = '" + typeList.Split('_')[2] + "') ";
                    table = " DsWhTotalWaybill left join DsWhCustomer on DsWhTotalWaybill.CusCode = DsWhCustomer.CusCode inner join LgDeliveryType on DsWhTotalWaybill.ServiceType=LgDeliveryType.OverseaCarrier  ";
                    break;
                case "Customer":
                    sqlWhere = " (DsWhTotalWaybill.CusCode = '" + typeList.Split('_')[1] + "') ";
                   
                    break;
            }

            if (!string.IsNullOrEmpty(pageCusList.PageFilter.StatusCode))
            { 
                if(sqlWhere!="")
                {
                    sqlWhere += " and ";
                }
                sqlWhere += " DsWhTotalWaybill.StatusCode='" + pageCusList.PageFilter.StatusCode + "' ";
            }

            using (var context = Context.UseSharedConnection(true))
            {
                pageCusList.Rows = context.Select<CBDsWhTotalWaybill>(" DsWhTotalWaybill.*,LgDeliveryType.DeliveryTypeName  ")
                           .From(table)
                           .Where(sqlWhere)
                           .Paging(pageCusList.CurrentPage, pageCusList.PageSize)
                           .OrderBy(" DsWhTotalWaybill.SysNo desc ")
                           .QueryMany();
                pageCusList.TotalRows = context.Select<int>("count(1)")
                           .From(table)
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }



        public override List<DsWhTotalWaybill> GetTotalWayBillByCourierNumber(string CourierNumber)
        {
            string sql = @"select DsWhTotalWaybill.* from DsWhTotalWaybill inner join DsWhTotalWaybillList on DsWhTotalWaybill.SysNo = DsWhTotalWaybillList.PSysNo
                        inner join DsWhPackage on DsWhTotalWaybillList.PackageNumber=DsWhPackage.PackageNumber 
                        inner join DsWhPackageList on DsWhPackage.SysNo=DsWhPackageList.PSysNo 
                        where DsWhPackageList.CourierNumber='" + CourierNumber + "'";
            return Context.Sql(sql).QueryMany<DsWhTotalWaybill>();
        }
    }
}
