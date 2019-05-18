using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Logis.XinYi
{
    /// <summary>
    /// 订单明细
    /// </summary>
    /// <remarks>2015-10-15 杨云奕 新建</remarks>
    public class OrderDetail
    {
        /// <summary>
        /// 行号 必选
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// 进仓单编号 来源编号 必选
        /// </summary>
        public string RoNumber { get; set; }
        /// <summary>
        /// 来源货号 必选
        /// </summary>
        public string CopGNo { get; set; }
        /// <summary>
        /// 商品批次号 可选
        /// </summary>
        public string GoodsBatchNo { get; set; }
        /// <summary>
        /// Sku编码 可选
        /// </summary>
        public string SkuCode { get; set; }
        /// <summary>
        /// 颜色 可选
        /// </summary>
        public string ItSkuColor { get; set; }
        /// <summary>
        /// 尺码 可选
        /// </summary>
        public string ItSkuSize { get; set; }
        /// <summary>
        /// 数量 必选
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 销售价 必选
        /// </summary>
        public int ProPrice { get; set; }
        /// <summary>
        /// 备注 可选
        /// </summary>
        public string Remark { get; set; }
    }
}
