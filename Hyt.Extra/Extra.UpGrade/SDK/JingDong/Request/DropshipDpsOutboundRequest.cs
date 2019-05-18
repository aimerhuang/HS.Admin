using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DropshipDpsOutboundRequest : IJdRequest<DropshipDpsOutboundResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   customOrderId  { get; set; }

                  
                                                            
                                                          
public   		string
   memoByVendor  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   isJdexpress  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dropship.dps.outbound";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("customOrderId", this.customOrderId);
			parameters.Add("memoByVendor", this.memoByVendor);
			parameters.Add("isJdexpress", this.isJdexpress);
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








        
 

