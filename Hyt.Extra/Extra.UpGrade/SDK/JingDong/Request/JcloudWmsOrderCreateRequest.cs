using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsOrderCreateRequest : IJdRequest<JcloudWmsOrderCreateResponse>
{
		                                                                                                                                                                                                    
public   		string
   orderNo  { get; set; }

                  
                                                            
                                                          
public   		string
   ownerNo  { get; set; }

                  
                                                            
                                                          
public   		string
   billType  { get; set; }

                  
                                                            
                                                          
public   		string
   carrierNo  { get; set; }

                  
                                                            
                                                          
public   		string
   waybillNo  { get; set; }

                  
                                                            
                                                          
public   		string
   costTotal  { get; set; }

                  
                                                            
                                                          
public   		string
   costPaid  { get; set; }

                  
                                                            
                                                          
public   		string
   packageCenterCode  { get; set; }

                  
                                                            
                                                          
public   		string
   packageCenterName  { get; set; }

                  
                                                            
                                                          
public   		string
   shipBranchCode  { get; set; }

                  
                                                            
                                                          
public   		string
   shortAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   distributeCode  { get; set; }

                  
                                                            
                                                          
public   		string
   orderPrice  { get; set; }

                  
                                                            
                                                          
public   		string
   discountPrice  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   codFlag  { get; set; }

                  
                                                            
                                                          
public   		string
   receivable  { get; set; }

                  
                                                            
                                                          
public   		string
   notes  { get; set; }

                  
                                                            
                                                          
public   		string
   sellerNotes  { get; set; }

                  
                                                            
                                                          
public   		string
   province  { get; set; }

                  
                                                            
                                                          
public   		string
   city  { get; set; }

                  
                                                            
                                                          
public   		string
   county  { get; set; }

                  
                                                            
                                                          
public   		string
   zipcode  { get; set; }

                  
                                                            
                                                          
public   		string
   contact  { get; set; }

                  
                                                            
                                                          
public   		string
   tel  { get; set; }

                  
                                                            
                                                          
public   		string
   phone  { get; set; }

                  
                                                            
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   orderTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   paymentTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   planDeliveryTime  { get; set; }

                  
                                                            
                                                          
public   		string
   deliverType  { get; set; }

                  
                                                            
                                                          
public   		string
   sendCode  { get; set; }

                  
                                                            
                                                          
public   		string
   arriveCode  { get; set; }

                  
                                                            
                                                          
public   		string
   paymentType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   invoiceFlag  { get; set; }

                  
                                                            
                                                          
public   		string
   invoiceTitle  { get; set; }

                  
                                                            
                                                          
public   		string
   invoiceContent  { get; set; }

                  
                                                            
                                                          
public   		string
   shop  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   skuNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   skuName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   expectedQty  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   price  { get; set; }
                                                                                                                                                                                                
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   tenantId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.order.create";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderNo", this.orderNo);
			parameters.Add("ownerNo", this.ownerNo);
			parameters.Add("billType", this.billType);
			parameters.Add("carrierNo", this.carrierNo);
			parameters.Add("waybillNo", this.waybillNo);
			parameters.Add("costTotal", this.costTotal);
			parameters.Add("costPaid", this.costPaid);
			parameters.Add("packageCenterCode", this.packageCenterCode);
			parameters.Add("packageCenterName", this.packageCenterName);
			parameters.Add("shipBranchCode", this.shipBranchCode);
			parameters.Add("shortAddress", this.shortAddress);
			parameters.Add("distributeCode", this.distributeCode);
			parameters.Add("orderPrice", this.orderPrice);
			parameters.Add("discountPrice", this.discountPrice);
			parameters.Add("codFlag", this.codFlag);
			parameters.Add("receivable", this.receivable);
			parameters.Add("notes", this.notes);
			parameters.Add("sellerNotes", this.sellerNotes);
			parameters.Add("province", this.province);
			parameters.Add("city", this.city);
			parameters.Add("county", this.county);
			parameters.Add("zipcode", this.zipcode);
			parameters.Add("contact", this.contact);
			parameters.Add("tel", this.tel);
			parameters.Add("phone", this.phone);
			parameters.Add("address", this.address);
			parameters.Add("orderTime", this.orderTime);
			parameters.Add("paymentTime", this.paymentTime);
			parameters.Add("planDeliveryTime", this.planDeliveryTime);
			parameters.Add("deliverType", this.deliverType);
			parameters.Add("sendCode", this.sendCode);
			parameters.Add("arriveCode", this.arriveCode);
			parameters.Add("paymentType", this.paymentType);
			parameters.Add("invoiceFlag", this.invoiceFlag);
			parameters.Add("invoiceTitle", this.invoiceTitle);
			parameters.Add("invoiceContent", this.invoiceContent);
			parameters.Add("shop", this.shop);
			parameters.Add("skuNo", this.skuNo);
			parameters.Add("skuName", this.skuName);
			parameters.Add("expectedQty", this.expectedQty);
			parameters.Add("price", this.price);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("tenantId", this.tenantId);
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








        
 

