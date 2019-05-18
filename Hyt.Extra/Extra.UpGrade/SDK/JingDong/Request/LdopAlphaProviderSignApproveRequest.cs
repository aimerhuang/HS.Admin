using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LdopAlphaProviderSignApproveRequest : IJdRequest<LdopAlphaProviderSignApproveResponse>
{
		                                                                                                                                  
public   		string
   requestId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   approveResult  { get; set; }

                  
                                                            
                                                          
public   		string
   approveMessage  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ldop.alpha.provider.sign.approve";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("requestId", this.requestId);
			parameters.Add("approveResult", this.approveResult);
			parameters.Add("approveMessage", this.approveMessage);
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








        
 

