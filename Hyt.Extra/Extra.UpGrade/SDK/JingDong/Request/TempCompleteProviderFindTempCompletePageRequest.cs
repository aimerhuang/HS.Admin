using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TempCompleteProviderFindTempCompletePageRequest : IJdRequest<TempCompleteProviderFindTempCompletePageResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   afsServiceId  { get; set; }

                  
                                                            
                                                          
public   		string
   buId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   afsApplyTimeBegin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   afsApplyTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   afsServiceProcessResult  { get; set; }

                  
                                                            
                                                          
public   		string
   waybill  { get; set; }

                  
                                                            
                                                          
public   		string
   customerPin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   orderType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   messageStatus  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   searchType  { get; set; }

                  
                                                            
                                                                                                                      
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

                  
                                                            
                                                                                           
public   		Nullable<int>
   afsCategoryIdPop  { get; set; }

                  
                                                            
                                                          
public   		string
   verificationCode  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   pageIndex  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.TempCompleteProvider.findTempCompletePage";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("afsServiceId", this.afsServiceId);
			parameters.Add("buId", this.buId);
			parameters.Add("orderId", this.orderId);
			parameters.Add("afsApplyTimeBegin", this.afsApplyTimeBegin);
			parameters.Add("afsApplyTimeEnd", this.afsApplyTimeEnd);
			parameters.Add("afsServiceProcessResult", this.afsServiceProcessResult);
			parameters.Add("waybill", this.waybill);
			parameters.Add("customerPin", this.customerPin);
			parameters.Add("orderType", this.orderType);
			parameters.Add("messageStatus", this.messageStatus);
			parameters.Add("searchType", this.searchType);
			parameters.Add("operatorPin", this.operatorPin);
			parameters.Add("operatorNick", this.operatorNick);
			parameters.Add("operatorRemark", this.operatorRemark);
			parameters.Add("operatorDate", this.operatorDate);
			parameters.Add("platformSrc", this.platformSrc);
			parameters.Add("afsCategoryIdPop", this.afsCategoryIdPop);
			parameters.Add("verificationCode", this.verificationCode);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("pageIndex", this.pageIndex);
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








        
 

