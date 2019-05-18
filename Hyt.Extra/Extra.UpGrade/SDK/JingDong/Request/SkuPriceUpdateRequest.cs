using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SkuPriceUpdateRequest : IJdRequest<SkuPriceUpdateResponse>
{
		                                                                                                                                  
public   		string
   skuId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   outerId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   price  { get; set; }

                  
                                                            
                                                          
public   		string
   tradeNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   marketPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   jdPrice  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.sku.price.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("sku_id", this.skuId);
			parameters.Add("outer_id", this.outerId);
			parameters.Add("price", this.price);
			parameters.Add("trade_no", this.tradeNo);
			parameters.Add("market_price", this.marketPrice);
			parameters.Add("jd_price", this.jdPrice);
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








        
 

