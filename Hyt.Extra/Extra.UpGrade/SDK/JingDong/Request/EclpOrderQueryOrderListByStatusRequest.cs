using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpOrderQueryOrderListByStatusRequest : IJdRequest<EclpOrderQueryOrderListByStatusResponse>
{
		                                                                                                                                  
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   soStatus  { get; set; }

                  
                                                            
                                                          
public   		string
   pageNo  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<DateTime>
   startDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endDate  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.order.queryOrderListByStatus";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("soStatus", this.soStatus);
			parameters.Add("pageNo", this.pageNo);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("startDate", this.startDate);
			parameters.Add("endDate", this.endDate);
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








        
 

