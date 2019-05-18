using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Caching;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 商品组业务逻辑
    /// </summary>
    /// <remarks>2013-08-06 黄波 创建</remarks>
    public class FeProductItemBo : BOBase<FeProductItemBo>
    {
        /// <summary>
        /// 根据商品组代码获取商品
        /// 返回部分列数据
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="customerLevel">会员级别</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-06 黄波 创建</remarks>
        /// <remarks>2013-11-25 苟治国 修改</remarks>
        public IList<Model.PdProductIndex> GetFeProductItems(string groupCode, int customerLevel, int storeId)
        {
            //var items = (List<Model.FeProductItem>)CacheManager.Get<IList<Model.FeProductItem>>(CacheKeys.Items.WebProductItem_, groupCode, () =>
            //{
              var items= Hyt.DataAccess.Web.IFeProductItemDao.Instance.GetFeProductItems(Model.WorkflowStatus.ForeStatus.商品组平台类型.PC网站, groupCode);
            //});

            //挑选出满足时间的项
            //var nowDateTime = DateTime.Now;
            //if (items != null)
            //{
            //    items = items.FindAll(o =>
            //    {
            //        return o.BeginDate <= nowDateTime && o.EndDate >= nowDateTime;
            //    });
            //}

            //ProductStatus.产品价格来源.会员等级价
            //return Hyt.BLL.Web.ProductIndexBo.Instance.Search(items.Where(p => p.DealerSysNo == storeId).ToList(),ProductStatus.产品价格来源.会员等级价,customerLevel);
            List<int> productSysNos=new List<int>();
            foreach(var item in items)
            {
                productSysNos.Add(item.ProductSysNo);
            }
            var allProduct = Hyt.BLL.Product.PdProductBo.Instance.GetAllProduct(productSysNos);
            foreach(Model.PdProductIndex index in allProduct)
            {
                index.ProductImage = Hyt.BLL.Web.ProductImageBo.Instance.GetProductImagePath(Hyt.BLL.Web.ProductThumbnailType.Image240, index.SysNo);
            }
            return allProduct;
         }

        /// <summary>
        /// 根据商品组代码获取商品
        /// </summary>
        /// <returns>一条有效团购记录</returns>
        /// <remarks>2013-09-28 苟治国 创建</remarks>
        public Model.GsGroupShopping GetGroupShopping()
        {
            var pager = new Pager<GsGroupShopping>();
            pager.PageFilter = new GsGroupShopping
            {
                StartTime = null,
                EndTime = DateTime.Now.AddDays(5)
            };
            var items = (Model.GsGroupShopping)CacheManager.Get<Model.GsGroupShopping>(CacheKeys.Items.WebGroupShopping_, () =>
                    {
                        return Hyt.DataAccess.Tuan.IGsGroupShoppingDao.Instance.GetGroupShopping(pager);
                    });
            return items;
        }

        /// <summary>
        /// 根据商品组代码获取商品
        /// 返回部分列数据
        /// </summary>
        /// <param name="platformType">平台类型</param>
        /// <param name="groupCode">组代码</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-21 周瑜 创建</remarks>
        public IList<Model.FeProductItem> GetFeProductItems(ForeStatus.商品组平台类型 platformType, string groupCode)
        {
            var items =
                (List<Model.FeProductItem>)
                CacheManager.Get(CacheKeys.Items.WebProductItem_, groupCode,
                                 () =>
                                 DataAccess.Web.IFeProductItemDao.Instance.GetFeProductItems(platformType, groupCode));

            return items;
        }

        /// <summary>
        /// 根据商品组系统编号获取商品
        /// 返回部分列数据
        /// </summary>
        /// <param name="groupSysNo">组系统编号</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-21 周瑜 创建</remarks>
        public IList<Model.CBFeProductItem> GetFeProductItems(int groupSysNo)
        {
            return DataAccess.Web.IFeProductItemDao.Instance.GetFeProductItems(groupSysNo);
        }
    }
}
