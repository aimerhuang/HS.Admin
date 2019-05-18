using System;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 仓库库位
    /// </summary>
    /// <remarks>
    /// 2016-06-15 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class WhWarehousePosition
    {
        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }

        /// <summary>
        /// WarehouseSysNo
        /// </summary>	
        [Description("WarehouseSysNo")]
        public int WarehouseSysNo { get; set; }

        /// <summary>
        /// WarehousePositionName
        /// </summary>	
        [Description("WarehousePositionName")]
        public string WarehousePositionName { get; set; }

        /// <summary>
        /// Description
        /// </summary>	
        [Description("Description")]
        public string Description { get; set; }

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
