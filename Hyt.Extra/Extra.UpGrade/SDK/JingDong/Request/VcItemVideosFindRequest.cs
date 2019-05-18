using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcItemVideosFindRequest : IJdRequest<VcItemVideosFindResponse>
{
		                                                                                                                                                                   
public   		string
   keywords  { get; set; }

                  
                                                            
                                                          
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   length  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.item.videos.find";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("keywords", this.keywords);
			parameters.Add("page", this.page);
			parameters.Add("length", this.length);
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








        
 

