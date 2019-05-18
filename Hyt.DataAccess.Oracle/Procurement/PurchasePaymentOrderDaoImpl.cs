using Hyt.DataAccess.Procurement;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Procurement
{
    public class PurchasePaymentOrderDaoImpl : IPurchasePaymentOrderDao
    {
        /// <summary>
        /// 添加采购付款单
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public override int InsertEntity(Model.Procurement.FnPurchasePaymentOrder mod)
        {
            return Context.Insert("FnPurchasePaymentOrder", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新采购付款单
        /// </summary>
        /// <param name="mod"></param>
        public override void UpdateEntity(Model.Procurement.FnPurchasePaymentOrder mod)
        {
            Context.Update("FnPurchasePaymentOrder", mod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 删除采购付款单
        /// </summary>
        /// <param name="SysNo"></param>
        public override void DeleteEntity(int SysNo)
        {
            string sql = " delete from FnPurchasePaymentOrder where SysNo = '" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 获取采购付款单编号
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Model.Procurement.CBPurchasePaymentOrder GetEntity(int SysNo)
        {
            CBPurchasePaymentOrder order = GetEntityBySysNo(SysNo) as CBPurchasePaymentOrder;
            if (order != null)
            {
                //order.PaymentItems = Hyt.DataAccess.Oracle.Finance.FnPaymentVoucherDaoImpl.Instance.GetVoucherItems(order.PVSysNo);
                order.PurchaseOrderItems = GetOrderItems(SysNo);
            }
            return order;
        }
        /// <summary>
        /// 获取采购付款单编号
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override Hyt.Model.Procurement.FnPurchasePaymentOrder GetEntityBySysNo(int SysNo)
        {
            string sql = " select * from  FnPurchasePaymentOrder where SysNo='" + SysNo + "'";
            return Context.Sql(sql).QuerySingle<CBPurchasePaymentOrder>();
        }
        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="pager"></param>
        public override void GetPmPointsOrderPager(ref Model.Pager<Model.Procurement.CBPurchasePaymentOrder> pager)
        {
            string sqlTable = " FnPurchasePaymentOrder ";
            string sqlSelect = @" *,
                                BankPaymentInfo=stuff((
                                        SELECT distinct '|' + CompanyName+','+PayBankName+','+PayBankIDCard+','+ Convert(varchar(50), PaymentAmount)
                                        FROM FnPurchasePaymentOrderItem
                                        WHERE FnPurchasePaymentOrderItem.PSysNo = [FnPurchasePaymentOrder].SysNo
        
                                        FOR xml path('')
                                       ) , 1, 1, '') ";
            string sqlWhere = "";

            var dataList = Context.Select<CBPurchasePaymentOrder>(sqlSelect).From(sqlTable);
            var dataCount = Context.Select<int>("count(0)").From(sqlTable);

            var rows = dataList.OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            var totalRows = dataCount.QuerySingle();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
        }

        public override List<CBPmPointsOrderItem> GetPurPointsOrderItemData(string ManuText, int pmSysNo)
        {
            throw new NotImplementedException();
        }

        #region 获取其采购单明细
        public override int InsertItem(FnPurchasePaymentOrderItem item)
        {
            return Context.Insert("FnPurchasePaymentOrderItem", item).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateItem(FnPurchasePaymentOrderItem item)
        {
            Context.Update("FnPurchasePaymentOrderItem", item).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override List<CBFnPurchasePaymentOrderItem> GetOrderItems(int PSysNo)
        {
            string sql = " select * from FnPurchasePaymentOrderItem where PSysNo = '"+PSysNo+"' ";
            return Context.Sql(sql).QueryMany<CBFnPurchasePaymentOrderItem>();
        }

        public override void DeleteItem(int SysNo)
        {
            string sql = " delete from FnPurchasePaymentOrderItem where sysno='" + SysNo + "' ";
            Context.Sql(sql).Execute();
        }
        #endregion
        /// <summary>
        /// 获取未登记付款单的采购登记单数据
        /// </summary>
        /// <returns></returns>
        public override List<PmProcurementOrder> ProcurementOrderByNotInPurchase()
        {
            string sql = @" select PmProcurementOrder.SysNo , PmProcurementOrder.Po_Number from FnPurchasePaymentOrder
                            right join PmProcurementOrder on  FnPurchasePaymentOrder.ProcurementSysNo=PmProcurementOrder.SysNo
                            where FnPurchasePaymentOrder.SysNo is null and Po_Status>1";
            return Context.Sql(sql).QueryMany<PmProcurementOrder>();
        }
    }
}
