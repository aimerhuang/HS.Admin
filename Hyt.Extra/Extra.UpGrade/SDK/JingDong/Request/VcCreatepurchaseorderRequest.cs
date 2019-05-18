using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcCreatepurchaseorderRequest : IJdRequest<VcCreatepurchaseorderResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   orderDeliverCenterId  { get; set; }

                  
                                                                                                                                                                                    
                                                          
public   		string
   purchaserErpCode  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   orderRemark  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      		public  		string
   wareId  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   wareDeliverCenterId  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   originalNum  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   wareRemark  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.createpurchaseorder";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("order_deliver_center_id", this.orderDeliverCenterId);
			parameters.Add("purchaser_erp_code", this.purchaserErpCode);
			parameters.Add("order_remark", this.orderRemark);
			parameters.Add("ware_id", this.wareId);
			parameters.Add("ware_deliver_center_id", this.wareDeliverCenterId);
			parameters.Add("original_num", this.originalNum);
			parameters.Add("ware_remark", this.wareRemark);
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








        
 

