using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SkuWriteMergeSkuFeaturesRequest : IJdRequest<SkuWriteMergeSkuFeaturesResponse>
{
		                                                                                                                                                                                                                                                                                                       
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   featureKey  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   featureValue  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.sku.write.mergeSkuFeatures";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("skuId", this.skuId);
			parameters.Add("featureKey", this.featureKey);
			parameters.Add("featureValue", this.featureValue);
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








        
 

