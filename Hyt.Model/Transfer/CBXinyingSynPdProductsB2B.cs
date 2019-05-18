using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// B2B商品信息同步模型
    /// </summary>
    /// <remarks>2107-10-11 罗勤瑶创建</remarks>
    public class CBXinyingSynPdProductsB2B : PdProduct
    {
        
        /// <summary>
        /// 商品价格
        /// </summary>
        public IList<PdPrice> PdPrice { get; set; }

        /// <summary>
        /// 商品属性
        /// </summary>
        public IList<PdProductAttribute> PdProductAttribute { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        public IList<PdCategory> PdCategory { get; set; }

        /// <summary>
        /// 关联商品
        /// </summary>
        public IList<PdProductAssociation> PdProductAssociation { get; set; }

        /// <summary>
        /// 商品品牌
        /// </summary>
        public PdBrand PdBrand { get; set; }

        /// <summary>
        /// 搭配销售商品
        /// </summary>
        public IList<PdProduct> PdProductCollocation { get; set; }

        /// <summary>
        /// 搭配销售分表
        /// </summary>
        public IList<PdProductCollocation> PdProductCollocationRelation { get; set; }

        /// <summary>
        /// 产品分类关联
        /// </summary>
        public IList<PdCategoryAssociation> PdCategoryAssociation { get; set; }

        /// <summary>
        /// 产品图片
        /// </summary>
        public IList<PdProductImage> PdProductImage { get; set; }

    }
}
