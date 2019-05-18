using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WarehouseOutboundOrderCreateRequest : IJdRequest<WarehouseOutboundOrderCreateResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   stockOutType  { get; set; }

                  
                                                            
                                                          
public   		string
   snNo  { get; set; }

                  
                                                            
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.warehouse.outbound.order.create";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("stockOutType", this.stockOutType);
			parameters.Add("snNo", this.snNo);
			parameters.Add("remark", this.remark);
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








        
 

