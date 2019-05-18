using Hyt.DataAccess.InventorySheet;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.InventorySheet;
using Hyt.Model.Parameter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.InventorySheet
{
    public class WhInventoryDao : IWhInventoryDao
    {
        /// <summary>
        /// 分页获取盘点作业单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// 2017-8-07
        public override Pager<Hyt.Model.InventorySheet.WhInventory> GetSoOrders(Pager<Hyt.Model.InventorySheet.WhInventory> pager)
        {
            var strSql = @" WhInventory ";
            var whereStr = "( @0 is null or status=@0) and (@1 is null or Code=@1) ";
            var paras = new object[]
                {
                    pager.PageFilter.Status==-1?null:pager.PageFilter.Status,
                    string.IsNullOrEmpty(pager.PageFilter.Code)?null:pager.PageFilter.Code,
                };
            pager.Rows = Context.Select<Hyt.Model.InventorySheet.WhInventory>("*").From(strSql).Where(whereStr).Parameters(paras).OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = Context.Select<int>("count(0)").From(strSql).Where(whereStr).Parameters(paras).QuerySingle();
            return pager;
        }


        /// <summary>
        /// 创建盘点作业
        /// </summary>
        /// <param name="model">盘点实体</param>
        /// <param name="productModel">盘点商品实体</param>
        /// <returns></returns>
        public override int AddWhInventory(Hyt.Model.InventorySheet.WhInventory model,List<WhInventoryProduct> productModel)
        { 
            int sysNo = 0;
            using (var context = Context.UseTransaction(true))
            {
                try
                {
                    sysNo = Context.Insert("WhInventory", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
                    if (sysNo > 0)
                    {
                        foreach (var item in productModel)
                        {
                            item.InventorySysNo = sysNo;
                            Context.Insert("WhInventoryProduct", item).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
                        }
                    }
                    context.Commit();
                }
                catch (Exception)
                {
                    //回滚
                    sysNo = 0;
                    context.Rollback();
                }
            }
            return sysNo;
        }


        /// <summary>
        /// 查询当天的盘点单总数
        /// </summary>
        /// <returns></returns>
        public override int GetWhInventoryCount()
        {
            var strSql = @" WhInventory ";
            var whereStr = " DateDiff(dd,AddTime,getdate())=0 ";
            return Context.Select<int>("count(0)").From(strSql).Where(whereStr).QuerySingle();
        }

        /// <summary>
        /// 根据盘点单系统编号获取明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override Pager<Hyt.Model.InventorySheet.WhInventoryDetail> GetWhInventoryDetail(int PageIndex, int sysNo)
        {
            Pager<Hyt.Model.InventorySheet.WhInventoryDetail> pager = new Pager<Model.InventorySheet.WhInventoryDetail>();
            var strSql = @" WhInventory ";
            var whereStr = "SysNo=@0 ";
            var paras = new object[] { sysNo };
            pager.Rows = Context.Select<Hyt.Model.InventorySheet.WhInventoryDetail>("*").From(strSql).Where(whereStr).Parameters(paras).OrderBy("SysNo desc").Paging(PageIndex, 15).QueryMany();
            pager.TotalRows = Context.Select<int>("count(0)").From(strSql).Where(whereStr).Parameters(paras).QuerySingle();
            return pager;
        }


        /// <summary>
        /// 根据盘点单系统编号获取明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override Hyt.Model.InventorySheet.WhInventoryDetail GetWhInventoryDetail(int sysNo)
        {
            Hyt.Model.InventorySheet.WhInventoryDetail model = new Model.InventorySheet.WhInventoryDetail();
            model = Context.Select<Hyt.Model.InventorySheet.WhInventoryDetail>("*").From("WhInventory").Where(" SysNo=@SysNo ").Parameter("SysNo", sysNo).QuerySingle();
            #region 获取商品明细
            model.dataList = Context.Select<Hyt.Model.InventorySheet.WhInventoryProductDetail>("w.*,wh.WarehouseName as WarehouseNameDate,p.ErpCode,p.ProductName,pb.Name as BrandName,p.Barcode,p.EasName,p.ProductType").From("WhWarehouse as wh, WhInventoryProduct as w ,PdProduct as p,PdBrand as pb").Where(" w.InventorySysNo=@InventorySysNo and wh.sysNo=w.WarehouseSysNo and w.ProductsysNo=p.SysNo  and pb.sysNo=p.BrandSysNo ").Parameter("InventorySysNo", sysNo).QueryMany();
            #endregion
            return model;

         
        }

        /// <summary>
        /// 根据仓库id查询仓库名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override string GetWhWarehouseName(int sysNo)
        {
            var model = Context.Select<WhWarehouse>("WarehouseName").From("WhWarehouse").Where("SysNo=@SysNo").Parameter("SysNo", sysNo).QuerySingle();
            if (model != null)
            {
                return model.WarehouseName;
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 根据商品编号查询商品名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override PdProduct GetProductName(int sysNo)
        {
            return Context.Select<PdProduct>("*").From("PdProduct").Where("SysNo=@SysNo").Parameter("SysNo", sysNo).QuerySingle();
        }

        /// <summary>
        /// 根据品牌编号查询品牌名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override string GetBrandName(int sysNo)
        {
            var model = Context.Select<PdBrand>("*").From("PdBrand").Where("SysNo=@SysNo").Parameter("SysNo", sysNo).QuerySingle();
            if (model != null)
            {
                return model.Name;
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 更新盘点库存
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override bool UploadPDQuantity(int sysNo, decimal Quantity, decimal ZhangCunQuantity)
        {
            return Context.Update("WhInventoryProduct").Column("InventoryQuantity", Quantity).Column("adjustmenQuantity",(Quantity - ZhangCunQuantity)).Where("SysNo", sysNo).Execute() > 0;
        }

        /// <summary>
        /// 更新调整数量/实际库存
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override bool UploadSJQuantity(int sysNo, decimal Quantity)
        {
            return Context.Update("WhInventoryProduct").Column("adjustmenQuantity", Quantity).Where("SysNo", sysNo).Execute() > 0;
        }


        /// <summary>
        /// 更新盘点单状态
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override bool UploadStatus(int sysNo, int status)
        {
            return Context.Update("WhInventory").Column("Status", status).Where("SysNo", sysNo).Execute() > 0;
        }


        /// <summary>
        /// 生成盘点报告单
        /// </summary>
        /// <returns></returns>
        public override bool AddWhInventoryRepor(WhInventoryRepor reporModl, List<WhIReporPrDetails> productModel)
        {
            int sysNo = 0;
            using (var context = Context.UseTransaction(true))
            {
                try
                {
                    sysNo = Context.Insert("WhInventoryRepor", reporModl).AutoMap(x => x.SysNo,x=>x.DataList,x=>x.PanKuiStatus,x=>x.PanYingStatus).ExecuteReturnLastId<int>("SysNo");
                    if (sysNo > 0)
                    {
                        foreach (var item in productModel)
                        {
                            item.WhInventoryReporSysNo = sysNo;
                            Context.Insert("WhIReporPrDetails", item).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
                        }
                    }
                    context.Commit();
                }
                catch (Exception e)
                {
                    var a = e.Message;
                    //回滚
                    sysNo = 0;
                    context.Rollback();
                }
            }
            return sysNo>0;
        }



        /// <summary>
        /// 分页获取盘点报告单
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// 2017-8-07
        public override Pager<WhInventoryRepor> GetWhInventoryReporPage(Pager<WhInventoryRepor> pager)
        {
            var strSql = @" WhInventoryRepor ";
            var whereStr = "( @0 is null or status=@0) ";
            var paras = new object[]
                {
                    pager.PageFilter.Status==-1?null:pager.PageFilter.Status,
                };
            pager.Rows = Context.Select<WhInventoryRepor>("*").From(strSql).Where(whereStr).Parameters(paras).OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = Context.Select<int>("count(0)").From(strSql).Where(whereStr).Parameters(paras).QuerySingle();
            return pager;
        }


        /// <summary>
        /// 根据id获取盘点报告单
        /// </summary>
        /// <returns></returns>
        public override WhInventoryRepor GetWhInventoryRepor(int sysNo)
        {
            var model = new WhInventoryRepor();
            model = Context.Select<WhInventoryRepor>("*").From("WhInventoryRepor").Where("SysNo=@SysNo").Parameter("SysNo", sysNo).QuerySingle();
            return model;
        }


        /// <summary>
        /// 根据id获取盘点报告单明细
        /// </summary>
        /// <returns></returns>
        public override WhInventoryRepor GetWhInventoryReporModel(int sysNo,int PageType)
        {
            var model = new WhInventoryRepor();
            model= Context.Select<WhInventoryRepor>("*").From("WhInventoryRepor").Where("SysNo=@SysNo").Parameter("SysNo", sysNo).QuerySingle();
            if (model == null)
            {
                return null;
            }
            else
            {
                if (PageType == 1)//盘盈
                {
                    model.DataList = Context.Select<WhIReporPrDetails>("*").From("WhIReporPrDetails")
                        .Where("WhInventoryReporSysNo=@WhInventoryReporSysNo and RealityQuantity>ADQuantity ")
                        .Parameter("WhInventoryReporSysNo", sysNo)
                         .QueryMany();
                }
                else if (PageType == 2) //盘亏
                {
                    model.DataList = Context.Select<WhIReporPrDetails>("*").From("WhIReporPrDetails")
                       .Where("WhInventoryReporSysNo=@WhInventoryReporSysNo and RealityQuantity<ADQuantity")
                       .Parameter("WhInventoryReporSysNo", sysNo)
                       .QueryMany();
                }
                else
                {
                    model.DataList = Context.Select<WhIReporPrDetails>("*").From("WhIReporPrDetails")
                       .Where("WhInventoryReporSysNo=@WhInventoryReporSysNo")
                       .Parameter("WhInventoryReporSysNo", sysNo)
                       .QueryMany();
                }
                return model;            
            }
        }

        /// <summary>
        /// 根据id获取是否已生成了盈亏报告单
        /// </summary>
        /// <param name="sysNo">盘点单id</param>
        /// <param name="status">盈亏状态  1盈 2亏</param>
        /// <returns></returns>
        public override bool GetIsWhInventoryRepor(string Code, int status)
        {
            return Context.Select<int>("count(0)").From("WhInventoryRepor").Where("WhInventoryCode=@WhInventoryCode and YingKuiStatus=@YingKuiStatus").Parameter("WhInventoryCode", Code)
                .Parameter("YingKuiStatus", status)
                .QuerySingle()>0;
        }


        /// <summary>
        /// 更新盘点报告单状态
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override bool UploadWhInventoryReporStatus(int sysNo, int status)
        {
            int rtsysNo = 0;
            if (status == (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhInventoryReporStatus.完成)
            {
                try
                {
                    string sqlstr = @"update WhInventory set status=@status
                                        where  code =(select WhInventoryCode from WhInventoryRepor where sysno=@sysNo)";
                    rtsysNo = Context.Sql(sqlstr)
                        .Parameter("status", (int)Hyt.Model.InventorySheet.WhlnventoryEnum.WhlnventoryStatus.完成)
                        .Parameter("sysNo", sysNo)
                        .Execute();

                   rtsysNo = Context.Update("WhInventoryRepor").Column("Status", status).Where("SysNo", sysNo).Execute();
                   Context.Commit();
                }
                catch (Exception e)
                {
                    //回滚
                    rtsysNo = 0;
                    Context.Rollback();
                }
            }
            else
            { 
               rtsysNo= Context.Update("WhInventoryRepor").Column("Status", status).Where("SysNo", sysNo).Execute();
            }
            return rtsysNo > 0;
        }


        /// <summary>
        /// 更新盘点报告单
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override bool UploadWhInventoryRepor(WhInventoryRepor model)
        {
            return Context.Update("WhInventoryRepor")
                .Column("Status", model.Status)
                .Column("Tally", model.Tally)
                .Column("CustodySysNo", model.CustodySysNo)
                .Column("HeadSysNo", model.HeadSysNo)
                .Column("AgentSysNo", model.AgentSysNo)
                .Column("AuditTime", model.AuditTime)
                .Column("Remarks", model.Remarks)
                .Where("SysNo", model.SysNo)
                .Execute() > 0;
        }




        /// <summary>
        /// 根据盘点报告单系统编号 查询盘点商品报告单列表
        /// </summary>
        /// <param name="sysNo">盘点单id</param>
        /// <returns></returns>
        public override List<WhIReporPrDetails> GetWhIReporPrDetailsPid(int sysNo,int status)
        {
            if (status == 1)
            {//盘盈
                return Context.Select<WhIReporPrDetails>("*").From("WhIReporPrDetails")
                          .Where("WhInventoryReporSysNo=@WhInventoryReporSysNo and ProfitAndLoss>0  ")
                          .Parameter("WhInventoryReporSysNo", sysNo)
                          .QueryMany();
            }
            else
            {//盘亏
                return Context.Select<WhIReporPrDetails>("*").From("WhIReporPrDetails")
                            .Where("WhInventoryReporSysNo=@WhInventoryReporSysNo  and ProfitAndLoss<0 ")
                            .Parameter("WhInventoryReporSysNo", sysNo)
                            .QueryMany();
            }
        }


        /// <summary>
        /// 更新产品库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool UpdatePdProductStock(List<WhIReporPrDetails> model)
        {
            int sysNo = 0;
            using (var context = Context.UseTransaction(true))
            {
                try
                {
                    foreach (var item in model)
                    {
                        string sqlstr = @"update PdProductStock set StockQuantity=@StockQuantity
                                        where sysno =(select sysNo from PdProductStock   
                                        where pdproductSysno=(SELECT  [SysNo]  FROM [xingying].[dbo].[PdProduct] 
                                        where erpCode=@erpCode and status!=2)  and WarehouseSysNo=@WarehouseSysNo )";
                        sysNo = Context.Sql(sqlstr)
                            .Parameter("StockQuantity", item.RealityQuantity)
                            .Parameter("erpCode", item.ProduceCode)
                            .Parameter("WarehouseSysNo", item.WarehouseSysNo)
                            .Execute();
                     }
                    context.Commit();
                }
                catch (Exception e)
                {
                    //回滚
                    sysNo = 0;
                    context.Rollback();
                }
            }
            return sysNo > 0;
        }


        /// <summary>
        /// 根据商品id获取商品和商品对应仓库信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override List<uditPdProductStock> GetuditPdProductStock(int SysNo, int? whSysId)
        {
            string see = @"pr.sysno as PrSysNo,pr.ErpCode as PrErpCode,pr.EasName as PrEasName,pr.SalesMeasurementUnit as PrSalesMeasurementUnit,  
     w.ErpCode as WhErpCode, w.BackWarehouseName as WhBackWarehouseName,w.sysNo as WhSysId, p.StockQuantity as WhStockQuantity, p.CostPrice as WhCostPrice";

            string form = @"WhWarehouse as w,PdProductStock as p, pdproduct as pr";

            string wherestr = @"w.sysNo=p.WarehouseSysNo
	 and  p.pdproductsysno=@pdproductsysno  and p.pdproductSysno=pr.sysno  and w.[Status]=1 and (@whSysId is null or w.sysNo=@whSysId) ";

            return Context.Select<uditPdProductStock>(see).From(form)
                            .Where(wherestr)
                            .Parameter("pdproductsysno",SysNo)
                            .Parameter("whSysId", whSysId)
                            .QueryMany();
        }

        /// <summary>
        /// 根据商品编码和仓库编码获取商品和商品对应仓库信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override uditPdProductStock GetuditPdProductStockTo(string SysNo, string whSysId)
        {
            string see = @"pr.sysno as PrSysNo,pr.ErpCode as PrErpCode,pr.EasName as PrEasName,pr.SalesMeasurementUnit as PrSalesMeasurementUnit,  
     w.ErpCode as WhErpCode, w.BackWarehouseName as WhBackWarehouseName,w.sysNo as WhSysId, p.StockQuantity as WhStockQuantity, p.CostPrice as WhCostPrice";

            string form = @"WhWarehouse as w,PdProductStock as p, pdproduct as pr";

            string wherestr = @"w.sysNo=p.WarehouseSysNo
	 and  p.pdproductsysno=(select sysno from pdproduct where ErpCode=@pdproductsysno)  and p.pdproductSysno=pr.sysno  and w.[Status]=1 and  w.sysNo=(select sysno from WhWarehouse where ErpCode=@whSysId) ";
            return Context.Select<uditPdProductStock>(see).From(form)
                            .Where(wherestr)
                            .Parameter("pdproductsysno", SysNo)
                            .Parameter("whSysId", whSysId)
                            .QuerySingle();
        }

        /// <summary>
        /// 分页获取其他出入库
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="dataType">1查询全部 2查询其他出库 3查询其他入库</param>
        /// <returns></returns>
        /// 2017-8-07
        public override Pager<OtherOutOfStorage> GetOtherOutOfStoragePage(Pager<OtherOutOfStorage> pager,int? dataTyp=1)
        {
            var strSql = @" OtherOutOfStorage ";
            var whereStr = "( @0 is null or Code=@0) and (@1 is null or Type=@1) and (@2 is null or status=@2 )";
            var paras = new object[]
                {
                   string.IsNullOrEmpty(pager.PageFilter.Code)?null:pager.PageFilter.Code.Trim(),
                   dataTyp,
                   pager.PageFilter.Status==-1?null:pager.PageFilter.Status
                };
            pager.Rows = Context.Select<OtherOutOfStorage>("*").From(strSql).Where(whereStr).Parameters(paras).OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            pager.TotalRows = Context.Select<int>("count(0)").From(strSql).Where(whereStr).Parameters(paras).QuerySingle();
            return pager;
        }

        #region 创建其他入库
        /// <summary>
        /// 添加其他入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int AddOtherOutOfStorage(OtherOutOfStorage model)
        {
            int sysNo = 0;
            using (var context = Context.UseTransaction(true))
            {
                try
                {
                    sysNo = Context.Insert("OtherOutOfStorage", model).AutoMap(x => x.SysNo,x=>x.ListData).ExecuteReturnLastId<int>("SysNo");
                    if (sysNo > 0)
                    {
                        foreach (var item in model.ListData)
                        {
                            item.OtherOutOfStorageCode = sysNo;
                            Context.Insert("OtherOutOfStorageDetailed", item).AutoMap(x => x.SysNo, x => x.CollectWarehouseCode, x => x.ZhangCount).ExecuteReturnLastId<int>("SysNo");
                        }
                    }
                    context.Commit();
                }
                catch (Exception e)
                {
                    if (sysNo > 0)
                    {
                        Context.Delete("OtherOutOfStorage").Where("SysNo", sysNo).Execute();
                    }
                    //回滚
                    sysNo = 0;
                    context.Rollback();
                }
            }
            return sysNo;
        }
        #endregion

        #region 根据id获取其他出入库明细
        /// <summary>
        /// 根据id获取其他出入库明细
        /// </summary>
        /// <returns></returns>
        public override OtherOutOfStorage GetOtherOutOfStorageModel(int sysNo)
        {
            var model = new OtherOutOfStorage();
            model = Context.Select<OtherOutOfStorage>("*").From("OtherOutOfStorage").Where("SysNo=@SysNo").Parameter("SysNo", sysNo).QuerySingle();
            if (model == null)
            {
                return null;
            }
            else
            {

                model.ListData = Context.Select<OtherOutOfStorageDetailed>("SysNo,ProductSysNo,ProductCode,BarCode,ProductName,Count,UnitPrice,Price,Remarks,CollectWarehouseSysNo,CollectWarehouseName").From("OtherOutOfStorageDetailed")
                       .Where("OtherOutOfStorageCode=@OtherOutOfStorageCode")
                       .Parameter("OtherOutOfStorageCode", sysNo)
                       .QueryMany();
                return model;
            }
        }
        #endregion


        /// <summary>
        /// 其他出入库更新库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool UpdateOtherOutPdProductStock(OtherOutOfStorage model)
        {
            int sysNo = 0;
            using (var context = Context.UseTransaction(true))
            {
                try
                {
                    string sqlstr = @"update OtherOutOfStorage set Status=@Status,ToexamineName=@ToexamineName,ToexamineSysNo=@ToexamineSysNo where sysNo=@sysNo ";
                    sysNo = Context.Sql(sqlstr).Parameter("Status", model.Status)
                        .Parameter("sysNo",model.SysNo)
                        .Parameter("ToexamineName", model.ToexamineName)
                        .Parameter("ToexamineSysNo", model.ToexamineSysNo)
                        .Execute();
                    string set = "StockQuantity-=@StockQuantity";
                    if (model.Type == (int)OtherOutOfStorageTypeEnum.其他入库)
                    {
                         set="StockQuantity+=@StockQuantity";
                    }
                    foreach (var item in model.ListData)
                    {
                        sqlstr = @"update PdProductStock set "+set+" where sysno =(select sysNo from PdProductStock where pdproductSysno=@pdproductSysno  and WarehouseSysNo=@WarehouseSysNo )";
                        sysNo = Context.Sql(sqlstr)
                            .Parameter("StockQuantity", item.Count)
                            .Parameter("pdproductSysno", item.ProductSysNo)
                            .Parameter("WarehouseSysNo", item.CollectWarehouseSysNo)
                            .Execute();
                    }
                    context.Commit();
                }
                catch (Exception e)
                {
                    //回滚
                    sysNo = 0;
                    context.Rollback();
                }
            }
            return sysNo > 0;
        }

    }
}
