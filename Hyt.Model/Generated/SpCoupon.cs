using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2014-01-06 杨浩 T4生成
    /// </remarks>
    [Serializable]
    [DataContract]
    public partial class SpCoupon
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        [DataMember]
        public int SysNo { get; set; }
        /// <summary>
        /// 促销系统编号
        /// </summary>
        [Description("促销系统编号")]
        [DataMember]
        public int PromotionSysNo { get; set; }
        /// <summary>
        /// 父级系统编号
        /// </summary>
        [Description("父级系统编号")]
        [DataMember]
        public int ParentSysNo { get; set; }
        /// <summary>
        /// 优惠券代码
        /// </summary>
        [Description("优惠券代码")]
        [DataMember]
        public string CouponCode { get; set; }
        /// <summary>
        /// 优惠券金额
        /// </summary>
        [Description("优惠券金额")]
        [DataMember]
        public decimal CouponAmount { get; set; }
        /// <summary>
        /// 所需消费金额
        /// </summary>
        [Description("所需消费金额")]
        [DataMember]
        public decimal RequirementAmount { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Description("开始时间")]
        [DataMember]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Description("结束时间")]
        [DataMember]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 优惠券类型:系统(10),私有(20)
        /// </summary>
        [Description("优惠券类型:系统(10),私有(20)")]
        public int Type { get; set; }
        /// <summary>
        /// 网站使用:是(1),否(0)
        /// </summary>
        [Description("网站使用:是(1),否(0)")]
        public int WebPlatform { get; set; }
        /// <summary>
        /// 门店使用:是(1),否(0)
        /// </summary>
        [Description("门店使用:是(1),否(0)")]
        public int ShopPlatform { get; set; }
        /// <summary>
        /// 手机商城使用:是(1),否(0)
        /// </summary>
        [Description("手机商城使用:是(1),否(0)")]
        public int MallAppPlatform { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Description("描述")]
        [DataMember]
        public string Description { get; set; }
        /// <summary>
        /// 来源描述
        /// </summary>
        [Description("来源描述")]
        [DataMember]
        public string SourceDescription { get; set; }
        /// <summary>
        /// 允许使用数量
        /// </summary>
        [Description("允许使用数量")]
        [DataMember]
        public int UseQuantity { get; set; }
        /// <summary>
        /// 已使用数量(公有类型使用时需创建私有优惠券记录)
        /// </summary>
        [Description("已使用数量(公有类型使用时需创建私有优惠券记录)")]
        [DataMember]
        public int UsedQuantity { get; set; }
        /// <summary>
        /// 客户系统编号
        /// </summary>
        [Description("客户系统编号")]
        [DataMember]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 是否优惠卡:是(1),否(0)
        /// </summary>
        [Description("是否优惠卡:是(1),否(0)")]
        public int IsCouponCard { get; set; }
        /// <summary>
        /// 状态:待审(10),已审(20),作废(-10)
        /// </summary>
        [Description("状态:待审(10),已审(20),作废(-10)")]
        public int Status { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        [Description("审核人")]
        public int AuditorSysNo { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        [Description("审核时间")]
        public DateTime AuditDate { get; set; }
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
        /// 物流App使用:是(1),否(0)
        /// </summary>
        [Description("物流App使用")]
        public int LogisticsAppPlatform { get; set; }
        /// <summary>
        /// 代理商编号（-1为所以代理商可用）
        /// </summary>
        [Description("代理商编号")]
        public int AgentSysNo { get; set; }
        /// <summary>
        /// 分销商编号（-1为所以分销商可用）
        /// </summary>
        [Description("分销商编号")]
        public int DealerSysNo { get; set; }
    }
}

