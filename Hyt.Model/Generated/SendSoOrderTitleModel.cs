
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2013-09-13 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class SendSoOrderTitleModel
    {
        /// <summary>
        /// 商家发货快递公司编号
        /// </summary>
        [Description("商家发货快递公司编号")]
        public string OverseaCarrier { get; set; }
        /// <summary>
        /// 商家发货快递编号
        /// </summary>
        [Description("商家发货快递编号")]
        public string OverseaTrackingNo { get; set; }
        /// <summary>
        /// 海外仓库编号
        /// </summary>
        [Description("海外仓库编号")]
        public string WarehouseId { get; set; }
        /// <summary>
        /// 客户关联单号
        /// </summary>
        [Description("客户关联单号")]
        public string CustomerReference { get; set; }
        /// <summary>
        /// 来件商家
        /// </summary>
        [Description("来件商家")]
        public string MerchantName { get; set; }
        /// <summary>
        /// 来件商家单号
        /// </summary>
        [Description("来件商家单号")]
        public string MerchantOrderNo { get; set; }
        /// <summary>
        /// 来件包裹面单上的收件人firstname
        /// </summary>
        [Description("来件包裹面单上的收件人firstname")]
        public string ConsigneeFirstName { get; set; }
        /// <summary>
        /// 来件包裹面单上的收件人lasttname
        /// </summary>
        [Description("来件包裹面单上的收件人lasttname")]
        public string ConsigneeLastName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
    }
}

