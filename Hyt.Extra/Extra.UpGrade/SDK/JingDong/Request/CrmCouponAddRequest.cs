using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CrmCouponAddRequest : IJdRequest<CrmCouponAddResponse>
{
		                                                                                                       
public   		string
   couponName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   couponAmount  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   condition  { get; set; }

                  
                                                            
                                                          
public   		string
   denomination  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.crm.coupon.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("coupon_name", this.couponName);
			parameters.Add("coupon_amount", this.couponAmount);
			parameters.Add("condition", this.condition);
			parameters.Add("denomination", this.denomination);
			parameters.Add("start_time", this.startTime);
			parameters.Add("end_time", this.endTime);
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








        
 

