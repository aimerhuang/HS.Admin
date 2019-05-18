using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpStockQueryVmiShopStockFlowRequest : IJdRequest<EclpStockQueryVmiShopStockFlowResponse>
{
		                                                                                                                                  
public   		Nullable<DateTime>
   startTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                            
                                                          
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   shopNo  { get; set; }

                  
                                                            
                                                          
public   		string
   goodsNo  { get; set; }

                  
                                                            
                                                          
public   		string
   startPage  { get; set; }

                  
                                                            
                                                          
public   		string
   onePageNum  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.stock.queryVmiShopStockFlow";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("startTime", this.startTime);
			parameters.Add("endTime", this.endTime);
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("shopNo", this.shopNo);
			parameters.Add("goodsNo", this.goodsNo);
			parameters.Add("startPage", this.startPage);
			parameters.Add("onePageNum", this.onePageNum);
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








        
 

