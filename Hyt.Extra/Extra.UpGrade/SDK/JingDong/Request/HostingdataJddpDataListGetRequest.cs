using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class HostingdataJddpDataListGetRequest : IJdRequest<HostingdataJddpDataListGetResponse>
{
		                                                                                                                                  
public   		string
   sqlId  { get; set; }

                  
                                                            
                                                          
public   		string
   parameter  { get; set; }

                  
                                                            
                                                                                                                                    
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.hostingdata.jddp.data.list.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("sqlId", this.sqlId);
			parameters.Add("parameter", this.parameter);
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








        
 

