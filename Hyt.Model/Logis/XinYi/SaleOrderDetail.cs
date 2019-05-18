using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Logis.XinYi
{
    /// <summary>
    /// 订单明细
    /// </summary>
    /// <remarks>2015-10-14 杨浩 创建</remarks>
    [Serializable]
    public class SaleOrderDetail
    {
        /// <summary>
        /// 行号(必选)
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// 来源编号
        /// </summary>
        public string SoNumber { get; set; }
        /// <summary>
        /// 订进仓单编号(来源编号)((必选)
        /// </summary>
        public string RoNumber { get; set; }
        /// <summary>
        /// 来源货号(电商平台唯一编码)(必选)
        /// </summary>
        public string CopGNo { get; set; }
        /// <summary>
        /// 商品批次号
        /// </summary>
        public string GoodsBatchNo { get; set; }

        /// <summary>
        /// Sku编码(必选)
        /// </summary>
        public string SkuCode { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string ItSkuColor { get; set; }
        /// <summary>
        /// 尺码
        /// </summary>
        public string ItSkuSize { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// 销售价(以分为单位，整形)(必选)
        /// </summary>
        public int ProPrice{ get; set; }
    }
}
