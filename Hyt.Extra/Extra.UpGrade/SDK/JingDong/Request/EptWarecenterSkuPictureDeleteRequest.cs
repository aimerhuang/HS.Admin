using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptWarecenterSkuPictureDeleteRequest : IJdRequest<EptWarecenterSkuPictureDeleteResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   attrValueId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   indexId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.warecenter.skuPicture.delete";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wareId", this.wareId);
			parameters.Add("attrValueId", this.attrValueId);
			parameters.Add("indexId", this.indexId);
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








        
 

