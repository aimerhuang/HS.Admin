using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareProductAllQueryRequest : IJdRequest<WareProductAllQueryResponse>
{
		                                                                      
public   		string
   skuStatus  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   thirdCid  { get; set; }

                  
                                                            
                                                          
public   		string
   scrollId  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ware.product.all.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("sku_status", this.skuStatus);
			parameters.Add("thirdCid", this.thirdCid);
			parameters.Add("scrollId", this.scrollId);
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








        
 

