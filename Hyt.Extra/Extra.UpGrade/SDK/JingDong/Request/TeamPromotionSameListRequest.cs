using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TeamPromotionSameListRequest : IJdRequest<TeamPromotionSameListResponse>
{
		                                                                      
public   		Nullable<long>
   teamId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   cityId  { get; set; }

                  
                                                            
                                                          
public   		string
   client  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.team.promotion.same.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("teamId", this.teamId);
			parameters.Add("cityId", this.cityId);
			parameters.Add("client", this.client);
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








        
 

