using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CrmMemberScanRequest : IJdRequest<CrmMemberScanResponse>
{
		                                                                                                       
public   		string
   pin  { get; set; }

                  
                                                            
                                                          
public   		string
   grade  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   minLastTradeTime  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   maxLastTradeTime  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		Nullable<int>
   minTradeCount  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<int>
   maxTradeCount  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   avgPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   minTradeAmount  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   scrollId  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.crm.member.scan";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("pin", this.pin);
			parameters.Add("grade", this.grade);
			parameters.Add("min_last_trade_time", this.minLastTradeTime);
			parameters.Add("max_last_trade_time", this.maxLastTradeTime);
			parameters.Add("min_trade_count", this.minTradeCount);
			parameters.Add("max_trade_count", this.maxTradeCount);
			parameters.Add("avg_price", this.avgPrice);
			parameters.Add("min_trade_amount", this.minTradeAmount);
			parameters.Add("page_size", this.pageSize);
			parameters.Add("scroll_id", this.scrollId);
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








        
 

