using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.WorkflowStatus
{
    /// <summary>
    /// 采购单状态
    /// </summary>
    /// <remarks>2016-6-16 杨浩 创建</remarks>
    public class PurchaseStatus
    {
        /// <summary>
        /// 采购单状态
        /// 数据表:PrPurchase 字段:Status
        /// </summary>
        /// <remarks>2016-6-16 杨浩 创建</remarks>
        public enum 采购单状态
        {
            待审核=10,
            待入库=20,
            入库中=30,
            已完成=40,
            作废=-10,          
        }
        /// <summary>
        /// 采购单付款状态
        /// 数据表:PrPurchase 字段:PaymentStatus
        /// </summary>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public enum 采购单付款状态
        {
            未付款 = 10,
            已付款 = 20,
        }
        /// <summary>
        /// 采购单入库状态
        /// 数据表:PrPurchase 字段:WarehousingStatus
        /// </summary>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public enum 采购单入库状态
        {
            入库中=10,
            已入库=20,
            未入库=30,
        }
        /// <summary>
        /// 采购退货单状态
        /// 数据表:PrPurchaseReturn 字段:Status
        /// </summary>
        /// <remarks>2016-6-16 杨浩 创建</remarks>
        public enum 采购退货单状态
        {
            待审核 = 10,
            已审核 = 20,
            作废 = -10,
        }
    }
}
