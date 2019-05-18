using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ImPopChatlogFuzzyQueryRequest : IJdRequest<ImPopChatlogFuzzyQueryResponse>
{
		                                                                                                       
public   		string
   waiter  { get; set; }

                  
                                                            
                                                          
public   		string
   customer  { get; set; }

                  
                                                            
                                                          
public   		string
   keyWord  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                            
                                                          
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.im.pop.chatlog.fuzzy.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("waiter", this.waiter);
			parameters.Add("customer", this.customer);
			parameters.Add("keyWord", this.keyWord);
			parameters.Add("startTime", this.startTime);
			parameters.Add("endTime", this.endTime);
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








        
 

