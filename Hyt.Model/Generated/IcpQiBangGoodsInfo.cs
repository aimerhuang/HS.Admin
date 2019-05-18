using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 启邦物流商品备案信息表
    /// </summary>
    [Serializable]
    public partial class IcpQiBangGoodsInfo
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 平台编码
        /// </summary>
        [Description("平台编码")]
        public string order_source { get; set; }
        /// <summary>
        /// 商品编码（启邦提供）
        /// </summary>
        [Description("商品编码（启邦提供）")]
        public string item_id { get; set; }
        /// <summary>
        /// 商品SKU
        /// </summary>
        [Description("商品SKU")]
        public string goods_sku { get; set; }
        /// <summary>
        /// 商品货号
        /// </summary>
        [Description("商品货号")]
        public string item_code { get; set; }
        /// <summary>
        /// 客户SKU
        /// </summary>
        [Description("客户SKU")]
        public string v_goods_regist_no { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string item_name { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [Description("规格型号")]
        public string item_spec { get; set; }
        /// <summary>
        /// 备案价格
        /// </summary>
        [Description("备案价格")]
        public decimal item_price { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        [Description("毛重")]
        public decimal n_kos { get; set; }
        /// <summary>
        /// 净重
        /// </summary>
        [Description("净重")]
        public decimal netweight { get; set; }
        /// <summary>
        /// 海关备案号
        /// </summary>
        [Description("海关备案号")]
        public string cusgoodsno { get; set; }
        /// <summary>
        /// 客户代码
        /// </summary>
        [Description("客户代码")]
        public string cus_code { get; set; }
        /// <summary>
        /// 原产国
        /// </summary>
        [Description("原产国")]
        public string origincountry { get; set; }
        /// <summary>
        /// 商检商品备案号
        /// </summary>
        [Description("商检商品备案号")]
        public string ciqgoodsno { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }
    }
}
