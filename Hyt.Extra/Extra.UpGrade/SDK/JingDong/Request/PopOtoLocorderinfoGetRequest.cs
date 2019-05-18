using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopOtoLocorderinfoGetRequest : IJdRequest<PopOtoLocorderinfoGetResponse>
{
		                                                                                                       
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   codeType  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.oto.locorderinfo.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("order_id", this.orderId);
			parameters.Add("code_type", this.codeType);
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








        
 

