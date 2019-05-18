using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class PdProductPrivate
    {

        /// <summary>
        /// 系统编号
        /// </summary>	
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>	
        [Description("品牌")]
        public int BrandSysNo { get; set; }
        /// <summary>
        /// 产品描述
        /// </summary>	
        [Description("产品描述")]
        public string ProductDesc { get; set; }
        /// <summary>
        /// 备注
        /// </summary>	
        [Description("备注")]
        public string ProductRemark { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>	
        [Description("联系方式")]
        public string ContactWay { get; set; }
        /// <summary>
        /// 图片
        /// </summary>	
        [Description("图片")]
        public string ProductImage { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>	
        [Description("审核意见")]
        public string AuditOpinion { get; set; }
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
        /// 审核日期
        /// </summary>	
        [Description("审核日期")]
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

    }
}