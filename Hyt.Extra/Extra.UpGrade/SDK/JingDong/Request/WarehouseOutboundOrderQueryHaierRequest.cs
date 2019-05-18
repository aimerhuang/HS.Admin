using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WarehouseOutboundOrderQueryHaierRequest : IJdRequest<WarehouseOutboundOrderQueryHaierResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   stockOutNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   createTimeBegin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   createTimeEnd  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.warehouse.outbound.order.query.haier";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("pageIndex", this.pageIndex);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("stockOutNo", this.stockOutNo);
			parameters.Add("createTimeBegin", this.createTimeBegin);
			parameters.Add("createTimeEnd", this.createTimeEnd);
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








        
 

