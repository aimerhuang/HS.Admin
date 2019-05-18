using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SendFactoryAbutmentReceiveInfoRequest : IJdRequest<SendFactoryAbutmentReceiveInfoResponse>
{
		                                                                      
public   		string
   authorizedSequence  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   serviceType  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   orderno  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   disposeTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   disposeResult  { get; set; }

                  
                                                            
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.sendFactoryAbutmentReceiveInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("authorizedSequence", this.authorizedSequence);
			parameters.Add("serviceType", this.serviceType);
			parameters.Add("orderno", this.orderno);
			parameters.Add("disposeTime", this.disposeTime);
			parameters.Add("disposeResult", this.disposeResult);
			parameters.Add("remark", this.remark);
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








        
 

