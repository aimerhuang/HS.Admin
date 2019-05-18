using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopCustomsCenterStorageJsfServiceCommonStorageJosTokenRequest : IJdRequest<PopCustomsCenterStorageJsfServiceCommonStorageJosTokenResponse>
{
		                                                                                                                                  
public   		string
   customsId  { get; set; }

                  
                                                            
                                                          
public   		string
   serviceId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                         
public   		string
   orderId  { get; set; }

                  
                                                            
                                                          
public   		string
   companyId  { get; set; }

                  
                                                            
                                                          
public   		string
   logiNo  { get; set; }

                  
                                                            
                                                          
public   		string
   logisticsCompanies  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.customs.center.StorageJsfService.commonStorageJosToken";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("customsId", this.customsId);
			parameters.Add("serviceId", this.serviceId);
			parameters.Add("orderId", this.orderId);
			parameters.Add("companyId", this.companyId);
			parameters.Add("logiNo", this.logiNo);
			parameters.Add("logisticsCompanies", this.logisticsCompanies);
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








        
 

