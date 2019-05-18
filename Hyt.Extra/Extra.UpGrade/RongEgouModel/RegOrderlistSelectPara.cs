using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.RongEgouModel
{
    /// <summary>
    /// 订单详情查询接口请求报文
    /// </summary>
    /// <remarks>2018-03-19 罗熙 创建</remarks>
    public class RegOrderlistSelectPara
    {
        /// <summary>
        /// 订单ID列表（多个订单以逗号分隔）
        /// </summary>
        public string order_ids { get; set; }
    }
}
