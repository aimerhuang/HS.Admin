using Hyt.DataAccess.Base;
using Hyt.Model.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Transport
{
    /// <summary>
    /// 转运系统客户实体
    /// </summary>
    /// <remarks>
    /// 2016-05-17 杨云奕 添加
    /// </remarks>
    public abstract class IDsWhCustomerDao : DaoBase<IDsWhCustomerDao>
    {
        /// <summary>
        /// 添加客户实体
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public abstract int InsertMod(DsWhCustomer mod);
        /// <summary>
        /// 修改客户实体
        /// </summary>
        /// <param name="mod"></param>
        public abstract void UpdateMod(DsWhCustomer mod);
        /// <summary>
        /// 获取客户列表
        /// </summary>
        /// <returns></returns>
        public abstract List<DsWhCustomer> GetCustomerList();
        /// <summary>
        /// 通过编号获取客户实体
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract DsWhCustomer GetCustomerBySysNo(int SysNo);
        /// <summary>
        /// 通过编号删除客户
        /// </summary>
        /// <param name="SysNo"></param>
        public abstract void DeleteCustomerBySysNo(int SysNo);
        /// <summary>
        /// 通过系统账号获取客户信息
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public abstract DsWhCustomer GetCustomerByAssAccount(string Account);
        /// <summary>
        /// 分页获取账户信息
        /// </summary>
        /// <param name="pageCusList"></param>
        public abstract void DoDsWhCustomerQuery(ref Model.Pager<CBDsWhCustomer> pageCusList);

        public abstract DsWhCustomer GetCustomerByCusCode(string CusCode);
    }
}
