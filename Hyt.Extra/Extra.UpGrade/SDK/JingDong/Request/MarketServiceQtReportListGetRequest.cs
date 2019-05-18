using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class MarketServiceQtReportListGetRequest : IJdRequest<MarketServiceQtReportListGetResponse>
{
		                                                                                                                                  
public   		string
   serviceItemCode  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<int>
   qtType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   spName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   startTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pin  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.market.service.qt.report.list.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("service_item_code", this.serviceItemCode);
			parameters.Add("qt_type", this.qtType);
			parameters.Add("sp_name", this.spName);
			parameters.Add("start_time", this.startTime);
			parameters.Add("end_time", this.endTime);
			parameters.Add("pin", this.pin);
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








        
 

