using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ProcurementOrderCreateRequest : IJdRequest<ProcurementOrderCreateResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   deliverCenterId  { get; set; }

                  
                                                            
                                                          
public   		string
   purchaserErpCode  { get; set; }

                  
                                                            
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   wareId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   allocationDeliverCenterId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   originalNum  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.procurement.order.create";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deliverCenterId", this.deliverCenterId);
			parameters.Add("purchaserErpCode", this.purchaserErpCode);
			parameters.Add("remark", this.remark);
			parameters.Add("wareId", this.wareId);
			parameters.Add("allocationDeliverCenterId", this.allocationDeliverCenterId);
			parameters.Add("originalNum", this.originalNum);
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








        
 

