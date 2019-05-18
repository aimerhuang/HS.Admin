using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;

namespace Hyt.Model
{
    public class CBWhProductWarehousePositionAssociation
    {
        /// <summary>
        /// 关联编号
        /// </summary>
        public int AssociationSysNo { get; set; }

        /// <summary>
        /// 定位编号
        /// </summary>
        public int PositionSysNo { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string WarehousePositionName { get; set; }

        ///商品编码
        public int PdProductSysNo { get; set; }
    }
}
