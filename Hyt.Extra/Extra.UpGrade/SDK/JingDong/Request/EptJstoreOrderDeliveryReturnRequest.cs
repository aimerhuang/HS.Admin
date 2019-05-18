using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptJstoreOrderDeliveryReturnRequest : IJdRequest<EptJstoreOrderDeliveryReturnResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   orderNo  { get; set; }

                  
                                                            
                                                          
public   		string
   carrierCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   wayBillNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   deliveryTime  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.jstore.order.delivery.return";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderNo", this.orderNo);
			parameters.Add("carrierCode", this.carrierCode);
			parameters.Add("wayBillNo", this.wayBillNo);
			parameters.Add("deliveryTime", this.deliveryTime);
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








        
 

