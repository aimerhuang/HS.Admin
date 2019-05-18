using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopAfsSoaRefundapplyQueryPageListRequest : IJdRequest<PopAfsSoaRefundapplyQueryPageListResponse>
{
		                                                                                                                                                                   
public   		string
   ids  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   status  { get; set; }

                  
                                                            
                                                          
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
			return "jingdong.pop.afs.soa.refundapply.queryPageList";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ids", this.ids);
			parameters.Add("status", this.status);
			parameters.Add("orderId", this.orderId);
			parameters.Add("buyerId", this.buyerId);
			parameters.Add("buyerName", this.buyerName);
			parameters.Add("applyTimeStart", this.applyTimeStart);
			parameters.Add("applyTimeEnd", this.applyTimeEnd);
			parameters.Add("checkTimeStart", this.checkTimeStart);
			parameters.Add("checkTimeEnd", this.checkTimeEnd);
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








        
 

