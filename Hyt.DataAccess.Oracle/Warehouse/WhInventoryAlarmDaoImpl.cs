using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 取仓库库存报警数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class WhInventoryAlarmDaoImpl : IWhInventoryAlarmDao
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override int Insert(WhInventoryAlarm entity)
        {
            entity.SysNo = Context.Insert("WhInventoryAlarm", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public override void Update(WhInventoryAlarm entity)
        {
            Context.Update("WhInventoryAlarm", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="ProductStockSysNo">库存编号</param>
        /// <returns></returns>
        /// <remarks>2016-06-15 王耀发 创建</remarks>
        public override WhInventoryAlarm GetAlarmByStockSysNo(int ProductStockSysNo)
        {
            return Context.Sql("select * from WhInventoryAlarm where ProductStockSysNo=@0", ProductStockSysNo).QuerySingle<WhInventoryAlarm>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        public override Pager<CBWhInventoryAlarm> Query(ParaWhInventoryAlarmFilter filter)
        {
            const string sql = @"(select wia.* ,pd.ErpCode,pd.ProductName,wh.BackWarehouseName,ps.StockQuantity 
                    from WhInventoryAlarm wia left join PdProductStock ps on wia.ProductStockSysNo = ps.SysNo
                    left join WhWarehouse wh on ps.WarehouseSysNo = wh.SysNo
                    left join PdProduct pd on ps.PdProductSysNo = pd.SysNo 
                    where (@0 is null or ps.WarehouseSysNo = @1) and         
                    ((@2 is null or charindex(pd.ErpCode,@3)>0) or 
                    (@4 is null or charindex(pd.ProductName,@5)>0)) 
                                   ) tb";

            var dataList = Context.Select<CBWhInventoryAlarm>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var paras = new object[]
                {
                    filter.WarehouseSysNo,filter.WarehouseSysNo,
                    filter.ErpCode,filter.ErpCode,
                    filter.ProductName,filter.ProductName
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBWhInventoryAlarm>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;

        }
        /// <summary>
        /// 搜索报警商品
        /// </summary>
        /// <returns></returns>
        public override List<CBWhInventoryAlarm> SearAlarmProductStockList()
        {
            string sql = @"SELECT PdProduct.*,[WhInventoryAlarm].*,PdProductStock.StockQuantity,WhWarehouse.BackWarehouseName
                              FROM [WhInventoryAlarm] inner join  PdProductStock on WhInventoryAlarm.ProductStockSysNo=PdProductStock.SysNo
                              inner join PdProduct on PdProductStock.PdProductSysNo=PdProduct.SysNo
                              inner join WhWarehouse on PdProductStock.WarehouseSysNo=WhWarehouse.sysNo
                              where (WhInventoryAlarm.Upperlimit>0 and PdProductStock.StockQuantity> WhInventoryAlarm.Upperlimit ) or 
		                            (WhInventoryAlarm.Lowerlimit>0 and PdProductStock.StockQuantity< WhInventoryAlarm.Lowerlimit ) ";
            return Context.Sql(sql).QueryMany<CBWhInventoryAlarm>();
        }

        public override int GetAlarmProductStockCount()
        {
            string sql = @"SELECT count(*)
                              FROM [WhInventoryAlarm] inner join  PdProductStock on WhInventoryAlarm.ProductStockSysNo=PdProductStock.SysNo
                              inner join PdProduct on PdProductStock.PdProductSysNo=PdProduct.SysNo
                              inner join WhWarehouse on PdProductStock.WarehouseSysNo=WhWarehouse.sysNo
                              where (WhInventoryAlarm.Upperlimit>0 and PdProductStock.StockQuantity> WhInventoryAlarm.Upperlimit ) or 
		                            (WhInventoryAlarm.Lowerlimit>0 and PdProductStock.StockQuantity< WhInventoryAlarm.Lowerlimit ) ";
            return Context.Sql(sql).QuerySingle<int>();
        }

        public override int GetAlarmProductStockCount(IList<WhWarehouse> list)
        {
            if (list != null)
            {
                string sql = @"SELECT count(*)
                              FROM [WhInventoryAlarm] inner join  PdProductStock on WhInventoryAlarm.ProductStockSysNo=PdProductStock.SysNo
                              inner join PdProduct on PdProductStock.PdProductSysNo=PdProduct.SysNo
                              inner join WhWarehouse on PdProductStock.WarehouseSysNo=WhWarehouse.sysNo
                              where ((WhInventoryAlarm.Upperlimit>0 and PdProductStock.StockQuantity> WhInventoryAlarm.Upperlimit ) or 
		                            (WhInventoryAlarm.Lowerlimit>0 and PdProductStock.StockQuantity< WhInventoryAlarm.Lowerlimit )) and  ";
                sql += "  WhWarehouse.SysNo in (" + string.Join(",", list.ToDictionary(x => x.SysNo).Keys) + ") ";
                return Context.Sql(sql).QuerySingle<int>();
            }
            else
            {
                return 0;
            }
        }

        public override List<WhInventoryAlarm> GetAllAlarm()
        {
            return Context.Sql(" select * from WhInventoryAlarm ").QueryMany<WhInventoryAlarm>();
        }
    }
}
