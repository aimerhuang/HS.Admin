using Hyt.DataAccess.Transport;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Transport
{
    /// <summary>
    /// 货物出库单操作实体
    /// </summary>
    /// <remarks>
    /// 2016-05-17 杨云奕 添加
    /// </remarks>
    public class DsWhOutStockDaoImpl : IDsWhOutStockDao
    {
        /// <summary>
        /// 添加出库实体
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertMod(Model.Transport.DsWhOutStock mod)
        {
            return Context.Insert("DsWhOutStock", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改出库实体
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdateMod(Model.Transport.DsWhOutStock mod)
        {
            Context.Update("DsWhOutStock", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 通过自动编号删除实体数据
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeleteBySysNo(int SysNo)
        {
            string sql = " delete from DsWhOutStock where SysNo = '" + SysNo + "' ";
            Context.Sql(sql).Execute();
            sql = "delete from DsWhOutStockList where PSysNo = '" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 获取出库单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Transport.DsWhOutStock GetOutStockBySysNo(int SysNo)
        {
            string sql = " select * from DsWhOutStock where SysNo  = '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<DsWhOutStock>();
        }
        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Transport.CBDsWhOutStock GetExtendsOutStockBySysNo(int SysNo)
        {
            string sql = " select DsWhOutStock.*,DsWhCustomer.CusName as CustomerName ,DsWhCustomer.CusAddress as CustomerAddress  from DsWhOutStock inner join DsWhCustomer on DsWhOutStock.CustomerCode=DsWhCustomer.CusCode   where DsWhOutStock.SysNo  = '" + SysNo + "' ";
            CBDsWhOutStock Mod =  Context.Sql(sql).QuerySingle<CBDsWhOutStock>();
            Mod.ModList = GetOutStockListByPSysNo(SysNo);
            return Mod;
        }

        /// <summary>
        /// 添加出库单明细记录
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertModList(Model.Transport.DsWhOutStockList mod)
        {
            return Context.Insert("DsWhOutStockList", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新出库单明细记录
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int UpdateModList(Model.Transport.DsWhOutStockList mod)
        {
            return Context.Update("DsWhOutStockList", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除出库单明细记录
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeleteModListBySysNo(int SysNo)
        {
            string sql = " delete from  DsWhOutStockList where SysNo = '"+SysNo+"' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 通过自动编号获取单独的出库单数据
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Transport.DsWhOutStockList GetOutStockListBySysNo(int SysNo)
        {
            string sql = " select * from DsWhOutStockList where SysNo= '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<DsWhOutStockList>();
        }
        /// <summary>
        /// 通过父ID获取出库单明细
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public override List<Model.Transport.DsWhOutStockList> GetOutStockListByPSysNo(int PSysNo)
        {
            string sql = " select * from DsWhOutStockList where PSysNo= '" + PSysNo + "' ";
            return Context.Sql(sql).QueryMany<DsWhOutStockList>();
        }

        public override void DsWhOutStockPager(ref Model.Pager<CBDsWhOutStock> pageCusList)
        {
            string table = " DsWhOutStock ";
            string sqlWhere = " 1=1 ";

            string typeList = pageCusList.PageFilter.CusCode;
            switch (typeList.Split('_')[0])
            {
                case "ALL":
                    sqlWhere = " 1=1 ";
                    break;
                case "Dealer":
                    sqlWhere = "( DsWhOutStock.CusCode = '" + typeList.Split('_')[1] + "' or  DsWhCustomer.DsSysNo = '" + typeList.Split('_')[2] + "'   )";
                    table = " DsWhOutStock left join DsWhCustomer on DsWhOutStock.CusCode = DsWhCustomer.CusCode ";
                    break;
                case "Customer":
                    sqlWhere = " (DsWhOutStock.CusCode = '" + typeList.Split('_')[1] + "' ) ";
                    
                    break;
            }
            if (pageCusList.PageFilter!=null)
            {
                if (typeList.Split('_')[0].Trim() != "Customer" && !string.IsNullOrEmpty(pageCusList.PageFilter.CustomerName))
                {
                    if (!string.IsNullOrEmpty(sqlWhere))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " CusCode = '" + pageCusList.PageFilter.CustomerName + "' ";
                }

                if(pageCusList.PageFilter.StartTime!=null)
                {
                    if (!string.IsNullOrEmpty(sqlWhere))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " OutStockTime>='" + pageCusList.PageFilter.StartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00' ";
                }

                if (pageCusList.PageFilter.EndTime != null)
                {
                    if (!string.IsNullOrEmpty(sqlWhere))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " OutStockTime<='" + pageCusList.PageFilter.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59' ";
                }

                if (!string.IsNullOrEmpty(pageCusList.PageFilter.StatusCode))
                {
                    if (!string.IsNullOrEmpty(sqlWhere))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " StatusCode ='" + pageCusList.PageFilter.StatusCode + "' ";
                }

                if (!string.IsNullOrEmpty(pageCusList.PageFilter.BatchOutNumber))
                {
                    if (!string.IsNullOrEmpty(sqlWhere))
                    {
                        sqlWhere += " and ";
                    }
                    sqlWhere += " BatchOutNumber ='" + pageCusList.PageFilter.BatchOutNumber + "' ";
                }
            }
            using (var context = Context.UseSharedConnection(true))
            {
                pageCusList.Rows = context.Select<CBDsWhOutStock>(" *  ")
                           .From(table)
                           .Where(sqlWhere)
                           .Paging(pageCusList.CurrentPage, pageCusList.PageSize)
                           .OrderBy(" DsWhOutStock.SysNo desc ")
                           .QueryMany();
                pageCusList.TotalRows = context.Select<int>("count(1)")
                           .From(table)
                           .Where(sqlWhere)
                           .QuerySingle();
            }
        }

        public override List<CBDsWhOutStockList> GetDsWhOutStockByBitNumber(string bitNumber)
        {
            string sql = @"select DsWhOutStockList.GMCourierNumber,DsWhOutStockList.CalculatePrice,DsWhOutStockList.CalculateDiscount,DsWhOutStockList.CalculateTotalValue,DsWhOutStockList.CalculateWeight,DsWhGoodsManagement.*" +
                @",ToTalValue = (select sum(Quantiyt*GoodsPrice) from DsWhGoodsManagementList where PSysNo=DsWhGoodsManagement.SysNo) from 
                         DsWhOutStockList 
                        inner join DsWhGoodsManagement on DsWhGoodsManagement.CourierNumber=DsWhOutStockList.GMCourierNumber ";
            sql += " where CustomsNum='" + bitNumber + "' ";
            return Context.Sql(sql).QueryMany<CBDsWhOutStockList>();
        }
    }
}
