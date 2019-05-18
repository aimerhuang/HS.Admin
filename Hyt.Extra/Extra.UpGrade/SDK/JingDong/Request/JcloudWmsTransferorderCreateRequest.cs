using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsTransferorderCreateRequest : IJdRequest<JcloudWmsTransferorderCreateResponse>
{
		                                                                                                                                                                                                    
public   		string
   receiptNo  { get; set; }

                  
                                                            
                                                          
public   		string
   ownerNo  { get; set; }

                  
                                                            
                                                          
public   		string
   billType  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                          
public   		string
   sourceWarehouseNo  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   skuNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   skuName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   expectedQty  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.transferorder.create";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("receiptNo", this.receiptNo);
			parameters.Add("ownerNo", this.ownerNo);
			parameters.Add("billType", this.billType);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("sourceWarehouseNo", this.sourceWarehouseNo);
			parameters.Add("skuNo", this.skuNo);
			parameters.Add("skuName", this.skuName);
			parameters.Add("expectedQty", this.expectedQty);
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








        
 

