using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsO2oOrderSearchRequest : IJdRequest<LogisticsO2oOrderSearchResponse>
{
		                                                                                                       
public   		string
   stationNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   orderState  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   orderTimeStart  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   orderTimeEnd  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   orderUpdateTimeStart  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		string
   orderUpdateTimeEnd  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		string
   currentPage  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.o2o.order.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("station_no", this.stationNo);
			parameters.Add("order_id", this.orderId);
			parameters.Add("order_state", this.orderState);
			parameters.Add("order_time_start", this.orderTimeStart);
			parameters.Add("order_time_end", this.orderTimeEnd);
			parameters.Add("order_update_time_start", this.orderUpdateTimeStart);
			parameters.Add("order_update_time_end", this.orderUpdateTimeEnd);
			parameters.Add("current_page", this.currentPage);
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








        
 

