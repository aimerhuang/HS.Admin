using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsMasterCarrierCreateRequest : IJdRequest<JcloudWmsMasterCarrierCreateResponse>
{
		                                                                                                                                                                                                    
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                          
public   		string
   carrierNo  { get; set; }

                  
                                                            
                                                          
public   		string
   carrierName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   useFlag  { get; set; }

                  
                                                            
                                                          
public   		string
   contact  { get; set; }

                  
                                                            
                                                          
public   		string
   contactPhone  { get; set; }

                  
                                                            
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		string
   memo  { get; set; }

                  
                                                            
                                                          
public   		string
   operateUser  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operateTime  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.master.carrier.create";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("carrierNo", this.carrierNo);
			parameters.Add("carrierName", this.carrierName);
			parameters.Add("useFlag", this.useFlag);
			parameters.Add("contact", this.contact);
			parameters.Add("contactPhone", this.contactPhone);
			parameters.Add("address", this.address);
			parameters.Add("memo", this.memo);
			parameters.Add("operateUser", this.operateUser);
			parameters.Add("operateTime", this.operateTime);
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








        
 

