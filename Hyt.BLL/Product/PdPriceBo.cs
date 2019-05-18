using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.BLL.Log;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.CRM;
using Hyt.Infrastructure.Caching;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 商品价格维护
    /// </summary>
    /// <remarks>2013-06-26 邵斌 创建</remarks>
    public class PdPriceBo : BOBase<PdPriceBo>
    {
        /// <summary>
        /// 删除产品重复的价格
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-3-11 杨浩 创建</remarks>
        public bool DeleleRepeatPrice(int productSysNo)
        {
            return IPdPriceDao.Instance.DeleleRepeatPrice(productSysNo);
        }
        /// <summary>
        /// 商品价格维护数据接口
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品价格列表</returns>
        /// <remarks>2016-06-28 王耀发 创建</remarks>
        public IList<PdPrice> GetProductPriceByStatus(int productSysNo)
        {
            return IPdPriceDao.Instance.GetProductPriceByStatus(productSysNo);
        }
        /// <summary>
        /// 商品价格维护数据接口
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品价格列表</returns>
        /// <remarks>2013-06-27 黄波 创建</remarks>
        public IList<PdPrice> GetProductPrice(int productSysNo)
        {
            return IPdPriceDao.Instance.GetProductPrice(productSysNo);
        }

        /// <summary>
        /// 创建商品价格信息
        /// </summary>
        /// <param name="model">价格信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2013-06-27 黄波 创建</remarks>
        public int Create(PdPrice model)
        {
            return IPdPriceDao.Instance.Create(model);
        }

        /// <summary>
        /// 根据价格编号获取商品价格详细信息
        /// </summary>
        /// <param name="sysNo">价格系统编号</param>
        /// <returns>价格详细信息</returns>
        /// <remarks>2013-06-27 黄波 创建</remarks>
        public PdPrice GetPrice(int sysNo)
        {
            return IPdPriceDao.Instance.GetPrice(sysNo);
        }

        /// <summary>
        /// 商品价格维护数据接口
        /// </summary>
        /// <returns>价格列表</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        public IList<CBPdPrice> GetProductPrice(int productSysNo, ProductStatus.产品价格来源[] showPriceType)
        {
            //读取价格
            IList<PdPrice> priceList = IPdPriceDao.Instance.GetProductPrice(productSysNo);

            //设置结果集
            IList<CBPdPrice> result = new List<CBPdPrice>();

            CBPdPrice newPriceMode;
            //组合商品价格名称
            IList<PdPriceType> priceTypes = GetPriceTypeItems();
            PdPriceType tempType;

            //对查询结果中的价格进行简单筛选
            foreach (PdPrice p in priceList)
            {
                
                //如果价格是需要用于显示的价格将添加到结果集中
                if (showPriceType.Contains((ProductStatus.产品价格来源) p.PriceSource))
                {
                    newPriceMode = new CBPdPrice()
                        {
                            Price = p.Price
                            ,
                            PriceSource = p.PriceSource
                            ,
                            ProductSysNo = p.ProductSysNo
                            ,
                            SourceSysNo = p.SourceSysNo
                            ,
                            Status = p.Status
                            ,
                            SysNo = p.SysNo
                        };

                    newPriceMode.PriceName = ((ProductStatus.产品价格来源)p.PriceSource).ToString();
                    tempType = priceTypes.FirstOrDefault(pn => pn.PriceSource == p.PriceSource && pn.SourceSysNo == p.SourceSysNo);
                    if (tempType != null && newPriceMode.PriceName != tempType.TypeName)
                    {
                        newPriceMode.PriceName += " " + tempType.TypeName;
                    }
                    
                    if (!result.Contains(newPriceMode))
                        result.Add(newPriceMode);
                }
            }

            //返回结果
            return result;
        }

        /// <summary>
        /// 操作价格来源对象，根据来源枚举查找对应对象
        /// </summary>
        /// <param name="sysNo">来源对象系统编号</param>
        /// <param name="source">来源类型</param>
        /// <returns>返回来源对象，该来源对象未知，由来源类型决定对象</returns>
        /// <remarks>2013-06-27 邵斌 创建</remarks>
        public string GetPriceSourceName(int sysNo, ProductStatus.产品价格来源 source)
        {
            //判断来源类型
            //TODO 其他来源类型请在使用时自己添加
            switch (source)
            {
                case ProductStatus.产品价格来源.分销商等级价:
                    return ProductStatus.产品价格来源.分销商等级价.ToString();
                case ProductStatus.产品价格来源.会员等级价:
                    CrCustomerLevel level = ICrCustomerLevelDao.Instance.GetCustomerLevel(sysNo);
                    return level == null ? "未知等级" : level.LevelName;
                case ProductStatus.产品价格来源.业务员销售限价:
                    return ProductStatus.产品价格来源.业务员销售限价.ToString();
                case ProductStatus.产品价格来源.配送员进货价:
                    return ProductStatus.产品价格来源.配送员进货价.ToString();
                default:
                    return ProductStatus.产品价格来源.基础价格.ToString();
            }
        }
        /// <summary>
        /// 获取价格来源类型列表
        /// </summary>
        /// <returns>价格来源类型列表</returns>
        /// <remarks>2013-07-17 黄波 创建</remarks>
        public IList<PdPriceType> GetPriceTypeItems()
        {
            return Hyt.DataAccess.Product.IPdPriceDao.Instance.GetPriceTypeItems();
        }

        /// <summary>
        /// 根据来源类型和来源编号取类型名称
        /// 页面如果需要调用此方法两次及以上
        /// 请使用该方法的重载方法配合GetPriceTypeItems()方法以提高效率
        /// </summary>
        /// <param name="priceSource">价格来源</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <returns>价格类型名称</returns>
        /// <remarks>2013-07-17 黄波 创建</remarks>
        public string GetPriceTypeName(int priceSource, int sourceSysNo)
        {
            return this.GetPriceTypeName(this.GetPriceTypeItems(), priceSource, sourceSysNo);
        }

        /// <summary>
        /// 根据来源类型和来源编号取类型名称
        /// </summary>
        /// <param name="priceTypeList">价格来源类型列表</param>
        /// <param name="priceSource">价格来源</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <returns>价格类型名称</returns>
        /// <remarks>2013-07-17 黄波 创建</remarks>
        public string GetPriceTypeName(IList<PdPriceType> priceTypeList, int priceSource, int sourceSysNo)
        {
            var returnValue = "未知类型";

            var priceType =
                priceTypeList.ToList().Find(o => (o.PriceSource == priceSource && o.SourceSysNo == sourceSysNo));
            if (priceType != null)
            {
                returnValue = priceType.TypeName;
            }

            return returnValue;
        }

        /// <summary>
        /// 获取指定商品的会员等级商品价格
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>        
        /// <returns>指定商品的价格列表</returns>
        /// <remarks>2014-09-17 余勇 创建</remarks>
        public IList<CBPdPrice> GetProductLevelPrice(int productSysNo)
        {
            return IPdPriceDao.Instance.GetProductLevelPrice(productSysNo); ;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public int Update(PdPrice model)
        {
            return IPdPriceDao.Instance.Update(model);
        }
        /// <summary>
        /// 更新申请价格到商品价格表
        /// </summary>
        /// <param name="list">审批通过的商品申请价格</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>
        /// 2013－06-19 杨晗 创建
        /// 2016-9-12 杨浩 增加修改价格时先检查是否有已审核过的价格记录有则更新现有的
        /// </remarks>
        public bool UpdateApplyPriceToPdPrice(IList<CBPdPriceHistory> list)
        {
            bool isPass = true;
            foreach (var item in list)
            {
                var prices = IPdPriceDao.Instance.GetProductPrices(item.ProductSysNo, item.PriceSource);
                int priceSysNo = item.PriceSysNo;
                if (prices != null && prices.Count > 0)
                {
                    var sourcePrices = prices.Where(p => p.SourceSysNo == item.SourceSysNo);

                    if (sourcePrices == null)
                        continue;

                    string priceSysNoList = "";
                    int i = 0;
                    foreach (var _price in sourcePrices)
                    {
                        if (i == 0)
                        {
                            priceSysNo = _price.SysNo;
                        }                         
                        else
                        {
                            if (priceSysNoList != "")
                                priceSysNoList += ",";
                            priceSysNoList += _price.SysNo;
                        }                     
                        i++;
                    }

                    if (priceSysNoList != "")//如果价格表中存在两个价格则删除重复保留第一个
                        Hyt.DataAccess.Product.IPdPriceDao.Instance.DeleltePrice(priceSysNoList);
                                                
                }
                int up = IPdPriceDao.Instance.Update(priceSysNo,item.ApplyPrice);
                SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "更新了商品价格", LogStatus.系统日志目标类型.商品调价申请, up, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
                if (up == 0)
                {
                    isPass = false;
                    break;
                }
            }
            return isPass;
        }

        /// <summary>
        /// 更一个指定商品的价格信息
        /// </summary>
        /// <param name="sysNo">商品系统编号</param>
        /// <param name="prices">更新的价格</param>
        /// <returns>价格更新结果对象</returns>
        /// <remarks>2013-07-26 邵斌 创建</remarks>
        public Result UpdateProductPrice(int sysNo, IList<PdPrice> prices)
        {
            Result result = new Result();

            //商品源价格
            IList<PdPrice> beforePrices = PdPriceBo.Instance.GetProductPrice(sysNo);

           
            //避免重复插入价格数据这里进行格式化价格系统编号
            if (beforePrices.Count > 0)
            {
                foreach (var singlePrice in prices)
                {
                    singlePrice.Status = 1;
                    singlePrice.SysNo =
                        beforePrices.Where(
                            p =>
                            p.PriceSource == singlePrice.PriceSource && p.SourceSysNo == singlePrice.SourceSysNo).Select(p => p.SysNo).FirstOrDefault();

                
                }
            }

            //通过对比原价格和申请价格找出不一致的价格对象，该对象集合就是要申请调价的价格
            var query = from b in beforePrices
                        join p in prices on new { b.PriceSource, b.SourceSysNo } equals new { p.PriceSource, p.SourceSysNo }
                        where !p.Price.Equals(b.Price)
                        select new
                        {
                            Price = p.Price,
                            Status = p.Status,
                            PriceSource = p.PriceSource,
                            SourceSysNo = p.SourceSysNo,
                            ProductSysNo = p.ProductSysNo,
                            SysNo = p.SysNo,
                            OriginalPrice = b.Price
                        };


            var newprices = query.ToList<dynamic>();//老的价格变动
            foreach (var pdPrice in prices)
                {
                    if (pdPrice.SysNo == 0)
                    {
                        pdPrice.ProductSysNo = sysNo;
                        pdPrice.Status = (int)ProductStatus.产品价格状态.有效;

                        PdPriceBo.Instance.Create(pdPrice);
                        pdPrice.SysNo = pdPrice.SysNo;
                        newprices.Add(new
                        {
                            Price = 0,
                            Status = (int)ProductStatus.产品价格状态.无效,
                            PriceSource = pdPrice.PriceSource,
                            ProductSysNo = sysNo,
                            SourceSysNo = pdPrice.SourceSysNo,
                            SysNo = pdPrice.SysNo,
                            OriginalPrice = 0
                        });//新的价格信息

                    }
                }

            //商品价格无任何改动时不做提交
            if (!newprices.Any())
            {
                result.Status = false;
                result.Message = " 商品价格没有做任何改动。";
                return result;
            }

            string groupCode = Guid.NewGuid().ToString("N"); //生成关联关系码

            IList<CBPdPriceHistory> pdPriceHistoryList = new List<CBPdPriceHistory>();
            foreach (var pdPrice in newprices)
            {
                pdPriceHistoryList.Add(new CBPdPriceHistory()
                {
                    PriceSource = pdPrice.PriceSource,
                    SourceSysNo = pdPrice.SourceSysNo,
                    OriginalPrice = pdPrice.OriginalPrice,
                    ApplyPrice = pdPrice.Price - pdPrice.OriginalPrice,
                    //ApplyPrice = pdPrice.Price,
                    PriceSysNo = pdPrice.SysNo,
                    RelationCode = groupCode,
                    ApplySysNo = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo,
                    ApplyDate = DateTime.Now,
                    Status = (int)ProductStatus.产品价格变更状态.待审
                });
            }

            result = PdPriceHistoryBo.Instance.SavePdPriceHistories(pdPriceHistoryList.ToArray(),
                                                                           new int[] { sysNo });

            if (result.Status)
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("提交商品{0}调整价申请格", sysNo), LogStatus.系统日志目标类型.商品描述, sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            else
            {
                try
                {
                    //用户操作日志
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("提交商品{0}调整价申请格失败"), LogStatus.系统日志目标类型.商品调价申请,
                                                      sysNo, AdminAuthenticationBo.Instance.Current.Base.SysNo);
                }
                catch (Exception e)
                {

                }
            }

            return result;

        }

        /// <summary>
        /// 更新商品的单个价格状态
        /// </summary>
        /// <param name="priceSysNo">商品价格系统编号</param>
        /// <param name="status">价格要更新的状态</param>
        /// <returns>返回商品更新的结果信息</returns>
        /// <remarks>2013-07-26 邵斌 创建</remarks>
        public Result UpdatePriceStatus(int priceSysNo, ProductStatus.产品价格状态 status)
        {
            Result result = new Result();

            result.Status = IPdPriceDao.Instance.UpdatePriceStatus(priceSysNo, status);

            //日志操作
            if (result.Status)
            {
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("改变商品{0}价格状态为{1}", priceSysNo,
                status.ToString()),
                LogStatus.系统日志目标类型.商品调价状态,
                priceSysNo,
                AdminAuthenticationBo.Instance.Current.Base.SysNo)
                ;
            }
            else
            {
               //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("改变商品{0}价格状态为{1}失败", priceSysNo,
                status.ToString()),
                LogStatus.系统日志目标类型.商品调价状态,
                priceSysNo,
                AdminAuthenticationBo.Instance.Current.Base.SysNo)
                ;  
            }

            return result;
        }

        /// <summary>
        /// 读取商品等级价格
        /// </summary>
        /// <param name="productSysno">商品名称</param>
        /// <param name="rankSysno">等级系统编号</param>
        /// <param name="priceType">等级类型</param>
        /// <returns>返回单个价格</returns>
        /// <remarks>2013-08-26 邵斌 创建</remarks>
        public decimal GetUserRankPrice(int productSysno, int rankSysno, ProductStatus.产品价格来源 priceType = ProductStatus.产品价格来源.会员等级价)
        {
            //CacheManager.Get<IList<CBPdPrice>>(CacheKeys.Items.
            IList<CBPdPrice> list = GetProductPrice(productSysno, new ProductStatus.产品价格来源[] { priceType });
            decimal price = 0;
            foreach (CBPdPrice p in list)
            {
                if (p.SourceSysNo == rankSysno)
                {
                    price = p.Price;
                    break;
                }
            }
            return price;
        }

        /// <summary>
        /// 2015-12-31 王耀发 创建
        /// </summary>
        /// <param name="ProductSysNo">商品编号</param>
        /// <param name="PriceSource">价格源</param>
        /// <returns></returns>
        public PdPrice GetSalesPrice(int ProductSysNo, int PriceSource)
        {
            return IPdPriceDao.Instance.GetSalesPrice(ProductSysNo, PriceSource);
        }

        /// <summary>
        /// 商品价格维护数据接口
        /// </summary>
        /// <returns>价格列表</returns>
        /// <remarks>2013-06-26 邵斌 创建</remarks>
        public IList<CBPdPrice> GetProductPrice(int productSysNo, ProductStatus.产品价格来源[] showPriceType, int storesId)
        {
            //读取价格
            IList<PdPrice> priceList = IPdPriceDao.Instance.GetProductPrice(productSysNo);

            //等级价转为店铺销售价
            BLL.Stores.DsSpecialPriceBo.Instance.SetStorePrice(productSysNo, priceList, storesId);


            //设置结果集
            IList<CBPdPrice> result = new List<CBPdPrice>();

            CBPdPrice newPriceMode;
            //组合商品价格名称
            IList<PdPriceType> priceTypes = GetPriceTypeItems();
            PdPriceType tempType;

            //对查询结果中的价格进行简单筛选
            foreach (PdPrice p in priceList)
            {

                //如果价格是需要用于显示的价格将添加到结果集中
                if (showPriceType.Contains((ProductStatus.产品价格来源)p.PriceSource))
                {
                    newPriceMode = new CBPdPrice()
                    {
                        Price = p.Price
                        ,
                        PriceSource = p.PriceSource
                        ,
                        ProductSysNo = p.ProductSysNo
                        ,
                        SourceSysNo = p.SourceSysNo
                        ,
                        Status = p.Status
                        ,
                        SysNo = p.SysNo
                    };

                    newPriceMode.PriceName = ((ProductStatus.产品价格来源)p.PriceSource).ToString();
                    tempType = priceTypes.FirstOrDefault(pn => pn.PriceSource == p.PriceSource && pn.SourceSysNo == p.SourceSysNo);
                    if (tempType != null && newPriceMode.PriceName != tempType.TypeName)
                    {
                        newPriceMode.PriceName += " " + tempType.TypeName;
                    }

                    if (!result.Contains(newPriceMode))
                        result.Add(newPriceMode);
                }
            }

            //返回结果
            return result;
        }
    }
}
