using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopOtoLocorderinfosGetRequest : IJdRequest<PopOtoLocorderinfosGetResponse>
{
		                                                                                                       
public   		Nullable<int>
   timeType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   startDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   codeStatus  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   codeType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.oto.locorderinfos.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("time_type", this.timeType);
			parameters.Add("start_date", this.startDate);
			parameters.Add("end_date", this.endDate);
			parameters.Add("code_status", this.codeStatus);
			parameters.Add("code_type", this.codeType);
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








        
 

