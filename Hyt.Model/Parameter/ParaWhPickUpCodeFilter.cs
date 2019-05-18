using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 出库单查询条件
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class ParaWhPickUpCodeFilter
    {
        /// <summary>
        /// 出库单系统编号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 出库单系统编号
        /// </summary>
        public int? StockOutSysNo { get; set; }

        /// <summary>
        /// 查询类型 1、提货码 2、验证码
        /// </summary>
        public int SearchType { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int id{ get; set; }
    }
}
