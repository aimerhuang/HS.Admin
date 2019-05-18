using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopAfterSaleGetWorkOrderByWorkOrderIdRequest : IJdRequest<PopAfterSaleGetWorkOrderByWorkOrderIdResponse>
{
		                                                                      
public   		Nullable<long>
   workorderId  { get; set; }

                  
                                                                                                                                    
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.afterSale.GetWorkOrderByWorkOrderId";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("workorder_id", this.workorderId);
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








        
 

