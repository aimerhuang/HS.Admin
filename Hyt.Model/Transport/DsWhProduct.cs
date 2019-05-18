using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class CBDsWhProduct:DsWhProduct
    {
        public bool IsAllDealer { get; set; }
        public bool IsDealer { get; set; }
        public bool IsCustomer { get; set; }

        public int goodsNum { get; set; }
    }

    /// <summary>
    /// 客户商品档案
    /// </summary>
    /// <remarks>
    /// 2016-5-16 杨云奕 添加
    /// </remarks>
    public class DsWhProduct
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int DsSysNo { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerCode { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProCode { get; set; }
        /// <summary>
        /// 商品品牌名称
        /// </summary>
        public string BrandName { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 商品税码
        /// </summary>
        public string HsCode { get; set; }
        /// <summary>
        /// 商品英文名称
        /// </summary>
        public string ProductEName { get; set; }
        /// <summary>
        /// 商品规格
        /// </summary>
        public string ProSpec { get; set; }
        /// <summary>
        /// 商品单位
        /// </summary>
        public string ProUnit { get; set; }
        /// <summary>
        /// 商品重量
        /// </summary>
        public decimal ProWeight { get; set; }
        /// <summary>
        /// 商品产地
        /// </summary>
        public string ProOrigin { get; set; }
        /// <summary>
        /// 商品销售价格
        /// </summary>
        public decimal ProPrice { get; set; }
        /// <summary>
        /// 商品货币
        /// </summary>
        public string Currency { get; set; }
    }
}
