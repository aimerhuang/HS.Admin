using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcConfirmpurchaseorderRequest : IJdRequest<VcConfirmpurchaseorderResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   deliveryTime  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      		public  		string
   wareId  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   confirmNum  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   backExplanation  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         		public  		string
   backExplanationType  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         		public  		string
   deliverCenterId  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.confirmpurchaseorder";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("order_id", this.orderId);
			parameters.Add("delivery_time", this.deliveryTime);
			parameters.Add("ware_id", this.wareId);
			parameters.Add("confirm_num", this.confirmNum);
			parameters.Add("back_explanation", this.backExplanation);
			parameters.Add("back_explanation_type", this.backExplanationType);
			parameters.Add("deliver_center_id", this.deliverCenterId);
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








        
 

