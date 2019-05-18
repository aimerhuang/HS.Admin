using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TeamKeywordListRequest : IJdRequest<TeamKeywordListResponse>
{
		                                                                      
public   		string
   keyWord  { get; set; }

                  
                                                            
                                                          
public   		string
   cityName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   start  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   limit  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   isTeamExternalUrl  { get; set; }

                  
                                                            
                                                          
public   		string
   client  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.team.keyword.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("keyWord", this.keyWord);
			parameters.Add("cityName", this.cityName);
			parameters.Add("start", this.start);
			parameters.Add("limit", this.limit);
			parameters.Add("isTeamExternalUrl", this.isTeamExternalUrl);
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








        
 

