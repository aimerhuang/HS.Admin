using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsO2oEndstatusUpdateRequest : IJdRequest<LogisticsO2oEndstatusUpdateResponse>
{
		                                                                                                       
public   		string
   stationNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   stateOperator  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   stationPaymentCash  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   stationPaymentPos  { get; set; }

                  
                                                                                                                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.o2o.endstatus.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("station_no", this.stationNo);
			parameters.Add("order_id", this.orderId);
			parameters.Add("state_operator", this.stateOperator);
			parameters.Add("station_payment_cash", this.stationPaymentCash);
			parameters.Add("station_payment_pos", this.stationPaymentPos);
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








        
 

