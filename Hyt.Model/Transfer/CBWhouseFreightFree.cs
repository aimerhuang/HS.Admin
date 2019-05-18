using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    [Serializable]
    public class CBWhouseFreightFree : WhouseFreightFree
    {
        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int WhSysNo { get; set; }
        /// <summary>
        /// 仓库ERP编号
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 前台仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 后台仓库名称
        /// </summary>
        public string BackWarehouseName { get; set; }
    }
}
