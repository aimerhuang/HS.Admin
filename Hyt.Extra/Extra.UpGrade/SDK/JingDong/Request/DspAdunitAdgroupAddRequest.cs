using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspAdunitAdgroupAddRequest : IJdRequest<DspAdunitAdgroupAddResponse>
{
		                                                                                                                                  
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   campaignId  { get; set; }

                  
                                                            
                                                                                           
public   		string
   position  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   inFee  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   outFee  { get; set; }

                  
                                                            
                                                                                                                                                             
public   		Nullable<int>
   adDevice  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.adunit.adgroup.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("name", this.name);
			parameters.Add("campaignId", this.campaignId);
			parameters.Add("position", this.position);
			parameters.Add("inFee", this.inFee);
			parameters.Add("outFee", this.outFee);
			parameters.Add("adDevice", this.adDevice);
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








        
 

