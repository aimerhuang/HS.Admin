using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptWarecenterWarelistGetRequest : IJdRequest<EptWarecenterWarelistGetResponse>
{
		                                                                                                                                                                   
public   		string
   wareIdsStr  { get; set; }

                  
                                                            
                                                          
public   		string
   wareStatus  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   categoryId  { get; set; }

                  
                                                            
                                                          
public   		string
   title  { get; set; }

                  
                                                            
                                                          
public   		string
   itemNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   transportId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startOnlineTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endOnlineTime  { get; set; }

                  
                                                            
                                                          
public   		string
   minSupplyPrice  { get; set; }

                  
                                                            
                                                          
public   		string
   maxSupplyPrice  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   recommendTpid  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   currentPage  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.warecenter.warelist.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wareIdsStr", this.wareIdsStr);
			parameters.Add("wareStatus", this.wareStatus);
			parameters.Add("categoryId", this.categoryId);
			parameters.Add("title", this.title);
			parameters.Add("itemNum", this.itemNum);
			parameters.Add("transportId", this.transportId);
			parameters.Add("startOnlineTime", this.startOnlineTime);
			parameters.Add("endOnlineTime", this.endOnlineTime);
			parameters.Add("minSupplyPrice", this.minSupplyPrice);
			parameters.Add("maxSupplyPrice", this.maxSupplyPrice);
			parameters.Add("recommendTpid", this.recommendTpid);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("currentPage", this.currentPage);
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








        
 

