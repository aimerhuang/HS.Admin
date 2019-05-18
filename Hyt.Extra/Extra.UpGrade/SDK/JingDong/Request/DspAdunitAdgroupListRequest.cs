using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspAdunitAdgroupListRequest : IJdRequest<DspAdunitAdgroupListResponse>
{
		                                                                                                       
public   		string
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   pageNum  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<long>
   campaignId  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.adunit.adgroup.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("pageNum", this.pageNum);
			parameters.Add("campaignId", this.campaignId);
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








        
 

