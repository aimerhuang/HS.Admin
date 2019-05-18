using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiPoaSendRequest : IJdRequest<EdiPoaSendResponse>
{
		                                                                                                                                  
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                          
public   		string
   vendorName  { get; set; }

                  
                                                            
                                                          
public   		string
   purchaseOrderCode  { get; set; }

                  
                                                            
                                                          
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
   purchaseDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   arrivalDate  { get; set; }

                  
                                                            
                                                          
public   		string
   purchaseContact  { get; set; }

                  
                                                            
                                                          
public   		string
   receivingAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   comments  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   currentRecordCount  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   linePurchaseOrderCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   productCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   buyerProductId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   vendorSku  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   productName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   quantity  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   orderQuantity  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   salePrice  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   listPrice  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   discountRate  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   backOrderProcessing  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   lineComments  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.poa.send";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("vendorName", this.vendorName);
			parameters.Add("purchaseOrderCode", this.purchaseOrderCode);
			parameters.Add("recordCount", this.recordCount);
			parameters.Add("categoryNumber", this.categoryNumber);
			parameters.Add("totalNubmer", this.totalNubmer);
			parameters.Add("totalAmount", this.totalAmount);
			parameters.Add("actualTotalAmount", this.actualTotalAmount);
			parameters.Add("purchaseDate", this.purchaseDate);
			parameters.Add("arrivalDate", this.arrivalDate);
			parameters.Add("purchaseContact", this.purchaseContact);
			parameters.Add("receivingAddress", this.receivingAddress);
			parameters.Add("comments", this.comments);
			parameters.Add("currentRecordCount", this.currentRecordCount);
			parameters.Add("linePurchaseOrderCode", this.linePurchaseOrderCode);
			parameters.Add("productCode", this.productCode);
			parameters.Add("buyerProductId", this.buyerProductId);
			parameters.Add("vendorSku", this.vendorSku);
			parameters.Add("productName", this.productName);
			parameters.Add("quantity", this.quantity);
			parameters.Add("orderQuantity", this.orderQuantity);
			parameters.Add("salePrice", this.salePrice);
			parameters.Add("listPrice", this.listPrice);
			parameters.Add("discountRate", this.discountRate);
			parameters.Add("backOrderProcessing", this.backOrderProcessing);
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








        
 

