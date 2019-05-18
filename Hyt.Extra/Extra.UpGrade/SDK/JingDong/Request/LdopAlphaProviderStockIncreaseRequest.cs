using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LdopAlphaProviderStockIncreaseRequest : IJdRequest<LdopAlphaProviderStockIncreaseResponse>
{
		                                                                                                                                  
public   		string
   operatorCode  { get; set; }

                  
                                                            
                                                          
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                          
public   		string
   vendorName  { get; set; }

                  
                                                            
                                                          
public   		string
   providerId  { get; set; }

                  
                                                            
                                                          
public   		string
   providerCode  { get; set; }

                  
                                                            
                                                          
public   		string
   providerName  { get; set; }

                  
                                                            
                                                          
public   		string
   branchCode  { get; set; }

                  
                                                            
                                                          
public   		string
   branchName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   amount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operatorTime  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   state  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ldop.alpha.provider.stock.increase";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("operatorCode", this.operatorCode);
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("vendorName", this.vendorName);
			parameters.Add("providerId", this.providerId);
			parameters.Add("providerCode", this.providerCode);
			parameters.Add("providerName", this.providerName);
			parameters.Add("branchCode", this.branchCode);
			parameters.Add("branchName", this.branchName);
			parameters.Add("amount", this.amount);
			parameters.Add("operatorTime", this.operatorTime);
			parameters.Add("operatorName", this.operatorName);
			parameters.Add("state", this.state);
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








        
 

