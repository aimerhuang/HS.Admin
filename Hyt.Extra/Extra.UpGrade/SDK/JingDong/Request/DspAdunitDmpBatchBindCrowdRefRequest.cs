using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspAdunitDmpBatchBindCrowdRefRequest : IJdRequest<DspAdunitDmpBatchBindCrowdRefResponse>
{
		                                                                                                       
public   		Nullable<long>
   campaignId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   adGroupId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   adGroupType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   opt  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   crowdId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   isUsed  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   adGroupPrice  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.adunit.dmp.batchBindCrowdRef";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("campaignId", this.campaignId);
			parameters.Add("adGroupId", this.adGroupId);
			parameters.Add("adGroupType", this.adGroupType);
			parameters.Add("opt", this.opt);
			parameters.Add("crowdId", this.crowdId);
			parameters.Add("isUsed", this.isUsed);
			parameters.Add("adGroupPrice", this.adGroupPrice);
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








        
 

