using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PayCorePayresultRequest : IJdRequest<PayCorePayresultResponse>
{
		                                                                                                                                  
public   		string
   payId  { get; set; }

                  
                                                            
                                                          
public   		string
   payTime  { get; set; }

                  
                                                            
                                                          
public   		string
   agencyCode  { get; set; }

                  
                                                            
                                                          
public   		string
   amount  { get; set; }

                  
                                                            
                                                          
public   		string
   bankCode  { get; set; }

                  
                                                            
                                                          
public   		string
   bankDetailId  { get; set; }

                  
                                                            
                                                          
public   		string
   platDetailId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   cardType  { get; set; }

                  
                                                            
                                                          
public   		string
   currency  { get; set; }

                  
                                                            
                                                          
public   		string
   merchantId  { get; set; }

                  
                                                            
                                                          
public   		string
   pin  { get; set; }

                  
                                                            
                                                          
public   		string
   productName  { get; set; }

                  
                                                            
                                                          
public   		string
   virtualType  { get; set; }

                  
                                                            
                                                          
public   		string
   sysCode  { get; set; }

                  
                                                            
                                                          
public   		string
   dataSign  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pay.core.payresult";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("payId", this.payId);
			parameters.Add("payTime", this.payTime);
			parameters.Add("agencyCode", this.agencyCode);
			parameters.Add("amount", this.amount);
			parameters.Add("bankCode", this.bankCode);
			parameters.Add("bankDetailId", this.bankDetailId);
			parameters.Add("platDetailId", this.platDetailId);
			parameters.Add("cardType", this.cardType);
			parameters.Add("currency", this.currency);
			parameters.Add("merchantId", this.merchantId);
			parameters.Add("pin", this.pin);
			parameters.Add("productName", this.productName);
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








        
 

