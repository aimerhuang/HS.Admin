using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpCategoryGetThirdLevelCategoriesRequest : IJdRequest<EclpCategoryGetThirdLevelCategoriesResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   secondCategoryNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   thirdCategoryNo  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.category.getThirdLevelCategories";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("secondCategoryNo", this.secondCategoryNo);
			parameters.Add("thirdCategoryNo", this.thirdCategoryNo);
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








        
 

