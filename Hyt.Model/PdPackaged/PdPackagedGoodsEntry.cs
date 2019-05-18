using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.PdPackaged
{
    /// <summary>
    /// 套装商品明细
    /// </summary>
    public class PdPackagedGoodsEntry
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 套装商品主表系统编号
        /// </summary>
        public int? PdPackagedGoodsSysNo { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int? PdSysNo { get; set; }


        /// <summary>
        /// 商品代码
        /// </summary>
        public string PdCode { get; set; }


        /// <summary>
        /// 商品名称
        /// </summary>
        public string PdName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Count { get; set; }


        /// <summary>
        /// 发料仓库系统编号
        /// </summary>
        public int? WarehouseSysNo { get; set; }

        /// <summary>
        /// 发料仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

       

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }


        #region 扩展属性
        /// <summary>
        /// 发料仓库代码
        /// </summary>
        public string WarehouseCode { get; set; }
        #endregion
    }
}
