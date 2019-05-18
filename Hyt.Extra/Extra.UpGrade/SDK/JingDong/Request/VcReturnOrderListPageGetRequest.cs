using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcReturnOrderListPageGetRequest : IJdRequest<VcReturnOrderListPageGetResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   returnId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   fromDeliverCenterId  { get; set; }

                  
                                                            
                                                          
public   		string
   returnStates  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   createDateBegin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   createDateEnd  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.return.order.list.page.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("returnId", this.returnId);
			parameters.Add("fromDeliverCenterId", this.fromDeliverCenterId);
			parameters.Add("returnStates", this.returnStates);
			parameters.Add("createDateBegin", this.createDateBegin);
			parameters.Add("createDateEnd", this.createDateEnd);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("pageIndex", this.pageIndex);
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








        
 

