using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiSnSendRequest : IJdRequest<EdiSnSendResponse>
{
		                                                                                                                                  
public   		string
   purchaseOrderCode  { get; set; }

                  
                                                            
                                                          
public   		string
   orderCode  { get; set; }

                  
                                                            
                                                          
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                          
public   		string
   vendorName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   recordCount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   categoryNumber  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   totalNubmer  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   totalAmount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   actualTotalAmount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   dispatchDate  { get; set; }

                  
                                                            
                                                          
public   		string
   receivingAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   transportationMode  { get; set; }

                  
                                                            
                                                          
public   		string
   station  { get; set; }

                  
                                                            
                                                          
public   		string
   purchaseContact  { get; set; }

                  
                                                            
                                                          
public   		string
   shipmentNumber  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   shipmentPackageNumber  { get; set; }

                  
                                                            
                                                          
public   		string
   paymentMode  { get; set; }

                  
                                                            
                                                          
public   		string
   ransportationCostsPerson  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   returnPeriod  { get; set; }

                  
                                                            
                                                          
public   		string
   comments  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   currentRecordCount  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   vendorSku  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   buyerProductId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   productCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   productName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   quantity  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   salePrice  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   listPrice  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   discountRate  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   backOrderProcessing  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   packageNumber  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   lineComments  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.sn.send";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("purchaseOrderCode", this.purchaseOrderCode);
			parameters.Add("orderCode", this.orderCode);
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("vendorName", this.vendorName);
			parameters.Add("recordCount", this.recordCount);
			parameters.Add("categoryNumber", this.categoryNumber);
			parameters.Add("totalNubmer", this.totalNubmer);
			parameters.Add("totalAmount", this.totalAmount);
			parameters.Add("actualTotalAmount", this.actualTotalAmount);
			parameters.Add("dispatchDate", this.dispatchDate);
			parameters.Add("receivingAddress", this.receivingAddress);
			parameters.Add("transportationMode", this.transportationMode);
			parameters.Add("station", this.station);
			parameters.Add("purchaseContact", this.purchaseContact);
			parameters.Add("shipmentNumber", this.shipmentNumber);
			parameters.Add("shipmentPackageNumber", this.shipmentPackageNumber);
			parameters.Add("paymentMode", this.paymentMode);
			parameters.Add("ransportationCostsPerson", this.ransportationCostsPerson);
			parameters.Add("returnPeriod", this.returnPeriod);
			parameters.Add("comments", this.comments);
			parameters.Add("currentRecordCount", this.currentRecordCount);
			parameters.Add("vendorSku", this.vendorSku);
			parameters.Add("buyerProductId", this.buyerProductId);
			parameters.Add("productCode", this.productCode);
			parameters.Add("productName", this.productName);
			parameters.Add("quantity", this.quantity);
			parameters.Add("salePrice", this.salePrice);
			parameters.Add("listPrice", this.listPrice);
			parameters.Add("discountRate", this.discountRate);
			parameters.Add("backOrderProcessing", this.backOrderProcessing);
			parameters.Add("packageNumber", this.packageNumber);
			parameters.Add("lineComments", this.lineComments);
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








        
 

