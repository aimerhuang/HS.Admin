using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.RongEgouModel
{
    /// <summary>
    /// 商品详情查询接口访问参数
    /// </summary>
    /// <remarks>2018-03-19 罗熙 创建</remarks>
    public class RegProductlistSelectPara
    {
        /// <summary>
        /// 商品编号列表（多个订单用逗号分割）
        /// </summary>
        public string product_ids { get; set; }
    }
}
