using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 入库单打印实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class PrtInstock :WhStockIn
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseName { get; set; }

        /// <summary>
        /// 来源单据名称
        /// </summary>
        public string SourceTypeName { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string SoOrderSysNo { get; set; }
        /// <summary>
        /// 淘宝单号
        /// </summary>
        public string TaoBaoSysNo { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int QuantityCount
        {
            get
            {
                return List != null ? List.Sum(item => item.RmaQuantity) : 0;
            }
        }
        /// <summary>
        /// 金额总计
        /// </summary>
        public decimal MoneyCount {
             get
            {
                return List != null ? List.Sum(item => item.Amount) : 0m;
            }
            }
        /// <summary>
        /// 明细列表
        /// </summary>
        public IList<PrtSubInstock> List;
    }

    /// <summary>
    /// 入库单打印明细实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// 2017-05-12 罗勤尧 修改
    /// </remarks>
    public class PrtSubInstock : WhStockInItem
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 条形码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 原单价
        /// </summary>
        public decimal OriginPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int RmaQuantity { get; set; }
        /// <summary>
        /// 实际销售金额
        /// </summary>
        public decimal Amount {
            get
            {
                return OriginPrice * RmaQuantity;
            }
        }
       
    }
}
