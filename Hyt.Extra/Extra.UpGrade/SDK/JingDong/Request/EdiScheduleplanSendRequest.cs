using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiScheduleplanSendRequest : IJdRequest<EdiScheduleplanSendResponse>
{
		                                                                                                                                  
public   		string
   schedulePlanCode  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   jdSku  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   vendorProductId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   schedulePlanTime  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   quantity  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.scheduleplan.send";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("schedulePlanCode", this.schedulePlanCode);
			parameters.Add("jdSku", this.jdSku);
			parameters.Add("vendorProductId", this.vendorProductId);
			parameters.Add("schedulePlanTime", this.schedulePlanTime);
			parameters.Add("quantity", this.quantity);
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








        
 

