using Hyt.DataAccess.Purchase;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Purchase
{
    /// <summary>
    /// 采购单详情
    /// </summary>
    /// <remarks>2016-6-17 杨浩 创建</remarks>
    public class PrPurchaseDetailsDaoImpl:IPrPurchaseDetailsDao
    {
        /// <summary>
        /// 添加采购单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public override int AddPurchaseDetails(PrPurchaseDetails model)
        {
            model.SysNo = Context.Insert("PrPurchaseDetails", model)
                                      .AutoMap(o => o.SysNo)
                                      .ExecuteReturnLastId<int>("SysNo");
            return model.SysNo;
        }
        /// <summary>
        /// 更新采购单详情
        /// </summary>
        /// <param name="model">采购单详情实体</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public override int UpdatePurchaseDetails(PrPurchaseDetails model)
        {
            int rows = Context.Update("PrPurchaseDetails", model)
                 .AutoMap(o => o.SysNo)
                 .Where("SysNo", model.SysNo)
                 .Execute();
            return rows;
        }
        /// <summary>
        /// 删除采购单详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public override int Delete(int sysNo)
        {
           return Context.Sql("Delete from PrPurchaseDetails where SysNo=@SysNo")
                .Parameter("SysNo",sysNo)
                .Execute();
        }

        /// <summary>
        /// 删除采购单详情
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-6-18 杨浩 创建</remarks>
        public override int Delete(string sysNos)
        {
            return Context.Sql("Delete from PrPurchaseDetails where SysNo in("+sysNos+")")           
             .Execute();
        }

        /// <summary>
        /// 删除采购单详情
        /// </summary>
        /// <param name="purchaseSysNos">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-20 杨浩 创建</remarks>
        public override int DeleteByPurchaseSysNos(string purchaseSysNos)
        {
            return Context.Sql("Delete from PrPurchaseDetails where PurchaseSysNo in(" + purchaseSysNos + ")")
           .Execute();
        }
        /// <summary>
        /// 获取采购单的所有有采购商品
        /// </summary>
        /// <param name="purchaseSysNo">采购单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-17 杨浩 创建</remarks>
        public override IList<PrPurchaseDetails> GetPurchaseDetailsList(int purchaseSysNo)
        {
            return Context.Sql("select * from PrPurchaseDetails where purchaseSysNo=@purchaseSysNo")
                .Parameter("purchaseSysNo", purchaseSysNo)
                .QueryMany<PrPurchaseDetails>();
        }

        /// <summary>
        /// 更新采购单详情已入库数
        /// </summary>
        /// <param name="purchaseSysNo">采购单系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="enterQuantity">已入库数</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 杨浩 创建</remarks>
        public override bool UpdateEnterQuantity(int purchaseSysNo, int productSysNo,int enterQuantity)
        {
            return Context.Sql(string.Format("UPDATE PrPurchaseDetails SET enterQuantity={0} where purchaseSysNo={1} and productSysNo={2}", enterQuantity, purchaseSysNo, productSysNo))
            .Execute()>0;
        }

        public override IList<PrPurchaseDetails> GetRePurchaseDetailsList(int purchaseSysNo)
        {
            return Context.Sql(@"select * from (
            select pde.ProductSysNo,pde.ErpCode,pde.ProductName,(pde.Quantity - isnull(prd.sumReturnQuantity,0)) as Quantity,pde.[Money],(pde.Quantity - isnull(prd.sumReturnQuantity,0)) * pde.[Money] as TotalMoney
            from PrPurchaseDetails pde left join 

            (
            select ProductSysNo,sum(ReturnQuantity) as sumReturnQuantity from PrPurchaseReturnDetails where PurchaseReturnSysNo in
            (
            select SysNo from PrPurchaseReturn where PurchaseSysNo = @PurchaseSysNo and [Status] <> -10
            )
            group by ProductSysNo
            ) prd on pde.ProductSysNo = prd.ProductSysNo

            where pde.PurchaseSysNo = @PurchaseSysNo  
            ) as tb
            where tb.Quantity <> 0")
           .Parameter("PurchaseSysNo", purchaseSysNo)
           .QueryMany<PrPurchaseDetails>();
        }
        /// <summary>
        /// 获取采购明细
        /// </summary>
        /// <param name="PurchaseSysNo"></param>
        /// <param name="ProductSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-6-21 王耀发 创建</remarks>
        public override PrPurchaseDetails GetPurchaseDetailByPurAndProSysNo(int PurchaseSysNo,int ProductSysNo)
        {
            return Context.Sql("select * from PrPurchaseDetails where PurchaseSysNo=@0 and ProductSysNo=@1", PurchaseSysNo, ProductSysNo)
                          .QuerySingle<PrPurchaseDetails>();
        }
    }
}
