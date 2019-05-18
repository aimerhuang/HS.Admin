using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionV2GetPromoLimitRequest : IJdRequest<SellerPromotionV2GetPromoLimitResponse>
{
		                                                                                                                                                                                                    
public   		string
   ip  { get; set; }

                  
                                                            
                                                          
public   		string
   port  { get; set; }

                  
                                                            
                                                                                                                                                                                                                         
public   		Nullable<long>
   categoryId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   startTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.v2.getPromoLimit";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ip", this.ip);
			parameters.Add("port", this.port);
			parameters.Add("category_id", this.categoryId);
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








        
 

