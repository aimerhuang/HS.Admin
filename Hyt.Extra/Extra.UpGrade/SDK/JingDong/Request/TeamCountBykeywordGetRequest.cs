using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TeamCountBykeywordGetRequest : IJdRequest<TeamCountBykeywordGetResponse>
{
		                                                                                                                                  
public   		string
   keyWord  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   cityName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   isTeamExternalUrl  { get; set; }

                  
                                                                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.team.count.bykeyword.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("key_word", this.keyWord);
			parameters.Add("city_name", this.cityName);
			parameters.Add("is_team_external_url", this.isTeamExternalUrl);
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








        
 

