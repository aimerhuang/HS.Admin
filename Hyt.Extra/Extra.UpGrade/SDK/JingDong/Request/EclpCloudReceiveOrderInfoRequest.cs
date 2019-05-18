using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpCloudReceiveOrderInfoRequest : IJdRequest<EclpCloudReceiveOrderInfoResponse>
{
		                                                                                                                                  
public   		string
   machiningNo  { get; set; }

                  
                                                            
                                                          
public   		string
   machiningType  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   ownerNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   skuNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   productLevel  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   qty  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  		public  		string
   destOwnerNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   destSkuNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   destQty  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   destProductLevel  { get; set; }
                                                                                                                                                                                                
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.cloud.receiveOrderInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("machiningNo", this.machiningNo);
			parameters.Add("machiningType", this.machiningType);
			parameters.Add("ownerNo", this.ownerNo);
			parameters.Add("skuNo", this.skuNo);
			parameters.Add("productLevel", this.productLevel);
			parameters.Add("qty", this.qty);
			parameters.Add("destOwnerNo", this.destOwnerNo);
			parameters.Add("destSkuNo", this.destSkuNo);
			parameters.Add("destQty", this.destQty);
			parameters.Add("destProductLevel", this.destProductLevel);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("tenantId", this.tenantId);
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








        
 

