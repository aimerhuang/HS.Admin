using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpOrderAddOrderRequest : IJdRequest<EclpOrderAddOrderResponse>
{
		                                                                                                                                  
public   		string
   isvUUID  { get; set; }

                  
                                                            
                                                          
public   		string
   isvSource  { get; set; }

                  
                                                            
                                                          
public   		string
   shopNo  { get; set; }

                  
                                                            
                                                          
public   		string
   bdOwnerNo  { get; set; }

                  
                                                            
                                                          
public   		string
   departmentNo  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   shipperNo  { get; set; }

                  
                                                            
                                                          
public   		string
   salesPlatformOrderNo  { get; set; }

                  
                                                            
                                                          
public   		string
   salePlatformSource  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   salesPlatformCreateTime  { get; set; }

                  
                                                            
                                                          
public   		string
   soType  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneeName  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneeMobile  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneePhone  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneeEmail  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   expectDate  { get; set; }

                  
                                                            
                                                          
public   		string
   addressProvince  { get; set; }

                  
                                                            
                                                          
public   		string
   addressCity  { get; set; }

                  
                                                            
                                                          
public   		string
   addressCounty  { get; set; }

                  
                                                            
                                                          
public   		string
   addressTown  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneeAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneePostcode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   receivable  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneeRemark  { get; set; }

                  
                                                            
                                                          
public   		string
   orderMark  { get; set; }

                  
                                                            
                                                          
public   		string
   thirdWayBill  { get; set; }

                  
                                                            
                                                          
public   		string
   packageMark  { get; set; }

                  
                                                            
                                                          
public   		string
   businessType  { get; set; }

                  
                                                            
                                                          
public   		string
   destinationCode  { get; set; }

                  
                                                            
                                                          
public   		string
   destinationName  { get; set; }

                  
                                                            
                                                          
public   		string
   sendWebsiteCode  { get; set; }

                  
                                                            
                                                          
public   		string
   sendWebsiteName  { get; set; }

                  
                                                            
                                                          
public   		string
   sendMode  { get; set; }

                  
                                                            
                                                          
public   		string
   receiveMode  { get; set; }

                  
                                                            
                                                          
public   		string
   appointDeliveryTime  { get; set; }

                  
                                                            
                                                          
public   		string
   insuredPriceFlag  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   insuredValue  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   insuredFee  { get; set; }

                  
                                                            
                                                          
public   		string
   thirdPayment  { get; set; }

                  
                                                            
                                                          
public   		string
   monthlyAccount  { get; set; }

                  
                                                            
                                                          
public   		string
   shipment  { get; set; }

                  
                                                            
                                                          
public   		string
   sellerRemark  { get; set; }

                  
                                                            
                                                          
public   		string
   thirdSite  { get; set; }

                  
                                                            
                                                          
public   		string
   customsStatus  { get; set; }

                  
                                                            
                                                          
public   		string
   customerName  { get; set; }

                  
                                                            
                                                          
public   		string
   invoiceTitle  { get; set; }

                  
                                                            
                                                          
public   		string
   invoiceContent  { get; set; }

                  
                                                            
                                                          
public   		string
   goodsType  { get; set; }

                  
                                                            
                                                          
public   		string
   goodsLevel  { get; set; }

                  
                                                            
                                                          
public   		string
   customsPort  { get; set; }

                  
                                                            
                                                          
public   		string
   billType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   orderPrice  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                             		public  		string
   goodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   price  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   quantity  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.order.addOrder";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("isvUUID", this.isvUUID);
			parameters.Add("isvSource", this.isvSource);
			parameters.Add("shopNo", this.shopNo);
			parameters.Add("bdOwnerNo", this.bdOwnerNo);
			parameters.Add("departmentNo", this.departmentNo);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("shipperNo", this.shipperNo);
			parameters.Add("salesPlatformOrderNo", this.salesPlatformOrderNo);
			parameters.Add("salePlatformSource", this.salePlatformSource);
			parameters.Add("salesPlatformCreateTime", this.salesPlatformCreateTime);
			parameters.Add("soType", this.soType);
			parameters.Add("consigneeName", this.consigneeName);
			parameters.Add("consigneeMobile", this.consigneeMobile);
			parameters.Add("consigneePhone", this.consigneePhone);
			parameters.Add("consigneeEmail", this.consigneeEmail);
			parameters.Add("expectDate", this.expectDate);
			parameters.Add("addressProvince", this.addressProvince);
			parameters.Add("addressCity", this.addressCity);
			parameters.Add("addressCounty", this.addressCounty);
			parameters.Add("addressTown", this.addressTown);
			parameters.Add("consigneeAddress", this.consigneeAddress);
			parameters.Add("consigneePostcode", this.consigneePostcode);
			parameters.Add("receivable", this.receivable);
			parameters.Add("consigneeRemark", this.consigneeRemark);
			parameters.Add("orderMark", this.orderMark);
			parameters.Add("thirdWayBill", this.thirdWayBill);
			parameters.Add("packageMark", this.packageMark);
			parameters.Add("businessType", this.businessType);
			parameters.Add("destinationCode", this.destinationCode);
			parameters.Add("destinationName", this.destinationName);
			parameters.Add("sendWebsiteCode", this.sendWebsiteCode);
			parameters.Add("sendWebsiteName", this.sendWebsiteName);
			parameters.Add("sendMode", this.sendMode);
			parameters.Add("receiveMode", this.receiveMode);
			parameters.Add("appointDeliveryTime", this.appointDeliveryTime);
			parameters.Add("insuredPriceFlag", this.insuredPriceFlag);
			parameters.Add("insuredValue", this.insuredValue);
			parameters.Add("insuredFee", this.insuredFee);
			parameters.Add("thirdPayment", this.thirdPayment);
			parameters.Add("monthlyAccount", this.monthlyAccount);
			parameters.Add("shipment", this.shipment);
			parameters.Add("sellerRemark", this.sellerRemark);
			parameters.Add("thirdSite", this.thirdSite);
			parameters.Add("customsStatus", this.customsStatus);
			parameters.Add("customerName", this.customerName);
			parameters.Add("invoiceTitle", this.invoiceTitle);
			parameters.Add("invoiceContent", this.invoiceContent);
			parameters.Add("goodsType", this.goodsType);
			parameters.Add("goodsLevel", this.goodsLevel);
			parameters.Add("customsPort", this.customsPort);
			parameters.Add("billType", this.billType);
			parameters.Add("orderPrice", this.orderPrice);
			parameters.Add("goodsNo", this.goodsNo);
			parameters.Add("price", this.price);
			parameters.Add("quantity", this.quantity);
            parameters.AddAll(this.otherParameters);
            return parameters;
        }

        public void Validate()
        {
        }

        public void AddOtherParameter(string key, string value)
        {
            if (this.otherParameters == null)
            {
                this.otherParameters = new JdDictionary();
            }
            this.otherParameters.Add(key, value);
        }

}
}








        
 

