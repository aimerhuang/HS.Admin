using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopCustomsCenterServiceCallbackJsfServiceServiceCallbackRequest : IJdRequest<PopCustomsCenterServiceCallbackJsfServiceServiceCallbackResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                          
public   		string
   serviceId  { get; set; }

                  
                                                            
                                                          
public   		string
   customsId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   orderStatus  { get; set; }

                  
                                                            
                                                          
public   		string
   orderDesc  { get; set; }

                  
                                                            
                                                                                           
public   		string
   goodsCheck  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.customs.center.ServiceCallbackJsfService.serviceCallback";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderId", this.orderId);
			parameters.Add("serviceId", this.serviceId);
			parameters.Add("customsId", this.customsId);
			parameters.Add("orderStatus", this.orderStatus);
			parameters.Add("orderDesc", this.orderDesc);
			parameters.Add("goodsCheck", this.goodsCheck);
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








        
 

