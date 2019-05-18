using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsPdjOrderQueryRequest : IJdRequest<LogisticsPdjOrderQueryResponse>
{
		                                                                      
public   		Nullable<int>
   currentPage  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageCount  { get; set; }

                  
                                                            
                                                                                                                                                       
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                          
public   		string
   stationNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   srcSysId  { get; set; }

                  
                                                            
                                                          
public   		string
   srcOrderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   orderStatus  { get; set; }

                  
                                                            
                                                          
public   		string
   orderTimeStart  { get; set; }

                  
                                                            
                                                          
public   		string
   orderTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		string
   orderUpdateTimeStart  { get; set; }

                  
                                                            
                                                          
public   		string
   orderUpdateTimeEnd  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.pdj.order.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("currentPage", this.currentPage);
			parameters.Add("pageCount", this.pageCount);
			parameters.Add("orderId", this.orderId);
			parameters.Add("stationNo", this.stationNo);
			parameters.Add("srcSysId", this.srcSysId);
			parameters.Add("srcOrderId", this.srcOrderId);
			parameters.Add("orderStatus", this.orderStatus);
			parameters.Add("orderTimeStart", this.orderTimeStart);
			parameters.Add("orderTimeEnd", this.orderTimeEnd);
			parameters.Add("orderUpdateTimeStart", this.orderUpdateTimeStart);
			parameters.Add("orderUpdateTimeEnd", this.orderUpdateTimeEnd);
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








        
 

