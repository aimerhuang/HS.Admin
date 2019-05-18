using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class KuaicheZnKeywordgroupListSearchRequest : IJdRequest<KuaicheZnKeywordgroupListSearchResponse>
{
		                                                                                                                                                                                                    
public   		Nullable<long>
   thirdCategoryId  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   sortField  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   sortType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.kuaiche.zn.keywordgroup.list.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("third_category_id", this.thirdCategoryId);
			parameters.Add("sort_field", this.sortField);
			parameters.Add("sort_type", this.sortType);
			parameters.Add("page_size", this.pageSize);
			parameters.Add("page_index", this.pageIndex);
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








        
 

