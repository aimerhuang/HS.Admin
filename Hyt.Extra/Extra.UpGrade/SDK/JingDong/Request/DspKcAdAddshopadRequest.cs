using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspKcAdAddshopadRequest : IJdRequest<DspKcAdAddshopadResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                                                                                    		public  		string
   name  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   skuId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   imgUrl  { get; set; }
                                                                                                                                                                                                
public   		Nullable<long>
   adGroupId  { get; set; }

                  
                                                            
                                                          
public   		string
   url  { get; set; }

                  
                                                            
                                                                                                                            
public   		string
   showSalesWord  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.kc.ad.addshopad";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("name", this.name);
			parameters.Add("skuId", this.skuId);
			parameters.Add("imgUrl", this.imgUrl);
			parameters.Add("adGroupId", this.adGroupId);
			parameters.Add("url", this.url);
			parameters.Add("showSalesWord", this.showSalesWord);
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








        
 

