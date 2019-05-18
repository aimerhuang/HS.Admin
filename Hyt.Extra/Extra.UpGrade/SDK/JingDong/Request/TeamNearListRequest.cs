using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TeamNearListRequest : IJdRequest<TeamNearListResponse>
{
		                                                                      
public   		Nullable<double>
   longitude  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   latitude  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   radius  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   isTeamExternalUrl  { get; set; }

                  
                                                            
                                                          
public   		string
   client  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.team.near.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("longitude", this.longitude);
			parameters.Add("latitude", this.latitude);
			parameters.Add("radius", this.radius);
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








        
 

