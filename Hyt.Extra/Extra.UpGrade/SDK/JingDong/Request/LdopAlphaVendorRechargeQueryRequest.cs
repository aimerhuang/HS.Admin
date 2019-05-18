using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LdopAlphaVendorRechargeQueryRequest : IJdRequest<LdopAlphaVendorRechargeQueryResponse>
{
		                                                                                                                                  
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   providerId  { get; set; }

                  
                                                            
                                                          
public   		string
   branchCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ldop.alpha.vendor.recharge.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("providerId", this.providerId);
			parameters.Add("branchCode", this.branchCode);
			parameters.Add("startTime", this.startTime);
			parameters.Add("endTime", this.endTime);
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








        
 

