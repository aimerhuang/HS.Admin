using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 订单修改界面交互Model 
    /// </summary>
    /// <remarks>2013－09-25 杨文兵 创建</remarks>
    public class EditOrderModel
    {

        public SoOrder Order { get; set; }
        public IList<SoOrderItem> OrderItems { get; set; }
        public CrShoppingCart ShoppingCart { get; set; }
        public IList<JsonCartItem> JsonCartItem { get; set; }

        #region 影响购物车金额变化
        /// <summary>
        /// 收货地址区域SysNo
        /// </summary>
        public int AreaSysNo { get; set; }
        /// <summary>
        /// 订单配送方式SysNo
        /// </summary>
        public int DeliveryTypeSysNo { get; set; }
        /// <summary>
        /// 使用的优惠券Code
        /// </summary>
        public string CouponCode { get; set; }
        /// <summary>
        /// 订单促销使用的系统编号集合
        /// </summary>
        public int[] UsedPromotionSysNo { get; set; }
        #endregion
    }
}
