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
    /// 采购退货单详情
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public abstract class IPrPurchaseReturnDetailsDao : DaoBase<IPrPurchaseReturnDetailsDao>
    {
        /// <summary>
        /// 添加采购退货单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public abstract int AddPrPurchaseReturnDetails(PrPurchaseReturnDetails model);
        /// <summary>
        /// 更新采购退货单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public abstract int UpdatePrPurchaseReturnDetails(PrPurchaseReturnDetails model);
        /// <summary>
        /// 获取采购单的所有有采购商品
        /// </summary>
        /// <param name="purchaseSysNo">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public abstract IList<PrPurchaseReturnDetails> GetPurchaseReturnDetailsList(int PurchaseReturnSysNo);
        /// <summary>
        /// 删除采购退货单详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public abstract int Delete(int sysNo);
        /// <summary>
        /// 删除采购退货单详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public abstract int Delete(string sysNos);
        /// <summary>
        /// 删除采购退货单详情
        /// </summary>
        /// <param name="purchaseSysNos">采购退货单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-20 王耀发 创建</remarks>
        public abstract int DeleteByPurchaseReturnSysNos(string purchaseReturnSysNos);
        /// <summary>
        /// 更新采购退货单详情已出库数
        /// </summary>
        /// <param name="purchaseReturnSysNo">采购退货单系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="outQuantity">已出库数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 王耀发 创建</remarks>
        public abstract bool UpdateOutQuantity(int purchaseReturnSysNo, int productSysNo, int outQuantity);
    }
}
