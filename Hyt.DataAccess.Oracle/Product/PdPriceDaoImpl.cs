using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Util;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;
using System.Collections;

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品价格维护数据接口
    /// </summary>
    /// <remarks>2013-06-26 邵斌 创建</remarks>
    public class PdPriceDaoImpl : IPdPriceDao
    {
        /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2016-06-28 王耀发 创建</remarks>
        public override IList<PdPrice> GetProductPriceByStatus(int productSysNo)
        {
            //查询指定商品的价格
            var list = Context.Select<PdPrice>("*")
                              .From("PdPrice")
                              .Where("ProductSysNo = @ProductSysNo and Status = @Status")
                              .Parameter("ProductSysNo", productSysNo)
                              .Parameter("Status", (int)ProductStatus.产品价格状态.有效)
                              .OrderBy("PriceSource,SourceSysNo")
                              .QueryMany();

            //返回结果集
            return list;
        }
        /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        public override IList<PdPrice> GetProductPrice(int productSysNo)
        {
            //查询指定商品的价格
            var list = Context.Select<PdPrice>("*")
                              .From("PdPrice")
                              .Where("ProductSysNo = @ProductSysNo")
                              .Parameter("ProductSysNo", productSysNo)
                              .OrderBy("PriceSource,SourceSysNo")
                              .QueryMany();

            //返回结果集
            return list;
        }
        /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="priceType">用于显示的价格类型</param>
        /// <param name="status">价格状态</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2016-09-12 杨浩 创建</remarks>
        public override IList<PdPrice> GetProductPrice(int productSysNo, int priceType, int status)
        {
            //查询指定商品的价格
            var list = Context.Select<PdPrice>("*")
                              .From("PdPrice")
                              .Where("ProductSysNo =@ProductSysNo and PriceSource = @PriceSource and Status=@Status")
                              .Parameter("ProductSysNo", productSysNo)
                              .Parameter("PriceSource", priceType)
                              .Parameter("Status", status)
                              //.Parameter("Status", (int)ProductStatus.产品价格状态.有效)
                              .QueryMany();

            //返回结果集
            return list;
        }
        /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="priceType">用于显示的价格类型</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        public override IList<PdPrice> GetProductPrice(int productSysNo, ProductStatus.产品价格来源 priceType)
        {
            //查询指定商品的价格
            var list = Context.Select<PdPrice>("*")
                              .From("PdPrice")
                              .Where("ProductSysNo =@ProductSysNo and PriceSource = @PriceSource and Status=@Status")
                              .Parameter("ProductSysNo", productSysNo)
                              .Parameter("PriceSource", (int)priceType)
                              .Parameter("Status", (int)ProductStatus.产品价格状态.有效)
                              .QueryMany();

            //返回结果集
            return list;
        }
        /// <summary>
        /// 获取指定商品的商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="priceType">用于显示的价格类型</param>
        /// <param name="status">价格状态</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2016-09-12 杨浩 创建</remarks>
        public override IList<PdPrice> GetProductPrices(int productSysNo, int priceType)
        {
            //查询指定商品的价格
            var list = Context.Select<PdPrice>("*")
                              .From("PdPrice")
                              .Where("ProductSysNo =@ProductSysNo and PriceSource = @PriceSource")
                              .Parameter("ProductSysNo", productSysNo)
                              .Parameter("PriceSource", priceType)
                //.Parameter("Status", (int)ProductStatus.产品价格状态.有效)
                              .QueryMany();

            //返回结果集
            return list;
        }
        /// <summary>
        /// 获取指定商品的会员等级商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>        
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2013-07-17 郑荣华 创建</remarks>
        public override IList<CBPdPrice> GetProductLevelPrice(int productSysNo)
        {
            const string sql = @"select t.*,b.levelname PriceName from pdprice t 
                                 left join CrCustomerLevel b on t.SourceSysNo=b.sysno
                                 where t.PriceSource=@0 and t.ProductSysNo=@1 and t.Status =@2
                                 order by b.lowerlimit";
            var model = Context.Sql(sql, (int)ProductStatus.产品价格来源.会员等级价, productSysNo, (int)ProductStatus.产品价格状态.有效)
                               .QueryMany<CBPdPrice>();

            return model;
        }

        /// <summary>
        /// 获取指定商品的分销商等级商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="sourceSysNo">来源系统编号</param>
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2013-09-17 周瑜 创建</remarks>
        public override IList<CBPdPrice> GetDealerProductLevelPrice(int productSysNo, int sourceSysNo)
        {
            const string sql = @"select t.*,b.levelname PriceName from pdprice t 
                                 left join CrCustomerLevel b on t.SourceSysNo=b.sysno
                                 where t.PriceSource=@0 and t.ProductSysNo=@1 and SourceSysNo =@2
                                 order by b.lowerlimit";
            var model = Context.Sql(sql, (int) ProductStatus.产品价格来源.分销商等级价,
                                    productSysNo, sourceSysNo)
                               .QueryMany<CBPdPrice>();

            return model;
        }

        /// <summary>
        /// 创建商品价格信息
        /// </summary>
        /// <param name="model">价格信息</param>
        /// <returns>价格系统编号</returns>
        /// <remarks>2013-06-27 黄波 创建</remarks>
        public override int Create(PdPrice model)
        {
            var sysNo = Context.Insert<PdPrice>("PdPrice", model)
                .AutoMap(o => o.SysNo)
                .ExecuteReturnLastId<int>("SysNo");

            return sysNo;
        }

        /// <summary>
        /// 根据价格编号获取商品价格详细信息
        /// </summary>
        /// <param name="sysNo">价格系统编号</param>
        /// <returns>价格详细信息</returns>
        /// <remarks>2013-06-27 黄波 创建</remarks>
        public override PdPrice GetPrice(int sysNo)
        {
            //查询指定商品的价格
            var returnValue = Context.Select<PdPrice>("*")
                .From("PdPrice")
                .Where("SysNo = @SysNo")
                .Parameter("SysNo", sysNo)
                .QuerySingle();

            //返回结果集
            return returnValue;
        }

        /// <summary>
        /// 获取价格来源类型列表
        /// </summary>
        /// <returns>价格来源类型列表</returns>
        /// <remarks>2013-07-17 黄波 创建</remarks>
        public override IList<PdPriceType> GetPriceTypeItems()
        {
            var returnValue = new List<PdPriceType>();

            var typeList = EnumUtil.ToDictionary(typeof(ProductStatus.产品价格来源));
            foreach (var item in typeList)
            {
                if (item.Key == (int)ProductStatus.产品价格来源.会员等级价)
                {
                    var customerLevel = Hyt.DataAccess.CRM.ICrCustomerLevelDao.Instance.List();
                    foreach (var levelItem in customerLevel)
                    {
                        returnValue.Add(new PdPriceType
                        {
                            PriceSource = item.Key,
                            SourceSysNo = levelItem.SysNo,
                            TypeName = levelItem.LevelName
                        });
                    }
                }
                else if (item.Key == (int)ProductStatus.产品价格来源.分销商等级价)
                {
                    IList<DsDealerLevel> dsDealerLevels = Context.Select<DsDealerLevel>("*")
                                                                 .From("DsDealerLevel")
                                                                 .QueryMany();
                    foreach (DsDealerLevel dsDealerLevel in dsDealerLevels)
                    {
                        returnValue.Add(new PdPriceType
                        {
                            PriceSource = item.Key,
                            SourceSysNo = dsDealerLevel.SysNo,
                            TypeName = dsDealerLevel.LevelName
                        });
                    }
                }
                else if (item.Key == (int)ProductStatus.产品价格来源.配送员进货价)
                {
                    returnValue.Add(new PdPriceType
                    {
                        PriceSource = item.Key,
                        SourceSysNo = 0,
                        TypeName = item.Value
                    });
                }
                else if (item.Key == (int)ProductStatus.产品价格来源.业务员销售限价)
                {
                    returnValue.Add(new PdPriceType
                    {
                        PriceSource = item.Key,
                        SourceSysNo = 0,
                        TypeName = item.Value
                    });
                }
                else
                {
                    returnValue.Add(new PdPriceType
                    {
                        PriceSource = item.Key,
                        SourceSysNo = 0,
                        TypeName = item.Value
                    });
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override int Update(PdPrice model)
        {
            int rowsAffected = Context.Update<PdPrice>("PdPrice", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sysNo">价格系统编码</param>
        /// <param name="price">新价格</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override int Update(int sysNo, decimal price)
        {
            int rowsAffected =
                Context.Sql(
                    "update PdPrice set price=@price,status=@status where sysNo=@sysNo")
                       .Parameter("price", price)
                       .Parameter("status", (int)ProductStatus.产品价格状态.有效)
                       .Parameter("sysNo", sysNo)
                       .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 更新单个商品价格状态
        /// </summary>
        /// <param name="priceSysNo">商品价格系统编号</param>
        /// <param name="status">商品状态</param>
        /// <returns>是否更新成功</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override bool UpdatePriceStatus(int priceSysNo, ProductStatus.产品价格状态 status)
        {
                return Context.Sql("update pdprice set status = @0 where sysno=@1", (int)status, priceSysNo).Execute() > 0;
        }

        /// <summary>
        /// 2015-12-31 王耀发 创建
        /// </summary>
        /// <param name="ProductSysNo">商品编号</param>
        /// <param name="PriceSource">价格源</param>
        /// <returns></returns>
        public override PdPrice GetSalesPrice(int ProductSysNo, int PriceSource)
        {
            //查询指定商品的价格
            return Context.Select<PdPrice>("*")
                .From("PdPrice")
                .Where("ProductSysNo = @ProductSysNo and PriceSource = @PriceSource and Status = 1")
                .Parameter("ProductSysNo", ProductSysNo)
                .Parameter("PriceSource", PriceSource)
                .QuerySingle();
        }

        public override List<PdPrice> GetProductPrices(List<int> productSysNoList, int PriceSource)
        {
            string sql = " select * from PdPrice where ProductSysNo in (" + string.Join(",", productSysNoList) + ") and PriceSource = '" + PriceSource + "' ";
            return Context.Sql(sql).QueryMany<PdPrice>();
        }

        /// <summary>
        /// 删除产品价格
        /// </summary>
        /// <param name="priceSysNoList">价格系统编号列表</param>
        /// <returns></returns>
        /// <remarks>2017-3-11 杨浩 创建</remarks>
        public override bool DeleltePrice(string priceSysNoList)
        {
            string sqlPriceHistory = " delete PdPriceHistory  where priceSysNo in(" + priceSysNoList + ")";

            string sqlPrice= "delete [PdPrice] where sysNo in (" + priceSysNoList + ")";

            if (Context.Sql(sqlPriceHistory).Execute() > 0)
                return Context.Sql(sqlPrice).Execute() > 0;

            return false;
        }

        /// <summary>
        /// 删除产品重复的价格
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-3-11 杨浩 创建</remarks>
        public override bool DeleleRepeatPrice(int productSysNo)
        {
            string sqlPriceHistory = string.Format(@" delete  PdPriceHistory where priceSysNo in 
                            (
                                select sysNo from [PdPrice] where sysNo not in 
                                (select  max(sysNo) FROM [PdPrice] where productSysNo={0} group by [ProductSysNo],[PriceSource],[SourceSysNo])
                                and productSysNo={0}
                            )", productSysNo);
            string sqlPrice = string.Format(@" delete [PdPrice]  where sysNo not in 
                    (select  max(sysNo) FROM [PdPrice] where productSysNo={0} group by [ProductSysNo],[PriceSource],[SourceSysNo])
                    and productSysNo={0}", productSysNo);

            if (Context.Sql(sqlPriceHistory).Execute() > 0)
                return Context.Sql(sqlPrice).Execute() > 0;

            return false;
        }

    }
}
