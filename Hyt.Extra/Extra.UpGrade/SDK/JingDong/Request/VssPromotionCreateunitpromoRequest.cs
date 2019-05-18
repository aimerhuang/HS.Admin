using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VssPromotionCreateunitpromoRequest : IJdRequest<VssPromotionCreateunitpromoResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   channels  { get; set; }

                  
                                                            
                                                          
public   		string
   promoName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   promoAdviceWord  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   actLinkName  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   actLinkUrl  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   startTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   numAvailable  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<bool>
   phoneLogo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   discountAmount  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   discountPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   discountType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   rebateFile  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   rebateName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   overlyingSuit  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vss.promotion.createunitpromo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ware_id", this.wareId);
			parameters.Add("channels", this.channels);
			parameters.Add("promo_name", this.promoName);
			parameters.Add("promo_advice_word", this.promoAdviceWord);
			parameters.Add("act_link_name", this.actLinkName);
			parameters.Add("act_link_url", this.actLinkUrl);
			parameters.Add("start_time", this.startTime);
			parameters.Add("end_time", this.endTime);
			parameters.Add("num_available", this.numAvailable);
			parameters.Add("phone_logo", this.phoneLogo);
			parameters.Add("discount_amount", this.discountAmount);
			parameters.Add("discount_price", this.discountPrice);
			parameters.Add("discount_type", this.discountType);
			parameters.Add("rebate_file", this.rebateFile);
			parameters.Add("rebate_name", this.rebateName);
			parameters.Add("overlying_suit", this.overlyingSuit);
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








        
 

