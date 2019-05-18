using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpSphQueryServiceOrderListRequest : IJdRequest<EclpSphQueryServiceOrderListResponse>
{
		                                                                                                                                  
public   		string
   timeType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   beginTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                            
                                                          
public   		string
   status  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   serviceId  { get; set; }

                  
                                                            
                                                          
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.sph.queryServiceOrderList";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("timeType", this.timeType);
			parameters.Add("beginTime", this.beginTime);
			parameters.Add("endTime", this.endTime);
			parameters.Add("status", this.status);
			parameters.Add("serviceId", this.serviceId);
			parameters.Add("deptNo", this.deptNo);
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








        
 

