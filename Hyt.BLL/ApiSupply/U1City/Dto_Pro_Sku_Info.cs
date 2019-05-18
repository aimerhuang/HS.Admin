using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiSupply.U1City
{
    [Serializable]
    public partial class Dto_Pro_Sku_Info
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProTitle { get; set; }
        /// <summary>
        /// 商品货号
        /// </summary>
        public string ProNo { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string ProBrand { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string ProClass { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal ProWeight { get; set; }
        /// <summary>
        /// 商品小图
        /// </summary>
        public string ProSimg { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string ProRemark { get; set; }
        /// <summary>
        /// 上架情况（0、销售中；1、下架；）
        /// </summary>
        public int ProSale { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Pro_Unit { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        public decimal ProTagPrice { get; set; }
        /// <summary>
        /// 分销价
        /// </summary>
        public decimal ProFxPrice { get; set; }
        /// <summary>
        /// 网上零售价
        /// </summary>
        public decimal ProRetPrice { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int ProCount { get; set; }
        /// <summary>
        /// 商品库存信息
        /// </summary>
        public List<ProductSpecs> ProductSpec { get; set; }
    }
}
