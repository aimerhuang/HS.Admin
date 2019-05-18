using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiPrepoGetRequest : IJdRequest<EdiPrepoGetResponse>
{
		                                                                                                                                  
public   		string
   prePurchaseOrderCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   receiveTimeStart  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   receiveTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.prepo.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("prePurchaseOrderCode", this.prePurchaseOrderCode);
			parameters.Add("status", this.status);
			parameters.Add("receiveTimeStart", this.receiveTimeStart);
			parameters.Add("receiveTimeEnd", this.receiveTimeEnd);
			parameters.Add("pageNum", this.pageNum);
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








        
 

