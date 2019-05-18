using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 前台商品信息
    /// </summary>
    /// <remarks>2013-08-14 邵斌 创建</remarks>
    public abstract class IPdProductDao : DaoBase<IPdProductDao>
    {
        /// <summary>
        /// 获取同类商品的前5个商品
        /// </summary>
        /// <param name="categorySysNo">商品分类系统编号</param>
        /// <param name="excludeProductSysNo">商品系统编号:为避免推荐时自身又出现在推荐列表中，若有冗余排除</param>
        /// <param name="topNum">前N个商品</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-07 邵斌 创建</remarks>
        public abstract IList<CBPdProductDetail> GetProductFromCategory(int categorySysNo, int excludeProductSysNo, int topNum);

        /// <summary>
        /// 获取好评的商品
        /// </summary>
        /// <param name="excludeProductSysNo">要排除的商品系统编号</param>
        /// <param name="topNum">前N个商品</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-07 邵斌 创建</remarks>
        public abstract IList<CBPdProductDetail> GetBestProductComment(int excludeProductSysNo, int topNum);

        /// <summary>
        /// 购买了同一商品的人还买了其他那些商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="customerLevelSysNo">会员等级系统编号</param>
        /// <param name="recordNum">记录数</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-08 邵斌 创建</remarks>
        public abstract IList<CBPdProductDetail> GetOtherBuy(int productSysNo, int customerLevelSysNo, int recordNum);

        /// <summary>
        /// 读取商品的属性相关内容
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回商品属性列表</returns>
        /// <remarks>2013-08-08 邵斌 创建</remarks>
        public abstract IList<CBPdProductAtttributeReadRelation> GetProductAttributeInfo(int productSysNo);

        /// <summary>
        /// 根据商品系统编号获取商品信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-08-14 邵斌 创建</remarks>
        public abstract CBSimplePdProduct GetProduct(int productSysNo);


        public abstract CBSimplePdProduct GetProductErpCode(string erpCode, string Barcode);
        
        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-9-12 杨浩 创建</remarks>
        public abstract PdProduct GetProductInfo(int productSysNo);

        /// <summary>
        /// 获取商品默认图片
        /// </summary>
        /// <param name="productSysno">商品编号</param>
        /// <returns>返回图片路径</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public abstract string GetImageDefaultImg(int productSysno);

        /// <summary>
        /// 读取商品关联属性值
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回商品的关联属性以及值列表</returns>
        /// <remarks>2013-08-16 邵斌 创建</remarks>
        public abstract IList<PdProductAttribute> GetProductAssociationAttributeValue(int productSysNo);

        /// <summary>
        /// 根据商品系统编号读取搭配销售商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号（搭配销售主商品）</param>
        /// <returns>返回商品列表</returns>
        /// <remarks>2013-09-10 邵斌 创建</remarks>
        public abstract IList<CBSimplePdProduct> GetProductCollocationListByProductSysNo(int productSysNo);

        /// <summary>
        /// 根据主商品系统编号获取组合套餐商品列表
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>返回商品列表</returns>
        /// <remarks>2013-09-10 邵斌 创建</remarks>
        public abstract IList<CBWebSpComboItem> GetSpComboByProductSysNo(int productSysNo);

        /// <summary>
        /// 获取商品静态统计数据
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>商品静态统计数据</returns>
        /// <remarks>2013-12-26 黄波 创建</remarks>
        public abstract PdProductStatistics GetProductStatistics(int productSysNo);

        /// <summary>
        /// 更新商品销售数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public abstract void UpdateProductSales(int productSysNo, int accelerate);

        /// <summary>
        /// 批量更新商品销售数量
        /// </summary>
        /// <param name="saleList">商品数据集合商品数据集合(key:productSysNo value:sales)</param>
        /// <remarks>2013-11-4 黄波 创建</remarks>
        public abstract void UpdateProductSales(IDictionary<int, int> saleList);

        /// <summary>
        /// 更新商品喜欢数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public abstract void UpdateProductLiking(int productSysNo, int accelerate);

        /// <summary>
        /// 更新商品收藏数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public abstract void UpdateProductFavorites(int productSysNo, int accelerate);

        /// <summary>
        /// 更新商品评论数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="score">评分</param>
        /// <param name="accelerate">增加的数量</param>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public abstract void UpdateProductComments(int productSysNo, int score, int accelerate);

        /// <summary>
        /// 更新商品晒单数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public abstract void UpdateProductShares(int productSysNo, int accelerate);

        /// <summary>
        /// 更新商品咨询数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public abstract void UpdateProductQuestion(int productSysNo, int accelerate);

        /// <summary>
        /// 更新商品浏览量
        /// </summary>
        /// <param name="SysNo">产品编号</param>
        /// <param name="accelerate">修改的数量</param>
        /// <returns></returns>
        /// <remarks>2016-03-02 罗远康 创建</remarks>
        public abstract int UPdatePdProductViewCount(int SysNo, int accelerate);

        /// <summary>
        /// 模糊查询商品
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public abstract List<CBSimplePdProduct> GetUtilLikePdProduct(string KeyWord);


        /// <summary>
        /// 根据商品代码查询商品
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public abstract CBSimplePdProduct GetUtilLikePdProductCode(string KeyWord);
       
    }
}
