using Hyt.BLL.Authentication;
using Hyt.DataAccess.Product;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Stores
{
    /// <summary>
    /// 经销商产品特殊价格表
    /// </summary>
    /// <remarks>2015-12-7 杨浩 创建</remarks>
    public class DsSpecialPriceBo : BOBase<DsSpecialPriceBo>
    {

        /// <summary>
        /// 获得指定经销商的产品价格
        /// </summary>
        /// <param name="sysNo">经销商编号</param>
        /// <returns></returns>
        public IList<DsSpecialPrice> GetSpecialPricesBySysNo(int sysNo)
        {
            var items = (IList<DsSpecialPrice>)CacheManager.Get(CacheKeys.Items.StoresProductList_, sysNo.ToString(), () =>
            {
                var specialPrices = Hyt.DataAccess.Stores.IDsSpecialPriceDao.Instance.GetSpecialPricesBySysNo(sysNo);
                return specialPrices;
            }
            );
            return items;
        }
        /// <summary>
        /// 获得指定经销商的产品价格
        /// </summary>
        /// <param name="sysNo">经销商编号</param>
        /// <param name="productSysNo">产品编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-3 杨浩 创建</remarks>
        public decimal GetSpecialPricesBySysNo(int sysNo, int productSysNo)
        {
            var items = (decimal)CacheManager.Get(CacheKeys.Items.StoresProduct_, sysNo.ToString() + "_" + productSysNo.ToString(), () =>
            {
                var specialPrices = Hyt.DataAccess.Stores.IDsSpecialPriceDao.Instance.GetSpecialPricesBySysNo(sysNo, productSysNo);
                if (specialPrices == null)
                    return -1;
                return specialPrices.Price;
            }
            );

            return items;
        }
        /// <summary>
        /// 获取经销商商品销售价格
        /// （没找到价格则返回-1）
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="currentRankPrice">会员等级价</param>
        /// <returns></returns>
        /// <remarks>2016-3-3  杨浩 添加会员等级价当店铺为总时显示会员等级价</remarks>
        public decimal GetSpecialPrice(int productSysNo,int storeId, decimal currentRankPrice = -1)
        {
         
            if (storeId == 0)
            {
                //var userInfo = CustomerAuthenticationBo.Instance.GetCurrentUser(WebStoreContext.Instance.EnableWeiXinApi);
                var LevelSysNo = 0;
                //if (userInfo != null)
                   // LevelSysNo = userInfo.LevelSysNo;

                var productInfo = Hyt.BLL.Web.PdProductBo.Instance.GetProduct(productSysNo);
                var pdPrice = Hyt.BLL.Web.PdProductBo.Instance.GetLevelPrice(productInfo.Prices, LevelSysNo);
                currentRankPrice = pdPrice.Price;
                return currentRankPrice;
            }

            var specialPrices = GetSpecialPricesBySysNo(storeId, productSysNo);

            var dsSpecialPrice = specialPrices;// specialPrices.Where(x => x.ProductSysNo == productSysNo).FirstOrDefault();

            //if (dsSpecialPrice == null ||(dsSpecialPrice!=null&&dsSpecialPrice.Status ==(int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格状态.无效))
            //return -1;

            return dsSpecialPrice;
            //return dsSpecialPrice.Price;
        }
        /// <summary>
        /// 设置店铺销售价格
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="prices">产品等级价</param>
        /// <returns></returns>
        /// <remarks>2016-3-2 杨浩 重构 修改当店铺id为0是价格用会员等级价</remarks>
        public void SetStorePrice(int productSysNo, IList<PdPrice> prices,int storeId)
        {
          

            if (prices == null)
                return;

            decimal price = GetSpecialPrice(productSysNo,storeId);

            //var specialPriceInfo = Grand.BLL.Stores.DsSpecialPriceBo.Instance.GetSpecialPricesBySysNo(WebStoreContext.Instance.StoreId);

            #region 重置产品的价格为经销商价格

            for (int i = 0; i < prices.Count(); i++)
            {
                if (prices[i].PriceSource == (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价)
                {
                    prices[i].Price = price;
                }
            }
            #endregion
        }
       
    }
}