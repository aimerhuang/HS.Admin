using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 订单其他费用
    /// </summary>
    /// <remarks>2016-09-28 罗远康 创建</remarks>
    public class SoOrderOtherExpensesBo : BOBase<SoOrderOtherExpensesBo>
    {
        /// <summary>
        /// 根据订单获取其他费用详情
        /// </summary>
        /// <param name="OrderSysno">订单编号</param>
        /// <returns></returns>
        public SoOrderOtherExpenses GetExpenses(int OrderSysNo)
        {
            return DataAccess.Order.ISoSoOrderOtherExpensesDao.Instance.GetExpenses(OrderSysNo);
        }

        /// <summary>
        /// 根据订单获取费用（太平洋保险费）
        /// </summary>
        /// <param name="OrderSysno">订单编号</param>
        /// <returns></returns>
        public decimal GetExpensesFee(int OrderSysNo)
        {
            return DataAccess.Order.ISoSoOrderOtherExpensesDao.Instance.GetExpensesFee(OrderSysNo);
        }

        /// <summary>
        /// 更新费用
        /// </summary>
        /// <param name="model">费用明细</param>
        /// <returns>返回受影响行</returns>
        public int UpdateExpenses(Model.SoOrderOtherExpenses model)
        {
            return DataAccess.Order.ISoSoOrderOtherExpensesDao.Instance.UpdateExpenses(model);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="OrderSysNo">订单编号</param>
        /// <returns></returns>
        public bool isExistsExpenses(int OrderSysNo)
        {
            return DataAccess.Order.ISoSoOrderOtherExpensesDao.Instance.isExistsExpenses(OrderSysNo);
        }
    }
}
