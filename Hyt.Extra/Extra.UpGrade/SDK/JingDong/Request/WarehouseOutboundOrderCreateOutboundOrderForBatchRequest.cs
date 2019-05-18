using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WarehouseOutboundOrderCreateOutboundOrderForBatchRequest : IJdRequest<WarehouseOutboundOrderCreateOutboundOrderForBatchResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   stockOutType  { get; set; }

                  
                                                            
                                                          
public   		string
   remarkForOutBound  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   snNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   spareCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   vendorCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   remark  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.warehouse.outbound.order.createOutboundOrderForBatch";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("stockOutType", this.stockOutType);
			parameters.Add("remarkForOutBound", this.remarkForOutBound);
			parameters.Add("snNo", this.snNo);
			parameters.Add("spareCode", this.spareCode);
			parameters.Add("vendorCode", this.vendorCode);
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








        
 

