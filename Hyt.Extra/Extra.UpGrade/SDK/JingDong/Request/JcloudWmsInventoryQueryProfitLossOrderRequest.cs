using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsInventoryQueryProfitLossOrderRequest : IJdRequest<JcloudWmsInventoryQueryProfitLossOrderResponse>
{
		                                                                                                                                                                                                    
public   		string
   inventoryNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   surplusDeficitType  { get; set; }

                  
                                                            
                                                          
public   		string
   TimeStart  { get; set; }

                  
                                                            
                                                          
public   		string
   TimeEnd  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   tenantId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.inventory.queryProfitLossOrder";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("inventoryNo", this.inventoryNo);
			parameters.Add("surplusDeficitType", this.surplusDeficitType);
			parameters.Add("TimeStart", this.TimeStart);
			parameters.Add("TimeEnd", this.TimeEnd);
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








        
 

