using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SkuWriteSaveWareSkusRequest : IJdRequest<SkuWriteSaveWareSkusResponse>
{
		                                                                                                                                                                                                                                                                                                       
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                     		public  		string
   skuId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   saleAttrs  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   skuFeatures  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   jdPrice  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   outerId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   stockNum  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   barCode  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.sku.write.saveWareSkus";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wareId", this.wareId);
			parameters.Add("skuId", this.skuId);
			parameters.Add("saleAttrs", this.saleAttrs);
			parameters.Add("skuFeatures", this.skuFeatures);
			parameters.Add("jdPrice", this.jdPrice);
			parameters.Add("outerId", this.outerId);
			parameters.Add("stockNum", this.stockNum);
			parameters.Add("barCode", this.barCode);
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








        
 

