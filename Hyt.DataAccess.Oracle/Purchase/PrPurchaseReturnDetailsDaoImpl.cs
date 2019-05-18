using Hyt.DataAccess.Purchase;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Purchase
{
    /// <summary>
    /// 采购单退货详情
    /// </summary>
    /// <remarks>2016-6-17 王耀发 创建</remarks>
    public class PrPurchaseReturnDetailsDaoImpl : IPrPurchaseReturnDetailsDao
    {
        /// <summary>
        /// 添加采购退货单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public override int AddPrPurchaseReturnDetails(PrPurchaseReturnDetails model)
        {
            model.SysNo = Context.Insert("PrPurchaseReturnDetails", model)
                                      .AutoMap(o => o.SysNo)
                                      .ExecuteReturnLastId<int>("SysNo");
            return model.SysNo;
        }
        /// <summary>
        /// 更新采购退货单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public override int UpdatePrPurchaseReturnDetails(PrPurchaseReturnDetails model)
        {
            int rows = Context.Update("PrPurchaseReturnDetails", model)
                 .AutoMap(o => o.SysNo)
                 .Where("SysNo", model.SysNo)
                 .Execute();
            return rows;
        }
        /// <summary>
        /// 获取采购单的所有有采购商品
        /// </summary>
        /// <param name="purchaseSysNo">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public override IList<PrPurchaseReturnDetails> GetPurchaseReturnDetailsList(int PurchaseReturnSysNo)
        {
            return Context.Sql("select * from PrPurchaseReturnDetails where PurchaseReturnSysNo=@PurchaseReturnSysNo")
                .Parameter("PurchaseReturnSysNo", PurchaseReturnSysNo)
                .QueryMany<PrPurchaseReturnDetails>();
        }
        /// <summary>
        /// 删除采购退货单详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-6-17 王耀发 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Sql("Delete from PrPurchaseReturnDetails where SysNo=@SysNo")
                .Parameter("SysNo",sysNo)
                .Execute();
        }
        /// <summary>
        /// 删除采购退货单详情
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-6-18 王耀发 创建</remarks>
        public override int Delete(string sysNos)
        {
            return Context.Sql("Delete from PrPurchaseReturnDetails where SysNo in(" + sysNos + ")")
             .Execute();
        }
        /// <summary>
        /// 删除采购退货单详情
        /// </summary>
        /// <param name="purchaseSysNos">采购退货单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-20 王耀发 创建</remarks>
        public override int DeleteByPurchaseReturnSysNos(string purchaseReturnSysNos)
        {
            return Context.Sql("Delete from PrPurchaseReturnDetails where PurchaseReturnSysNo in(" + purchaseReturnSysNos + ")")
           .Execute();
        }

        /// <summary>
        /// 更新采购退货单详情已出库数
        /// </summary>
        /// <param name="purchaseReturnSysNo">采购退货单系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="outQuantity">已出库数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 王耀发 创建</remarks>
        public override bool UpdateOutQuantity(int purchaseReturnSysNo, int productSysNo, int outQuantity)
        {
            return Context.Sql(string.Format("UPDATE PrPurchaseReturnDetails SET outQuantity={0} where purchaseReturnSysNo={1} and productSysNo={2}", outQuantity, purchaseReturnSysNo, productSysNo))
            .Execute() > 0;
        }
    }
}
