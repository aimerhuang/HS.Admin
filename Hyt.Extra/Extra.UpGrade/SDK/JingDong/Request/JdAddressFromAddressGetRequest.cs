using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JdAddressFromAddressGetRequest : IJdRequest<JdAddressFromAddressGetResponse>
{
		                                                                                                                                  
public   		string
   userid  { get; set; }

                  
                                                            
                                                          
public   		string
   key  { get; set; }

                  
                                                            
                                                          
public   		string
   provinceId  { get; set; }

                  
                                                            
                                                          
public   		string
   cityId  { get; set; }

                  
                                                            
                                                          
public   		string
   countryId  { get; set; }

                  
                                                            
                                                          
public   		string
   townId  { get; set; }

                  
                                                            
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		string
   shipping  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.JdAddressFromAddress.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("userid", this.userid);
			parameters.Add("key", this.key);
			parameters.Add("provinceId", this.provinceId);
			parameters.Add("cityId", this.cityId);
			parameters.Add("countryId", this.countryId);
			parameters.Add("townId", this.townId);
			parameters.Add("address", this.address);
			parameters.Add("shipping", this.shipping);
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








        
 

