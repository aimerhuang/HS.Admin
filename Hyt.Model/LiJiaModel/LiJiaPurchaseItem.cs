using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 利嘉入库单同步模型明细
    /// </summary>
    /// <remarks>2017-5-18 罗勤尧 创建</remarks>
   public class LiJiaPurchaseItem
    {
        /// <summary>
        /// ERP商品编码
        /// </summary>
       public string PluNo { get; set; }
        /// <summary>
        /// 第三方商品编码
        /// </summary>
        public string GPluNo { get; set; }
        /// <summary>
        ///商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        ///入库数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 单价(人民币)
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 商品总价(人民币)
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 外币币种CNY=人民币,USD=美元,EUR=欧元,GBP=英镑,HKD=港元,JPY=日元,NZD=纽币,AUD=澳币填写币种英文标识,如:USD
        /// </summary>
        public string ForeignCurency { get; set; }

        /// <summary>
        /// 汇率(外币对人民币汇率)
        /// </summary>
        public float ForeignRate { get; set; }
        /// <summary>
        /// 外币商品总价
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 外币单价
        /// </summary>
        public decimal ForeignPrice { get; set; }
    }
}
