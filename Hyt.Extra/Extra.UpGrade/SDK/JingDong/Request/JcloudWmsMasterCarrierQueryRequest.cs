using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsMasterCarrierQueryRequest : IJdRequest<JcloudWmsMasterCarrierQueryResponse>
{
		                                                                                                                                                                                                    
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                          
public   		string
   carrierNo  { get; set; }

                  
                                                            
                                                          
public   		string
   carrierName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   carrierType  { get; set; }

                  
                                                            
                                                          
public   		string
   englishName  { get; set; }

                  
                                                            
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		string
   tel  { get; set; }

                  
                                                            
                                                          
public   		string
   contact  { get; set; }

                  
                                                            
                                                          
public   		string
   postCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   useFlag  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.master.carrier.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("carrierNo", this.carrierNo);
			parameters.Add("carrierName", this.carrierName);
			parameters.Add("carrierType", this.carrierType);
			parameters.Add("englishName", this.englishName);
			parameters.Add("address", this.address);
			parameters.Add("tel", this.tel);
			parameters.Add("contact", this.contact);
			parameters.Add("postCode", this.postCode);
			parameters.Add("useFlag", this.useFlag);
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








        
 

