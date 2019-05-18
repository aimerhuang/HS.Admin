using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.RongEgouModel
{
    /// <summary>
    /// 订单列表查询接口请求报文（N为非必填）
    /// </summary>
    /// <remarks>2018-03-19 罗熙 创建</remarks>
    public class RegOrderSelectPara
    {
        /// <summary>
        /// 下单时间的起始值（N）
        /// </summary>
        public string create_start_time { get; set; }

        /// <summary>
        /// 下单时间的结束值（N）
        /// </summary>
        public string create_end_time { get; set; }

        /// <summary>
        /// 订单更新时间的起始值（N）
        /// </summary>
        public string modify_time_from { get; set; }

        /// <summary>
        /// 订单更新时间的结束值（N）
        /// </summary>
        public string modify_time_to { get; set; }

        /// <summary>
        /// 订单状态（N）
        /// </summary>
        public int order_status { get; set; }
    }
}
