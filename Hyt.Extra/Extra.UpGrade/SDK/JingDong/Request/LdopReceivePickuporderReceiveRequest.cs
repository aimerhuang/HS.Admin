using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LdopReceivePickuporderReceiveRequest : IJdRequest<LdopReceivePickuporderReceiveResponse>
{
		                                                                                                                                  
public   		string
   pickupAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   pickupName  { get; set; }

                  
                                                            
                                                          
public   		string
   pickupTel  { get; set; }

                  
                                                            
                                                          
public   		string
   customerTel  { get; set; }

                  
                                                            
                                                          
public   		string
   customerCode  { get; set; }

                  
                                                            
                                                          
public   		string
   backAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   customerContract  { get; set; }

                  
                                                            
                                                          
public   		string
   desp  { get; set; }

                  
                                                            
                                                          
public   		string
   orderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   weight  { get; set; }

                  
                                                            
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   volume  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ldop.receive.pickuporder.receive";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("pickupAddress", this.pickupAddress);
			parameters.Add("pickupName", this.pickupName);
			parameters.Add("pickupTel", this.pickupTel);
			parameters.Add("customerTel", this.customerTel);
			parameters.Add("customerCode", this.customerCode);
			parameters.Add("backAddress", this.backAddress);
			parameters.Add("customerContract", this.customerContract);
			parameters.Add("desp", this.desp);
			parameters.Add("orderId", this.orderId);
			parameters.Add("weight", this.weight);
			parameters.Add("remark", this.remark);
			parameters.Add("volume", this.volume);
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








        
 

