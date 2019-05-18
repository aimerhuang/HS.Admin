using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ImageWriteUpdateRectangleRequest : IJdRequest<ImageWriteUpdateRectangleResponse>
{
		                                                                                                                                                                                                                                                                                                       
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                          
public   		string
   colorId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   imgId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   imgRectangleUrl  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   imgIndex  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.image.write.updateRectangle";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wareId", this.wareId);
			parameters.Add("colorId", this.colorId);
			parameters.Add("imgId", this.imgId);
			parameters.Add("imgRectangleUrl", this.imgRectangleUrl);
			parameters.Add("imgIndex", this.imgIndex);
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








        
 

