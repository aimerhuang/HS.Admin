using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBOutputPdProductsExcel
    {
        public string 自动编码 { get; set; }
        public string 商品编码 { get; set; }
        public string 前台显示名称 { get; set; }
        public string 后台显示名称 { get; set; }
        public string 分类 { get; set; }
        public string 品牌 { get; set; }
        public string 类型 { get; set; }
        public string 原产地 { get; set; }
        public string 条形码 { get; set; }
        public decimal 毛重 { get; set; }
        public decimal 税率 { get; set; }
        public decimal 直营利润比例 { get; set; }
        public decimal 直营分销商利润金额 { get; set; }
        public decimal 商品价格 { get; set; }
        public decimal 会员价 { get; set; }
        public decimal 批发价 { get; set; }
        public decimal 门店销售价 { get; set; }
        public string 商品简称 { get; set; }
    }
}
