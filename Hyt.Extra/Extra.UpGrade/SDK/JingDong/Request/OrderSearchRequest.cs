using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class OrderSearchRequest : IJdRequest<OrderSearchResponse>
{
		                                                                                                                                  
public   		string
   startDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   orderState  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   optionalFields  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   sortType  { get; set; }

                  
                                                            
                                                          
public   		string
   dateType  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.order.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("start_date", this.startDate);
			parameters.Add("end_date", this.endDate);
			parameters.Add("order_state", this.orderState);
			parameters.Add("page", this.page);
			parameters.Add("page_size", this.pageSize);
			parameters.Add("optional_fields", this.optionalFields);
			parameters.Add("sortType", this.sortType);
			parameters.Add("dateType", this.dateType);
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








        
 

