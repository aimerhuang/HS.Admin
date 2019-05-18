using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsO2oOrderCarriageRequest : IJdRequest<LogisticsO2oOrderCarriageResponse>
{
		                                                                                                       
public   		string
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   carrierNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   deliverNo  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.o2o.order.carriage";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("order_id", this.orderId);
			parameters.Add("carrier_no", this.carrierNo);
			parameters.Add("deliver_no", this.deliverNo);
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








        
 

