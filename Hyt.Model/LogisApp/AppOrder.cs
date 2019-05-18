using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// APP使用会员信息扩展
    /// </summary>
    /// <remarks>2013-07-30 周唐炬 创建</remarks>
    [DataContract]
    public class AppOrder
    {
        /// <summary>
        /// 订单
        /// </summary>
        [DataMember]
        public SoOrder SoOrder { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        [DataMember]
        public SoReceiveAddress SoReceiveAddress { get; set; }

        /// <summary>
        /// 订购商品
        /// </summary>
        [DataMember]
        public IList<SoOrderItem> Products { get; set; }

        /// <summary>
        /// 发票
        /// </summary>
        [DataMember]
        public FnInvoice Invoice { get; set; }
    }
}
