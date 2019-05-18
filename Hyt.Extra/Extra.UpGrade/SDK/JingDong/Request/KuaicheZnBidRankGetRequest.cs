using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class KuaicheZnBidRankGetRequest : IJdRequest<KuaicheZnBidRankGetResponse>
{
		                                                                                                       
public   		string
   planJson  { get; set; }

                  
                                                                                                                                    
                                                                                                                      
public   		Nullable<long>
   cid  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   kwgId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   planDate  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.kuaiche.zn.bid.rank.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("plan_json", this.planJson);
			parameters.Add("cid", this.cid);
			parameters.Add("kwg_id", this.kwgId);
			parameters.Add("plan_date", this.planDate);
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








        
 

