using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ImgzonePictureQueryRequest : IJdRequest<ImgzonePictureQueryResponse>
{
		                                                                                                                                                                   
public   		string
   pictureId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pictureCateId  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   pictureName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   startDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   endDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageNum  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.imgzone.picture.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("picture_id", this.pictureId);
			parameters.Add("picture_cate_id", this.pictureCateId);
			parameters.Add("picture_name", this.pictureName);
			parameters.Add("start_date", this.startDate);
			parameters.Add("end_Date", this.endDate);
			parameters.Add("page_num", this.pageNum);
			parameters.Add("page_size", this.pageSize);
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








        
 

