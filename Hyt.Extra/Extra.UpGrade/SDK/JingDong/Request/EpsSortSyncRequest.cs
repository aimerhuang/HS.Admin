using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EpsSortSyncRequest : IJdRequest<EpsSortSyncResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                        		public  		string
   deliveryNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   provinceId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   provinceName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   cityId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   cityName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   countyId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   countyName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   townId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   townName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   sortCenterCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   siteNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   sortAddress  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   type  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eps.sortSync";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deliveryNo", this.deliveryNo);
			parameters.Add("provinceId", this.provinceId);
			parameters.Add("provinceName", this.provinceName);
			parameters.Add("cityId", this.cityId);
			parameters.Add("cityName", this.cityName);
			parameters.Add("countyId", this.countyId);
			parameters.Add("countyName", this.countyName);
			parameters.Add("townName", this.townName);
			parameters.Add("sortCenterCode", this.sortCenterCode);
			parameters.Add("siteNo", this.siteNo);
			parameters.Add("sortAddress", this.sortAddress);
			parameters.Add("type", this.type);
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








        
 

