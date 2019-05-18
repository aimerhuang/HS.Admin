using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcAplsStockBatchGetProdStockInfoRequest : IJdRequest<VcAplsStockBatchGetProdStockInfoResponse>
{
		                                                                                                                                  
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                                                      
public   		string
   skuList  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.apls.stock.batchGetProdStockInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("skuList", this.skuList);
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








        
 

