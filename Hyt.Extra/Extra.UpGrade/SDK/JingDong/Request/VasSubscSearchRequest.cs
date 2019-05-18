using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VasSubscSearchRequest : IJdRequest<VasSubscSearchResponse>
{
		                                                                                                                                                                   
public   		string
   serviceCode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   timeQueryType  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   startDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   endDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   articleType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   buyer  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vas.subsc.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("service_code", this.serviceCode);
			parameters.Add("time_query_type", this.timeQueryType);
			parameters.Add("start_date", this.startDate);
			parameters.Add("end_date", this.endDate);
			parameters.Add("article_type", this.articleType);
			parameters.Add("buyer", this.buyer);
			parameters.Add("page_index", this.pageIndex);
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








        
 

