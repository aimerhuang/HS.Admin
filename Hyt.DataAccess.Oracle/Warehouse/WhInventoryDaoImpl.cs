using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    public class WhInventoryDaoImpl : IWhInventoryDao
    {
        /// <summary>
        /// 创建盘点单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public override WhInventory CreateWhInventory(WhInventory model)
        {
            model.SysNo = Context.Insert<WhInventory>("WhInventory", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
            return model;
        }

        /// <summary>
        /// 分页查询盘点单列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public override Pager<CBWhInventory> QueryWhInventoryPager(Pager<CBWhInventory> pager)
        {
            //string sql = @"SELECT * FROM WhInventory";

            string sqlCountText = "SELECT COUNT(1) FROM WhInventory";

            pager.Rows = Context.Select<CBWhInventory>("*").From("WhInventory").OrderBy("SysNo DESC").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = Context.Sql(sqlCountText).QuerySingle<int>();

            return pager;
        }

        /// <summary>
        /// 获取盘点单实体
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-21 陈海裕 创建</remarks>
        public override WhInventory GetWhInventoryEntity(int sysNo)
        {
            return Context.Sql("SELECT * FROM WhInventory WHERE SysNo=" + sysNo).QuerySingle<WhInventory>();
        }

        /// <summary>
        /// 获取库存盘点单商品列表（添加盘点商品用）
        /// </summary>
        /// <param name="inventorySysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public override List<WhInventoryDetail> GetWhInventoryProducts(int inventorySysNo)
        {
            string sqlText = "SELECT * FROM WhInventoryDetail WHERE InventorySysNo=" + inventorySysNo;
            return Context.Sql(sqlText).QueryMany<WhInventoryDetail>();
        }

        /// <summary>
        /// 获取库存盘点单商品列表（单个）
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <param name="inventorySysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-21 陈海裕 创建</remarks>
        public override WhInventoryDetail GetWhInventoryDetailEntity(int productSysNo, int inventorySysNo)
        {
            return Context.Sql("SELECT * FROM WhInventoryDetail WHERE InventorySysNo=" + inventorySysNo + " AND ProductSysNo=" + productSysNo).QuerySingle<WhInventoryDetail>();
        }

        /// <summary>
        /// 更新盘点单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-24 陈海裕 创建</remarks>
        public override int UpdateWhInventory(WhInventory model)
        {
            return Context.Update("WhInventory", model).AutoMap(o => o.SysNo).Where("SysNo", model.SysNo).Execute();
        }

        /// <summary>
        /// 添加盘点单明细
        /// </summary>
        /// <param name="inventoryDetail"></param>
        /// <returns></returns>
        /// <remarks>2016-06-20 陈海裕 创建</remarks>
        public override int AddWhInventoryDetail(WhInventoryDetail inventoryDetail)
        {
            return Context.Insert<WhInventoryDetail>("WhInventoryDetail", inventoryDetail).AutoMap(x => x.SysNo).Execute();
        }

        /// <summary>
        /// 分页查询盘点单明细
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-21 陈海裕 创建</remarks>
        public override Pager<CBWhInventoryDetail> QueryWhInventoryDetailPager(Pager<CBWhInventoryDetail> pager, int whPositionSysNo = 0, string searchKeyWord = "")
        {
            StringBuilder _select = new StringBuilder();
            StringBuilder _from = new StringBuilder();
            StringBuilder _where = new StringBuilder();
            _select.AppendLine("WID.*,pri1.Price");
            _from.AppendLine("WhInventoryDetail WID left join (select * from PdPrice where PriceSource=10 and SourceSysNo=1) pri1 on pri1.ProductSysNo =  WID.ProductSysNo");
            _where.AppendLine("WID.InventorySysNo=" + pager.PageFilter.InventorySysNo);
            if (whPositionSysNo > 0)
            {
                _from.AppendLine("INNER JOIN WhProductWarehousePositionAssociation WPA ON WID.ProductStockSysNo=WPA.ProductStockSysNo");
                _from.AppendLine("INNER JOIN WhWarehousePosition WP ON WPA.WarehousePositionSysNo=WP.SysNo");
                _where.AppendLine("AND (0=" + whPositionSysNo + " OR WP.SysNo=" + whPositionSysNo + ")");
            }
            if (!string.IsNullOrWhiteSpace(searchKeyWord))
            {
                _from.AppendLine("INNER JOIN PdProduct PRO ON WID.ProductSysNo=PRO.SysNo");
                _where.Append("AND (PRO.ProductName LIKE '%").Append(searchKeyWord).Append("%' OR PRO.EasName LIKE '%")
                    .Append(searchKeyWord).Append("%' OR PRO.ErpCode LIKE '%").Append(searchKeyWord).AppendLine("%')");
            }
            pager.Rows = Context.Select<CBWhInventoryDetail>(_select.ToString()).From(_from.ToString()).Where(_where.ToString()).OrderBy("WID.SysNo DESC")
                .Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = Context.Select<int>("COUNT(1)").From(_from.ToString()).Where(_where.ToString()).QuerySingle();

            return pager;
        }

        /// <summary>
        /// 删除盘点单商品
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-23 陈海裕 创建</remarks>
        public override int DeleteWhInventoryDetail(int sysNo)
        {
            return Context.Delete("WhInventoryDetail").Where("sysNo", sysNo).Execute();
        }

        /// <summary>
        /// 更新盘点单商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-24 陈海裕 创建</remarks>
        public override int UpdateWhInventoryDetail(WhInventoryDetail model)
        {
            return Context.Update("WhInventoryDetail", model).AutoMap(o => o.SysNo).Where("SysNo", model.SysNo).Execute();
        }

        /// <summary>
        /// 批量插入导入盘点记录
        /// </summary>
        /// <param name="lstToInsert">待插入数据</param>
        /// <remarks>2016-08-19 刘伟豪 创建</remarks>
        public override void InsertWhInventoryDetail(List<WhInventoryDetail> models)
        {
            foreach (var model in models)
            {
                try
                {
                    Context.Insert<WhInventoryDetail>("WhInventoryDetail", model).AutoMap(x => x.SysNo).Execute();
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 批量更新导入盘点记录
        /// </summary>
        /// <param name="lstToUpdate">待更新数据</param>
        /// <remarks>2016-08-19 刘伟豪 创建</remarks>
        public override void UpdateWhInventoryDetail(List<WhInventoryDetail> models)
        {
            foreach (var model in models)
            {
                try
                {
                    Context.Update("WhInventoryDetail", model).AutoMap(o => o.SysNo).Where("SysNo", model.SysNo).Execute();
                }
                catch
                {
                    continue;
                }
            }
        }
        /// <summary>
        /// 通过日期获取所有的盘点单数据
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        /// <remarks>2016-12-26 杨云奕 添加</remarks>
        public override List<WhInventory> GetAllWhInventoryListByDate(DateTime dateTime)
        {
            string sql = " select * from WhInventory where Status=1  and EndDate>='" + dateTime.ToString("yyyy-MM-dd") + " 00:00:00'  ";
            return Context.Sql(sql).QueryMany<WhInventory>();
        }
    }
}