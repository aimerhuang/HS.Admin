using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ProcurementOrderConfirmRequest : IJdRequest<ProcurementOrderConfirmResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   deliveryTime  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   wareId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   deliverCenterId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   backExplanationType  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   confirmNum  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.procurement.order.confirm";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderId", this.orderId);
			parameters.Add("deliveryTime", this.deliveryTime);
			parameters.Add("wareId", this.wareId);
			parameters.Add("deliverCenterId", this.deliverCenterId);
			parameters.Add("backExplanationType", this.backExplanationType);
			parameters.Add("confirmNum", this.confirmNum);
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








        
 

