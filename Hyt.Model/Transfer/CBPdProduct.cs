using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 商品基础信息 扩展
    /// </summary>
    /// <remarks>2013-06-25 黄波 创建</remarks>
    [Serializable]
    public class CBPdProduct : PdProduct
    {
        /// <summary>
        /// 商品品牌
        /// </summary>
        /// <remarks>2013-07-18 邵斌 创建</remarks>
        public string BrandName { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public Lazy<IList<PdPrice>> PdPrice { get; set; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public Lazy<IList<PdProductAttribute>> PdProductAttribute { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        public Lazy<IList<PdCategory>> PdCategory { get; set; }

        /// <summary>
        /// 关联商品
        /// </summary>
        public Lazy<IList<PdProductAssociation>> PdProductAssociation { get; set; }

        /// <summary>
        /// 商品品牌
        /// </summary>
        public Lazy<PdBrand> PdBrand { get; set; }

        /// <summary>
        /// 搭配销售商品
        /// </summary>
        public Lazy<IList<PdProduct>> PdProductCollocation { get; set; }

        /// <summary>
        /// 搭配销售分表
        /// </summary>
        public Lazy<IList<PdProductCollocation>> PdProductCollocationRelation { get; set; }

        /// <summary>
        /// 产品分类关联
        /// </summary>
        public Lazy<IList<PdCategoryAssociation>> PdCategoryAssociation { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        public Lazy<IList<PdProductImage>> PdProductImage { get; set; }

        /// <summary>
        /// 商品评分
        /// </summary>
        public Lazy<decimal> ProductCommentScore { get; set; }

        /// <summary>
        /// 商品评分总和
        /// </summary>
        public Lazy<decimal> ProductCommentScoreTotal { get; set; }

        /// <summary>
        /// 商品评论次数
        /// </summary>
        public Lazy<int> CommentTimesCount { get; set; }

        /// <summary>
        /// 商品组ID
        /// </summary>
        /// <remarks>2015-10-28 王耀发 创建</remarks>
        public string GroupSysNoList { get; set; }

        /// <summary>
        /// 商品组名
        /// </summary>
        /// <remarks>2015-10-28 王耀发 创建</remarks>
        public string GroupName { get; set; }

        /// <summary>
        /// 商检商品编号
        /// </summary>
        public string CIQGoodsNo { get; set; }
        /// <summary>
        /// 商品海关编号
        /// </summary>
        public string CUSGoodsNo { get; set; }
    }
}
