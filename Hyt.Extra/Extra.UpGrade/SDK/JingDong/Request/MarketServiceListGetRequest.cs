using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class MarketServiceListGetRequest : IJdRequest<MarketServiceListGetResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   page  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   serviceStatus  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   startDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   endDate  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.market.service.list.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("page_size", this.pageSize);
			parameters.Add("page", this.page);
			parameters.Add("service_status", this.serviceStatus);
			parameters.Add("start_date", this.startDate);
			parameters.Add("end_date", this.endDate);
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








        
 

