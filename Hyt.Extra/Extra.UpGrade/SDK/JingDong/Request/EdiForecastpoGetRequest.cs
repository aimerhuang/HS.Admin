using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiForecastpoGetRequest : IJdRequest<EdiForecastpoGetResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   forecastTimeStart  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   forecastTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.forecastpo.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("status", this.status);
			parameters.Add("forecastTimeStart", this.forecastTimeStart);
			parameters.Add("forecastTimeEnd", this.forecastTimeEnd);
			parameters.Add("pageNum", this.pageNum);
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








        
 

