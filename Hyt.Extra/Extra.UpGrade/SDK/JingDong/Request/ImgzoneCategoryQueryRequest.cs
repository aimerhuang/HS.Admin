using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ImgzoneCategoryQueryRequest : IJdRequest<ImgzoneCategoryQueryResponse>
{
		                                                                                                       
public   		Nullable<long>
   cateId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   cateName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   parentCateId  { get; set; }

                  
                                                                                                                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.imgzone.category.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("cate_id", this.cateId);
			parameters.Add("cate_name", this.cateName);
			parameters.Add("parent_cate_id", this.parentCateId);
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








        
 

