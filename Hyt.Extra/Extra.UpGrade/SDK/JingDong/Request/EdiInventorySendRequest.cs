using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiInventorySendRequest : IJdRequest<EdiInventorySendResponse>
{
		                                                                                                                                  
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                          
public   		string
   vendorName  { get; set; }

                  
                                                            
                                                          
public   		string
   vendorProductId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   inventoryDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   totalQuantity  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   estimateDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   totalEstimateQuantity  { get; set; }

                  
                                                            
                                                          
public   		string
   costPrice  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   storeId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   storeName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   quantity  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   estimateQuantity  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.inventory.send";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("vendorName", this.vendorName);
			parameters.Add("vendorProductId", this.vendorProductId);
			parameters.Add("inventoryDate", this.inventoryDate);
			parameters.Add("totalQuantity", this.totalQuantity);
			parameters.Add("estimateDate", this.estimateDate);
			parameters.Add("totalEstimateQuantity", this.totalEstimateQuantity);
			parameters.Add("costPrice", this.costPrice);
			parameters.Add("storeId", this.storeId);
			parameters.Add("storeName", this.storeName);
			parameters.Add("quantity", this.quantity);
			parameters.Add("estimateQuantity", this.estimateQuantity);
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








        
 

