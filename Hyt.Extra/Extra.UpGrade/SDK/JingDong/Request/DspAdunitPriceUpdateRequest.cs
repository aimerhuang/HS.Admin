using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspAdunitPriceUpdateRequest : IJdRequest<DspAdunitPriceUpdateResponse>
{
		                                                                      
public   		Nullable<long>
   id  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   inFee  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   outFee  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   adxFee  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.adunit.price.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("id", this.id);
			parameters.Add("inFee", this.inFee);
			parameters.Add("outFee", this.outFee);
			parameters.Add("adxFee", this.adxFee);
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








        
 

