using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 商品商检明细表
    /// </summary>
    /// <remarks>
    /// 2016-03-23 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class CBIcpGoodsItem : CIcpGoodsItem
    {
        /// <summary>
        /// 商品检验系统编号
        /// </summary>	
        public int GoodsItemSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string ErpCode { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 报文类型
        /// </summary>
        public string MessageType { get; set; }
        /// <summary>
        /// 接受状态
        /// </summary>
        public string PlatStatus { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string CiqStatus { get; set; }
    }
}
