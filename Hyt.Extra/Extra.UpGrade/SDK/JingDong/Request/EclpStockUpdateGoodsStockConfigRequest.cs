using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpStockUpdateGoodsStockConfigRequest : IJdRequest<EclpStockUpdateGoodsStockConfigResponse>
{
		                                                                      
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   goodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   sellerGoodsSign  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   shopNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   stockWay  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   percent  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		Nullable<int>
   fixedNum  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   warehouseNo  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.stock.updateGoodsStockConfig";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("goodsNo", this.goodsNo);
			parameters.Add("sellerGoodsSign", this.sellerGoodsSign);
			parameters.Add("shopNo", this.shopNo);
			parameters.Add("stockWay", this.stockWay);
			parameters.Add("percent", this.percent);
			parameters.Add("fixedNum", this.fixedNum);
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








        
 

