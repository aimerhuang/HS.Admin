using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 用于App接口的优惠卷对象
    /// </summary>
    /// <remarks>2013-12-01 沈强 创建</remarks>
    public class AppSpCoupon
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 促销系统编号
        /// </summary>
        public int PromotionSysNo { get; set; }
        /// <summary>
        /// 优惠券代码
        /// </summary>
        public string CouponCode { get; set; }
        /// <summary>
        /// 优惠券金额
        /// </summary>
        public decimal CouponAmount { get; set; }
        /// <summary>
        /// 所需消费金额
        /// </summary>
        public decimal RequirementAmount { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 优惠券类型:系统(10),私有(20)
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 来源描述
        /// </summary>
        public string SourceDescription { get; set; }
        /// <summary>
        /// 允许使用数量
        /// </summary>
        public int UseQuantity { get; set; }
        /// <summary>
        /// 已使用数量(公有类型使用时需创建私有优惠券记录)
        /// </summary>
        public int UsedQuantity { get; set; }
        /// <summary>
        /// 客户系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 状态:待审(10),已审(20),作废(-10)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public int AuditorSysNo { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// 绑定次数
        /// </summary>
        public int BindNumber { get; set; }
    }
}
