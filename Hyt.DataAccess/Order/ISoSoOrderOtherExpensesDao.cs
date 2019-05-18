using Hyt.DataAccess.Base;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 订单其他费用
    /// </summary>
    /// <remarks>2016-09-28 罗远康 创建</remarks>
    public abstract class ISoSoOrderOtherExpensesDao : DaoBase<ISoSoOrderOtherExpensesDao>
    {
        /// <summary>
        /// 根据订单获取其他费用详情
        /// </summary>
        /// <param name="OrderSysno">订单编号</param>
        /// <returns></returns>
        public abstract SoOrderOtherExpenses GetExpenses(int OrderSysno);
        /// <summary>
        /// 根据订单获取费用（太平洋保险费）
        /// </summary>
        /// <param name="OrderSysno">订单编号</param>
        /// <returns></returns>
        public abstract decimal GetExpensesFee(int OrderSysno);

        /// <summary>
        /// 更新广告组
        /// </summary>
        /// <param name="model">广告组明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        public abstract int UpdateExpenses(Model.SoOrderOtherExpenses model);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="OrderSysNo">订单编号</param>
        /// <returns></returns>
        public abstract bool isExistsExpenses(int OrderSysNo);

    }
}
