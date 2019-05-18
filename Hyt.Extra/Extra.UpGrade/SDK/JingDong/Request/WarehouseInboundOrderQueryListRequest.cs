using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WarehouseInboundOrderQueryListRequest : IJdRequest<WarehouseInboundOrderQueryListResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   createTimeBegin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   createTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   unpackingTimeBegin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   unpackingTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		string
   remark1  { get; set; }

                  
                                                            
                                                          
public   		string
   remark2  { get; set; }

                  
                                                            
                                                          
public   		string
   remark3  { get; set; }

                  
                                                            
                                                          
public   		string
   remark4  { get; set; }

                  
                                                            
                                                          
public   		string
   remark5  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.warehouse.inbound.order.query.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("pageIndex", this.pageIndex);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("createTimeBegin", this.createTimeBegin);
			parameters.Add("createTimeEnd", this.createTimeEnd);
			parameters.Add("unpackingTimeBegin", this.unpackingTimeBegin);
			parameters.Add("unpackingTimeEnd", this.unpackingTimeEnd);
			parameters.Add("remark1", this.remark1);
			parameters.Add("remark2", this.remark2);
			parameters.Add("remark3", this.remark3);
			parameters.Add("remark4", this.remark4);
			parameters.Add("remark5", this.remark5);
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








        
 

