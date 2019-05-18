
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
    public partial class CrSsoCustomerAssociation
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
        /// SSO系统编号
		/// </summary>
		[Description("SSO系统编号")]
		public int SsoId { get; set; }
 		/// <summary>
		/// 客户编号
		/// </summary>
        [Description("客户编号")]
        public int CustomerSysNo { get; set; }
 	}
}

	