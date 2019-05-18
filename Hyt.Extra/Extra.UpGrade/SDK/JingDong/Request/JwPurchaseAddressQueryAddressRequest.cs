using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JwPurchaseAddressQueryAddressRequest : IJdRequest<JwPurchaseAddressQueryAddressResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   addressLevel  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   addressLevel1Id  { get; set; }

                  
                                                            
                                                          
public   		string
   addressLevel2Id  { get; set; }

                  
                                                            
                                                          
public   		string
   addressLevel3Id  { get; set; }

                  
                                                            
                                                                                                                                    
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jw.purchase.address.queryAddress";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("addressLevel", this.addressLevel);
			parameters.Add("addressLevel1Id", this.addressLevel1Id);
			parameters.Add("addressLevel2Id", this.addressLevel2Id);
			parameters.Add("addressLevel3Id", this.addressLevel3Id);
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








        
 

