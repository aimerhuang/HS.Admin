using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;

namespace Hyt.Model
{
    /// <summary>
    /// 商品信息扩展类
    /// </summary>
    /// <remarks>
    /// 2013-07-15 唐永勤 创建
    /// 2013-08-08 邵  斌 添加价格名称
    /// </remarks>
    public class CBPdProductDetail : PdProduct
    {
        /// <summary>
        /// 商品基础价格
        /// </summary>
        public decimal BasicPrice { get; set; }

        /// <summary>
        /// 商品销售
        /// </summary>
        public decimal SalesPrice { get; set; }

        /// <summary>
        /// 商品分类编号
        /// </summary>
        public int ProductCategorySysno { get; set; }

        /// <summary>
        /// 商品分类名称
        /// </summary>
        public string ProductCategoryName { get; set; }

        /// <summary>
        /// 价格名称
        /// </summary>
        public string PriceName { get; set; }

        /// <summary>
        /// 商品销量
        /// </summary>
        public int ProductSalesNum { get; set; }
        /// <summary>
        /// 商品关联编号
        /// </summary>
        public int PAssociationSysNo { get; set; }
        /// <summary>
        /// 特殊价格商品编号
        /// </summary>
        public int SpecialPriceSysNo { get; set; }
        /// <summary>
        /// 分销商商品价格
        /// </summary>
        public decimal spSalesPrice { get; set; }
        /// <summary>
        /// 线下门店商品价格
        /// </summary>
        public decimal spShopPrice { get; set; }
        /// <summary>
        /// 线下批发商品价格
        /// </summary>
        public decimal spWholesalePrice { get; set; }
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }
        /// <summary>
        /// 总部商品状态
        /// </summary>
        public int MainStatus { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProSysNo { get; set; }

        /// <summary>
        /// 仓库数量
        /// </summary>
        public int WareNum { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 国家名称
        /// </summary>
        public string OriginName { get; set; }
    }
}
