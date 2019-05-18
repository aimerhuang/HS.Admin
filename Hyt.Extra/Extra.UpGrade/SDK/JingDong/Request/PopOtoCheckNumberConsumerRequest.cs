using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopOtoCheckNumberConsumerRequest : IJdRequest<PopOtoCheckNumberConsumerResponse>
{
		                                                                                                       
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   cardNumber  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pwdUmber  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   shopId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   shopName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   codeType  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.oto.CheckNumber.consumer";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("order_id", this.orderId);
			parameters.Add("card_number", this.cardNumber);
			parameters.Add("pwd_umber", this.pwdUmber);
			parameters.Add("shop_id", this.shopId);
			parameters.Add("shop_name", this.shopName);
			parameters.Add("code_type", this.codeType);
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








        
 

