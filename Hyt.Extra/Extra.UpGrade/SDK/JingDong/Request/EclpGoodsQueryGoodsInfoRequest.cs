using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpGoodsQueryGoodsInfoRequest : IJdRequest<EclpGoodsQueryGoodsInfoResponse>
{
		                                                                                                                                  
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                          
public   		string
   isvGoodsNos  { get; set; }

                  
                                                            
                                                          
public   		string
   goodsNos  { get; set; }

                  
                                                            
                                                          
public   		string
   queryType  { get; set; }

                  
                                                            
                                                          
public   		string
   barcodes  { get; set; }

                  
                                                            
                                                          
public   		string
   pageNo  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.goods.queryGoodsInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("isvGoodsNos", this.isvGoodsNos);
			parameters.Add("goodsNos", this.goodsNos);
			parameters.Add("queryType", this.queryType);
			parameters.Add("barcodes", this.barcodes);
			parameters.Add("pageNo", this.pageNo);
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








        
 

