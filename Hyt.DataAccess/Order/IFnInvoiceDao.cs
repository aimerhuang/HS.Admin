using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 订单发票信息
    /// </summary>
    /// <remarks>2013-06-21 朱家宏 创建</remarks>
    public abstract class IFnInvoiceDao : DaoBase<IFnInvoiceDao>
    {
        /// <summary>
        /// 根据订单号获取订单发票信息
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns></returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public abstract FnInvoice GetFnInvoiceByOrderID(int orderID);

       /// <summary>
       /// 根据系统编号获取发票信息
       /// </summary>
       /// <param name="sysNo">发票编号</param>
       /// <returns></returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public abstract FnInvoice GetFnInvoice(int sysNo);

        /// <summary>
        /// 插入发票信息
        /// </summary>
        /// <param name="entity">发票实体</param>
        /// <returns>发票编号</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract int  InsertEntity(FnInvoice entity);
        /// <summary>
        /// 删除发票信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-13 王耀发 创建</remarks>
        public abstract int DeleteEntity(int sysNo);

        /// <summary>
        /// 更新发票信息
        /// </summary>
        /// <param name="entity">发票实体</param>
        /// <returns></returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract void  UpdateEntity(FnInvoice entity);

        /// <summary>
        /// 获取发票类型列表
        /// </summary>
        /// <returns>发票类型列表</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract IList<FnInvoiceType> GetFnInvoiceTypeList();

        /// <summary>
        /// 获取发票类型信息
        /// </summary>
        /// <param name="sysNo">发票类型编号</param>
        /// <returns>发票类型实体</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public abstract FnInvoiceType GetFnInvoiceType(int sysNo);

        /// <summary>
        /// 获取订单有效发票(已开票的发票)
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns>发票</returns>
        /// <remarks>2013-10-29 吴文强 创建</remarks>
        public abstract FnInvoice GetOrderValidInvoice(int orderSysNo);
    }
}
