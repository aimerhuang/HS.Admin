using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ServicePromotionAppReportRequest : IJdRequest<ServicePromotionAppReportResponse>
{
		                                                                                                                                                                   
public   		string
   time  { get; set; }

                  
                                                            
                                                          
public   		string
   siteKey  { get; set; }

                  
                                                            
                                                          
public   		string
   ext1  { get; set; }

                  
                                                            
                                                          
public   		string
   ext2  { get; set; }

                  
                                                            
                                                          
public   		string
   ext3  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.service.promotion.appReport";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("time", this.time);
			parameters.Add("siteKey", this.siteKey);
			parameters.Add("ext1", this.ext1);
			parameters.Add("ext2", this.ext2);
			parameters.Add("ext3", this.ext3);
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








        
 

