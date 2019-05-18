using System;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 商品商检明细表
    /// </summary>
    /// <remarks>
    /// 2016-03-23 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class CIcpGoodsItem
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
        public int? IcpType { get; set; }
        /// <summary>
        /// MessageID
        /// </summary>	
        [Description("MessageID")]
        public string MessageID { get; set; }
        /// <summary>
        /// IcpGoodsSysNo
        /// </summary>	
        [Description("IcpGoodsSysNo")]
        public int IcpGoodsSysNo { get; set; }

        /// <summary>
        /// ProductSysNo
        /// </summary>	
        [Description("ProductSysNo")]
        public int ProductSysNo { get; set; }

        /// <summary>
        /// EntGoodsNo
        /// </summary>	
        [Description("EntGoodsNo")]
        public string EntGoodsNo { get; set; }

        /// <summary>
        /// CIQGRegStatus
        /// </summary>	
        [Description("CIQGRegStatus")]
        public string CIQGRegStatus { get; set; }

        /// <summary>
        /// CIQNotes
        /// </summary>	
        [Description("CIQNotes")]
        public string CIQNotes { get; set; }

        /// <summary>
        /// OpResult
        /// </summary>	
        [Description("OpResult")]
        public string OpResult { get; set; }

        /// <summary>
        /// CustomsNotes
        /// </summary>	
        [Description("CustomsNotes")]
        public string CustomsNotes { get; set; }

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
