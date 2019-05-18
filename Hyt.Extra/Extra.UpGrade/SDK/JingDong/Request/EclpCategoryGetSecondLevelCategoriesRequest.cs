using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpCategoryGetSecondLevelCategoriesRequest : IJdRequest<EclpCategoryGetSecondLevelCategoriesResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   firstCategoryNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   secondCategoryNo  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.category.getSecondLevelCategories";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("firstCategoryNo", this.firstCategoryNo);
			parameters.Add("secondCategoryNo", this.secondCategoryNo);
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








        
 

