using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspAdkcunitAdgroupAddRequest : IJdRequest<DspAdkcunitAdgroupAddResponse>
{
		                                                                                                                                  
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   campaignId  { get; set; }

                  
                                                            
                                                          
public   		string
   newAreaId  { get; set; }

                  
                                                            
                                                                                           
public   		string
   feeStr  { get; set; }

                  
                                                            
                                                          
public   		string
   inSearchFeeStr  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   mobilePriceCoef  { get; set; }

                  
                                                            
                                                                                                   
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.adkcunit.adgroup.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("name", this.name);
			parameters.Add("campaignId", this.campaignId);
			parameters.Add("newAreaId", this.newAreaId);
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








        
 

