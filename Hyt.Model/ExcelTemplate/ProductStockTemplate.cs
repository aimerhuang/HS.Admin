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
    /// <remarks>2017-1-12 杨浩 创建</remarks>
    public class ProductStockTemplate : ITemplateBase
    {

        /// <summary>
        /// 仓库名称
        /// </summary>
        [Description("仓库名称")]
        public string BackWarehouseName{get;set;}
        /// <summary>
        /// 商品编码
        /// </summary>
        [Description("商品编码")]
        public string  ErpCode{get;set;}
        /// <summary>
        /// 后台显示名称
        /// </summary>
        [Description("后台显示名称")]
        public string  EasName{get;set;}
        /// <summary>
        /// 条形码
        /// </summary>
       [Description("条形码")]
        public string  Barcode{get;set;}
        /// <summary>
        /// 海关备案号
        /// </summary>
         [Description("海关备案号")]
       public string  CustomsNo {get;set;} 
       /// <summary>
       /// 采购价格
       /// </summary>
       [Description("采购价格")]
       public string  CostPrice {get;set;}
      /// <summary>
      /// 库存数量
      /// </summary>
      [Description("库存数量")]
      public string  StockQuantity {get;set;}

      /// <summary>
      /// 待发货
      /// </summary>
      [Description("待发货")]
      public int ProductQuantity { get; set; }
      /// <summary>
      /// "Kis库存
      /// </summary>
      [Description("Kis库存")]
      public string KisStock { get; set; }

      /// <summary>
      /// 日期
      /// </summary>
      [Description("日期")]
      public string InStockTime { get; set; }
      /// <summary>
      /// 备注
      /// </summary>
      [Description("备注")]
      public string Remark { get; set; }


    }
}
