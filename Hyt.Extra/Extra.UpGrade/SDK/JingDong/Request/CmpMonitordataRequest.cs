using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CmpMonitordataRequest : IJdRequest<CmpMonitordataResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                        		public  		string
   devCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   time  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   devMac  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   dataType  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   val  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   longitude  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   latitude  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   battery  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   status  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.cmp.monitordata";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("devCode", this.devCode);
			parameters.Add("time", this.time);
			parameters.Add("devMac", this.devMac);
			parameters.Add("dataType", this.dataType);
			parameters.Add("val", this.val);
			parameters.Add("longitude", this.longitude);
			parameters.Add("latitude", this.latitude);
			parameters.Add("battery", this.battery);
			parameters.Add("status", this.status);
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








        
 

