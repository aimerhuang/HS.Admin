using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspAdreportQueryaddailysumRequest : IJdRequest<DspAdreportQueryaddailysumResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   campaignId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   groupId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   adId  { get; set; }

                  
                                                            
                                                          
public   		string
   platform  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startDay  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endDay  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   OrderStatusCategory  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isTodayOr15Days  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isOrderOrClick  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isDaily  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.adreport.queryaddailysum";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("campaignId", this.campaignId);
			parameters.Add("groupId", this.groupId);
			parameters.Add("adId", this.adId);
			parameters.Add("platform", this.platform);
			parameters.Add("startDay", this.startDay);
			parameters.Add("endDay", this.endDay);
			parameters.Add("OrderStatusCategory", this.OrderStatusCategory);
			parameters.Add("isTodayOr15Days", this.isTodayOr15Days);
			parameters.Add("isOrderOrClick", this.isOrderOrClick);
			parameters.Add("isDaily", this.isDaily);
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








        
 

