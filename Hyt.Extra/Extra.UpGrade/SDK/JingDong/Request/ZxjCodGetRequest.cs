using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ZxjCodGetRequest : IJdRequest<ZxjCodGetResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   provinceId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   cityId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   countyId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   townId  { get; set; }

                  
                                                                                                                                    
                                                                                                   
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.zxj.cod.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("province_id", this.provinceId);
			parameters.Add("city_id", this.cityId);
			parameters.Add("county_id", this.countyId);
			parameters.Add("town_id", this.townId);
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








        
 

