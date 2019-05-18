using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopCustomsCenterStorageJsfServiceCommonStorageRequest : IJdRequest<PopCustomsCenterStorageJsfServiceCommonStorageResponse>
{
		                                                                                                                                                                   
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
			return "jingdong.pop.customs.center.StorageJsfService.commonStorage";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
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








        
 

