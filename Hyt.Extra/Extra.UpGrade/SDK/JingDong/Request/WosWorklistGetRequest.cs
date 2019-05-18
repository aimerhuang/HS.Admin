using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WosWorklistGetRequest : IJdRequest<WosWorklistGetResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   beginDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   endDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pageNumber  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.wos.worklist.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("status", this.status);
			parameters.Add("order_id", this.orderId);
			parameters.Add("begin_date", this.beginDate);
			parameters.Add("end_date", this.endDate);
			parameters.Add("page_number", this.pageNumber);
			parameters.Add("page_size", this.pageSize);
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








        
 

