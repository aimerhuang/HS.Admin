using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 销售明细/退换货明细查询参数
    /// </summary>
    /// <remarks>2013-9-16 黄伟 创建</remarks>
    public class SalesRmaParams : RP_退换货明细
    {
        /// <summary>
        /// 订单开始日期,用于筛选查找
        /// </summary>
        public DateTime? OrderBeginDate { get; set; }

        /// <summary>
        /// 出库单结束日期,用于筛选查找
        /// </summary>
        public DateTime? OrderEndDate { get; set; }

        /// <summary>
        /// 出库单开始日期,用于筛选查找
        /// </summary>
        public  DateTime? BeginDate { get; set; }

        /// <summary>
        /// 出库单结束日期,用于筛选查找
        /// </summary>
        public DateTime? EndDate { get; set; }


        /// <summary>
        /// 发货开始日期,用于筛选查找
        /// </summary>
        public DateTime? FaHuoBeginDate { get; set; }


        /// <summary>
        /// 发货结束日期,用于筛选查找
        /// </summary>
        public DateTime? FaHuoEndDate { get; set; }

        /// <summary>
        /// 入库单开始日期,用于筛选查找
        /// </summary>
        public DateTime? RBeginDate { get; set; }

        /// <summary>
        /// 入库单结束日期,用于筛选查找
        /// </summary>
        public DateTime? REndDate { get; set; }

        /// <summary>
        /// 仓库,用于筛选查找
        /// </summary>
        public List<string> WareHouses { get; set; }

        /// <summary>
        /// 配送方式,用于筛选查找
        /// </summary>
        public List<string> DelType { get; set; }

        /// <summary>
        /// 售后方式,用于筛选查找
        /// </summary>
        public List<string> PickType { get; set; }

        /// <summary>
        /// 收款方式,用于筛选查找
        /// </summary>
        public List<string> PayType { get; set; }

        /// <summary>
        /// 退换货方式,用于筛选查找
        /// </summary>
        public List<string> RMAType { get; set; }

        /// <summary>
        /// 退换货方式,用于筛选查找
        /// </summary>
        public List<string> SettlementType { get; set; }

        /// <summary>
        /// 商品编号,用于筛选查找
        /// </summary>
        public string ProNo { get; set; }

        /// <summary>
        /// 商品名称,用于筛选查找
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// 客户ID,用于筛选查找
        /// </summary>
        public string CusId { get; set; }

        /// <summary>
        /// 收货电话,用于筛选查找
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 结算状态,用于筛选查找
        /// </summary>
        public string SettlementStatus { get; set; }

        /// <summary>
        /// 开票状态,用于筛选查找
        /// </summary>
        public string InvStatus { get; set; }

        /// <summary>
        /// 订单号,用于筛选查找
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单来源,用于筛选查找
        /// </summary>
        public List<string> OrderSource { get; set; }

        /// <summary>
        /// 是否自营
        /// </summary>
        /// <remarks>2014-08-27 余勇 创建</remarks>
        public int? IsSelfSupport { get; set; }

    }
}
