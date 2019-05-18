using System;
using System.Collections.Generic;
using System.ComponentModel;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.DataAccess.Warehouse;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    ///调货单 数据访问实现
    ///</summary>
    /// <remarks> 2016-04-01 朱成果 创建</remarks>
    public class TransferCargoDaoImpl : ITransferCargoDao
    {
        #region 自动生成代码
        /// <summary>
        /// 插入(调货单)
        ///</summary>
        /// <param name="entity">调货单</param>
        /// <returns>新增记录编号</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public override int Insert(TransferCargo entity)
        {
            entity.SysNo = Context.Insert("TransferCargo", entity)
                                .AutoMap(o => o.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新(调货单)
        ///</summary>
        /// <param name="entity">调货单</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public override int Update(TransferCargo entity)
        {
            return Context.Update("TransferCargo", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取(调货单)
        ///</summary>
        /// <param name="sysno">编号</param>
        /// <returns>调货单</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public override TransferCargo GetEntity(int sysno)
        {
            return Context.Sql("select * from TransferCargo where SysNo=@sysno")
                           .Parameter("sysno", sysno)
                           .QuerySingle<TransferCargo>();

        }

        /// <summary>
        /// 删除(调货单)
        ///</summary>
        /// <param name="sysno">编号</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public override int Delete(int sysno)
        {
            return Context.Sql("delete  from TransferCargo where SysNo=@sysno")
                           .Parameter("sysno", sysno)
                           .Execute();
        }
        #endregion

        /// <summary>
        /// 调货单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库的权限</param>
        /// <returns></returns>
        /// <remarks>2016-04-05 杨浩 创建</remarks>
        public override Pager<CBTransferCargo> SearchTransferCargo(ParaTransferCargoFilter filter, int pageIndex, int pageSize, int userSysNo, bool isHasAllWarehouse)
        {
            var pager = new Pager<CBTransferCargo>();
            using (IDbContext context = Context.UseSharedConnection(true))
            {
                string select =
                    @"a.sysno,a.ordersysno,a.stockoutsysno,wh.warehousename as applywarehouse,wh.StreetAddress,wh.Contact,wh.Phone,sy.username as CreatedName,a.createddate,wwh.warehousename as deliverywarehouse,sys.username as LastName,a.lastupdatedate,a.status,a.remarks,a.applywarehousesysno,a.deliverywarehousesysno";
                string table = @"transfercargo a 
                left join whwarehouse wh on a.applywarehousesysno=wh.sysno
                left join syuser sy on sy.sysno=a.createdby
                left join whwarehouse wwh on a.deliverywarehousesysno=wwh.sysno
                left join syuser sys on sys.sysno=a.lastupdateby";

                string where = " 1=1 ";
                var list = new List<object>();
                if (filter.SysNo.HasValue && filter.SysNo>0)
                {
                    list.Add(filter.SysNo);
                    where +="and a.stockoutsysno = @"+(list.Count-1);
                }
                if (filter.Status.HasValue)
                {
                    list.Add(filter.Status);
                    where += "and a.Status = @" + (list.Count - 1);
                }

                if (filter.WarehouseSysNo.HasValue && filter.WarehouseSysNo>0)
                {
                    list.Add(filter.WarehouseSysNo);
                    where += "and a.applywarehousesysno = @" + (list.Count - 1);
                }

                if (filter.DeliveryWarehouseSysNo.HasValue &&filter.DeliveryWarehouseSysNo>0)
                {
                    list.Add(filter.DeliveryWarehouseSysNo);
                    where += "and a.deliverywarehousesysno = @" + (list.Count - 1);
                }
       

                var parms = list.ToArray();
                if (!isHasAllWarehouse)
                {
                    where += string.Format(" and exists (select 1 from SyUserWarehouse f  where (f.WarehouseSysNo = a.deliverywarehousesysno or f.WarehouseSysNo = a.applywarehousesysno) and f.UserSysNo = {0})", userSysNo);
                }
           
                
                pager.TotalRows = context.Sql(@"
                    select count(1) from TransferCargo a  where " + where)
                                         .Parameters(parms)                                 
                                         .QuerySingle<int>();

                pager.Rows = context.Select<CBTransferCargo>(select)
                                                      .From(table)
                                                      .AndWhere(where)
                                                      .Parameters(parms)
                                                      .OrderBy("a.SysNo desc")
                                                      .Paging(pageIndex, pageSize)
                                                      .QueryMany();
            }
            return pager;
        }

    

        /// <summary>
        /// 根据出库单编号获取调货单
        /// </summary>
        /// <param name="stockOutSysno">出库单编号</param>
        /// <returns>调货单</returns>
        /// <remarks>2016-04-06 谭显锋 创建</remarks>
        public override TransferCargo GetExistTransferCargoByStockOutSysno(int stockOutSysno)
        {
            return Context.Sql("select * from TransferCargo where StockOutSysNo=@StockOutSysNo")
                           .Parameter("StockOutSysNo", stockOutSysno)
                           .QuerySingle<TransferCargo>();
        }

        /// <summary>
        /// 根据订单号查询调货单
        /// </summary>
        /// <param name="orderSysno">订单号</param>
        /// <returns>调货单</returns>
        /// <remarks>2016-04-08 杨浩 创建</remarks>
        public override IList<CBTransferCargo> GetTransferCargoesByOrderSysno(int orderSysno)
        {
            return Context.Sql(@"select 
                a.sysno,a.ordersysno,a.stockoutsysno,wh.warehousename as applywarehouse,sy.username as CreatedName,a.createddate,
                wwh.warehousename as deliverywarehouse,sys.username as LastName,a.lastupdatedate,a.status,a.remarks
                 from transfercargo a 
                left join whwarehouse wh on a.applywarehousesysno=wh.sysno
                left join syuser sy on sy.sysno=a.createdby
                left join whwarehouse wwh on a.deliverywarehousesysno=wwh.sysno
                left join syuser sys on sys.sysno=a.lastupdateby where ordersysno=@ordersysno")
                .Parameter("ordersysno", orderSysno)
                .QueryMany<CBTransferCargo>();
        }
    }

}
