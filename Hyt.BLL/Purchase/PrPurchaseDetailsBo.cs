using Hyt.DataAccess.Purchase;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Purchase
{
    /// <summary>
    /// 采购单详情
    /// </summary>
    /// <remarks>2016-6-17 杨浩 创建</remarks>
    public class PrPurchaseDetailsBo : BOBase<PrPurchaseDetailsBo>
    {
        /// <summary>
        /// 添加采购单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public  int AddPurchaseDetails(PrPurchaseDetails model)
        {
           return IPrPurchaseDetailsDao.Instance.AddPurchaseDetails(model);
        }
        /// <summary>
        /// 更新采购单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public  int UpdatePurchaseDetails(PrPurchaseDetails model)
        {
            return IPrPurchaseDetailsDao.Instance.UpdatePurchaseDetails(model);
        }
        /// <summary>
        /// 删除采购单详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public  int Delete(int sysNo)
        {
            return IPrPurchaseDetailsDao.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 删除采购单详情
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public  int Delete(string sysNos)
        {
            return IPrPurchaseDetailsDao.Instance.Delete(sysNos);
        }

        /// <summary>
        /// 删除采购单详情
        /// </summary>
        /// <param name="purchaseSysNos">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-20 杨浩 创建</remarks>
        public int DeleteByPurchaseSysNos(string purchaseSysNos)
        {
            return IPrPurchaseDetailsDao.Instance.DeleteByPurchaseSysNos(purchaseSysNos);
        }
        /// <summary>
        /// 获取采购单的所有有采购商品
        /// </summary>
        /// <param name="purchaseSysNo">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public  IList<PrPurchaseDetails> GetPurchaseDetailsList(int purchaseSysNo)
        {
            return IPrPurchaseDetailsDao.Instance.GetPurchaseDetailsList(purchaseSysNo);
        }

        /// <summary>
        /// 更新采购单详情已入库数
        /// </summary>
        /// <param name="purchaseSysNo">采购单系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="enterQuantity">已入库数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 创建</remarks>
        public bool UpdateEnterQuantity(int purchaseSysNo, int productSysNo, int enterQuantity)
        {
            return IPrPurchaseDetailsDao.Instance.UpdateEnterQuantity(purchaseSysNo,productSysNo,enterQuantity);
        }
        /// <summary>
        /// 获取采购明细
        /// </summary>
        /// <param name="PurchaseSysNo"></param>
        /// <param name="ProductSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-6-21 王耀发 创建</remarks>
        public PrPurchaseDetails GetPurchaseDetailByPurAndProSysNo(int PurchaseSysNo, int ProductSysNo)
        {
            return IPrPurchaseDetailsDao.Instance.GetPurchaseDetailByPurAndProSysNo(PurchaseSysNo, ProductSysNo);
        }
    }
}
