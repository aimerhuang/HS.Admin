using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ImgzonePictureUpdateRequest : IJdRequest<ImgzonePictureUpdateResponse>
{
		                                                                                                       
public   		string
   pictureId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pictureName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   pictureCateId  { get; set; }

                  
                                                                                                                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.imgzone.picture.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("picture_id", this.pictureId);
			parameters.Add("picture_name", this.pictureName);
			parameters.Add("picture_cate_id", this.pictureCateId);
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








        
 

