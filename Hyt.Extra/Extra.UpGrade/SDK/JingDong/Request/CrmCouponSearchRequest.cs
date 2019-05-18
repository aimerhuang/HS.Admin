using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CrmCouponSearchRequest : IJdRequest<CrmCouponSearchResponse>
{
		                                                                                                       
public   		Nullable<long>
   couponId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   denomination  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   currentPage  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.crm.coupon.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("coupon_id", this.couponId);
			parameters.Add("denomination", this.denomination);
			parameters.Add("end_time", this.endTime);
			parameters.Add("current_page", this.currentPage);
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








        
 

