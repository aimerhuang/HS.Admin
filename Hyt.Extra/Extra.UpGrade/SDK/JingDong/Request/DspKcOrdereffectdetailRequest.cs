using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DspKcOrdereffectdetailRequest : IJdRequest<DspKcOrdereffectdetailResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   campaignId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   groupId  { get; set; }

                  
                                                            
                                                          
public   		string
   mySelf  { get; set; }

                  
                                                            
                                                          
public   		string
   platform  { get; set; }

                  
                                                            
                                                          
public   		string
   province  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   retrievalType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   orderStatus  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   clickStartDay  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   clickEndDay  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   orderStartDay  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   orderEndDay  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   realTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.dsp.kc.ordereffectdetail";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("campaignId", this.campaignId);
			parameters.Add("groupId", this.groupId);
			parameters.Add("mySelf", this.mySelf);
			parameters.Add("platform", this.platform);
			parameters.Add("province", this.province);
			parameters.Add("retrievalType", this.retrievalType);
			parameters.Add("orderStatus", this.orderStatus);
			parameters.Add("clickStartDay", this.clickStartDay);
			parameters.Add("clickEndDay", this.clickEndDay);
			parameters.Add("orderStartDay", this.orderStartDay);
			parameters.Add("orderEndDay", this.orderEndDay);
			parameters.Add("realTime", this.realTime);
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








        
 

