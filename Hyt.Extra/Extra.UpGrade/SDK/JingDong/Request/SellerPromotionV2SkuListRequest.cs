using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionV2SkuListRequest : IJdRequest<SellerPromotionV2SkuListResponse>
{
		                                                                                                                                                                                                    
public   		string
   ip  { get; set; }

                  
                                                            
                                                          
public   		string
   port  { get; set; }

                  
                                                            
                                                                                                                                                                                        
public   		Nullable<long>
   promoId  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   bindType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   promoType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSSize  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.v2.sku.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ip", this.ip);
			parameters.Add("port", this.port);
			parameters.Add("promo_id", this.promoId);
			parameters.Add("ware_id", this.wareId);
			parameters.Add("sku_id", this.skuId);
			parameters.Add("bind_type", this.bindType);
			parameters.Add("promo_type", this.promoType);
			parameters.Add("page", this.page);
			parameters.Add("pageS_size", this.pageSSize);
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








        
 

