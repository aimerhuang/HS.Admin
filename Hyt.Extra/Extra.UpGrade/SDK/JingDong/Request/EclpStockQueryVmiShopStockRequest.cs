using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpStockQueryVmiShopStockRequest : IJdRequest<EclpStockQueryVmiShopStockResponse>
{
		                                                                                                                                  
public   		string
   goodsNos  { get; set; }

                  
                                                            
                                                          
public   		string
   shopNos  { get; set; }

                  
                                                            
                                                          
public   		string
   currentPage  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.stock.queryVmiShopStock";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("goodsNos", this.goodsNos);
			parameters.Add("shopNos", this.shopNos);
			parameters.Add("currentPage", this.currentPage);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("warehouseNo", this.warehouseNo);
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








        
 

