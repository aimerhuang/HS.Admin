using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionV2OrderDiscountCreateRequest : IJdRequest<SellerPromotionV2OrderDiscountCreateResponse>
{
		                                                                                                                                                                                                    
public   		string
   ip  { get; set; }

                  
                                                            
                                                          
public   		string
   port  { get; set; }

                  
                                                            
                                                          
public   		string
   requestId  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                         
public   		string
   promoName  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		string
   beginTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   bound  { get; set; }

                  
                                                            
                                                          
public   		string
   slogan  { get; set; }

                  
                                                            
                                                          
public   		string
   comment  { get; set; }

                  
                                                            
                                                          
public   		string
   link  { get; set; }

                  
                                                            
                                                                                           
public   		string
   allowOthersOperate  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   allowOthersCheck  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   allowOtherUserOperate  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		string
   allowOtherUserCheck  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		string
   needManualCheck  { get; set; }

                  
                                                                                                                                                            
                                                                                           
public   		string
   rate  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     		public  		string
   skuId  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.v2.order.discount.create";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ip", this.ip);
			parameters.Add("port", this.port);
			parameters.Add("request_id", this.requestId);
			parameters.Add("promo_name", this.promoName);
			parameters.Add("begin_time", this.beginTime);
			parameters.Add("end_time", this.endTime);
			parameters.Add("bound", this.bound);
			parameters.Add("slogan", this.slogan);
			parameters.Add("comment", this.comment);
			parameters.Add("link", this.link);
			parameters.Add("allow_others_operate", this.allowOthersOperate);
			parameters.Add("allow_others_check", this.allowOthersCheck);
			parameters.Add("allow_other_user_operate", this.allowOtherUserOperate);
			parameters.Add("allow_other_user_check", this.allowOtherUserCheck);
			parameters.Add("need_manual_check", this.needManualCheck);
			parameters.Add("rate", this.rate);
			parameters.Add("sku_id", this.skuId);
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








        
 

