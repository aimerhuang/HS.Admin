using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TeamCurrentListRequest : IJdRequest<TeamCurrentListResponse>
{
		                                                                      
public   		Nullable<long>
   cityId  { get; set; }

                  
                                                            
                                                          
public   		string
   teamType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   areaId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   sqId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   groupId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   group2Id  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   sort  { get; set; }

                  
                                                            
                                                          
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
			return "jingdong.team.current.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("cityId", this.cityId);
			parameters.Add("teamType", this.teamType);
			parameters.Add("areaId", this.areaId);
			parameters.Add("sqId", this.sqId);
			parameters.Add("groupId", this.groupId);
			parameters.Add("group2Id", this.group2Id);
			parameters.Add("sort", this.sort);
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








        
 

