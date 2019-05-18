using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SmartOpenNotifyActionRequest : IJdRequest<SmartOpenNotifyActionResponse>
{
		                                                                      
public   		Nullable<long>
   feedId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   accessKey  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   action  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.smart.open.notifyAction";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("feed_id", this.feedId);
			parameters.Add("access_key", this.accessKey);
			parameters.Add("action", this.action);
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








        
 

