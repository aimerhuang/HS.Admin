using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CrmSamsMemberSearchRequest : IJdRequest<CrmSamsMemberSearchResponse>
{
		                                                                      
public   		Nullable<int>
   type  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   page  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.crm.sams.member.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("type", this.type);
			parameters.Add("startDate", this.startDate);
			parameters.Add("endDate", this.endDate);
			parameters.Add("page", this.page);
			parameters.Add("pageSize", this.pageSize);
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








        
 

