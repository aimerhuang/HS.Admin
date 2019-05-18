using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptWarecenterSkuimgUpdateRequest : IJdRequest<EptWarecenterSkuimgUpdateResponse>
{
		                                                                                                       
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   saleAttrValue  { get; set; }

                  
                                                            
                                                          
public   		string
   img  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.warecenter.skuimg.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wareId", this.wareId);
			parameters.Add("saleAttrValue", this.saleAttrValue);
			parameters.Add("img", this.img);
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








        
 

