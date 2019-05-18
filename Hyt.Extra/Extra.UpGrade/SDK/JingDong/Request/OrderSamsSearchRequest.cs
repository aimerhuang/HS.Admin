using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class OrderSamsSearchRequest : IJdRequest<OrderSamsSearchResponse>
{
		                                                                                                                                  
public   		string
   orderId  { get; set; }

                  
                                                            
                                                                                           
public   		string
   startDate  { get; set; }

                  
                                                            
                                                          
public   		string
   endDate  { get; set; }

                  
                                                            
                                                          
public   		string
   orderState  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   clubId  { get; set; }

                  
                                                            
                                                          
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.order.sams.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderId", this.orderId);
			parameters.Add("startDate", this.startDate);
			parameters.Add("endDate", this.endDate);
			parameters.Add("orderState", this.orderState);
			parameters.Add("clubId", this.clubId);
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








        
 

