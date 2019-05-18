using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class AddressCompleteServiceRequest : IJdRequest<AddressCompleteServiceResponse>
{
		                                                                      
public   		Nullable<int>
   provinceid  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   cityid  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   countryid  { get; set; }

                  
                                                            
                                                          
public   		string
   address  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.AddressCompleteService";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("provinceid", this.provinceid);
			parameters.Add("cityid", this.cityid);
			parameters.Add("countryid", this.countryid);
			parameters.Add("address", this.address);
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








        
 

