using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LdopAlphaWaybillUnbindRequest : IJdRequest<LdopAlphaWaybillUnbindResponse>
{
		                                                                                                                                  
public   		string
   platformOrderNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   providerId  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operatorTime  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                           		public  		string
   waybillCode  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ldop.alpha.waybill.unbind";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("platformOrderNo", this.platformOrderNo);
			parameters.Add("providerId", this.providerId);
			parameters.Add("operatorName", this.operatorName);
			parameters.Add("operatorTime", this.operatorTime);
			parameters.Add("waybillCode", this.waybillCode);
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








        
 

