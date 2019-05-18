using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PoListPageGetRequest : IJdRequest<PoListPageGetResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   deliverCenterId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   createdDateStart  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   createdDateEnd  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<bool>
   isEptCustomized  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                                      
public   		string
   orderIds  { get; set; }

                  
                                                            
                                                                                      
public   		string
   wareIds  { get; set; }

                  
                                                            
                                                                                      
public   		string
   states  { get; set; }

                  
                                                            
                                                                                      
public   		string
   confirmStates  { get; set; }

                  
                                                            
                                                                                      
public   		string
   orderAttributes  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.po.list.page.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deliverCenterId", this.deliverCenterId);
			parameters.Add("status", this.status);
			parameters.Add("createdDateStart", this.createdDateStart);
			parameters.Add("createdDateEnd", this.createdDateEnd);
			parameters.Add("isEptCustomized", this.isEptCustomized);
			parameters.Add("pageIndex", this.pageIndex);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("orderIds", this.orderIds);
			parameters.Add("wareIds", this.wareIds);
			parameters.Add("states", this.states);
			parameters.Add("confirmStates", this.confirmStates);
			parameters.Add("orderAttributes", this.orderAttributes);
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








        
 

