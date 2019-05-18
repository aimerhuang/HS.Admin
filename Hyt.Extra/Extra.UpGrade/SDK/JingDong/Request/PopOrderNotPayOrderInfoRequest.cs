using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopOrderNotPayOrderInfoRequest : IJdRequest<PopOrderNotPayOrderInfoResponse>
{
		                                                                                                                                                                   
public   		Nullable<DateTime>
   startDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   page  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.order.notPayOrderInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("startDate", this.startDate);
			parameters.Add("endDate", this.endDate);
			parameters.Add("page", this.page);
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








        
 

