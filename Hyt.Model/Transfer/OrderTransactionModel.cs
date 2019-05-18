using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 百城通快递单查询结果分组模型
    /// </summary>
    /// <remarks>2013-09-18 周唐炬 创建</remarks>
    public class OrderTransactionModel
    {
        public OrderTransactionModel()
        {
            TransactionGroups = new List<TransactionGroup>();
            PendingProducts = new List<SoOrderItem>();
            OrderTransactionLogs = new List<SoTransactionLog>();
        }
        /// <summary>
        /// 订单号
        /// </summary>
        public int SoOrderSysNo { get; set; }

        /// <summary>
        /// 订单快递方式
        /// </summary>
        public int SoOrderDeliveryType { get; set; }

        /// <summary>
        /// 事务单号->传入的快递单号
        /// </summary>
        public string TransactionSysNo { get; set; }
        /// <summary>
        /// 包含订单、出库、配送信息
        /// </summary>
        public List<TransactionGroup> TransactionGroups { get; set; }
        /// <summary>
        /// 未出库或未配送商品
        /// </summary>
        public IList<SoOrderItem> PendingProducts { get; set; }
        /// <summary>
        /// 订单跟踪
        /// </summary>
        public List<SoTransactionLog> OrderTransactionLogs { get; set; }
    }
    /// <summary>
    /// 百城通快递单查询结果信息实体
    /// </summary>
    /// <remarks>2013-09-18 周唐炬 创建</remarks>
    public class TransactionGroup
    {
        /// <summary>
        /// 出库单
        /// </summary>
        public WhStockOut WhStockOutModel { get; set; }
        /// <summary>
        /// 配送单
        /// </summary>
        public LgDelivery LgDeliveryModel { get; set; }
        /// <summary>
        /// 配送跟踪
        /// </summary>
        public List<SoTransactionLog> SoTransactionLogs { get; set; }
        /// <summary>
        /// 配送地图轨迹
        /// </summary>
        public List<LgDeliveryUserLocation> LgDeliveryUserLocations { get; set; }
    }
    /// <summary>
    /// 地图轨迹查询模型
    /// </summary>
    /// <remarks>2013-09-18 周唐炬 创建</remarks>
    public class MapSearchModel
    {
        /// <summary>
        /// 配送员编号
        /// </summary>
        public int DeliveryUserSysNo { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
