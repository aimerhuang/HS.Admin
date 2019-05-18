using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 用户调价申请中的传输分页数据
    /// </summary>
    /// <remarks>2013-06-26 邵斌 创建</remarks>
    /// <remarks>2013-07-17 杨晗 修改</remarks>
    [Serializable]
    public class CBPdPriceHistory:PdPriceHistory
    {
        #region 自定义属性

        /// <summary>
        /// 价格来源类型:0-基础价格 10-会员等级价格 40-分销商等价格 70-配送员进货价
        /// </summary>
        public int PriceSource { get; set; }

        /// <summary>
        /// 来源编号，会员等级系统编号（可以是用户等级，分销商等级，配送员等级）
        /// </summary>
        public int SourceSysNo { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyName { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public string AuditorName { get; set; }

        /// <summary>
        /// 商品Erp编码
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        #endregion
    }
}
