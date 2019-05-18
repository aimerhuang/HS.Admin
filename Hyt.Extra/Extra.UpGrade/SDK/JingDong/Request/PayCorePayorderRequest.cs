using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PayCorePayorderRequest : IJdRequest<PayCorePayorderResponse>
{
		                                                                                                                                  
public   		string
   businessId  { get; set; }

                  
                                                            
                                                          
public   		string
   payOrderType  { get; set; }

                  
                                                            
                                                          
public   		string
   agencyCode  { get; set; }

                  
                                                            
                                                          
public   		string
   amount  { get; set; }

                  
                                                            
                                                          
public   		string
   bankCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   cardType  { get; set; }

                  
                                                            
                                                          
public   		string
   needCheck  { get; set; }

                  
                                                            
                                                          
public   		string
   orderAgencyCode  { get; set; }

                  
                                                            
                                                          
public   		string
   payChannelType  { get; set; }

                  
                                                            
                                                          
public   		string
   payMethod  { get; set; }

                  
                                                            
                                                          
public   		string
   payableAmount  { get; set; }

                  
                                                            
                                                          
public   		string
   pin  { get; set; }

                  
                                                            
                                                          
public   		string
   sourcePlat  { get; set; }

                  
                                                            
                                                          
public   		string
   sourceType  { get; set; }

                  
                                                            
                                                          
public   		string
   virtualType  { get; set; }

                  
                                                            
                                                          
public   		string
   sysCode  { get; set; }

                  
                                                            
                                                          
public   		string
   dataSign  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pay.core.payorder";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("businessId", this.businessId);
			parameters.Add("payOrderType", this.payOrderType);
			parameters.Add("agencyCode", this.agencyCode);
			parameters.Add("amount", this.amount);
			parameters.Add("bankCode", this.bankCode);
			parameters.Add("cardType", this.cardType);
			parameters.Add("needCheck", this.needCheck);
			parameters.Add("orderAgencyCode", this.orderAgencyCode);
			parameters.Add("payChannelType", this.payChannelType);
			parameters.Add("payMethod", this.payMethod);
			parameters.Add("payableAmount", this.payableAmount);
			parameters.Add("pin", this.pin);
			parameters.Add("sourcePlat", this.sourcePlat);
			parameters.Add("sourceType", this.sourceType);
			parameters.Add("virtualType", this.virtualType);
			parameters.Add("sysCode", this.sysCode);
			parameters.Add("dataSign", this.dataSign);
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








        
 

