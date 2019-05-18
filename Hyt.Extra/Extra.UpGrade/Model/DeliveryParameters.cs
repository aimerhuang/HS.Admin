using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 发货参数
    /// </summary>
    /// <remarks>2013-12-31 陶辉 创建</remarks>
    public class DeliveryParameters
    {
        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string MallOrderId { get; set; }

        /// <summary>
        /// [商城]物流单号
        /// </summary>
        public string HytExpressNo { get; set; }

        /// <summary>
        /// 物流公司代码
        /// (自有物流可以直接填写物流名称)
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 获取订单查询的属性
        /// </summary>
        public OrderParameters OrderParam { get; set; }
    }
}
