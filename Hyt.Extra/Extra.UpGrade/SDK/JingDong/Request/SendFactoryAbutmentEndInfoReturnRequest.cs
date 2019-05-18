using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SendFactoryAbutmentEndInfoReturnRequest : IJdRequest<SendFactoryAbutmentEndInfoReturnResponse>
{
		                                                                      
public   		string
   authorizedSequence  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   orderno  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   serviceEndState  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   serviceEndStateLevelTow  { get; set; }

                  
                                                            
                                                          
public   		string
   serviceEndStateLevelTowDescribe  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   serviceEndTime  { get; set; }

                  
                                                            
                                                          
public   		string
   cancelRemark  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.sendFactoryAbutmentEndInfoReturn";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("authorizedSequence", this.authorizedSequence);
			parameters.Add("orderno", this.orderno);
			parameters.Add("serviceEndState", this.serviceEndState);
			parameters.Add("serviceEndStateLevelTow", this.serviceEndStateLevelTow);
			parameters.Add("serviceEndStateLevelTowDescribe", this.serviceEndStateLevelTowDescribe);
			parameters.Add("serviceEndTime", this.serviceEndTime);
			parameters.Add("cancelRemark", this.cancelRemark);
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








        
 

