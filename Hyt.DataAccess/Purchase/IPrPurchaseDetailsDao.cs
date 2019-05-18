using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Purchase
{
    /// <summary>
    /// 采购单详情
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public abstract class IPrPurchaseDetailsDao : DaoBase<IPrPurchaseDetailsDao>
    {
        /// <summary>
        /// 添加采购单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public abstract int AddPurchaseDetails(PrPurchaseDetails model);
        /// <summary>
        /// 更新采购单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public abstract int UpdatePurchaseDetails(PrPurchaseDetails model);
        /// <summary>
        /// 删除采购单详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public abstract int Delete(int sysNo);
        /// <summary>
        /// 删除采购单详情
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public abstract int Delete(string sysNos);
        /// <summary>
        /// 删除采购单详情
        /// </summary>
        /// <param name="purchaseSysNos">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-20 杨浩 创建</remarks>
        public abstract int DeleteByPurchaseSysNos(string purchaseSysNos);
        /// <summary>
        /// 获取采购单的所有有采购商品
        /// </summary>
        /// <param name="purchaseSysNo">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public abstract IList<PrPurchaseDetails> GetPurchaseDetailsList(int purchaseSysNo);
        /// <summary>
        /// 更新采购单详情已入库数
        /// </summary>
        /// <param name="purchaseSysNo">采购单系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="enterQuantity">已入库数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 创建</remarks>
        public abstract bool UpdateEnterQuantity(int purchaseSysNo, int productSysNo, int enterQuantity);

        public abstract IList<PrPurchaseDetails> GetRePurchaseDetailsList(int purchaseSysNo);
        /// <summary>
        /// 获取采购明细
        /// </summary>
        /// <param name="PurchaseSysNo"></param>
        /// <param name="ProductSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-6-21 王耀发 创建</remarks>
        public abstract PrPurchaseDetails GetPurchaseDetailByPurAndProSysNo(int PurchaseSysNo, int ProductSysNo);
    }
}
