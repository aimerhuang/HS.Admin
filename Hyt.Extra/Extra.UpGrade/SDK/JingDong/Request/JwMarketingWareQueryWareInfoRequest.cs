using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JwMarketingWareQueryWareInfoRequest : IJdRequest<JwMarketingWareQueryWareInfoResponse>
{
		                                                                                                                                  
public   		string
   storeCode  { get; set; }

                  
                                                            
                                                                                      
public   		string
   skus  { get; set; }

                  
                                                            
                                                                                      
public   		string
   fieldNames  { get; set; }

                  
                                                            
                                                                                                   
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jw.marketing.ware.queryWareInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("storeCode", this.storeCode);
			parameters.Add("skus", this.skus);
			parameters.Add("fieldNames", this.fieldNames);
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








        
 

