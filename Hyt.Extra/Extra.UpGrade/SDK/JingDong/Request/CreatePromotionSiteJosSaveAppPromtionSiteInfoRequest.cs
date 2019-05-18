using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CreatePromotionSiteJosSaveAppPromtionSiteInfoRequest : IJdRequest<CreatePromotionSiteJosSaveAppPromtionSiteInfoResponse>
{
		                                                                      
public   		string
   pin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   appId  { get; set; }

                  
                                                            
                                                          
public   		string
   adName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   adType  { get; set; }

                  
                                                            
                                                          
public   		string
   adSize  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.CreatePromotionSiteJos.saveAppPromtionSiteInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("pin", this.pin);
			parameters.Add("appId", this.appId);
			parameters.Add("adName", this.adName);
			parameters.Add("adType", this.adType);
			parameters.Add("adSize", this.adSize);
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








        
 

