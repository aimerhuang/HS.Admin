using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspFeaturedUpdatestatusRequest : IJdRequest<DspFeaturedUpdatestatusResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                           		public  		string
   id  { get; set; }
                                                                                                                                                                                                        
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.featured.updatestatus";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("status", this.status);
			parameters.Add("id", this.id);
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








        
 

