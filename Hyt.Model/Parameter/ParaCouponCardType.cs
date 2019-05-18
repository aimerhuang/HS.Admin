using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
   /// <summary>
   /// 优惠卡类型筛选
   /// </summary>
   /// <remarks>2014-01-08 朱成果 创建</remarks>
    public class ParaCouponCardType
    {
        /// <summary>
        /// 优惠卡状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 优惠卡类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 当前页号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
