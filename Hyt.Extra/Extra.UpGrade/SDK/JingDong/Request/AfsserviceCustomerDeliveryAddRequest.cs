using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class AfsserviceCustomerDeliveryAddRequest : IJdRequest<AfsserviceCustomerDeliveryAddResponse>
{
		                                                                                                       
public   		Nullable<int>
   afsServiceId  { get; set; }

                  
                                                            
                                                                                                                                                  
public   		string
   afsApplyDetailIds  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   customerPin  { get; set; }

                  
                                                            
                                                          
public   		string
   customerName  { get; set; }

                  
                                                            
                                                          
public   		string
   customerPhone  { get; set; }

                  
                                                            
                                                          
public   		string
   afterserviceReceiver  { get; set; }

                  
                                                            
                                                          
public   		string
   afterserviceAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   afterserviceTel  { get; set; }

                  
                                                            
                                                          
public   		string
   afterserviceZipcode  { get; set; }

                  
                                                            
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.afsservice.customer.delivery.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("afsServiceId", this.afsServiceId);
			parameters.Add("afsApplyDetailIds", this.afsApplyDetailIds);
			parameters.Add("customerPin", this.customerPin);
			parameters.Add("customerName", this.customerName);
			parameters.Add("customerPhone", this.customerPhone);
			parameters.Add("afterserviceReceiver", this.afterserviceReceiver);
			parameters.Add("afterserviceAddress", this.afterserviceAddress);
			parameters.Add("afterserviceTel", this.afterserviceTel);
			parameters.Add("afterserviceZipcode", this.afterserviceZipcode);
			parameters.Add("remark", this.remark);
			parameters.Add("orderId", this.orderId);
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








        
 

