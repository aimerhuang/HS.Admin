using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JwPurchaseOrderOrderHistoryRequest : IJdRequest<JwPurchaseOrderOrderHistoryResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   pageNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   clientId  { get; set; }

                  
                                                            
                                                          
public   		string
   clientBusinessNo  { get; set; }

                  
                                                            
                                                                                                   
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jw.purchase.order.orderHistory";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("pageNo", this.pageNo);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("clientId", this.clientId);
			parameters.Add("clientBusinessNo", this.clientBusinessNo);
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








        
 

