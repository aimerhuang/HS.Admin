using System;
using Hyt.DataAccess.Order;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Order
{   
    /// <summary>
    /// 订单其他费用
    /// </summary>
    /// <remarks>2016-09-28 罗远康 创建</remarks>
    public class SoOrderOtherExpensesDaoImpl : ISoSoOrderOtherExpensesDao
    {
        /// <summary>
        /// 根据订单获取其他费用详情
        /// </summary>
        /// <param name="OrderSysno">订单编号</param>
        /// <returns></returns>
        public override SoOrderOtherExpenses GetExpenses(int OrderSysNo)
        {
            return Context.Select<SoOrderOtherExpenses>("TOP 1 *")
              .From("SoOrderOtherExpenses")
              .Where("OrderSysNo = @OrderSysNo")
              .Parameter("OrderSysNo", OrderSysNo)
              .QuerySingle();
        }

        /// <summary>
        /// 根据订单获取费用（太平洋保险费）
        /// </summary>
        /// <param name="OrderSysno">订单编号</param>
        /// <returns></returns>
        public override decimal GetExpensesFee(int OrderSysNo)
        {
            return Context.Sql("select ExpensesAmount from SoOrderOtherExpenses where OrderSysNo=@OrderSysNo")
           .Parameter("OrderSysNo", OrderSysNo)
           .QuerySingle<decimal>();//返回费用
        }

        /// <summary>
        /// 更新费用
        /// </summary>
        /// <param name="model">费用明细</param>
        /// <returns>返回受影响行</returns>
        public override int UpdateExpenses(Model.SoOrderOtherExpenses model)
        {
            var r = Context.Update("SoOrderOtherExpenses", model)
                           .AutoMap(o => o.OrderSysNo)
                           .Where("OrderSysNo", model.OrderSysNo).Execute();
            return r;
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="OrderSysNo">订单编号</param>
        /// <returns></returns>
        public override bool isExistsExpenses(int OrderSysNo)
        {
            bool result = false;
            SoOrderOtherExpenses entity = Context.Select<SoOrderOtherExpenses>("*")
                .From("SoOrderOtherExpenses")
                .Where("OrderSysNo= @OrderSysNo")
                .Parameter("OrderSysNo", OrderSysNo)
                .QuerySingle();

            if (entity != null && entity.SysNo > 0)
            {
                result = true;
            }
            return result;
        }
    }
}
