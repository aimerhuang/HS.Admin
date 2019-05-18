using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 配送单打印实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class PrtDelivery :LgDelivery
    {
        /// <summary>
        /// 明细列表
        /// </summary>
        public IList<PrtSubDelivery> List;
    }

    /// <summary>
    /// 配送单打印明细实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class PrtSubDelivery : LgDeliveryItem
    {
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 收货地区编号
        /// </summary>
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 收货街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 收货人固定电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderSysNo { get; set; }
    }
}
