using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.PdPackaged
{
    /// <summary>
    /// 套装商品表
    /// </summary>
    public class PdPackagedGoods
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int? SysNo { get; set; }


        /// <summary>
        /// 单据编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 套装代码
        /// </summary>
        public string SetCode { get; set; }

        /// <summary>
        /// 套装名称
        /// </summary>
        public string SetName { get; set; }

        /// <summary>
        /// 套装数量
        /// </summary>
        public decimal SetCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreatedBy { get; set; }


        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatedName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public int? LastUpdateBy { get; set; }

        /// <summary>
        /// 最后更新人名称
        /// </summary>
        public string LastUpdateName { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// 审核人员
        /// </summary>
        public int? Auditor { get; set; }

        /// <summary>
        /// 审核人员名称
        /// </summary>
        public string AuditorName { get; set; }

        /// <summary>
        /// 审核日期
        /// </summary>
        public DateTime? AuditorDate { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }


        #region 扩展属性
        /// <summary>
        /// 套装商品详情
        /// </summary>
        public List<PdPackagedGoodsEntry> PdList { get; set; }

        #endregion

        /// <summary>
        /// 套装商品单据状态
        /// </summary>
        public enum PdPackagedGoodsStatusEnum
        {
            待审核 = 1,
            完成 = 2,
            作废=3
        }
    }
}
