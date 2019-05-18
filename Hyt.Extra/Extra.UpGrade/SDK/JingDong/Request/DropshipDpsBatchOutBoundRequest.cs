using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DropshipDpsBatchOutBoundRequest : IJdRequest<DropshipDpsBatchOutBoundResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                                                         		public  		string
   customOrderId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   memoByVendor  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   isJdexpress  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   parentOrderId  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dropship.dps.batchOutBound";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("customOrderId", this.customOrderId);
			parameters.Add("memoByVendor", this.memoByVendor);
			parameters.Add("isJdexpress", this.isJdexpress);
			parameters.Add("parentOrderId", this.parentOrderId);
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








        
 

