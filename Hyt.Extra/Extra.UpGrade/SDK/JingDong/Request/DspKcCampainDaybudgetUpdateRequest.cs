using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspKcCampainDaybudgetUpdateRequest : IJdRequest<DspKcCampainDaybudgetUpdateResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   campaignId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   dayBudget  { get; set; }

                  
                                                            
                                                                                                                                    
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.kc.campain.daybudget.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("campaignId", this.campaignId);
			parameters.Add("dayBudget", this.dayBudget);
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








        
 

