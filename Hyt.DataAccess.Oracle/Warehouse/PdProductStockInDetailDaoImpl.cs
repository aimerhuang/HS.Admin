using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 取定制商品数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class PdProductStockInDetailDaoImpl : IPdProductStockInDetailDao
    {
        /// <summary>
        /// 入库信息
        /// </summary>
        /// <param name="filter">入库信息</param>
        /// <returns>返回入库信息</returns>
        /// <remarks>2015-08-27 王耀发 创建</remarks>
        public override Pager<PdProductStockInDetailList> GetPdProductStockInDetailList(ParaProductStockInDetailFilter filter)
        {
            //            const string sql = @"(select a.*,b.StockInNo,b.StorageTime,c.WarehouseName,c.StreetAddress,d.ErpCode,d.EasName from PdProductStockInDetail a ,PdProductStockIn b ,WhWarehouse c ,PdProduct d
            //                     where a.ProductStockInSysNo = b.SysNo and a.WarehouseSysNo = c.SysNo and a.PdProductSysNo = d.SysNo and 
            //                    (@0 is null or charindex(b.StockInNo,@1)>0) and                    
            //                    (@2 is null or charindex(d.ErpCode,@3)>0) and 
            //                    (@4 is null or charindex(d.EasName,@5)>0) and
            //                    (@6 is null or a.WarehouseSysNo = @7)
            //                                   ) tb";

            const string sql = @"(select a.*
                    from PdProductStockIn a 
                    where a.SysNo in(select distinct ProductStockInSysNo from PdProductStockInDetail where WarehouseSysNo = @0) and (@1 is null or charindex(a.StockInNo,@2)>0) and                                                                                                                                --促销名称
                    (@3 is null or Status = @4)
                                   ) tb";

            var dataList = Context.Select<PdProductStockInDetailList>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            //var paras = new object[]
            //    {
            //        filter.StockInNo, filter.StockInNo,
            //        filter.ErpCode,filter.ErpCode,
            //        filter.EasName,filter.EasName,
            //        filter.WarehouseSysNo,filter.WarehouseSysNo
            //    };
            var paras = new object[]
                {
                    filter.WarehouseSysNo,
                    filter.StockInNo, filter.StockInNo,
                    filter.Status,filter.Status
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<PdProductStockInDetailList>
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
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public override PdProductStockInDetailList GetEntity(int sysNo)
        {

            return Context.Sql("select a.*,b.StockInNo,c.WarehouseName,c.StreetAddress,d.ErpCode,d.EasName from PdProductStockInDetail a ,PdProductStockIn b ,WhWarehouse c ,PdProduct d where a.ProductStockInSysNo = b.SysNo and a.WarehouseSysNo = c.SysNo and a.PdProductSysNo = d.SysNo and a.SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<PdProductStockInDetailList>();
        }

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Insert(PdProductStockInDetail entity)
        {
            entity.SysNo = Context.Insert("PdProductStockInDetail", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Update(PdProductStockInDetail entity)
        {

            return Context.Update("PdProductStockInDetail", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ProductStockInSysNo">库存主表编号</param>
        /// <returns></returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override void DeleteByProductStockInSysNo(int ProductStockInSysNo)
        {
            Context.Sql("Delete from PdProductStockInDetail where ProductStockInSysNo=@ProductStockInSysNo")
                 .Parameter("ProductStockInSysNo", ProductStockInSysNo)
            .Execute();
        }
        /// <summary>
        /// 查询当前仓库未入库商品
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <returns></returns>
        public override List<PdProduct> GetNotStockInPd(int WarehouseSysNo)
        {
            return Context.Sql(@"
                                select * from PdProduct where sysno not in(
			                    select PdProductSysNo from PdProduct c 
			                    left join PdProductStock a  on c.SysNo=a.PdProductSysNo
			                    left join WhWarehouse b on a.WarehouseSysNo = b.SysNo
			                    where a.WarehouseSysNo =@WarehouseSysNo) and Status<>2")
                .Parameter("WarehouseSysNo", WarehouseSysNo)
                .QueryMany<PdProduct>();
        }

        /// <summary>
        /// 获得入库明细
        /// </summary>
        /// <param name="ProductStockInSysNo"></param>
        /// <returns></returns>
        /// /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override List<PdProductStockInDetailList> GetProductStockInDetailBy(int ProductStockInSysNo)
        {
            return Context.Sql(@"
                                select a.*,b.ErpCode,b.EasName
                                from  PdProductStockInDetail a ,PdProduct b
                                where a.PdProductSysNo = b.SysNo and a.ProductStockInSysNo=@ProductStockInSysNo")
                .Parameter("ProductStockInSysNo", ProductStockInSysNo)
                .QueryMany<PdProductStockInDetailList>();
        }

        /// <summary>
        /// 获得入库明细-审核
        /// </summary>
        /// <param name="ProductStockInSysNo"></param>
        /// <returns></returns>
        /// /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override List<PdProductStockInDetailList> GetAduitProductStockInDetailBy(int ProductStockInSysNo)
        {
            return Context.Sql(@"
                                select a.*,b.ErpCode,b.EasName
                                from  PdProductStockInDetail a ,PdProduct b
                                where a.StorageQuantity <> a.DoStorageQuantity and a.PdProductSysNo = b.SysNo and a.ProductStockInSysNo=@ProductStockInSysNo")
                .Parameter("ProductStockInSysNo", ProductStockInSysNo)
                .QueryMany<PdProductStockInDetailList>();
        }
        public override List<PdProductStockInDetail> GetProductStockInDetail(int ProductStockInSysNo)
        {
            return Context.Sql(@"SELECT * FROM PdProductStockInDetail WHERE StorageQuantity <> DoStorageQuantity and ProductStockInSysNo=@0", ProductStockInSysNo).QueryMany<PdProductStockInDetail>();
        }
        #endregion

        /// <summary>
        /// 获得推送入库单需要的参数信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public override IList<SendSoOrderModel> GetSendSoOrderModelByStockInSysNo(int ProductStockInSysNo)
        {
            var items =
                Context.Select<SendSoOrderModel>("'' as OverseaCarrier,'' as OverseaTrackingNo,'1' as WarehouseId,'' as CustomerReference,'' as MerchantName,'' as MerchantOrderNo,'' as ConsigneeFirstName," +
                                                 "'' as ConsigneeLastName,'' as Remark,b.ErpCode as SKU,isnull(b.BarCode,'') as UPC,b.ProductName as CommodityName,'' as Category,'' as Brand,'' as Color," +
                                                 "'' as Size,'' as Material,'' as CommoditySourceURL,'' as CommodityImageUrlList, isnull(dbo.[func_GetProductPrice](b.SysNo),0) as UnitPrice," +
                                                 "isnull(dbo.[func_GetProductPrice](b.SysNo),0) as DeclaredValue,b.ValueUnit as ValueUnit, b.NetWeight as [Weight], b.SalesMeasurementUnit as WeightUnit," +
                                                 "isnull(b.Volume,'') as Volume,isnull(b.VolumeUnit,'') as VolumeUnit,a.DoStorageQuantity as Quantity,b.SysNo as CustomerReferenceSub")
                       .From("PdProductStockInDetail a left join PdProduct b on a.PdProductSysNo = b.SysNo ")
                       .Where("a.ProductStockInSysNo=@ProductStockInSysNo")
                       .Parameter("ProductStockInSysNo", ProductStockInSysNo)
                       .QueryMany();
            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysNoList"></param>
        /// <returns></returns>
        public override List<PdProductStock> GetSelectedPdProductStock(List<int> sysNoList)
        {
            return Context.Sql("SELECT * FROM PdProductStock WHERE SYSNO IN (" + sysNoList.ToString() + ")").QueryMany<PdProductStock>();
        }
    }
}
