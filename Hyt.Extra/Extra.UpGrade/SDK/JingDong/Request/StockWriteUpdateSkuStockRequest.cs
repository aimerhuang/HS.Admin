using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class StockWriteUpdateSkuStockRequest : IJdRequest<StockWriteUpdateSkuStockResponse>
{
		                                                                                                                                                                                                                                                                                                                                  
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   stockNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   storeId  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.stock.write.updateSkuStock";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("skuId", this.skuId);
			parameters.Add("stockNum", this.stockNum);
			parameters.Add("storeId", this.storeId);
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








        
 

