using Hyt.DataAccess.Base;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 会员卡
    /// </summary>
    /// <remarks>2017-1-16 杨浩 创建</remarks>
    public abstract class ICrCustomerShipCardDao : DaoBase<ICrCustomerShipCardDao>
    {
        /// <summary>
        /// 根据商客户系统编号获取会员卡
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-01-16 杨浩 创建</remarks>
        public abstract string GetCardNumber(int customerSysNo);
         /// <summary>
        /// 根据商会员卡获取客户系统编号
        /// </summary>
        /// <param name="cardNumber">卡号</param>
        /// <remarks>2017-01-16 杨浩 创建</remarks>
        public abstract int GetCustomerSysNo(string cardNumber);
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-01-16 杨浩 创建</remarks>
        public abstract int Insert(CrCustomerShipCard model);
    }
}
