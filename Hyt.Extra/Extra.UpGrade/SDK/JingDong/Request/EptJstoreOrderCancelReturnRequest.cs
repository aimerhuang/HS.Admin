using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptJstoreOrderCancelReturnRequest : IJdRequest<EptJstoreOrderCancelReturnResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   orderNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   cancelResult  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.jstore.order.cancel.return";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderNo", this.orderNo);
			parameters.Add("cancelResult", this.cancelResult);
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








        
 

