
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class LgFreightModule
    {
        /// <summary>
        /// 系统编号
        /// </summary>	
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 模板代码
        /// </summary>	
        [Description("模板代码")]
        public string ModuleCode { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>	
        [Description("模板名称")]
        public string ModuleName { get; set; }
        /// <summary>
        /// 商品地址
        /// </summary>	
        [Description("商品地址")]
        public int ProductAddress { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>	
        [Description("发货时间")]
        public string DeliveryTime { get; set; }
        /// <summary>
        /// 是否包邮
        /// </summary>	
        [Description("是否包邮")]
        public int IsPost { get; set; }
        /// <summary>
        /// 计价方式
        /// </summary>	
        [Description("计价方式")]
        public int ValuationStyle { get; set; }
        /// <summary>
        /// 运送方式-快递
        /// </summary>	
        [Description("运送方式-快递")]
        public int Express { get; set; }
        /// <summary>
        /// 运送方式-EMS
        /// </summary>	
        [Description("运送方式-EMS")]
        public int EMS { get; set; }
        /// <summary>
        /// 运送方式-平邮
        /// </summary>	
        [Description("运送方式-平邮")]
        public int SurfaceMail { get; set; }
        /// <summary>
        /// 状态
        /// </summary>	
        [Description("状态")]
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
        /// 最后更新时间
        /// </summary>	
        [Description("是否保价")]
        public int IsValuation { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>	
        [Description("报价费率")]
        public decimal ValuationRate { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>	
        [Description("报价上限")]
        public decimal ValuationMaxValue { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>	
        [Description("是否开启重量换算功能")]
        public int IsTurnVtW { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>	
        [Description("换算类型")]
        public string TurnVtWType { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>	
        [Description("换算公式")]
        public string TurnVtWFormula { get; set; }
    }
}

