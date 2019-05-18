using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.ExcelTemplate
{
    /// <summary>
    /// 产品excel导入导出表头模板基类（客户端需继承此类才可以使用）
    /// </summary>
    /// <remarks>2016-11-28 杨浩 创建</remarks>
    public class ProductTemplateBase:ITemplateBase
    {         
        /// <summary>
        /// 商品编码 
        /// </summary>
        [Description("商品编码")]
        public string ErpCode { get; set; }
        /// <summary>
        /// 前台显示名称
        /// </summary>
        [Description("前台显示名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// 后台显示名称
        /// </summary>
        [Description("后台显示名称")]
        public string EasName { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
       [Description("类别名称")]
        public string CategoryName { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Description("品牌")]
        public string BrandName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        [Description("类型")]
        public string TypeName { get; set; }
        /// <summary>
        /// 原产地
        /// </summary>
        [Description("原产地")]
        public string OriginName { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        [Description("条形码")]
        public string Barcode { get; set; }
        /// <summary>
        /// 毛重
        /// </summary>
        [Description("毛重")]
        public string GrosWeight { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        [Description("税率")]
        public string Tax { get; set; }

        /// <summary>
        /// 直营利润比例
        /// </summary>
        [Description("直营利润比例")]
        public string PriceRate { get; set; }
        /// <summary>
        /// 直营分销商利润金额
        /// </summary>
        [Description("直营分销商利润金额")]
        public string PriceValue { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        [Description("商品价格")]
        public string Price { get; set; }

        /// <summary>
        /// 会员价
        /// </summary>
        [Description("会员价")]
        public string SalePrice { get; set; }


    }
}
