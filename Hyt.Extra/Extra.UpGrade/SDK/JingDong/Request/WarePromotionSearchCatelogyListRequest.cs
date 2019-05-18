using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WarePromotionSearchCatelogyListRequest : IJdRequest<WarePromotionSearchCatelogyListResponse>
{
		                                                                      
public   		string
   catelogyId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   page  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   client  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ware.promotion.search.catelogy.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("catelogyId", this.catelogyId);
			parameters.Add("page", this.page);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("client", this.client);
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








        
 

