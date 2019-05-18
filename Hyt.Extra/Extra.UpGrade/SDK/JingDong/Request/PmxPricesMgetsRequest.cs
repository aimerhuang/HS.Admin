using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PmxPricesMgetsRequest : IJdRequest<PmxPricesMgetsResponse>
{
		                                                                      
public   		string
   skuids  { get; set; }

                  
                                                            
                                                          
public   		string
   source  { get; set; }

                  
                                                            
                                                          
public   		string
   area  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pmx.prices.mgets";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("skuids", this.skuids);
			parameters.Add("source", this.source);
			parameters.Add("area", this.area);
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








        
 

