using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class OrderOrderDeleteApplyRequest : IJdRequest<OrderOrderDeleteApplyResponse>
{
		                                                                                                                                                                                                    
public   		string
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   delApplyType  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   delApplyReason  { get; set; }

                  
                                                                                                                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.order.orderDelete.apply";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("order_id", this.orderId);
			parameters.Add("del_apply_type", this.delApplyType);
			parameters.Add("del_apply_reason", this.delApplyReason);
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








        
 

