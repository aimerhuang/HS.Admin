using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopOrderGetRemarkByCreateTimeRequest : IJdRequest<PopOrderGetRemarkByCreateTimeResponse>
{
		                                                                                                                                                                   
public   		string
   startTime  { get; set; }

                  
                                                            
                                                          
public   		string
   endTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   page  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   sortTime  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.order.getRemarkByCreateTime";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("startTime", this.startTime);
			parameters.Add("endTime", this.endTime);
			parameters.Add("page", this.page);
			parameters.Add("sortTime", this.sortTime);
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








        
 

