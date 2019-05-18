using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.ExcelTemplate
{
    /// <summary>
    /// 产品库存excel导入导出表头模板基类（客户端需继承此类才可以使用）
    /// </summary>
    /// <remarks>2017-6-23 罗勤尧 创建</remarks>
    public class ProductStockTemplateSnDate : ITemplateBase
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Description("后台仓库名称")]
        public string BackWarehouseName { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        [Description("商品编码")]
        public string ErpCode { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [Description("日期")]
        public string DateTime { get; set; }
        
    }
}
