using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 用于创建配送单 参数类
    /// </summary>
    /// <remarks>
    /// 2013-07-04 沈强 创建
    /// </remarks>
    public class ParaLogisticsControllerGetDeliveryNoteItemList
    {
        /// <summary>
        /// 单据类型(出库单/取件单)
        /// </summary>
        public int NoteType { get; set; }
        /// <summary>
        /// 单据编号（根据单据类型决定，此字段是一个用“,”分隔的字符串）
        /// </summary>
        public string NoteNumber { get; set; }
        /// <summary>
        /// 是否强制扫描（1：是；0：否）
        /// </summary>
        public int IsForce { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNumber { get; set; }
        /// <summary>
        /// 配送员系统编号
        /// </summary>
        public int DeliverymanSysNo { get; set; }
        /// <summary>
        /// 配送类型，是否为第三方快递
        /// </summary>
        public int DeliveryType { get; set; }
        /// <summary>
        /// 第三方配送系统编号
        /// </summary>
        public int ThirdPartyExpressSysNo { get; set; }
    }
}
