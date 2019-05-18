using System;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 商检表
    /// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class CIcp
    {
        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }

        /// <summary>
        /// SourceSysNo
        /// </summary>	
        [Description("SourceSysNo")]
        public int SourceSysNo { get; set; }

        /// <summary>
        /// IcpType
        /// </summary>	
        [Description("IcpType")]
        public int IcpType { get; set; }

        /// <summary>
        /// MessageID
        /// </summary>	
        [Description("MessageID")]
        public string MessageID { get; set; }

        /// <summary>
        /// MessageType
        /// </summary>	
        [Description("MessageType")]
        public string MessageType { get; set; }

        /// <summary>
        /// SerialNumber
        /// </summary>	
        [Description("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// PlatDocRec
        /// </summary>	
        [Description("PlatDocRec")]
        public string PlatDocRec { get; set; }

        /// <summary>
        /// PlatStatus
        /// </summary>	
        [Description("PlatStatus")]
        public string PlatStatus { get; set; }

        /// <summary>
        /// CiqDocRec
        /// </summary>	
        [Description("CiqDocRec")]
        public string CiqDocRec { get; set; }

        /// <summary>
        /// CiqStatus
        /// </summary>	
        [Description("CiqStatus")]
        public string CiqStatus { get; set; }

        /// <summary>
        /// XmlContent
        /// </summary>	
        [Description("XmlContent")]
        public string XmlContent { get; set; }

        /// <summary>
        /// Status
        /// </summary>	
        [Description("Status")]
        public int Status { get; set; }

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
    }
}
