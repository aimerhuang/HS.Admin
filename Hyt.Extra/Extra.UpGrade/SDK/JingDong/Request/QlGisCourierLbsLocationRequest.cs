using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class QlGisCourierLbsLocationRequest : IJdRequest<QlGisCourierLbsLocationResponse>
{
		                                                                      
public   		string
   courierId  { get; set; }

                  
                                                            
                                                          
public   		string
   waybillCode  { get; set; }

                  
                                                            
                                                          
public   		string
   appCode  { get; set; }

                  
                                                            
                                                          
public   		string
   sign  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ql.gis.courier.lbs.location";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("courierId", this.courierId);
			parameters.Add("waybillCode", this.waybillCode);
			parameters.Add("appCode", this.appCode);
			parameters.Add("sign", this.sign);
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








        
 

