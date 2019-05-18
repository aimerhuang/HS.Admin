
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
	public partial class ICStockBill : BaseEntity
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
		public int FTranType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FDate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FUse { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FNote { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FDCStockID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSCStockID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FDeptID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FEmpID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSupplyID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPosterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FCheckerID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FFManagerID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSManagerID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FBillerID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FReturnBillInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FSCBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FHookInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FVchInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPosted { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FCheckSelect { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FCurrencyID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSaleStyle { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FAcctID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FROB { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FRSCBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FStatus { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public bool FUpStockWhenSave { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public bool FCancellation { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FOrgBillInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FBillTypeID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPOStyle { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FMultiCheckLevel1 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FMultiCheckLevel2 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FMultiCheckLevel3 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FMultiCheckLevel4 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FMultiCheckLevel5 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FMultiCheckLevel6 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FMultiCheckDate1 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FMultiCheckDate2 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FMultiCheckDate3 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FMultiCheckDate4 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FMultiCheckDate5 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FMultiCheckDate6 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FCurCheckLevel { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FTaskID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FResourceID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public bool FBackFlushed { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FWBInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FTranStatus { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FZPBillInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FRelateBrID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPurposeID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FUUID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FRelateInvoiceID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FOperDate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FImport { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSystemType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FMarketingStyle { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPayBillID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FCheckDate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FExplanation { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FFetchAdd { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FFetchDate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FManagerID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FRefType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSelTranType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FChildren { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FHookStatus { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FActPriceVchTplID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPlanPriceVchTplID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FProcID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FActualVchTplID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPlanVchTplID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FBrID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FVIPCardID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FVIPScore { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FHolisticDiscountRate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FPOSName { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FWorkShiftId { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FCussentAcctID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public bool FZanGuCount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FPOOrdBillNo { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FLSSrcInterID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FSettleDate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FManageType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FOrderAffirm { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FAutoCreType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FConsignee { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FDrpRelateTranType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPrintCount { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSourceType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public bool FDiscountType { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FAutoCreatePeriod { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FReceiver { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FBuyerMessage { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FSellerMessage { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FBuyerNick { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FTelephone { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FMobile { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FDeliveryProvince { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FDeliveryCity { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FDeliveryDistrict { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FDeliveryAddress { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPeerStateCancelVerification { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FWLNumber { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string FWLCompany { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FSettCycle { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FIsFromPos { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FClassTypeID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FPOMode { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FShopFManagerID { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public DateTime FShopArriveDate { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int FWeiRetail { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal FHeadSelfB0159 { get; set; }
 	}
}

	