using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WarePropimgUpdateRequest : IJdRequest<WarePropimgUpdateResponse>
{
		                                                                                                                                  
public   		string
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   attributeValueId  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<bool>
   isMainPic  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   imageId  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.ware.propimg.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ware_id", this.wareId);
			parameters.Add("attribute_value_id", this.attributeValueId);
			parameters.Add("is_main_pic", this.isMainPic);
			parameters.Add("image_id", this.imageId);
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








        
 
