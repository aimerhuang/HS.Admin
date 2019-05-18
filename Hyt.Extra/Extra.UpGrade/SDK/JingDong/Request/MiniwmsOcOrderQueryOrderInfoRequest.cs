using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class MiniwmsOcOrderQueryOrderInfoRequest : IJdRequest<MiniwmsOcOrderQueryOrderInfoResponse>
{
		                                                                      
public   		string
   stationId  { get; set; }

                  
                                                            
                                                          
public   		string
   orderId  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.miniwms.oc.order.queryOrderInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("stationId", this.stationId);
			parameters.Add("orderId", this.orderId);
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








        
 

