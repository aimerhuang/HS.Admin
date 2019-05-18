
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
	public partial class LgDeliveryUserLocation
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 配送员编号
		/// </summary>
		[Description("配送员编号")]
		public int DeliveryUserSysNo { get; set; }
 		/// <summary>
		/// 纬度
		/// </summary>
		[Description("纬度")]
		public decimal Latitude { get; set; }
 		/// <summary>
		/// 经度
		/// </summary>
		[Description("经度")]
		public decimal Longitude { get; set; }
 		/// <summary>
		/// 定位时间
		/// </summary>
		[Description("定位时间")]
		public DateTime GpsDate { get; set; }
 		/// <summary>
		/// 定位类型编码:Gps(10),基站(20)
		/// </summary>
		[Description("定位类型编码:Gps(10),基站(20)")]
		public int LocationType { get; set; }
 		/// <summary>
		/// 定位误差
		/// </summary>
		[Description("定位误差")]
		public decimal Radius { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 	}
}

	