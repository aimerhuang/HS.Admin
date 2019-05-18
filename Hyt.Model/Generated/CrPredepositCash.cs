
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2013-10-30 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class CrPredepositCash
    {
        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }
        /// <summary>
        /// 交易号
        /// </summary>	
        [Description("交易号")]
        public string PdcTradeNo { get; set; }
        /// <summary>
        /// 外部交易号
        /// </summary>	
        [Description("外部交易号")]
        public string PdcOutTradeNo { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>	
        [Description("会员编号")]
        public int PdcUserId { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>	
        [Description("会员名称")]
        public string PdcUserName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>	
        [Description("金额")]
        public decimal PdcAmount { get; set; }
        /// <summary>
        /// 支付方式名称
        /// </summary>	
        [Description("支付方式名称")]
        public string PdcPaymentName { get; set; }
        /// <summary>
        /// 支付方式编号
        /// </summary>	
        [Description("支付方式编号")]
        public int PdcPaymentId { get; set; }
        /// <summary>
        /// 收款账号
        /// </summary>	
        [Description("收款账号")]
        public string PdcCashAccount { get; set; }
        /// <summary>
        /// 收款人姓名
        /// </summary>	
        [Description("收款人姓名")]
        public string PdcToName { get; set; }
        /// <summary>
        /// 收款银行
        /// </summary>	
        [Description("收款银行")]
        public string PdcToBank { get; set; }
        /// <summary>
        /// 会员提现备注
        /// </summary>	
        [Description("会员提现备注")]
        public string PdcUserRemark { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>	
        [Description("添加时间")]
        public DateTime PdcAddTime { get; set; }
        /// <summary>
        /// 提现支付状态 0为未支付 1为支付
        /// </summary>	
        [Description("提现支付状态 0为未支付 1为支付")]
        public int PdcPayState { get; set; }
        /// <summary>
        /// 管理员编号
        /// </summary>	
        [Description("管理员编号")]
        public int PdcAdminId { get; set; }
        /// <summary>
        /// 管理员姓名
        /// </summary>	
        [Description("管理员姓名")]
        public string PdcAdminName { get; set; }
        /// <summary>
        /// 管理员备注
        /// </summary>	
        [Description("管理员备注")]
        public string PdcAdminRemark { get; set; }
        /// <summary>
        /// 管理员和会员都可查看的备注
        /// </summary>	
        [Description("管理员和会员都可查看的备注")]
        public string PdcRemark { get; set; }
        /// <summary>
        /// 删除状态
        /// </summary>	
        [Description("PdcDeleteStatic")]
        public int PdcDeleteStatic { get; set; }
        /// <summary>
        /// 提现类型：0 会员，1 分销商
        /// </summary>	
        [Description("提现类型")]
        public int PdType { get; set; }
        /// <summary>
        /// 审核状态（0:未审核 1:已审核 -1 作废）
        /// </summary>
        public int Status{ get; set; }
    }
}

