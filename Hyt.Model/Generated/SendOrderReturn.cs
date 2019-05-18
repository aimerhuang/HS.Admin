
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 推送订单返回信息表
	/// </summary>
    /// <remarks>
    /// 2015-09-02 王耀发 T4生成
    /// </remarks>
	[Serializable]
    public partial class SendOrderReturn
	{

        /// <summary>
        /// 系统编号
        /// </summary>	
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 发货快递公司编号
        /// </summary>	
        [Description("发货快递公司编号")]
        public string OverseaCarrier { get; set; }
        /// <summary>
        /// 发货快递单号编号
        /// </summary>	
        [Description("发货快递单号编号")]
        public string OverseaTrackingNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>	
        [Description("订单编号")]
        public int soOrderSysNo { get; set; }
        /// <summary>
        /// 返回标示
        /// </summary>	
        [Description("返回标示")]
        public string Code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>	
        [Description("返回信息")]
        public string Msg { get; set; }
        /// <summary>
        /// 返回订单号
        /// </summary>	
        [Description("返回订单号")]
        public string OrderNo { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>	
        [Description("创建人")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>	
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>	
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>	
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }     
 	}
}

	