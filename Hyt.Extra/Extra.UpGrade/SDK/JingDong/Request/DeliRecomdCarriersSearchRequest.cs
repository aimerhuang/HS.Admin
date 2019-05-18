using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DeliRecomdCarriersSearchRequest : IJdRequest<DeliRecomdCarriersSearchResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                           		public  		string
   sku  { get; set; }
                                                                                                                                                                                                
public   		Nullable<int>
   sendProvinceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   sendCityId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   sendCountyId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   sendTownId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   receiveProvinceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   receiveCityId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   receiveCountyId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   receiveTownId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.deliRecomdCarriers.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderId", this.orderId);
			parameters.Add("sku", this.sku);
			parameters.Add("sendProvinceId", this.sendProvinceId);
			parameters.Add("sendCityId", this.sendCityId);
			parameters.Add("sendCountyId", this.sendCountyId);
			parameters.Add("sendTownId", this.sendTownId);
			parameters.Add("receiveProvinceId", this.receiveProvinceId);
			parameters.Add("receiveCityId", this.receiveCityId);
			parameters.Add("receiveCountyId", this.receiveCountyId);
			parameters.Add("receiveTownId", this.receiveTownId);
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








        
 

