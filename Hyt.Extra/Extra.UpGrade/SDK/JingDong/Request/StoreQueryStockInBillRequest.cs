using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class StoreQueryStockInBillRequest : IJdRequest<StoreQueryStockInBillResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   stockInStatus  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<long>
   stockInBillId  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		Nullable<long>
   comId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   orgId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   whId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   beginTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   page  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.store.queryStockInBill";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("stock_in_status", this.stockInStatus);
			parameters.Add("stock_in_bill_id", this.stockInBillId);
			parameters.Add("com_id", this.comId);
			parameters.Add("org_id", this.orgId);
			parameters.Add("wh_id", this.whId);
			parameters.Add("sku_id", this.skuId);
			parameters.Add("begin_time", this.beginTime);
			parameters.Add("end_time", this.endTime);
			parameters.Add("page", this.page);
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








        
 

