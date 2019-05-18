using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// app sign info required
    /// </summary>
    /// <remarks>2014-1-14 黄伟 创建</remarks>
    public class LgAppSignInfo : BaseEntity
    {
        /// <summary>
        /// 配送单系统编号
        /// </summary>
        public int DelSysNo { get; set; }

        /// <summary>
        /// 出库单/取件单
        /// </summary>
        public int NoteType { get; set; }

        /// <summary>
        /// 单据系统编号
        /// </summary>
        public int NoteSysNo { get; set; }

        public int SignOption { get; set; }

        /// <summary>
        /// 部分签收商品信息(订单明细编号,签收数量)
        /// </summary>
        public List<SignedProductInfo> SignedProductInfos { get; set; }
    }
}