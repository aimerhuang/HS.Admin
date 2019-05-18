using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspAdkckeywordRecommendkeywordGetRequest : IJdRequest<DspAdkckeywordRecommendkeywordGetResponse>
{
		                                                                      
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<int>
   searchType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   order  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   sortType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   keyWordType  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.adkckeyword.recommendkeyword.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("skuId", this.skuId);
			parameters.Add("searchType", this.searchType);
			parameters.Add("order", this.order);
			parameters.Add("sortType", this.sortType);
			parameters.Add("keyWordType", this.keyWordType);
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








        
 

