using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopAfsRefundapplyQuerylistRequest : IJdRequest<PopAfsRefundapplyQuerylistResponse>
{
		                                                                                                                                                                   
public   		string
   status  { get; set; }

                  
                                                            
                                                          
public   		string
   id  { get; set; }

                  
                                                            
                                                          
public   		string
   orderId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   buyerId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   buyerName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   applyTimeStart  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   applyTimeEnd  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   checkTimeStart  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   checkTimeEnd  { get; set; }

                  
                                                                                                                                                            
                                                                                           
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.afs.refundapply.querylist";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("status", this.status);
			parameters.Add("id", this.id);
			parameters.Add("order_id", this.orderId);
			parameters.Add("buyer_id", this.buyerId);
			parameters.Add("buyer_name", this.buyerName);
			parameters.Add("apply_time_start", this.applyTimeStart);
			parameters.Add("apply_time_end", this.applyTimeEnd);
			parameters.Add("check_time_start", this.checkTimeStart);
			parameters.Add("check_time_end", this.checkTimeEnd);
			parameters.Add("page_index", this.pageIndex);
			parameters.Add("page_size", this.pageSize);
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








        
 

