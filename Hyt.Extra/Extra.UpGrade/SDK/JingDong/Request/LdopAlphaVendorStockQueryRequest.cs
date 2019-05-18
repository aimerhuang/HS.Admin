using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LdopAlphaVendorStockQueryRequest : IJdRequest<LdopAlphaVendorStockQueryResponse>
{
		                                                                      
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                          
public   		string
   providerCode  { get; set; }

                  
                                                            
                                                          
public   		string
   branchCode  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ldop.alpha.vendor.stock.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("providerCode", this.providerCode);
			parameters.Add("branchCode", this.branchCode);
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








        
 

