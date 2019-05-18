using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionV2ListRequest : IJdRequest<SellerPromotionV2ListResponse>
{
		                                                                                                                                                                                                    
public   		string
   ip  { get; set; }

                  
                                                            
                                                          
public   		string
   port  { get; set; }

                  
                                                            
                                                                                                                                                                                        
public   		Nullable<long>
   promoId  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   type  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   favorMode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   beginTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   promoStatus  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   srcType  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.v2.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ip", this.ip);
			parameters.Add("port", this.port);
			parameters.Add("promo_id", this.promoId);
			parameters.Add("name", this.name);
			parameters.Add("type", this.type);
			parameters.Add("favor_mode", this.favorMode);
			parameters.Add("begin_time", this.beginTime);
			parameters.Add("end_time", this.endTime);
			parameters.Add("promo_status", this.promoStatus);
			parameters.Add("ware_id", this.wareId);
			parameters.Add("sku_id", this.skuId);
			parameters.Add("page", this.page);
			parameters.Add("pageS_size", this.pageSSize);
			parameters.Add("src_type", this.srcType);
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








        
 

