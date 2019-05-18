using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.LogisApp;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Model.Parameter;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Distribution;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 分销商产品特殊价格
    /// </summary>
    /// <remarks>
    /// 2013-09-04 周瑜 创建
    /// </remarks>
    public class DsSpecialPriceImpl : IDsSpecialPriceDao
    {
        /// <summary>
        /// 快速搜索
        /// </summary>
        /// <param name="condition">搜索条件实体</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>符合搜索条件的实体集合</returns>
        /// <remarks>2013-08-12 周瑜 创建</remarks>
        public override Pager<CBDsSpecialPrice> QuickSearch(DsSpecialPriceSearchCondition condition, int pageIndex, int pageSize)
        {
            //2015-12-19 王耀发 创建
            var DealerSysNoList = string.Empty;
            if (null != condition.DealerSysNoList)
            {
                DealerSysNoList = string.Join(",", condition.DealerSysNoList);
            } 
            var pager = new Pager<CBDsSpecialPrice>();
            using (var context = Context.UseSharedConnection(true))
            {
                const string @where = @"
                                (@0 is null or (a.ProductSysNo = @0 or a.DealerSysNo = @0))
                                and (@1 is null or (charindex(b.DealerName,@1)> 0 or charindex(c.ProductName,@1)> 0))
                                and (@2 is null or a.Status = @2)
                                and (a.dealersysno = @3 or a.dealersysno in (select col from splitstr(@4, ',')))
                                ";

                var parms = new object[]
                    {
                        condition.SysNo,
                        condition.Name,
                        condition.Status,
                        condition.DealerSysNo,
                        DealerSysNoList
                    };

                pager.TotalRows = context.Sql(@"select count(1) from DsSpecialPrice a
                    left join dsdealer b on a.dealersysno = b.sysno
                    left join pdproduct c on a.productsysno = c.sysno where " + where)
                                         .Parameters(parms)
                                         .QuerySingle<int>();

                pager.Rows = context.Select<CBDsSpecialPrice>("a.*,b.DealerName,c.ProductName,c.ErpCode")
                                                      .From(@"DsSpecialPrice a
                                                left join dsdealer b on a.dealersysno = b.sysno
                                                left join pdproduct c on a.productsysno = c.sysno")
                                                      .AndWhere(where)
                                                      .Parameters(parms)
                                                      .OrderBy("a.SysNo desc")
                                                      .Paging(pageIndex, pageSize)
                                                      .QueryMany();

            }
            return pager;
        }

        /// <summary>
        /// 检查数据表是否存在相应的字段，如果存在则不增加，不存在则增加
        /// </summary>
        /// <param name="keyList"></param>
        /// <returns></returns>
        /// <remarks>2016-11-14 杨云奕 添加</remarks>
        public void CheckKeyExcel(List<DBKey> keyList)
        {
            string checkWhere = "";
            foreach (DBKey key in keyList) 
            { 
                if(!string.IsNullOrEmpty(checkWhere))
                {
                    checkWhere += " or ";
                }
                checkWhere += "  name='" + key.KeyName + "' ";
            }
            string sql = "select   *   from   syscolumns   where   id=object_id('DsSpecialPrice')   and (  " + checkWhere + " )";
            List<SelectKeyData> selectList = Context.Sql(sql).QueryMany<SelectKeyData>();

            foreach (DBKey key in keyList)
            {
                SelectKeyData tempKeyData = selectList.Find(p => p.name == key.KeyName);
                if(tempKeyData==null)
                {
                    sql = " alter table DsSpecialPrice add " + key.KeyName + " " + key.Type + " null ";
                    Context.Sql(sql).Execute();
                }
                
            }


        }

        /// <summary>
        /// 创建分销商产品特殊价格
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public override int Create(DsSpecialPrice model)
        {
            List<DBKey> keyList=new List<DBKey>();
            keyList.Add(new DBKey() { KeyName = "WholesalePrice", Type = "decimal(18, 2)" });
            CheckKeyExcel(keyList);

            int sysno= Context.Insert("DsSpecialPrice", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
            ///更新库存信息，修改商品档案的更新时间

            string sql = " update PdProduct set LastUpdateDate='" + DateTime.Now.ToString() + "' where SysNo='" + model.ProductSysNo + "' ";
            Context.Sql(sql).Execute();
            return sysno;
        }

        /// <summary>
        /// 更新分销商产品特殊价格
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public override int Update(DsSpecialPrice model)
        {
            List<DBKey> keyList = new List<DBKey>();
            keyList.Add(new DBKey() { KeyName = "WholesalePrice", Type = "decimal(18, 2)" });
            CheckKeyExcel(keyList);

            return Context.Update("DsSpecialPrice", model).AutoMap(x => x.SysNo).Where(x => x.SysNo).Execute();
        }

        /// <summary>
        /// 修改特殊价格状态: 禁用/启用
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        public override int UpdateStatus(DsSpecialPrice model)
        {
            int sysno= Context.Sql("update DsSpecialPrice set Status = @Status, LastUpdateBy = @LastUpdateBy, LastUpdateDate = @LastUpdateDate where SysNo = @SysNo")
              .Parameter("Status", model.Status)
              .Parameter("LastUpdateBy", model.LastUpdateBy)
              .Parameter("LastUpdateDate", model.LastUpdateDate)
              .Parameter("SysNo", model.SysNo)
              .Execute();
            string sql = " update PdProduct set LastUpdateDate='" + DateTime.Now.ToString() + "' where SysNo='" + model.ProductSysNo + "' ";
            Context.Sql(sql).Execute();
            return sysno;
        }

        /// <summary>
        /// 修改价格
        /// </summary>
        /// <param name="model">分销商产品特殊价格实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-09-06 周瑜 创建</remarks>
        public override int UpdatePrice(DsSpecialPrice model)
        {
            return Context.Sql("update DsSpecialPrice set Price = @Price, LastUpdateBy = @LastUpdateBy, LastUpdateDate = @LastUpdateDate where SysNo = @SysNo")
              .Parameter("Price", model.Price)
              .Parameter("LastUpdateBy", model.LastUpdateBy)
              .Parameter("LastUpdateDate", model.LastUpdateDate)
              .Parameter("SysNo", model.SysNo)
              .Execute();
        }

        /// <summary>
        /// 获取特殊价格信息
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>特殊价格信息</returns>
        /// <remarks>2013-09-10 朱成果 创建</remarks>
        public override DsSpecialPrice GetEntity(int dealerSysNo, int productSysNo)
        {
            return Context.Sql("select * from DsSpecialPrice where DealerSysNo=@DealerSysNo and ProductSysNo=@ProductSysNo and Status=@Status")
               .Parameter("DealerSysNo", dealerSysNo)
                .Parameter("ProductSysNo", productSysNo)
                .Parameter("Status", (int)Hyt.Model.WorkflowStatus.DistributionStatus.分销商特殊价格状态.启用)
          .QuerySingle<DsSpecialPrice>();

        }

        /// <summary>
        /// 删除特殊价格信息
        /// </summary>
        /// <param name="sysNo">特殊价格编号</param>
        /// <returns>删除特殊价格信息</returns>
        /// <remarks>2015-12-16 王耀发 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("DsSpecialPrice")
                               .Where("SysNo", sysNo)
                               .Execute();
        }

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>
        /// 2015-12-10 王耀发 创建
        /// 2016-6-27 杨浩 增加产品价格只查有效的（PdPrice + Status=1）
        /// </remarks>
        public override void GetSpecialPriceProductList(ref Pager<CBPdProductDetail> pager, ParaProductFilter condition)
        {
            //List<DBKey> keyList = new List<DBKey>();
            //keyList.Add(new DBKey() { KeyName = "WholesalePrice", Type = "decimal(18, 2)" });
            //CheckKeyExcel(keyList);

            using (var _context = Context.UseSharedConnection(true))
            {
                //只能看到总部和分销商对应自己创建的商品
                //var sqlWhere = " 1=1 and p.Status <> 2 and (p.DealerSysNo = 0 or p.DealerSysNo = " + condition.SelectedDealerSysNo + ")";

                var sqlWhere = " 1=1 and p.Status <> 2 ";
                if (condition.IsFrontDisplay >= 0)
                    sqlWhere += " and p.IsFrontDisplay=@IsFrontDisplay";

                if (condition.MainStatus >= 0)
                    sqlWhere += " and p.Status=@MainStatus";

                if (condition.Status >= 0)
                    sqlWhere += " and isnull(sp.Status,0)=@Status";

                if (condition.SysNo > 0)
                    sqlWhere += " and p.Sysno=@Sysno";


                if (!string.IsNullOrEmpty(condition.ErpCode) && condition.ErpCode != "")
                    sqlWhere += " and p.ErpCode=@ErpCode";

                if (!string.IsNullOrEmpty(condition.Barcode) && condition.Barcode != "")
                    sqlWhere += " and p.Barcode=@Barcode";

                if (condition.ProductType > 0)
                    sqlWhere += " and p.ProductType=@ProductType";

                if (!string.IsNullOrEmpty(condition.ProductName) && condition.ProductName != "")
                    sqlWhere += " and (p.EasName like @name1 or  p.ErpCode = @name or p.Barcode=@name)";

                if (condition.StartTime != null)
                    sqlWhere += " and p.LastUpdateDate >= @StartTime";

                if (condition.EndTime != null)
                    sqlWhere += " and p.LastUpdateDate <= @EndTime";

                if (condition.CreateStartTime != null)
                    sqlWhere += " and p.CreatedDate >= @CreateStartTime";

                if (condition.CreateEndTime != null)
                    sqlWhere += " and  p.CreatedDate <= @CreateEndTime";

                if (condition.ProductCategorySysno > 0)
                    sqlWhere += " and SysNos like '%," + condition.ProductCategorySysno + ",%' ";

                if (condition.HasChangePrice.HasValue && condition.HasChangePrice == 1)
                    sqlWhere += " and case d.IsWholeSaler when 1 then p.TradePrice else price1.Price end<>sp.Price ";

                if (condition.HasChangePrice.HasValue && condition.HasChangePrice == 2)
                    sqlWhere += " and case d.IsWholeSaler when 1 then p.TradePrice else price1.Price end=sp.Price ";

                //获取当前分销商的订单和当前用户对应所有分销商订单
                string sqlWhere2 = "1=1";
                //判断是否绑定所有分销商
                if (!condition.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (condition.IsBindDealer)
                    {
                        sqlWhere2 += " and sp.DealerSysNo = @DealerSysNo";
                    }
                    else
                    {
                        sqlWhere2 += " and sp.DealerSysNo in (select SysNo from  DsDealer where CreatedBy = @DealerCreatedBy) ";
                    }
                }
                if (condition.SelectedDealerSysNo != -1)
                {
                    sqlWhere2 += " and sp.DealerSysNo = @SelectedDealerSysNo";
                }

                pager.Rows = _context.Select<CBPdProductDetail>(@"p.barcode ,p.ProductName,p.SalesMeasurementUnit, sp.SysNo as SpecialPriceSysNo,p.SysNo,p.ErpCode,p.EasName,p.ProductType,p.CanFrontEndOrder,p.Status as MainStatus,isnull(sp.Status,0) as Status, c.categoryname as ProductCategoryName, c.sysno as ProductCategorySysno, 
                             price.Price as BasicPrice, case d.IsWholeSaler when 1 then p.TradePrice else price1.Price end as SalesPrice,sp.Price as spSalesPrice,sp.DealerSysNo,d.DealerName,sp.ShopPrice as spShopPrice,sp.WholesalePrice as spWholesalePrice")
                              .From(@"PdProduct p 
                                    left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=@IsMaster) ca on ca.productsysno = p.sysno 
                                    left join  PdCategory c on ca.categorysysno = c.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource and SourceSysNo =@SourceSysNo and Status=1) price  on price.productsysno = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource1 and SourceSysNo =@SourceSysNo1 and Status=1) price1  on price1.productsysno = p.sysno
                                    left join DsSpecialPrice sp on p.SysNo = sp.ProductSysNo and " + sqlWhere2 + " left join DsDealer d on sp.DealerSysNo = d.SysNo")
                                     .Where(sqlWhere)

                              .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                              .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                              .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                              .Parameter("SourceSysNo", 0)  //基础价
                              .Parameter("SourceSysNo1", 1) //会员价

                              .Parameter("DealerSysNo", condition.DealerSysNo)
                              .Parameter("DealerCreatedBy", condition.DealerCreatedBy)
                              .Parameter("SelectedDealerSysNo", condition.SelectedDealerSysNo)

                              .Parameter("IsFrontDisplay", condition.IsFrontDisplay)

                              .Parameter("MainStatus", condition.MainStatus)

                              .Parameter("Status", condition.Status)

                              .Parameter("name", condition.ProductName)
                              .Parameter("name1", "%" + condition.ProductName + "%")

                              .Parameter("Sysno", condition.SysNo)

                              .Parameter("ErpCode", condition.ErpCode)

                              .Parameter("Barcode", condition.Barcode)

                              .Parameter("ProductType", condition.ProductType)

                              .Parameter("StartTime", condition.StartTime)
                              .Parameter("EndTime", (condition.EndTime == null) ? condition.EndTime : ((DateTime)condition.EndTime).AddDays(1))

                              .Parameter("CreateStartTime", condition.CreateStartTime)

                              .Parameter("CreateEndTime", (condition.CreateEndTime == null) ? condition.CreateEndTime : ((DateTime)condition.CreateEndTime).AddDays(1))

                              .Parameter("CanFrontEndOrder", condition.CanFrontEndOrder)

                              .OrderBy("sp.DealerSysNo desc , sp.sysno desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From(@"PdProduct p 
                                    left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=@IsMaster) ca on ca.productsysno = p.sysno 
                                    left join  PdCategory c on ca.categorysysno = c.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource and SourceSysNo =@SourceSysNo and Status=1) price  on price.productsysno = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource1 and SourceSysNo =@SourceSysNo1 and Status=1) price1  on price1.productsysno = p.sysno
                                    left join DsSpecialPrice sp on p.SysNo = sp.ProductSysNo and " + sqlWhere2 + " left join DsDealer d on sp.DealerSysNo = d.SysNo")
                                     .Where(sqlWhere)

                              .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                              .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                              .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                              .Parameter("SourceSysNo", 0)  //基础价
                              .Parameter("SourceSysNo1", 1) //会员价

                              .Parameter("DealerSysNo", condition.DealerSysNo)
                              .Parameter("DealerCreatedBy", condition.DealerCreatedBy)
                              .Parameter("SelectedDealerSysNo", condition.SelectedDealerSysNo)

                              .Parameter("IsFrontDisplay", condition.IsFrontDisplay)

                              .Parameter("MainStatus", condition.MainStatus)

                              .Parameter("Status", condition.Status)

                              .Parameter("name", condition.ProductName)
                              .Parameter("name1", "%" + condition.ProductName + "%")

                              .Parameter("Sysno", condition.SysNo)

                              .Parameter("ErpCode", condition.ErpCode)

                              .Parameter("Barcode", condition.Barcode)

                              .Parameter("ProductType", condition.ProductType)

                              .Parameter("StartTime", condition.StartTime)

                              .Parameter("EndTime", (condition.EndTime == null) ? condition.EndTime : ((DateTime)condition.EndTime).AddDays(1))

                              .Parameter("CreateStartTime", condition.CreateStartTime)

                              .Parameter("CreateEndTime", (condition.CreateEndTime == null) ? condition.CreateEndTime : ((DateTime)condition.CreateEndTime).AddDays(1))

                              .Parameter("CanFrontEndOrder", condition.CanFrontEndOrder)
                              .QuerySingle();

                pager.IdRows = _context.Select<int>(@"sp.SysNo as SpecialPriceSysNo")
                            .From(@"PdProduct p 
                                    left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=@IsMaster) ca on ca.productsysno = p.sysno 
                                    left join  PdCategory c on ca.categorysysno = c.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource and SourceSysNo =@SourceSysNo and Status=1) price  on price.productsysno = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource1 and SourceSysNo =@SourceSysNo1 and Status=1) price1  on price1.productsysno = p.sysno
                                    left join DsSpecialPrice sp on p.SysNo = sp.ProductSysNo and " + sqlWhere2 + " left join DsDealer d on sp.DealerSysNo = d.SysNo")
                                   .Where(sqlWhere)

                            .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                            .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                            .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                            .Parameter("SourceSysNo", 0)  //基础价
                            .Parameter("SourceSysNo1", 1) //会员价

                            .Parameter("DealerSysNo", condition.DealerSysNo)
                            .Parameter("DealerCreatedBy", condition.DealerCreatedBy)
                            .Parameter("SelectedDealerSysNo", condition.SelectedDealerSysNo)

                            .Parameter("IsFrontDisplay", condition.IsFrontDisplay)

                            .Parameter("MainStatus", 1)

                            .Parameter("Status", 1)

                            .Parameter("name", condition.ProductName)
                            .Parameter("name1", "%" + condition.ProductName + "%")

                            .Parameter("Sysno", condition.SysNo)

                            .Parameter("ErpCode", condition.ErpCode)

                            .Parameter("Barcode", condition.Barcode)

                            .Parameter("ProductType", condition.ProductType)

                            .Parameter("StartTime", condition.StartTime)

                            .Parameter("EndTime", (condition.EndTime == null) ? condition.EndTime : ((DateTime)condition.EndTime).AddDays(1))

                            .Parameter("CreateStartTime", condition.CreateStartTime)

                            .Parameter("CreateEndTime", (condition.CreateEndTime == null) ? condition.CreateEndTime : ((DateTime)condition.CreateEndTime).AddDays(1))

                            .Parameter("CanFrontEndOrder", condition.CanFrontEndOrder)
                            .QueryMany();
            }
        }
        /// <summary>
        /// 获取经销商升舱订单
        /// </summary>
        /// <returns>2017-8-21 罗熙 创建</returns>
        public override void GetDealerOrder(ref Pager<DsOrder> pager, ParaDsOrderFilter dsDetail)
        {  
            using (var _context = Context.UseSharedConnection(true))
            {
                var sqlWhere = " 1=1 ";
                if (dsDetail.status != 0)
                    sqlWhere += " and Status=@Status";
                if (dsDetail.MallOrderId != "")
                    sqlWhere += " and MallOrderId=@MallOrderId";
                //select dm.ShopName,dso.* from DsDealerMall dm,DsOrder dso where dm.SysNo=dso.DealerMallSysNo and dso.[Status] = 10
                pager.Rows = _context.Select<DsOrder>("*").From("DsOrder").Where(sqlWhere).Parameter("Status", dsDetail.status).Parameter("MallOrderId", dsDetail.MallOrderId).OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
                pager.TotalRows = _context.Select<int>("count(1)").From("DsOrder").Where(sqlWhere).Parameter("Status", dsDetail.status).Parameter("MallOrderId", dsDetail.MallOrderId).QuerySingle();
                pager.IdRows = _context.Select<int>("SysNo").From("DsOrder").Where(sqlWhere).Parameter("Status", dsDetail.status).Parameter("MallOrderId", dsDetail.MallOrderId).QueryMany();
            }
        }
        /// <summary>
        /// 获取经销商退换货订单
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="dsDetail">2017-8-29 罗熙 创建</param>
        public override void GetDsRMAorder(ref Pager<DsReturn> pager, ParaDsReturnFilter dsRMADetail)
        {
            using (var _context = Context.UseSharedConnection(true))
            {
                var sqlWhere = " 1=1 ";
                pager.Rows = _context.Select<DsReturn>("*").From("DsReturn").Where(sqlWhere).Parameter("RmaType", dsRMADetail.RmaType).Parameter("MallOrderId", dsRMADetail.MallOrderId).OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
                pager.TotalRows = _context.Select<int>("count(1)").From("DsReturn").Where(sqlWhere).Parameter("RmaType", dsRMADetail.RmaType).Parameter("MallOrderId", dsRMADetail.MallOrderId).QuerySingle();
                pager.IdRows = _context.Select<int>("SysNo").From("DsReturn").Where(sqlWhere).Parameter("RmaType", dsRMADetail.RmaType).Parameter("MallOrderId", dsRMADetail.MallOrderId).QueryMany();
            }
        }

        /// <summary>
        /// 获取商城名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public override string GetmallName(int sysNo)
        {
            return Context.Sql("select MallName from DsMallType where SysNo = (select MallTypeSysNo from DsDealerMall where SysNo=@sysNo)").Parameter("sysNo", sysNo).QuerySingle<string>();
        }
        /// <summary>
        /// 获取分销商商城名称
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public override string GetdealerName(int sysNo)
        {
            return Context.Sql("select DealerName from DsDealer where SysNo = (select DealerSysNo from DsDealerMall where SysNo=@sysNo)").Parameter("sysNo", sysNo).QuerySingle<string>();
        }
        /// <summary>
        /// 获取升舱订单号
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public override int GetorderSysNo(int sysNo)
        {
            return Context.Sql(@"select so.SysNo from SoOrderItem si,SoOrder so where
si.TransactionSysNo =(select dsi.OrderTransactionSysNo from DsOrder dso,DsOrderItem dsi where dsi.DsOrderSysNo=dso.SysNo and dso.SysNo=@sysNo )
and si.OrderSysNo=so.SysNo").Parameter("sysNo", sysNo).QuerySingle<int>();
        }
        
        public override string GetdealerSysNo(int sysNo)
        {
            return Context.Sql("select SysNo from DsDealer where SysNo = (select DealerSysNo from DsDealerMall where SysNo=@sysNo)").Parameter("sysNo", sysNo).QuerySingle<string>();
        }
        /// <summary>
        /// 查看分销商详情
        /// </summary>
        /// <param name="sysNo">分销商id</param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public override CBDsDealer GetDealerInfo(int sysNo)
        {
            return Context.Sql("select ddl.LevelName as LevelName,bs.AreaName as AreaAllName,dd.DealerName as DealerName,dd.StreetAddress as StreetAddress,dd.AppID as AppID,dd.AppSecret as AppSecret,dd.Contact as Contact,dd.MobilePhoneNumber as MobilePhoneNumber,dd.ThreeLevels as ThreeLevels,dd.ErpName as ErpName,dd.CreatedDate as CreatedDate,dd.LastUpdateDate as LastUpdateDate from DsDealer dd,DsDealerLevel ddl,BsArea bs where ddl.SysNo = dd.LevelSysNo and bs.SysNo=dd.AreaSysNo and dd.SysNo=@sysNo").Parameter("sysNo", sysNo).QuerySingle<CBDsDealer>();
        }
        /// <summary>
        /// 查看经销商退换货订单信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override DsReturnItem GetRMADealerInfo(int sysNo)
        {
            return Context.Sql("select * from DsReturnItem where DsReturnSysNo = @sysNo ").Parameter("sysNo", sysNo).QuerySingle<DsReturnItem>();
        }
        
        /// <summary>
        /// 升舱订单商品
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-24 罗熙 创建</returns>
        public override List<DsOrderItem> GetDealerOrderPdInfo(int sysNo)
        {
            return Context.Sql("select * from DsOrderItem where DsOrderSysNo = @sysNo").Parameter("sysNo", sysNo).QueryMany<DsOrderItem>();
        }
        /// <summary>
        /// 获取升舱订单
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-24 罗熙 创建</returns>
        public override DsOrder GetUpOrderModel(int sysNo)
        {
            return Context.Sql("select * from DsOrder where SysNo=@sysNo").Parameter("sysNo", sysNo).QuerySingle<DsOrder>();
        }
        /// 查看升舱订单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        public override CBDsOrder GetUpOrderInfo(int sysNo)
        {
            return Context.Sql(@"select si.OrderSysNo as OrderSysNo,cc.Name as CustomerName,si.ProductName as ProductName,si.Quantity as Quantity,si.SalesAmount as SalesAmount,so.OnlineStatus as OnlineStatus,cc.MobilePhoneNumber as MobilePhoneNumber,dm.ShopName as ShopName,dm.ShopAccount as ShopAccount
from SoOrderItem si,SoOrder so,CrCustomer cc ,DsDealerMall dm,DsOrder dso,DsOrderItem dsi
where
si.TransactionSysNo =(select dsi.OrderTransactionSysNo from DsOrder dso,DsOrderItem dsi where dsi.DsOrderSysNo=dso.SysNo and dso.SysNo=@sysNo )
 and so.SysNo=si.OrderSysNo
  and cc.SysNo=so.CustomerSysNo
and dm.SysNo=(select DealerMallSysNo from DsOrder dso,DsOrderItem dsi where dsi.DsOrderSysNo=dso.SysNo and dso.SysNo=@sysNo)
and dsi.DsOrderSysNo=dso.SysNo and dso.SysNo=@sysNo").Parameter("sysNo", sysNo).QuerySingle<CBDsOrder>();
        }
        /// <summary>
        /// 2015-12-31 王耀发 创建
        /// </summary>
        /// <param name="ProductSysNo">商品编号</param>
        /// <returns></returns>
        public override int DeleteByProSysNo(int ProductSysNo)
        {
            return Context.Delete("DsSpecialPrice")
                               .Where("ProductSysNo", ProductSysNo)
                               .Execute();
        }
        /// <summary>
        /// 2015-12-31 王耀发 创建
        /// </summary>
        /// <param name="ProductSysNo">商品编号</param>
        /// <returns></returns>
        public override int DeleteDealerByProSysNo(int DealerSysNo,int ProductSysNo)
        {
            return Context.Delete("DsSpecialPrice")
                               .Where("DealerSysNo", DealerSysNo)
                               .Where("ProductSysNo", ProductSysNo)
                               .Execute();
        }
        /// <summary>
        /// 获取特殊价格信息
        /// </summary>
        /// <param name="dealerSysNo">分销商系统编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>特殊价格信息</returns>
        /// <remarks>2016-1-3 王耀发 创建</remarks>
        public override DsSpecialPrice GetEntityByDPSysNo(int dealerSysNo, int productSysNo)
        {
            return Context.Sql("select * from DsSpecialPrice where DealerSysNo=@DealerSysNo and ProductSysNo=@ProductSysNo")
               .Parameter("DealerSysNo", dealerSysNo)
                .Parameter("ProductSysNo", productSysNo)
          .QuerySingle<DsSpecialPrice>();

        }
        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-3 王耀发 创建</remarks>
        public override Result UpdateSSPriceStatus(int status, int sysNo)
        {
            Result result = new Result();

            string Sql = string.Format("update DsSpecialPrice set Status = {0} where SysNo = {1}", status, sysNo);
            int rowsAffected = Context.Sql(Sql).Execute();
            if (rowsAffected > 0)
            {
                result.Status = true;
                result.Message = "更新成功";
                result.StatusCode = 1;
            }
            else
            {
                result.Status = false;
                result.Message = "更新状态失败";
                result.StatusCode = -1;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 更新经销商商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="ProductSysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-12 王耀发 创建</remarks>
        public override Result UpdatePriceStatusByPro(int status, int productSysNo)
        {
            Result result = new Result();

            string Sql = string.Format("update DsSpecialPrice set Status = {0} where ProductSysNo = {1}", status, productSysNo);
            int rowsAffected = Context.Sql(Sql).Execute();
            if (rowsAffected > 0)
            {
                result.Status = true;
                result.Message = "更新成功";
                result.StatusCode = 1;
            }
            else
            {
                result.Status = false;
                result.Message = "更新状态失败";
                result.StatusCode = -1;
                return result;
            }
            //string sql = " update PdProduct set LastUpdateDate='" + DateTime.Now.ToString() + "' where SysNo='" + productSysNo + "' ";
            //Context.Sql(sql).Execute();
            return result;
        }

        /// <summary>
        /// 更新经销商商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="ProductSysNo">商品编号</param>
        ///  <param name="DealerSysNo">分销商编号编号</param>s
        /// <returns>更新行数</returns>
        /// <remarks>2017-9-12 罗勤尧 创建</remarks>
        public override Result UpdatePriceStatusByPro(int status, int productSysNo, int DealerSysNo)
        {
            Result result = new Result();

            string Sql = string.Format("update DsSpecialPrice set Status = {0} where ProductSysNo = {1} and DealerSysNo={2}", status, productSysNo, DealerSysNo);
            int rowsAffected = Context.Sql(Sql).Execute();
            if (rowsAffected > 0)
            {
                result.Status = true;
                result.Message = "更新成功";
                result.StatusCode = 1;
            }
            else
            {
                result.Status = false;
                result.Message = "更新状态失败";
                result.StatusCode = -1;
                return result;
            }
            //string sql = " update PdProduct set LastUpdateDate='" + DateTime.Now.ToString() + "' where SysNo='" + productSysNo + "' ";
            //Context.Sql(sql).Execute();
            return result;
        }

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-1-3 王耀发 创建</remarks>
        public override Result UpdatePriceStatus(decimal price, int status, int sysNo)
        {
            Result result = new Result();

            string Sql = string.Format("update DsSpecialPrice set Price = {0},Status = {1} where ProductSysNo = {2}", price, status, sysNo);
            int rowsAffected = Context.Sql(Sql).Execute();
            if (rowsAffected > 0)
            {
                result.Status = true;
                result.Message = "更新成功";
                result.StatusCode = 1;
            }
            else
            {
                result.Status = false;
                result.Message = "更新上架状态失败";
                result.StatusCode = -1;
                return result;
            }
            DsSpecialPrice entity = GetEntityBySysNo(sysNo);
            string sql = " update PdProduct set LastUpdateDate='" + DateTime.Now.ToString() + "' where SysNo='" + entity.ProductSysNo + "' ";
            Context.Sql(sql).Execute();
            return result;
        }

        /// <summary>
        /// 未选中时更新全部分销商商品状态
        /// </summary>
        /// <param name="DealerSysNo">分销商编号</param>
        /// <param name="status">状态</param>
        /// <returns>更新行数</returns>
        /// <remarks>2016-9-8 罗远康 创建</remarks>
        public override Result UpdateAllPriceStatus(int DealerSysNo,int status)
        {
            Result result = new Result();
            string Sql = "";
            if (status == 1)//批量上架
            {
                Sql = string.Format(@"UPDATE DsSpecialPrice SET Status = {1} 
                                  WHERE SysNo IN(SELECT DSP.SysNo FROM DsSpecialPrice DSP INNER JOIN PdProduct PP ON PP.SysNo=DSP.ProductSysNo
                                                   WHERE DSP.DealerSysNo={0} AND DSP.Status=0 AND PP.Status=1)", DealerSysNo, status);
            }
            else//批量下架
            {
                Sql = string.Format(@"UPDATE DsSpecialPrice SET Status = {1} 
                                    WHERE SysNo IN(SELECT DSP.SysNo FROM DsSpecialPrice DSP INNER JOIN PdProduct PP ON PP.SysNo=DSP.ProductSysNo
                                                   WHERE DSP.DealerSysNo={0} AND DSP.Status=1)", DealerSysNo, status);
            }
            int rowsAffected = Context.Sql(Sql).Execute();
            if (rowsAffected > 0)
            {
                result.Status = true;
                result.Message = "更新成功";
                result.StatusCode = 1;
            }
            else
            {
                result.Status = false;
                result.Message = "更新上架状态失败";
                result.StatusCode = -1;
                return result;
            }
            return result;
        }

        /// <summary>
        /// 同步总部已上架商品到分销商商品表中
        /// 王耀发 2016-1-5 创建
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        public override int ProCreateSpecialPrice(int DealerSysNo, int CreatedBy)
        {
            string Sql = string.Format("pro_CreateSpecialPrice {0},{1}", DealerSysNo, CreatedBy);
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 
        /// 更新分销商商品价格
        /// </summary>
        /// <param name="ProductSysNos">分销商选中商品组</param>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="Percentage">修改价格百分比（传入值为除以100的值）</param>
        /// <returns>2016-09-06 罗远康 创建</returns>
        public override int ProUpdateSpecialPrice(string ProductSysNos, int DealerSysNo, decimal Percentage)
        {
            string Sql = string.Format("pro_UpdateSpecialPrice {0},{1},'{2}'", Percentage, DealerSysNo, ProductSysNos);
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 获取特殊价格信息
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <returns>特殊价格信息</returns>
        /// <remarks>2016-2-24 王耀发 创建</remarks>
        public override DsSpecialPrice GetEntityBySysNo(int SysNo)
        {
            return Context.Sql("select * from DsSpecialPrice where SysNo=@SysNo")
               .Parameter("SysNo", SysNo)
          .QuerySingle<DsSpecialPrice>();

        }

        /// <summary>
        /// 获取特殊价格信息
        /// </summary>
        /// <param name="SysNo">分销商商品系统编号</param>
        /// <returns>特殊价格信息</returns>
        /// <remarks>2016-2-24 王耀发 创建</remarks>
        //public override DsSpecialPrice GetEntityByProductSysNo(int ProductSysNo)
        //{
        //    return Context.Sql("select * from DsSpecialPrice where ProductSysNo=@ProductSysNo")
        //       .Parameter("ProductSysNo", ProductSysNo)
        //  .QuerySingle<DsSpecialPrice>();

        //}

        public override Result UpdatePriceStatus(decimal price, decimal shopPrice, int status, int sysNo)
        {
            Result result = new Result();

            string Sql = string.Format("update DsSpecialPrice set Price = {0},ShopPrice = {3},Status = {1} where SysNo = {2}", price, status, sysNo, shopPrice);
            int rowsAffected = Context.Sql(Sql).Execute();
            if (rowsAffected > 0)
            {
                result.Status = true;
                result.Message = "更新成功";
                result.StatusCode = 1;
            }
            else
            {
                result.Status = false;
                result.Message = "更新上架状态失败";
                result.StatusCode = -1;
                return result;
            }
            DsSpecialPrice entity = GetEntityBySysNo(sysNo);
            string sql = " update PdProduct set LastUpdateDate='" + DateTime.Now.ToString() + "' where SysNo='" + entity.ProductSysNo + "' ";
            Context.Sql(sql).Execute();
            return result;
        }
        public override Result UpdatePriceStatus(decimal price, decimal shopPrice, decimal wholesalePrice, int status, int sysNo)
        {
            List<DBKey> keyList = new List<DBKey>();
            keyList.Add(new DBKey() { KeyName = "WholesalePrice", Type = "decimal(18, 2)" });
            CheckKeyExcel(keyList);

            Result result = new Result();

            string Sql = string.Format("update DsSpecialPrice set Price = {0},ShopPrice = {3},WholesalePrice = {4},Status = {1} where SysNo = {2}", price, status, sysNo, shopPrice, wholesalePrice);
            int rowsAffected = Context.Sql(Sql).Execute();
            if (rowsAffected > 0)
            {
                result.Status = true;
                result.Message = "更新成功";
                result.StatusCode = 1;
            }
            else
            {
                result.Status = false;
                result.Message = "更新上架状态失败";
                result.StatusCode = -1;
                return result;
            }
            DsSpecialPrice entity = GetEntityBySysNo(sysNo);
            string sql = " update PdProduct set LastUpdateDate='" + DateTime.Now.ToString() + "' where SysNo='" + entity.ProductSysNo + "' ";
            Context.Sql(sql).Execute();
            return result;
        }
        public override List<DsSpecialPrice> GetAllProductDsSpecialPrice()
        {
            string sql = "select * from DsSpecialPrice";
            return Context.Sql(sql).QueryMany<DsSpecialPrice>();
        }
    }
}
