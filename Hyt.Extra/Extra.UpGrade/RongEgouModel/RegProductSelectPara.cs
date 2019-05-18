using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.RongEgouModel
{
    /// <summary>
    /// 商品列表查询接口访问参数（N为非必填）
    /// </summary>
    /// <remarks>2018-03-19 罗熙 创建</remarks>
    public class RegProductSelectPara
    {
        /// <summary>
        /// 商品录入时起始时间（N）
        /// </summary>
        public string create_start_time { get; set; }

        /// <summary>
        /// 商品录入时结束时间（N）
        /// </summary>
        public string create_end_time { get; set; }

        /// <summary>
        /// 商品更新时起始时间（N）
        /// </summary>
        public string modify_time_from { get; set; }

        /// <summary>
        /// 商品更新时结束时间（N）
        /// </summary>
        public string modify_time_to { get; set; }

        /// <summary>
        /// 商品上架时的起始时间（N）
        /// </summary>
        public string put_time_from { get; set; }

        /// <summary>
        /// 商品上架时的结束时间（N）
        /// </summary>
        public string put_time_to { get; set; }

        /// <summary>
        /// 商品状态,空默认全部状态（N）
        /// </summary>
        public string product_status { get; set; }
    }
}
