
using System;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2017-08-02 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class ICStockBillEntry : BaseEntity
	{
	  
		/// <summary>
		/// 
		/// </summary>
		public string FBrNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FEntryID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FItemID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FQtyMust { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FPrice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FBatchNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAmount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FNote { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSCBillInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FSCBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FUnitID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxPrice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxQtyMust { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FQtyActual { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxQtyActual { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FPlanPrice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxPlanPrice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSourceEntryID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FKFDate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FKFPeriod { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FDCSPID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSCSPID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FConsignPrice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FConsignAmount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FProcessCost { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FMaterialCost { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FTaxAmount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FMapNumber { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FMapName { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FOrgBillEntryID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FOperID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FPlanAmount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FProcessPrice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FTaxRate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSnListID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAmtRef { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FAuxPropID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FCost { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FPriceRef { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxPriceRef { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FFetchDate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FQtyInvoice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FQtyInvoiceBase { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FUnitCost { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSecCoefficient { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSecQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSecCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSourceTranType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSourceInterId { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FSourceBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FContractInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FContractEntryID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FContractBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FICMOBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FICMOInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPPBomEntryID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FOrderInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FOrderEntryID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FOrderBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAllHookQTY { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAllHookAmount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FCurrentHookQTY { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FCurrentHookAmount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FStdAllHookAmount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FStdCurrentHookAmount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSCStockID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FDCStockID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FPeriodDate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FCostObjGroupID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FCostOBJID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FDetailID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FMaterialCostPrice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FReProduceType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FBomInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FDiscountRate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FDiscountAmount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSepcialSaleId { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FOutCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FOutSecCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FDBCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FDBSecCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxQtyInvoice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FOperSN { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FCheckStatus { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSplitSecQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FInStockID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSaleCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSaleSecCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSaleAuxCommitQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSelectedProcID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FVWInStockQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxVWInStockQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSecVWInStockQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSecInvoiceQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FCostCenterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPlanMode { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FMTONo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSecQtyActual { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FSecQtyMust { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FClientOrderNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FClientEntryID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FRowClosed { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FCostPercentage { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FOLOrderBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FNeedPickQTY { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FInvStockQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FMachinePos { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FUniDiscount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FBarCode_EntrySelfA0156 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FChkPassItem { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FBuyerFreight { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FComplexQty { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FPDASn { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FBarCode { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FAuxTaxPrice { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FComCategoryID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FComBrandID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FID_Request { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FEntryID_Request { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FRecOK { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSrcSupplyID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSrcSettcycle { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSrcEmpID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FID_SRC { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FClassID_SRC { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FEntryID_SRC { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FBillNo_SRC { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FBCStockID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FBCSPID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FEntrySelfB0167 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FEntrySelfB0169 { get; set; }
 	}
}

	