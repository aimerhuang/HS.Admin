using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopCustomsCenterClearanceCallbackJsfServiceCustomClearanceRequest : IJdRequest<PopCustomsCenterClearanceCallbackJsfServiceCustomClearanceResponse>
{
		                                                                                                                                  
public   		string
   customsId  { get; set; }

                  
                                                            
                                                          
public   		string
   serviceId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                         
public   		string
   orderId  { get; set; }

                  
                                                            
                                                          
public   		string
   result  { get; set; }

                  
                                                            
                                                          
public   		string
   message  { get; set; }

                  
                                                            
                                                          
public   		string
   goodsCheck  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.customs.center.ClearanceCallbackJsfService.customClearance";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("customsId", this.customsId);
			parameters.Add("serviceId", this.serviceId);
			parameters.Add("orderId", this.orderId);
			parameters.Add("result", this.result);
			parameters.Add("message", this.message);
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








        
 

