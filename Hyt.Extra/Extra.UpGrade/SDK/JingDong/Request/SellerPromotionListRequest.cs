using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionListRequest : IJdRequest<SellerPromotionListResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   type  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                                          
public   		string
   beginTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   favorMode  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   size  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("type", this.type);
			parameters.Add("status", this.status);
			parameters.Add("begin_time", this.beginTime);
			parameters.Add("end_time", this.endTime);
			parameters.Add("sku_id", this.skuId);
			parameters.Add("favor_mode", this.favorMode);
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








        
 

