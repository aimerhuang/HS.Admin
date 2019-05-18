
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class LgDelivery
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 仓库编号
		/// </summary>
		[Description("仓库编号")]
		public int StockSysNo { get; set; }
 		/// <summary>
		/// 配送员编号
		/// </summary>
		[Description("配送员编号")]
		public int DeliveryUserSysNo { get; set; }
 		/// <summary>
		/// 预付金额
		/// </summary>
		[Description("预付金额")]
		public decimal PaidAmount { get; set; }
 		/// <summary>
		/// 到付金额
		/// </summary>
		[Description("到付金额")]
		public decimal CODAmount { get; set; }
 		/// <summary>
		/// 状态：待配送（10）、配送在途（20）、已结算（30）、
		/// </summary>
		[Description("状态：待配送（10）、配送在途（20）、已结算（30）、")]
		public int Status { get; set; }
 		/// <summary>
		/// 是否在额度不够时强制放行：是（1）、否（0）
		/// </summary>
		[Description("是否在额度不够时强制放行：是（1）、否（0）")]
		public int IsEnforceAllow { get; set; }
 		/// <summary>
		/// 配送方式
		/// </summary>
		[Description("配送方式")]
		public int DeliveryTypeSysNo { get; set; }
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
 		/// <summary>
		/// 时间戳
		/// </summary>
		[Description("时间戳")]
		public DateTime Stamp { get; set; }
 	}
    ///第三方快递配置定义表
    ///2015-09-30 杨云奕添加。快递信息配置表
    public class ThirdDeliveryConfig
    {

        /// <summary>
        /// 快递ApiID
        /// </summary>		

        public string tdc_ApiID
        {
            get;
            set;
        }
        /// <summary>
        /// 快递Api密钥
        /// </summary>		

        public string tdc_Sceret
        {
            get;
            set;
        }
        /// <summary>
        /// 快递第三方URL连接
        /// </summary>		

        public string tdc_ThirdUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 开关
        /// </summary>		

        public bool tdc_bOpen
        {
            get;
            set;
        }

        /// <summary>
        /// 未付款订单作废时间
        /// </summary>		
        public int OrderTimeOut
        {
            get;
            set;
        }

        public int SysNo
        {
            get;
            set;
        }

    }
}

	