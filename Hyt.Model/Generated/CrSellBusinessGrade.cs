
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
    public partial class CrSellBusinessGrade
	{

        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }
        /// <summary>
        /// 等级名称
        /// </summary>	
        [Description("等级名称")]
        public string Name { get; set; }
        /// <summary>
        /// 最低升级条件
        /// </summary>	
        [Description("最低升级条件")]
        public int MinCondition { get; set; }
        /// <summary>
        /// 等级升级上限
        /// </summary>	
        [Description("等级升级上限")]
        public int MaxCondition { get; set; }
        /// <summary>
        /// 直接推荐人返利比例
        /// </summary>	
        [Description("直接推荐人返利比例")]
        public decimal Direct { get; set; }
        /// <summary>
        /// 间1返利比例
        /// </summary>	
        [Description("间1返利比例")]
        public decimal Indirect1 { get; set; }
        /// <summary>
        /// 间2返利比例
        /// </summary>	
        [Description("间2返利比例")]
        public decimal Indirect2 { get; set; }
        /// <summary>
        /// 创建人系统编号
        /// </summary>	
        [Description("创建人系统编号")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>	
        [Description("创建日期")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 最后更新人
        /// </summary>	
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新日期
        /// </summary>	
        [Description("最后更新日期")]
        public DateTime LastUpdateDate { get; set; }        
 	}
}

	