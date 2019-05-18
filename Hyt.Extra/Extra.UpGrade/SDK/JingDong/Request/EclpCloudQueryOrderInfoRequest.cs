using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpCloudQueryOrderInfoRequest : IJdRequest<EclpCloudQueryOrderInfoResponse>
{
		                                                                                                                                  
public   		string
   machiningNo  { get; set; }

                  
                                                            
                                                          
public   		string
   machiningType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   timeStart  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   timeEnd  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.cloud.queryOrderInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("machiningNo", this.machiningNo);
			parameters.Add("machiningType", this.machiningType);
			parameters.Add("timeStart", this.timeStart);
			parameters.Add("timeEnd", this.timeEnd);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("tenantId", this.tenantId);
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








        
 

