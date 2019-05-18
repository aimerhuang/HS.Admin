
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    public class CBDsDealerWharehouse:Hyt.Model.DsDealerWharehouse
    {
        public string DealerName { get; set; }
        public string WarehouseName { get; set; }
    }
    /// <summary>
    /// 分销商仓库关系
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
    public partial class DsDealerWharehouse
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商系统编号
		/// </summary>
		[Description("分销商系统编号")]
		public int DealerSysNo { get; set; }
 		/// <summary>
		/// 仓库系统编号
		/// </summary>
        [Description("仓库系统编号")]
        public int WarehouseSysNo { get; set; }
 	}
}

	