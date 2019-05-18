using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.RMA;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.RMA
{
    /// <summary>
    /// RMA销售单明细关系表
    /// </summary>
    /// <remarks>2014-06-16 朱成果 创建</remarks>
    public class SoReturnOrderItemDaoImpl : ISoReturnOrderItemDao
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-06-16  朱成果 创建</remarks>
        public override void Insert(SoReturnOrderItem entity)
        {
            Context
                .Insert("SoReturnOrderItem")
                .Column("OrderItemSysNo",entity.OrderItemSysNo)
                .Column("TransactionSysNo",entity.TransactionSysNo)
                .Column("FromStockOutItemAmount",entity.FromStockOutItemAmount)
                .Column("FromStockOutItemQuantity",entity.FromStockOutItemQuantity)
                .Column("FromStockOutItemSysNo",entity.FromStockOutItemSysNo)
                .Column("OrderItemQuantity",entity.OrderItemQuantity)
                .Execute();
        }

        /// <summary>
        /// 获销售单明细关系表详情
        /// </summary>
        /// <param name="orderItemSysNo">事物编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-16  朱成果 创建</remarks>
        public override SoReturnOrderItem GetSoReturnOrderItem(int orderItemSysNo)
        {
          return Context.Sql("select * from SoReturnOrderItem where OrderItemSysNo=@OrderItemSysNo")
                .Parameter("OrderItemSysNo", orderItemSysNo)
                .QuerySingle<SoReturnOrderItem>();
        }

        /// <summary>
        /// 获销售单明细关系表详情
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-16  杨浩 创建</remarks>
        public override IList<SoReturnOrderItem> GetSoReturnOrderItem(string transactionSysNo)
        {
            return Context.Sql("select * from SoReturnOrderItem where transactionSysNo=@transactionSysNo")
                  .Parameter("transactionSysNo", transactionSysNo)
                  .QueryMany<SoReturnOrderItem>();
        }
    }
}
