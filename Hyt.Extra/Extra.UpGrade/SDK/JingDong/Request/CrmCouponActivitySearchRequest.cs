using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CrmCouponActivitySearchRequest : IJdRequest<CrmCouponActivitySearchResponse>
{
		                                                                                                       
public   		Nullable<long>
   couponId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   activityId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   activityName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   ativityStatus  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   startSendTime  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   endSendTime  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   currentPage  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.crm.coupon.activity.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("coupon_id", this.couponId);
			parameters.Add("activity_id", this.activityId);
			parameters.Add("activity_name", this.activityName);
			parameters.Add("ativity_status", this.ativityStatus);
			parameters.Add("start_send_time", this.startSendTime);
			parameters.Add("end_send_time", this.endSendTime);
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








        
 

