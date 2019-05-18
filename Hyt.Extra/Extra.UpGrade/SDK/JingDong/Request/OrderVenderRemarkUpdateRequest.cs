using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class OrderVenderRemarkUpdateRequest : IJdRequest<OrderVenderRemarkUpdateResponse>
{
		                                                                                                                                  
public   		string
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                                          
public   		string
   tradeNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   flag  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.order.vender.remark.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("order_id", this.orderId);
			parameters.Add("remark", this.remark);
			parameters.Add("trade_no", this.tradeNo);
			parameters.Add("flag", this.flag);
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








        
 

