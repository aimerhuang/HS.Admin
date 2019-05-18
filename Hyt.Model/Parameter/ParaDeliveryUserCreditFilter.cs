using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 配送员信用查询筛选字段
    /// </summary>
    /// <remarks>
    /// 2013-07-04 郑荣华 创建
    /// </remarks>
    public class ParaDeliveryUserCreditFilter
    {

        /// <summary>
        /// 配送员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否允许借货：是（1）、否（0）
        /// </summary>
        public int? IsAllowBorrow { get; set; }

        /// <summary>
        /// 是否允许配送：是（1）、否（0）
        /// </summary>
        public int? IsAllowDelivery { get; set; }

        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }
   
        /// <summary>
        /// 页码
        /// </summary>
        public int? CurrentPage { get; set; }

        
    }
}
