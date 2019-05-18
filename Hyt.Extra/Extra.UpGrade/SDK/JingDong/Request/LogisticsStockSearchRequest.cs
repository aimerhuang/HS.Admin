using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsStockSearchRequest : IJdRequest<LogisticsStockSearchResponse>
{
		                                                                      
public   		string
   warehouseNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   goodsNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   currentPage  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.stock.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("warehouse_no", this.warehouseNo);
			parameters.Add("goods_no", this.goodsNo);
			parameters.Add("current_page", this.currentPage);
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








        
 

