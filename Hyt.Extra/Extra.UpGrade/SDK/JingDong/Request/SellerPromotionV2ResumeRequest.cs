using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionV2ResumeRequest : IJdRequest<SellerPromotionV2ResumeResponse>
{
		                                                                                                                                                                                                    
public   		string
   ip  { get; set; }

                  
                                                            
                                                          
public   		string
   port  { get; set; }

                  
                                                            
                                                          
public   		string
   requestId  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                             
public   		Nullable<long>
   promoId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   promoType  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.v2.resume";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ip", this.ip);
			parameters.Add("port", this.port);
			parameters.Add("request_id", this.requestId);
			parameters.Add("promo_id", this.promoId);
			parameters.Add("promo_type", this.promoType);
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








        
 

