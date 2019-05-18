using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ImgzonePictureUploadRequest : IJdRequest<ImgzonePictureUploadResponse>
{
		                                                                                                       
public   		string
   imageData  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   pictureCateId  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   pictureName  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.imgzone.picture.upload";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("image_data", this.imageData);
			parameters.Add("picture_cate_id", this.pictureCateId);
			parameters.Add("picture_name", this.pictureName);
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








        
 

