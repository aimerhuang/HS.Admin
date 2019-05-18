using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class NewWareBaseproductGetRequest : IJdRequest<NewWareBaseproductGetResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                       		public  		string
   ids  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   basefields  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.new.ware.baseproduct.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ids", this.ids);
			parameters.Add("basefields", this.basefields);
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








        
 

