using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SrmBookInfoQueryRequest : IJdRequest<SrmBookInfoQueryResponse>
{
		                                                                      
public   		string
   pin  { get; set; }

                  
                                                            
                                                          
public   		string
   venderId  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   poNo  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.srm.book.info.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("pin", this.pin);
			parameters.Add("venderId", this.venderId);
			parameters.Add("poNo", this.poNo);
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








        
 

