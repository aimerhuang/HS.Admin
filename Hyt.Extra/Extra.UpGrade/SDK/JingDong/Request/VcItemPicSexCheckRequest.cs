using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcItemPicSexCheckRequest : IJdRequest<VcItemPicSexCheckResponse>
{
		                                                                                                       
public   		string
   cid3  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                           		public  		string
   path  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.item.pic.sex.check";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("cid3", this.cid3);
			parameters.Add("path", this.path);
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








        
 

