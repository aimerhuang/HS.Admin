using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WaitAuditApplysProviderFindWaitAuditApplysRequest : IJdRequest<WaitAuditApplysProviderFindWaitAuditApplysResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   afsServiceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                          
public   		string
   customerPin  { get; set; }

                  
                                                            
                                                          
public   		string
   customerName  { get; set; }

                  
                                                            
                                                          
public   		string
   customerTel  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   orderType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   afsApplyTimeBegin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   afsApplyTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   customerExpect  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   afsServiceStatus  { get; set; }

                  
                                                            
                                                          
public   		string
   buId  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   pageIndex  { get; set; }

                  
                                                            
                                                                                                                                                       
public   		string
   operatorPin  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorNick  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorRemark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operatorDate  { get; set; }

                  
                                                            
                                                          
public   		string
   platformSrc  { get; set; }

                  
                                                            
                                                                                           
public   		string
   verificationCode  { get; set; }

                  
                                                            
                                                          
public   		string
   queryTabName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   afsServiceState  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.WaitAuditApplysProvider.findWaitAuditApplys";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("afsServiceId", this.afsServiceId);
			parameters.Add("orderId", this.orderId);
			parameters.Add("customerPin", this.customerPin);
			parameters.Add("customerName", this.customerName);
			parameters.Add("customerTel", this.customerTel);
			parameters.Add("orderType", this.orderType);
			parameters.Add("afsApplyTimeBegin", this.afsApplyTimeBegin);
			parameters.Add("afsApplyTimeEnd", this.afsApplyTimeEnd);
			parameters.Add("customerExpect", this.customerExpect);
			parameters.Add("afsServiceStatus", this.afsServiceStatus);
			parameters.Add("buId", this.buId);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("pageIndex", this.pageIndex);
			parameters.Add("operatorPin", this.operatorPin);
			parameters.Add("operatorNick", this.operatorNick);
			parameters.Add("operatorRemark", this.operatorRemark);
			parameters.Add("operatorDate", this.operatorDate);
			parameters.Add("platformSrc", this.platformSrc);
			parameters.Add("verificationCode", this.verificationCode);
			parameters.Add("queryTabName", this.queryTabName);
			parameters.Add("afsServiceState", this.afsServiceState);
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








        
 

