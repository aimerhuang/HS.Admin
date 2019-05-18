using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspAdkcunitGoodspriceUpdateRequest : IJdRequest<DspAdkcunitGoodspriceUpdateResponse>
{
		                                                                      
public   		Nullable<long>
   id  { get; set; }

                  
                                                            
                                                          
public   		string
   feeStr  { get; set; }

                  
                                                            
                                                          
public   		string
   inSearchFeeStr  { get; set; }

                  
                                                            
                                                                                                                            
public   		Nullable<double>
   mobilePriceCoef  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.adkcunit.goodsprice.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("id", this.id);
			parameters.Add("feeStr", this.feeStr);
			parameters.Add("inSearchFeeStr", this.inSearchFeeStr);
			parameters.Add("mobilePriceCoef", this.mobilePriceCoef);
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








        
 

