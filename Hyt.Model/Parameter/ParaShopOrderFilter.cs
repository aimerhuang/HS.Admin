using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 门店订单筛选字段
    /// </summary>
    /// <remarks>2013-06-24 余勇 创建</remarks>
    public class ParaShopOrderFilter
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 会员姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 会员会员号
        /// </summary>
        public string CustomerSysNo { get; set; }

        /// <summary>
        /// 门店编号
        /// </summary>
        public int StoreSysNo { get; set; }

        /// <summary>
        /// 配送类型
        /// </summary>
        public int DeliveryTypeSysNo { get; set; }

        /// <summary>
        /// 订单创建日(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 订单创建日(止)
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 当前页号
        /// </summary>
        public int CurrentPage { get; set; }
    }
}
