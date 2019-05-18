using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EpsEditCheckStatusRequest : IJdRequest<EpsEditCheckStatusResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   deliveryNo  { get; set; }

                  
                                                            
                                                          
public   		string
   uuid  { get; set; }

                  
                                                            
                                                                                           
public   		string
   siteNo  { get; set; }

                  
                                                            
                                                          
public   		string
   checkTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eps.editCheckStatus";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deliveryNo", this.deliveryNo);
			parameters.Add("uuid", this.uuid);
			parameters.Add("siteNo", this.siteNo);
			parameters.Add("checkTime", this.checkTime);
			parameters.Add("status", this.status);
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








        
 

