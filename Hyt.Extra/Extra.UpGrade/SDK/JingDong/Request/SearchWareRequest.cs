using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SearchWareRequest : IJdRequest<SearchWareResponse>
{
		                                                                      
public   		string
   key  { get; set; }

                  
                                                            
                                                          
public   		string
   filtType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   areaIds  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   sortType  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		string
   page  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.search.ware";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("key", this.key);
			parameters.Add("filt_type", this.filtType);
			parameters.Add("area_ids", this.areaIds);
			parameters.Add("sort_type", this.sortType);
			parameters.Add("page", this.page);
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








        
 

