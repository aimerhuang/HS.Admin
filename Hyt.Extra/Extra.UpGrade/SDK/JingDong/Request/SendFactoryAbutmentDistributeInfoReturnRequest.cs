using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SendFactoryAbutmentDistributeInfoReturnRequest : IJdRequest<SendFactoryAbutmentDistributeInfoReturnResponse>
{
		                                                                      
public   		string
   authorizedSequence  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   orderno  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   distributeTime  { get; set; }

                  
                                                            
                                                          
public   		string
   distributeOutletsName  { get; set; }

                  
                                                            
                                                          
public   		string
   distributeOutletsPhone  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.sendFactoryAbutmentDistributeInfoReturn";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("authorizedSequence", this.authorizedSequence);
			parameters.Add("orderno", this.orderno);
			parameters.Add("distributeTime", this.distributeTime);
			parameters.Add("distributeOutletsName", this.distributeOutletsName);
			parameters.Add("distributeOutletsPhone", this.distributeOutletsPhone);
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








        
 

