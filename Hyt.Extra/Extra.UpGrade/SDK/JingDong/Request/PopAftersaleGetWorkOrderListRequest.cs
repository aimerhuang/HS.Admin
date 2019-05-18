using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopAftersaleGetWorkOrderListRequest : IJdRequest<PopAftersaleGetWorkOrderListResponse>
{
		                                                                      
public   		Nullable<long>
   workorderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		string
   status  { get; set; }

                  
                                                            
                                                          
public   		string
   beginDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pageNumber  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   queryType  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.aftersale.GetWorkOrderList";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("workorder_id", this.workorderId);
			parameters.Add("order_id", this.orderId);
			parameters.Add("status", this.status);
			parameters.Add("begin_date", this.beginDate);
			parameters.Add("end_date", this.endDate);
			parameters.Add("page_number", this.pageNumber);
			parameters.Add("page_size", this.pageSize);
			parameters.Add("query_type", this.queryType);
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








        
 

