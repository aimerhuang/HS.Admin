using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// APP订单扩展类
    /// </summary>
    /// <remarks>2014-09-18 杨文兵 创建 用于APP创建二次销售调价的订单</remarks>
    [DataContract]
    public class AppOrder2
    {
        /// <summary>
        /// 订单
        /// </summary>
        [DataMember]
        public AppShopCartOrder Order { get; set; }

        /// <summary>
        /// 客户地址
        /// </summary>
        [DataMember]
        public SoReceiveAddress SoReceiveAddress { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        [DataMember]
        public IList<AppOrderItem> OrderItems { get; set; }
    }

    /// <summary>
    /// APP订单明细扩展类
    /// </summary>
    [DataContract]
    public class AppOrderItem
    {
        /// <summary>
        /// 商品SysNo
        /// </summary>
        [DataMember]
        public int SysNo { get; set; }

        /// <summary>
        /// 自定义调价之后的价格
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        [DataMember]
        public int Quantity { get; set; }
    }
}
