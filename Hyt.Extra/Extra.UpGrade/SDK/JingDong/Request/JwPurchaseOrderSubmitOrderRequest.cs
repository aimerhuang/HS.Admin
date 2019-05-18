using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JwPurchaseOrderSubmitOrderRequest : IJdRequest<JwPurchaseOrderSubmitOrderResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                                                                                    		public  		string
   sku  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   num  { get; set; }
                                                                                                                                                                                                                                                            
public   		string
   addressLevel1Id  { get; set; }

                  
                                                            
                                                          
public   		string
   addressLevel2Id  { get; set; }

                  
                                                            
                                                          
public   		string
   addressLevel3Id  { get; set; }

                  
                                                            
                                                          
public   		string
   addressLevel4Id  { get; set; }

                  
                                                            
                                                          
public   		string
   addressDetail  { get; set; }

                  
                                                            
                                                          
public   		string
   phone  { get; set; }

                  
                                                            
                                                          
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		string
   idCard  { get; set; }

                  
                                                            
                                                                                                                                                       
public   		Nullable<int>
   invoiceType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   billingType  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<bool>
   autoDeductRebate  { get; set; }

                  
                                                            
                                                          
public   		string
   rebate  { get; set; }

                  
                                                            
                                                          
public   		string
   clientId  { get; set; }

                  
                                                            
                                                          
public   		string
   clientBusinessNo  { get; set; }

                  
                                                            
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                                                                                   
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jw.purchase.order.submitOrder";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("sku", this.sku);
			parameters.Add("num", this.num);
			parameters.Add("addressLevel1Id", this.addressLevel1Id);
			parameters.Add("addressLevel2Id", this.addressLevel2Id);
			parameters.Add("addressLevel3Id", this.addressLevel3Id);
			parameters.Add("addressLevel4Id", this.addressLevel4Id);
			parameters.Add("addressDetail", this.addressDetail);
			parameters.Add("phone", this.phone);
			parameters.Add("name", this.name);
			parameters.Add("idCard", this.idCard);
			parameters.Add("invoiceType", this.invoiceType);
			parameters.Add("billingType", this.billingType);
			parameters.Add("autoDeductRebate", this.autoDeductRebate);
			parameters.Add("rebate", this.rebate);
			parameters.Add("clientId", this.clientId);
			parameters.Add("clientBusinessNo", this.clientBusinessNo);
			parameters.Add("remark", this.remark);
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








        
 

