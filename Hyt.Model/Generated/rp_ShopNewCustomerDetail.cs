using System;
namespace Hyt.Model
{
    /// <summary>
    /// RP_绩效_门店新增会员明细
	/// </summary>
    /// <remarks>
    /// 2014-1-8 黄志勇 创建
    /// </remarks>
	[Serializable]
	public class rp_ShopNewCustomerDetail : BaseEntity
	{
        /// <summary>
        /// 客户编号
        /// </summary>
        public int CustomerSysno { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public int WarehouseSysNo { get; set; }

		/// <summary>
		/// 仓库
		/// </summary>
        public string Warehousename { get; set; }

        /// <summary>
        /// 内勤编号
        /// </summary>
        public int IndoorStaffSysNo { get; set; }
 
		/// <summary>
		/// 内勤姓名
		/// </summary>
        public string IndoorStaffName { get; set; }
 
		/// <summary>
		/// 客户姓名
		/// </summary>
        public string CustomerName { get; set; }
 
		/// <summary>
        /// 客户手机
		/// </summary>
        public string MobilePhoneNumber { get; set; }
 
		/// <summary>
        /// 消费金额
		/// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterDate { get; set; }
 	}
}

	