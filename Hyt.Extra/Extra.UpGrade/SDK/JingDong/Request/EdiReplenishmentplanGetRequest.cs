using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiReplenishmentplanGetRequest : IJdRequest<EdiReplenishmentplanGetResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   sendTimeStart  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   sendTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.replenishmentplan.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("status", this.status);
			parameters.Add("sendTimeStart", this.sendTimeStart);
			parameters.Add("sendTimeEnd", this.sendTimeEnd);
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








        
 

