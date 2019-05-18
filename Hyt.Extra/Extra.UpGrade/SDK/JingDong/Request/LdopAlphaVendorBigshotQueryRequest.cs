using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LdopAlphaVendorBigshotQueryRequest : IJdRequest<LdopAlphaVendorBigshotQueryResponse>
{
		                                                                                                                                  
public   		string
   waybillCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   providerId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ldop.alpha.vendor.bigshot.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("waybillCode", this.waybillCode);
			parameters.Add("providerId", this.providerId);
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








        
 

