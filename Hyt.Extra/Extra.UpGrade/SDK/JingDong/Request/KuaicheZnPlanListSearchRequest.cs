using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class KuaicheZnPlanListSearchRequest : IJdRequest<KuaicheZnPlanListSearchResponse>
{
		                                                                                                                                                                   
public   		string
   planName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   mode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                                          
public   		string
   isQueryByStatus  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		Nullable<int>
   begin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   end  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.kuaiche.zn.plan.list.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("plan_name", this.planName);
			parameters.Add("mode", this.mode);
			parameters.Add("status", this.status);
			parameters.Add("is_query_by_status", this.isQueryByStatus);
			parameters.Add("begin", this.begin);
			parameters.Add("end", this.end);
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








        
 

