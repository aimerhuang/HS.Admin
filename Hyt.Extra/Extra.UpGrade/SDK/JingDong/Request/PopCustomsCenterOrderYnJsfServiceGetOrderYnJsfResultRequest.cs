using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopCustomsCenterOrderYnJsfServiceGetOrderYnJsfResultRequest : IJdRequest<PopCustomsCenterOrderYnJsfServiceGetOrderYnJsfResultResponse>
{
		                                                                                                                                  
public   		string
   customsId  { get; set; }

                  
                                                            
                                                          
public   		string
   serviceId  { get; set; }

                  
                                                            
                                                                                                                                                             
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.customs.center.OrderYnJsfService.getOrderYnJsfResult";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("customsId", this.customsId);
			parameters.Add("serviceId", this.serviceId);
			parameters.Add("orderId", this.orderId);
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








        
 

