using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ShopcategoriesWriteSaveWareShopCategoriesRequest : IJdRequest<ShopcategoriesWriteSaveWareShopCategoriesResponse>
{
		                                                                                                                                                                                                                                                                                                       
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                           		public  		string
   shopCategory  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.shopcategories.write.saveWareShopCategories";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wareId", this.wareId);
			parameters.Add("shopCategory", this.shopCategory);
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








        
 

