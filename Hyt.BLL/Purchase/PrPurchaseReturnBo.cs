using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Purchase;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Basic;
using Hyt.BLL.Finance;
using Hyt.Model.Generated;

namespace Hyt.BLL.Purchase
{
    /// <summary>
    /// 采购单
    /// </summary>
    /// <remarks>2016-6-15 杨浩 创建</remarks>
    public class PrPurchaseReturnBo : BOBase<PrPurchaseReturnBo>
    {
        /// <summary>
        /// 添加采购退货单
        /// </summary>
        /// <param name="purchase">采购单实体类对象</param>
        /// <returns></returns>
        public void AddPurchaseReturn(PrPurchaseReturn PurchaseReturn)
        {
            var sysNo = 0;
            PurchaseReturn.ReturnQuantity = PurchaseReturn.PurchaseReturnDetails.Sum(x => x.ReturnQuantity);
            PurchaseReturn.ReturnTotalMoney = PurchaseReturn.PurchaseReturnDetails.Sum(x => x.ReturnTotalMoney);
            if (PurchaseReturn.SysNo > 0)
            {
                IPrPurchaseReturnDao.Instance.UpdatePurchaseReturn(PurchaseReturn);
                sysNo = PurchaseReturn.SysNo;
            }
            else
                sysNo = IPrPurchaseReturnDao.Instance.AddPurchaseReturn(PurchaseReturn);

            if (sysNo > 0)
            {
                foreach (var item in PurchaseReturn.PurchaseReturnDetails)
                {
                    item.PurchaseReturnSysNo = sysNo;
                    if (item.SysNo <= 0)
                        IPrPurchaseReturnDetailsDao.Instance.AddPrPurchaseReturnDetails(item);
                    else
                        IPrPurchaseReturnDetailsDao.Instance.UpdatePrPurchaseReturnDetails(item);
                }
            }

        }
        /// <summary>
        /// 获取采购退货单详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-18 王耀发 创建</remarks>
        public PrPurchaseReturn GetPrPurchaseReturnInfo(int sysNo)
        {
            var purchaseReturn = IPrPurchaseReturnDao.Instance.GetPrPurchaseReturn(sysNo);
            purchaseReturn.PurchaseReturnDetails = IPrPurchaseReturnDetailsDao.Instance.GetPurchaseReturnDetailsList(sysNo);
            return purchaseReturn;
        }
        /// <summary>
        /// 查询采购退货单
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        public PagedList<CBPrPurchaseReturn> QueryPrPurchaseReturn(ParaPrPurchaseReturnFilter para)
        {
            PagedList<CBPrPurchaseReturn> model = null;
            if (para != null)
            {
                model = new PagedList<CBPrPurchaseReturn>();
                var pager = IPrPurchaseReturnDao.Instance.QueryPrPurchaseReturn(para);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = para.Id;
                }
            }
            return model;
        }

        /// <summary>
        /// 删除采购退货单
        /// </summary>
        /// <param name="sysNos">采购单系统编号(多个已‘,'分隔)</param>
        /// <remarks>2016-6-20 王耀发 创建</remarks>
        public void DeletePrPurchaseReturn(string sysNos)
        {
            IPrPurchaseReturnDetailsDao.Instance.DeleteByPurchaseReturnSysNos(sysNos);
            IPrPurchaseReturnDao.Instance.Delete(sysNos);
        }
        /// <summary>
        /// 审核采购退货单
        /// </summary>
        /// <param name="sysNo">采购单编号</param>
        /// <remarks>2016-6-18 王耀发 创建</remarks>
        public bool AuditPurchaseReturn(int sysNo)
        {
            int CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
            var purchaseReturn = IPrPurchaseReturnDao.Instance.GetPrPurchaseReturn(sysNo);
            if (UpdateStatus(sysNo, (int)PurchaseStatus.采购退货单状态.已审核))
            {
                var receiptVoucherMod = new FnReceiptVoucher();
                receiptVoucherMod.TransactionSysNo = "";
                receiptVoucherMod.IncomeType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单收入类型.预付;
                receiptVoucherMod.Source = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.采购退货单;
                receiptVoucherMod.SourceSysNo = sysNo;
                receiptVoucherMod.IncomeAmount = purchaseReturn.ReturnTotalMoney;
                receiptVoucherMod.ReceivedAmount = 0;
                receiptVoucherMod.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单状态.待确认;
                receiptVoucherMod.Remark = "采购退货收款单";
                receiptVoucherMod.CreatedBy = CreatedBy;
                receiptVoucherMod.CreatedDate = DateTime.Now;
                receiptVoucherMod.ConfirmedBy = 0;
                receiptVoucherMod.ConfirmedDate = DateTime.Now;
                receiptVoucherMod.LastUpdateBy = CreatedBy;
                receiptVoucherMod.LastUpdateDate = DateTime.Now;
                receiptVoucherMod.PaymentType = null;
                FinanceBo.Instance.InsertReceiptVoucher(receiptVoucherMod);
                //创建出库单
                WhInventoryOut outEntity = new WhInventoryOut
                {
                    TransactionSysNo = ReceiptNumberBo.Instance.GetPurchaseOutNo(),
                    WarehouseSysNo = purchaseReturn.WarehouseSysNo,
                    SourceType = (int)WarehouseStatus.出库单来源.采购单,
                    SourceSysNO = sysNo,
                    DeliveryType = 0,
                    Remarks = "采购退货出库",
                    IsPrinted = 0,
                    Status = (int)WarehouseStatus.采购退货出库单状态.待出库,
                    CreatedBy = CreatedBy,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = CreatedBy,
                    LastUpdateDate = DateTime.Now,
                    Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                };

                outEntity.ItemList = new List<WhInventoryOutItem>();
                var purchaseReturnDetails = BLL.Purchase.PrPurchaseReturnDetailsBo.Instance.GetPurchaseReturnDetailsList(sysNo);
                PrPurchase purMod = PrPurchaseBo.Instance.GetPrPurchaseInfo(purchaseReturn.PurchaseSysNo);

                //出库明细
                foreach (var item in purchaseReturnDetails)
                {
                    Hyt.Model.PdProduct Product = Hyt.BLL.Product.PdProductBo.Instance.GetProductBySysNo(item.ProductSysNo);
                    outEntity.ItemList.Add(new WhInventoryOutItem
                    {
                        ProductSysNo = item.ProductSysNo,
                        ProductName = Product.ProductName,
                        StockOutQuantity = item.ReturnQuantity,
                        RealStockOutQuantity = 0,
                        SourceItemSysNo = item.SysNo,//记录出库单明细来源单号（采购退货单明细编号)
                        Remarks = "",
                        CreatedBy = CreatedBy,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = CreatedBy,
                        LastUpdateDate = DateTime.Now
                    });

                }
                var inSysNo = WhInventoryOutBo.Instance.CreateWhInventoryOut(outEntity); //保存出库单数据      
            }
            return true;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="sysNo">采购退货单系统编号</param>
        /// <param name="statusType">状态 :待审核（10）、已审核（20）、作废（-10）</param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateStatus(int sysNo, int status)
        {
            return IPrPurchaseReturnDao.Instance.UpdateStatus(sysNo, status);
        }
        /// <summary>
        /// 更新采购退货单已出库数
        /// </summary>
        /// <param name="sysNo">采购退货单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-6-21 王耀发 创建</remarks>
        public bool UpdateOutQuantity(int sysNo)
        {
            return IPrPurchaseReturnDao.Instance.UpdateOutQuantity(sysNo);
        }
    }
}
