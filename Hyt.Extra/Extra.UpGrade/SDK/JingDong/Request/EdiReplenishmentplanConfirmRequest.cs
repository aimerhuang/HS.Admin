using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiReplenishmentplanConfirmRequest : IJdRequest<EdiReplenishmentplanConfirmResponse>
{
		                                                                                                                                  
public   		string
   replenishmentPlanCode  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   jdSku  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   vendorProductId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   productName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   week  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   weekStartTime  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   weekEndTime  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   sureQuantity  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.replenishmentplan.confirm";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("replenishmentPlanCode", this.replenishmentPlanCode);
			parameters.Add("jdSku", this.jdSku);
			parameters.Add("vendorProductId", this.vendorProductId);
			parameters.Add("productName", this.productName);
			parameters.Add("week", this.week);
			parameters.Add("weekStartTime", this.weekStartTime);
			parameters.Add("weekEndTime", this.weekEndTime);
			parameters.Add("sureQuantity", this.sureQuantity);
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








        
 

