using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class MarketFuwuAddRequest : IJdRequest<MarketFuwuAddResponse>
{
		                                                                                                                                  
public   		string
   serviceCode  { get; set; }

                  
                                                            
                                                          
public   		string
   serviceName  { get; set; }

                  
                                                            
                                                          
public   		string
   serviceStatus  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   serviceDetailUrl  { get; set; }

                  
                                                            
                                                          
public   		string
   serviceLogo  { get; set; }

                  
                                                            
                                                          
public   		string
   fwsPin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   fwsId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   cid  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   serviceType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   appKey  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   itemCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   chargeType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   itemVersion  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   chargeDays  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   pageDisplay  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   price  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.market.fuwu.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("serviceCode", this.serviceCode);
			parameters.Add("serviceName", this.serviceName);
			parameters.Add("service_status", this.serviceStatus);
			parameters.Add("serviceDetailUrl", this.serviceDetailUrl);
			parameters.Add("serviceLogo", this.serviceLogo);
			parameters.Add("fwsPin", this.fwsPin);
			parameters.Add("fwsId", this.fwsId);
			parameters.Add("cid", this.cid);
			parameters.Add("service_type", this.serviceType);
			parameters.Add("appKey", this.appKey);
			parameters.Add("itemCode", this.itemCode);
			parameters.Add("chargeType", this.chargeType);
			parameters.Add("itemVersion", this.itemVersion);
			parameters.Add("chargeDays", this.chargeDays);
			parameters.Add("pageDisplay", this.pageDisplay);
			parameters.Add("price", this.price);
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








        
 

