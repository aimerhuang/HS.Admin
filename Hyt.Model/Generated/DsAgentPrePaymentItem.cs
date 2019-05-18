using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 代理商预存款往来账明细
    /// </summary>
    /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
    [Serializable]
    public partial class DsAgentPrePaymentItem
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 分销商预存款系统编号
        /// </summary>
        [Description("代理商预存款系统编号")]
        public int AgentPrePaymentSysNo { get; set; }
        /// <summary>
        /// 单据来源:预存款(10),冻结返回(20),退换货(30),订单消
        /// </summary>
        [Description("单据来源:预存款(10),冻结返回(20),退换货(30),订单消")]
        public int Source { get; set; }
        /// <summary>
        /// 来源编号
        /// </summary>
        [Description("来源编号")]
        public int SourceSysNo { get; set; }
        /// <summary>
        /// 增加
        /// </summary>
        [Description("增加")]
        public decimal Increased { get; set; }
        /// <summary>
        /// 减少
        /// </summary>
        [Description("减少")]
        public decimal Decreased { get; set; }
        /// <summary>
        /// 剩余
        /// </summary>
        [Description("剩余")]
        public decimal Surplus { get; set; }
        /// <summary>
        /// 状态:冻结(10),完成(20),失败(-10)
        /// </summary>
        [Description("状态:冻结(10),完成(20),失败(-10)")]
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
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