using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    /// <summary>
    /// 包裹信息管理明细
    /// </summary>
    /// <remarks>
    /// 2016-5-16 杨云奕 添加
    /// </remarks>
    public class DsWhGoodsManagementList
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 父表编号
        /// </summary>
        public int PSysNo { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string GoodsCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品品牌名称
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 商品单位
        /// </summary>
        public string GoodsUnit { get; set; }
        /// <summary>
        /// 商品销售数量
        /// </summary>
        public int Quantiyt { get; set; }
        /// <summary>
        /// 商品实际售价
        /// </summary>
        public decimal GoodsPrice { get; set; }
        public string Dis { get; set; }
        public decimal GoodsWeight { get; set; }

        public string GoodsPostTax { get; set; }

        public string GoodsSpec { get; set; }

        public string GoodsCusCode { get; set; }
    }
}
