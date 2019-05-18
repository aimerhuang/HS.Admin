using System;
using System.ComponentModel;

namespace Hyt.Model
{	
    /// <summary>
    /// 运费模板项价格
    /// </summary>
    /// <remarks>2015-11-21 杨浩 创建</remarks>
    [Serializable]
    public partial class LgFreightModuleItemPrices
    {
        /// <summary>
        /// 系统编号
        /// </summary>	
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 运费模板详情编号
        /// </summary>	
        [Description("运费模板详情编号")]
        public int FreightModuleDetailsSysNo { get; set; }
        /// <summary>
        /// 首重
        /// </summary>	
        [Description("首重")]
        public decimal FirstWeight { get; set; }
        /// <summary>
        /// 首费
        /// </summary>	
        [Description("首费")]
        public decimal FirstCost { get; set; }
        /// <summary>
        /// 续重
        /// </summary>	
        [Description("续重")]
        public decimal ContinuousWeight { get; set; }
        /// <summary>
        /// 续费
        /// </summary>	
        [Description("续费")]
        public decimal ContinuousCost { get; set; }
        /// <summary>
        /// 偏差
        /// </summary>	
        [Description("偏差")]
        public decimal Offset { get; set; }
        /// <summary>
        /// 最低收费
        /// </summary>	
        [Description("最低收费")]
        public decimal LowestCost { get; set; }
        /// <summary>
        /// 服务费
        /// </summary>	
        [Description("服务费")]
        public decimal ServiceCost { get; set; }
        /// <summary>
        /// 重量下限
        /// </summary>
        [Description("重量下限")]
        public decimal MinWeight { get; set; }
        /// <summary>
        /// 重量上限
        /// </summary>
        [Description("重量上限")]
        public decimal MaxWeight { get; set; }
    }
}
