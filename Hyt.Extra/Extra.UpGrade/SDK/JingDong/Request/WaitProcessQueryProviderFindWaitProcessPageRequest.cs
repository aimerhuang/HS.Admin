using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WaitProcessQueryProviderFindWaitProcessPageRequest : IJdRequest<WaitProcessQueryProviderFindWaitProcessPageResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   afsApplyTimeBegin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   afsApplyTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                          
public   		string
   expressCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   afsServiceStatus  { get; set; }

                  
                                                            
                                                          
public   		string
   customerPin  { get; set; }

                  
                                                            
                                                          
public   		string
   customerName  { get; set; }

                  
                                                            
                                                          
public   		string
   customerMobile  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   orderType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   customerExpect  { get; set; }

                  
                                                            
                                                          
public   		string
   buId  { get; set; }

                  
                                                            
                                                                                                                      
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
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   pageIndex  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<int>
   afsCategoryIdPop  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   approvedDateBegin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   approvedDateEnd  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   afsServiceId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.WaitProcessQueryProvider.findWaitProcessPage";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderId", this.orderId);
			parameters.Add("afsApplyTimeBegin", this.afsApplyTimeBegin);
			parameters.Add("afsApplyTimeEnd", this.afsApplyTimeEnd);
			parameters.Add("wareId", this.wareId);
			parameters.Add("expressCode", this.expressCode);
			parameters.Add("afsServiceStatus", this.afsServiceStatus);
			parameters.Add("customerPin", this.customerPin);
			parameters.Add("customerName", this.customerName);
			parameters.Add("customerMobile", this.customerMobile);
			parameters.Add("orderType", this.orderType);
			parameters.Add("customerExpect", this.customerExpect);
			parameters.Add("buId", this.buId);
			parameters.Add("operatorPin", this.operatorPin);
			parameters.Add("operatorNick", this.operatorNick);
			parameters.Add("operatorRemark", this.operatorRemark);
			parameters.Add("operatorDate", this.operatorDate);
			parameters.Add("platformSrc", this.platformSrc);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("pageIndex", this.pageIndex);
			parameters.Add("afsCategoryIdPop", this.afsCategoryIdPop);
			parameters.Add("approvedDateBegin", this.approvedDateBegin);
			parameters.Add("approvedDateEnd", this.approvedDateEnd);
			parameters.Add("afsServiceId", this.afsServiceId);
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








        
 

