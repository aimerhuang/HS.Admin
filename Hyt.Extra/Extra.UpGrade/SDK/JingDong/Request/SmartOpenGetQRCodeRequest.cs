using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SmartOpenGetQRCodeRequest : IJdRequest<SmartOpenGetQRCodeResponse>
{
		                                                                      
public   		string
   productUuid  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   productSecret  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   deviceId  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.smart.open.getQRCode";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("product_uuid", this.productUuid);
			parameters.Add("product_secret", this.productSecret);
			parameters.Add("device_id", this.deviceId);
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








        
 

