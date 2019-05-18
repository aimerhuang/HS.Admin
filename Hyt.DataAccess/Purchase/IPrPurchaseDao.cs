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
    /// 采购单
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public abstract class IPrPurchaseDao : DaoBase<IPrPurchaseDao>
    {
        /// <summary>
        /// 添加采购单
        /// </summary>
        /// <param name="purchase">采购单实体类对象</param>
        /// <returns></returns>
        public abstract int AddPurchase(PrPurchase purchase);
        /// <summary>
        /// 更新采购单
        /// </summary>
        /// <param name="purchase">采购单实体类对象</param>
        /// <returns></returns>
        public abstract int UpdatePurchase(PrPurchase purchase);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo"></param>
        public abstract  void Delete(int sysNo);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNos"></param>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public abstract void Delete(string sysNos);
        /// <summary>
        /// 获取采购单详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public abstract PrPurchase GetPrPurchaseInfo(int sysNo);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <param name="paymentStatus">支付状态</param>
        /// <param name="warehousingStatus">入库状态</param>
        /// <param name="status">采购单状态</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 创建</remarks>
        public abstract bool UpdateStatus(int sysNo, int paymentStatus,int warehousingStatus,int status);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <param name="statusType">状态类型（0：状态，1：付款状态，2：入库状态）</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public abstract bool UpdateStatus(int sysNo, int statusType, int status);
        /// <summary>
        /// 查询采购单
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        public abstract Pager<CBPrPurchase> QueryPrPurchase(ParaPrPurchaseFilter para);
        /// <summary>
        /// 更新采购单已入库数
        /// </summary>
        /// <param name="sysNo">采购单系统编号</param>
        /// <param name="enterQuantity">已入库数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 创建</remarks>
        public abstract bool UpdateEnterQuantity(int sysNo, int enterQuantity);
        /// <summary>
        /// 获取采购单导出明细
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public abstract List<CBPrPurchaseDetails> QueryPrPurchaseByOrderDetail(ParaPrPurchaseFilter para);

        /// <summary>
        /// 获取采购单明细
        /// </summary>
        /// <param name="purchaseSysno">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-01-04 杨浩 创建</remarks>
        public abstract IList<PrPurchaseDetails> GetPurchaseDetailsByPurchaseSysNo(int purchaseSysno);
    }
}
