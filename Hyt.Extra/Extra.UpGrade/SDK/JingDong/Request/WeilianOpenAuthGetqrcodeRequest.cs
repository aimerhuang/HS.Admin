using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WeilianOpenAuthGetqrcodeRequest : IJdRequest<WeilianOpenAuthGetqrcodeResponse>
{
		                                                                                                       
public   		string
   deviceId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   userIdentity  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   deviceName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   osType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   osVer  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   osVerName  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   deviceType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   appName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   appPackageName  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   appVersion  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   appChannel  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.weilian.open.auth.getqrcode";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("device_id", this.deviceId);
			parameters.Add("user_identity", this.userIdentity);
			parameters.Add("device_name", this.deviceName);
			parameters.Add("os_type", this.osType);
			parameters.Add("os_ver", this.osVer);
			parameters.Add("os_ver_name", this.osVerName);
			parameters.Add("device_type", this.deviceType);
			parameters.Add("app_name", this.appName);
			parameters.Add("app_package_name", this.appPackageName);
			parameters.Add("app_version", this.appVersion);
			parameters.Add("app_channel", this.appChannel);
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








        
 

