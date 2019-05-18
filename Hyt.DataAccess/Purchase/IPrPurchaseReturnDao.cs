using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Hyt.DataAccess.Purchase
{
    /// <summary>
    /// 采购退货单
    /// </summary>
    /// <remarks>2016-6-15 王耀发 创建</remarks>
    public abstract class IPrPurchaseReturnDao : DaoBase<IPrPurchaseReturnDao>
    {
        /// <summary>
        /// 添加采购退货单
        /// </summary>
        /// <param name="PurchaseReturn">采购退货单实体类对象</param>
        /// <returns></returns>
        public abstract int AddPurchaseReturn(PrPurchaseReturn PurchaseReturn);
        /// <summary>
        /// 更新采购退货单
        /// </summary>
        /// <param name="purchase">采购退货单实体类对象</param>
        /// <returns></returns>
        public abstract int UpdatePurchaseReturn(PrPurchaseReturn PurchaseReturn);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo"></param>
        public abstract void Delete(int sysNo);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo"></param>
        public abstract void Delete(string sysNos);
        /// <summary>
        /// 获取采购退货单详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public abstract PrPurchaseReturn GetPrPurchaseReturn(int sysNo);
        /// <summary>
        /// 查询采购单
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        public abstract Pager<CBPrPurchaseReturn> QueryPrPurchaseReturn(ParaPrPurchaseReturnFilter para);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">采购退货单系统编号</param>
        /// <param name="statusType">状态 :待审核（10）、已审核（20）、作废（-10）</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public abstract bool UpdateStatus(int sysNo, int status);
        /// <summary>
        /// 更新采购退货单已出库数
        /// </summary>
        /// <param name="sysNo">采购退货单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 王耀发 创建</remarks>
        public abstract bool UpdateOutQuantity(int sysNo);
    }
}
