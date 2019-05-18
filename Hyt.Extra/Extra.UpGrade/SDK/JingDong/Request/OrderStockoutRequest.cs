using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class OrderStockoutRequest : IJdRequest<OrderStockoutResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   venderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   completeDate  { get; set; }

                  
                                                            
                                                          
public   		string
   operName  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.orderStockout";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("venderId", this.venderId);
			parameters.Add("orderId", this.orderId);
			parameters.Add("completeDate", this.completeDate);
			parameters.Add("operName", this.operName);
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








        
 

