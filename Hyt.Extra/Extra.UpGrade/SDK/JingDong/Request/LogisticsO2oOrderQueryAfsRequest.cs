using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsO2oOrderQueryAfsRequest : IJdRequest<LogisticsO2oOrderQueryAfsResponse>
{
		                                                                                                       
public   		string
   stationNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   customerOrderId  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   customerOrderState  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   customerOrderTimeStart  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		string
   customerOrderTimeEnd  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		string
   customerOrderUpdateTimeStart  { get; set; }

                  
                                                                                                                                                                                                            
                                                          
public   		string
   customerOrderUpdateTimeEnd  { get; set; }

                  
                                                                                                                                                                                                            
                                                          
public   		string
   currentPage  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pageNum  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.o2o.order.queryAfs";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("station_no", this.stationNo);
			parameters.Add("order_id", this.orderId);
			parameters.Add("customer_order_id", this.customerOrderId);
			parameters.Add("customer_order_state", this.customerOrderState);
			parameters.Add("customer_order_time_start", this.customerOrderTimeStart);
			parameters.Add("customer_order_time_end", this.customerOrderTimeEnd);
			parameters.Add("customer_order_update_time_start", this.customerOrderUpdateTimeStart);
			parameters.Add("customer_order_update_time_end", this.customerOrderUpdateTimeEnd);
			parameters.Add("current_page", this.currentPage);
			parameters.Add("page_num", this.pageNum);
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








        
 

