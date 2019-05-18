using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 优惠券卡号筛选字段
    /// </summary>
    /// <remarks>2014-01-08 余勇 创建</remarks>
    public class ParaCouponCard
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }
       
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public int? CardTypeSysNo { get; set; }
       
        /// <summary>
        /// 开始卡号
        /// </summary>
        public string StartCardNo { get; set; }

        /// <summary>
        /// 结束卡号
        /// </summary>
        public string EndCardNo{ get; set; }
    }
}
