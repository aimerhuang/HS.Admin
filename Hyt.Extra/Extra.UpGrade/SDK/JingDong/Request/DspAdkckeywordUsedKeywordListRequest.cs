using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspAdkckeywordUsedKeywordListRequest : IJdRequest<DspAdkckeywordUsedKeywordListResponse>
{
		                                                                                                                                  
public   		Nullable<DateTime>
   startDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   id  { get; set; }

                  
                                                            
                                                          
public   		string
   campaignId  { get; set; }

                  
                                                            
                                                          
public   		string
   platform  { get; set; }

                  
                                                            
                                                          
public   		string
   valType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isTodayOr15Days  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isOrderOrClick  { get; set; }

                  
                                                            
                                                                                                                            
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.adkckeyword.usedKeyword.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("startDate", this.startDate);
			parameters.Add("endDate", this.endDate);
			parameters.Add("id", this.id);
			parameters.Add("campaignId", this.campaignId);
			parameters.Add("platform", this.platform);
			parameters.Add("valType", this.valType);
			parameters.Add("isTodayOr15Days", this.isTodayOr15Days);
			parameters.Add("isOrderOrClick", this.isOrderOrClick);
			parameters.Add("pageIndex", this.pageIndex);
			parameters.Add("pageSize", this.pageSize);
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








        
 

