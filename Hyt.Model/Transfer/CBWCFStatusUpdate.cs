using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// wcf服务实体-更新单据状态
    /// </summary>
    /// <remarks>2013-06-24 黄伟 创建</remarks>
    [DataContract]
    public class CBWCFStatusUpdate
    {
        /// <summary>
        /// 单据类型
        /// </summary>
        [DataMember]
        public int NoteType { get; set; }

        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember]
        public int NoteSysNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remarks { get; set; }
        /// <summary>
        /// 配送单系统编号
        /// </summary>
        [DataMember]
        public int DeliverySysNo { get; set; }
        ///// <summary>
        ///// 产品列表
        ///// </summary>
        //[DataMember]
        //public List<CBWCFProductSubset> Products { get; set; }

        /// <summary>
        /// 单据明细集合-部分签收明细
        /// </summary>
        [DataMember]
        public List<CBWCFStatusItem> CbwcfStatusItems { get; set; }
    }

    /// <summary>
    /// 部分签收明细
    /// </summary>
    [DataContract]
    public class CBWCFStatusItem
    {
        /// <summary>
        /// 出库单明细系统编号
        /// </summary>
        [DataMember]
        public int NoteItemSysNo { get; set; }

        /// <summary>
        /// 签收数量
        /// </summary>
        [DataMember]
        public int SignQuantity { get; set; }
    }

    /// <summary>
    /// reserved
    /// </summary>
    /// <remarks>2013-06-24 黄伟 创建</remarks>
    [DataContract]
    public class CBWCFProductSubset
    {
         
    }
}
