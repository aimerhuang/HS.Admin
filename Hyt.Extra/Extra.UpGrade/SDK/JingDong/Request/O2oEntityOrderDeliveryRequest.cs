using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class O2oEntityOrderDeliveryRequest : IJdRequest<O2oEntityOrderDeliveryResponse>
{
		                                                                                                                                  
public   		string
   orderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   orderSource  { get; set; }

                  
                                                            
                                                          
public   		string
   carrierNo  { get; set; }

                  
                                                            
                                                          
public   		string
   updatePin  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.o2o.entity.order.delivery";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderId", this.orderId);
			parameters.Add("orderSource", this.orderSource);
			parameters.Add("carrierNo", this.carrierNo);
			parameters.Add("updatePin", this.updatePin);
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








        
 

