using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DropshipDpsPrerefundRequest : IJdRequest<DropshipDpsPrerefundResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   customOrderId  { get; set; }

                  
                                                            
                                                          
public   		string
   approvalSuggestion  { get; set; }

                  
                                                            
                                                          
public   		string
   approvalState  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   id  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorState  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dropship.dps.prerefund";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("customOrderId", this.customOrderId);
			parameters.Add("approvalSuggestion", this.approvalSuggestion);
			parameters.Add("approvalState", this.approvalState);
			parameters.Add("id", this.id);
			parameters.Add("operatorState", this.operatorState);
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








        
 

