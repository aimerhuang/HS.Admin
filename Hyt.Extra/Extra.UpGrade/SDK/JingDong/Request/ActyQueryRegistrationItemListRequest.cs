using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ActyQueryRegistrationItemListRequest : IJdRequest<ActyQueryRegistrationItemListResponse>
{
		                                                                                                       
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                          
public   		string
   beginDate  { get; set; }

                  
                                                            
                                                          
public   		string
   endDate  { get; set; }

                  
                                                            
                                                          
public   		string
   pageIndex  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.acty.queryRegistrationItemList";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("skuId", this.skuId);
			parameters.Add("orderId", this.orderId);
			parameters.Add("beginDate", this.beginDate);
			parameters.Add("endDate", this.endDate);
			parameters.Add("pageIndex", this.pageIndex);
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








        
 

