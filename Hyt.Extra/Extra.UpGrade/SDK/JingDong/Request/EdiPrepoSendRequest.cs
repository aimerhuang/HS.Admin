using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiPrepoSendRequest : IJdRequest<EdiPrepoSendResponse>
{
		                                                                                                                                  
public   		string
   forecastPurchaseOrderCode  { get; set; }

                  
                                                            
                                                          
public   		string
   prePurchaseOrderCode  { get; set; }

                  
                                                            
                                                          
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                          
public   		string
   vendorName  { get; set; }

                  
                                                            
                                                          
public   		string
   orgCode  { get; set; }

                  
                                                            
                                                          
public   		string
   orgName  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   jdSku  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   vendorProductId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   productName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   quantity  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.prepo.send";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("forecastPurchaseOrderCode", this.forecastPurchaseOrderCode);
			parameters.Add("prePurchaseOrderCode", this.prePurchaseOrderCode);
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("vendorName", this.vendorName);
			parameters.Add("orgCode", this.orgCode);
			parameters.Add("orgName", this.orgName);
			parameters.Add("jdSku", this.jdSku);
			parameters.Add("vendorProductId", this.vendorProductId);
			parameters.Add("productName", this.productName);
			parameters.Add("quantity", this.quantity);
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








        
 

