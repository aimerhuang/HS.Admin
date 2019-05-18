using System;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 仓库库存报警
    /// </summary>
    /// <remarks>
    /// 2016-06-15 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class WhInventoryAlarm
    {
        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }

        /// <summary>
        /// ProductStockSysNo
        /// </summary>	
        [Description("ProductStockSysNo")]
        public int ProductStockSysNo { get; set; }

        /// <summary>
        /// Upperlimit
        /// </summary>	
        [Description("Upperlimit")]
        public int Upperlimit { get; set; }

        /// <summary>
        /// Lowerlimit
        /// </summary>	
        [Description("Lowerlimit")]
        public int Lowerlimit { get; set; }

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
