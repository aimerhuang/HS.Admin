using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LdopReceiveOrderInterceptRequest : IJdRequest<LdopReceiveOrderInterceptResponse>
{
		                                                                                                                                  
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                          
public   		string
   deliveryId  { get; set; }

                  
                                                            
                                                          
public   		string
   interceptReason  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ldop.receive.order.intercept";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("deliveryId", this.deliveryId);
			parameters.Add("interceptReason", this.interceptReason);
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








        
 

