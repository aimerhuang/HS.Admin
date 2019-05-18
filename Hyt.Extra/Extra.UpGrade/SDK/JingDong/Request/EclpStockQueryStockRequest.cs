using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpStockQueryStockRequest : IJdRequest<EclpStockQueryStockResponse>
{
		                                                                                                                                  
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   stockStatus  { get; set; }

                  
                                                            
                                                          
public   		string
   stockType  { get; set; }

                  
                                                            
                                                          
public   		string
   goodsNo  { get; set; }

                  
                                                            
                                                          
public   		string
   currentPage  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.stock.queryStock";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("stockStatus", this.stockStatus);
			parameters.Add("stockType", this.stockType);
			parameters.Add("goodsNo", this.goodsNo);
			parameters.Add("currentPage", this.currentPage);
			parameters.Add("pageSize", this.pageSize);
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








        
 

