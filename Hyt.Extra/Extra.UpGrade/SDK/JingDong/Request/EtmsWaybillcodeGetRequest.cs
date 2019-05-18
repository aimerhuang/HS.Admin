using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EtmsWaybillcodeGetRequest : IJdRequest<EtmsWaybillcodeGetResponse>
{
		                                                                                                                                                                                                    
public   		string
   preNum  { get; set; }

                  
                                                            
                                                          
public   		string
   customerCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   orderType  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.etms.waybillcode.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("preNum", this.preNum);
			parameters.Add("customerCode", this.customerCode);
			parameters.Add("orderType", this.orderType);
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








        
 

