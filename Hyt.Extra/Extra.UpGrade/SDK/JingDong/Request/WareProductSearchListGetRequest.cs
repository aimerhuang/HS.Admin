using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareProductSearchListGetRequest : IJdRequest<WareProductSearchListGetResponse>
{
		                                                                      
public   		Nullable<bool>
   isLoadAverageScore  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isLoadPromotion  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   sort  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   page  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   keyword  { get; set; }

                  
                                                            
                                                          
public   		string
   client  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ware.product.search.list.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("isLoadAverageScore", this.isLoadAverageScore);
			parameters.Add("isLoadPromotion", this.isLoadPromotion);
			parameters.Add("sort", this.sort);
			parameters.Add("page", this.page);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("keyword", this.keyword);
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








        
 

