
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 T4生成
    /// 2016-4-18 杨云奕 修改
    /// </remarks>
	[Serializable]
    public partial class Origin
	{
        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }
        /// <summary>
        /// Origin_Name
        /// </summary>	
        [Description("Origin_Name")]
        public string Origin_Name { get; set; }
        /// <summary>
        /// Origin_Img
        /// </summary>	
        [Description("Origin_Img")]
        public string Origin_Img { get; set; }
        /// <summary>
        /// Origin_Describe
        /// </summary>	
        [Description("Origin_Describe")]
        public string Origin_Describe { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>	
        [Description("CreatedBy")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// CreatedDate
        /// </summary>	
        [Description("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// LastUpdateBy
        /// </summary>	
        [Description("LastUpdateBy")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// LastUpdateDate
        /// </summary>	
        [Description("LastUpdateDate")]
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// 国家商检编码
        /// </summary> 
        [Description("国家商检编码")]
        public string CIQOriginNO { get; set; }
        /// <summary>
        /// 国家海关编码
        /// </summary>
        [Description("国家海关编码")]
        public string CusOriginNO { get; set; }
 	}
}

	