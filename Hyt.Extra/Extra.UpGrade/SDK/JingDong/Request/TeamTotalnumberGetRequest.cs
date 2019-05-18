using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TeamTotalnumberGetRequest : IJdRequest<TeamTotalnumberGetResponse>
{
		                                                                                                                                  
public   		string
   cityId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   groupId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   teamType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   districtId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   areaId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   group2Id  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   isTeamExternalUrl  { get; set; }

                  
                                                                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.team.totalnumber.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("city_id", this.cityId);
			parameters.Add("group_id", this.groupId);
			parameters.Add("team_type", this.teamType);
			parameters.Add("district_id", this.districtId);
			parameters.Add("area_id", this.areaId);
			parameters.Add("group2_id", this.group2Id);
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








        
 

