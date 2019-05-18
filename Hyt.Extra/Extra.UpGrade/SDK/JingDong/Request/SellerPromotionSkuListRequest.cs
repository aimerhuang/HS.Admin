using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionSkuListRequest : IJdRequest<SellerPromotionSkuListResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   promoId  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		Nullable<int>
   bindType  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   size  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.sku.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ware_id", this.wareId);
			parameters.Add("sku_id", this.skuId);
			parameters.Add("promo_id", this.promoId);
			parameters.Add("bind_type", this.bindType);
			parameters.Add("page", this.page);
			parameters.Add("size", this.size);
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








        
 

