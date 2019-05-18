using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SamOrderInfoQueryRequest : IJdRequest<SamOrderInfoQueryResponse>
{
		                                                                                                                                                                                                    
public   		Nullable<int>
   orderStatus  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   payStartTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   payEndTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.sam.order.info.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderStatus", this.orderStatus);
			parameters.Add("payStartTime", this.payStartTime);
			parameters.Add("payEndTime", this.payEndTime);
			parameters.Add("startTime", this.startTime);
			parameters.Add("endTime", this.endTime);
			parameters.Add("pageNum", this.pageNum);
			parameters.Add("pageSize", this.pageSize);
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








        
 

