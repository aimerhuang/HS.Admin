
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-09-16 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class ReportMarketDepartmentSale
	{
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 收件人 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 地址 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 联系电话 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public DateTime 发货时间 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 订单号 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 产品名称 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 数量 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 物流类型 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 备注 { get; set; }

 	}
}

	