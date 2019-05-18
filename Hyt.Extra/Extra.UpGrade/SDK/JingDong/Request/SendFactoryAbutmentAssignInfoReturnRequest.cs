using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SendFactoryAbutmentAssignInfoReturnRequest : IJdRequest<SendFactoryAbutmentAssignInfoReturnResponse>
{
		                                                                      
public   		string
   authorizedSequence  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   orderno  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   assignTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   atHomeTime  { get; set; }

                  
                                                            
                                                          
public   		string
   assignerName  { get; set; }

                  
                                                            
                                                          
public   		string
   assignerTel  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.sendFactoryAbutmentAssignInfoReturn";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("authorizedSequence", this.authorizedSequence);
			parameters.Add("orderno", this.orderno);
			parameters.Add("assignTime", this.assignTime);
			parameters.Add("atHomeTime", this.atHomeTime);
			parameters.Add("assignerName", this.assignerName);
			parameters.Add("assignerTel", this.assignerTel);
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








        
 

