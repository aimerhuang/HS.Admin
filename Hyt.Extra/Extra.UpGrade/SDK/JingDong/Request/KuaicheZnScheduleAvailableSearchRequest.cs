using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class KuaicheZnScheduleAvailableSearchRequest : IJdRequest<KuaicheZnScheduleAvailableSearchResponse>
{
		                                                                      
public   		Nullable<long>
   spaceId  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.kuaiche.zn.schedule.available.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("space_id", this.spaceId);
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








        
 

