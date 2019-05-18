using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class MiniwmsOcGoodsQuerySkuJdPriceRequest : IJdRequest<MiniwmsOcGoodsQuerySkuJdPriceResponse>
{
		                                                                                                                                  
public   		string
   sku  { get; set; }

                  
                                                            
                                                          
public   		string
   stationId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.miniwms.oc.goods.querySkuJdPrice";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("sku", this.sku);
			parameters.Add("stationId", this.stationId);
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








        
 

