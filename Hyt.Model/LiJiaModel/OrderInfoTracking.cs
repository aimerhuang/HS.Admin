using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LiJiaModel
{
    /// <summary>
    /// 利嘉出库同步模型
    /// </summary>
    /// <remarks>2017-5-18 罗勤尧 创建</remarks>
    public class OrderInfoTracking
    {
        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string SourceOrderNo { get; set; }
        /// <summary>
        /// 仓库编码(由仓库接口返回)
        /// </summary>
        public string StoreCode { get; set; }
        /// <summary>
        ///物流单号
        /// </summary>
        public string TrackingNo { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceiptAddress { get; set; }
        /// <summary>
        /// 物流公司
        /// </summary>
        public string LogisticsServiceName { get; set; }
        /// <summary>
        ///发货时间
        /// </summary>
        public string OrderShippingTime { get; set; }
    }
}
