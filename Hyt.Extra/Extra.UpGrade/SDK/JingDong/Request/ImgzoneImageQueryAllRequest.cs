using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ImgzoneImageQueryAllRequest : IJdRequest<ImgzoneImageQueryAllResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   categoryId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   imageName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   scrollId  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.imgzone.image.queryAll";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("category_id", this.categoryId);
			parameters.Add("image_name", this.imageName);
			parameters.Add("scroll_id", this.scrollId);
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








        
 

