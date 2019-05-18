using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpCloudQueryCenterUserRequest : IJdRequest<EclpCloudQueryCenterUserResponse>
{
		                                                                                                                                  
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                          
public   		string
   userId  { get; set; }

                  
                                                            
                                                          
public   		string
   origin  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.cloud.queryCenterUser";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("userId", this.userId);
			parameters.Add("origin", this.origin);
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








        
 

