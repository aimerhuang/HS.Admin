using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WxsqMjgjLinkGetOpenLinkRequest : IJdRequest<WxsqMjgjLinkGetOpenLinkResponse>
{
		                                                                      
public   		string
   rurl  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   jump  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.wxsq.mjgj.link.GetOpenLink";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("rurl", this.rurl);
			parameters.Add("jump", this.jump);
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








        
 

