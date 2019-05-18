using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class KuaicheZnPlanSearchKeywordUpdateRequest : IJdRequest<KuaicheZnPlanSearchKeywordUpdateResponse>
{
		                                                                                                                                        
public   		Nullable<long>
   planId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   keywordPrice  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.kuaiche.zn.plan.search.keyword.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("plan_id", this.planId);
			parameters.Add("keyword_price", this.keywordPrice);
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








        
 

