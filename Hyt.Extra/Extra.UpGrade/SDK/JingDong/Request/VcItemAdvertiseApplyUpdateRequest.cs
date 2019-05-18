using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcItemAdvertiseApplyUpdateRequest : IJdRequest<VcItemAdvertiseApplyUpdateResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   skuId  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   adWord  { get; set; }
                                                                                                                                                                                                
public   		string
   applyId  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.item.advertise.apply.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("sku_id", this.skuId);
			parameters.Add("ad_word", this.adWord);
			parameters.Add("apply_id", this.applyId);
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








        
 

