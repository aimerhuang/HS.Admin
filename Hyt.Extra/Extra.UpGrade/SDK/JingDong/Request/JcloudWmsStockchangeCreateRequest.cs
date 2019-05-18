using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsStockchangeCreateRequest : IJdRequest<JcloudWmsStockchangeCreateResponse>
{
		                                                                                                                                                                                                    
public   		string
   changeNo  { get; set; }

                  
                                                            
                                                          
public   		string
   changeType  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   ownerNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   skuNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   productLevel  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   changeQty  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   toOwnerNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   toSkuNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   toProductLevel  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.stockchange.create";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("changeNo", this.changeNo);
			parameters.Add("changeType", this.changeType);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("ownerNo", this.ownerNo);
			parameters.Add("skuNo", this.skuNo);
			parameters.Add("productLevel", this.productLevel);
			parameters.Add("changeQty", this.changeQty);
			parameters.Add("toOwnerNo", this.toOwnerNo);
			parameters.Add("toSkuNo", this.toSkuNo);
			parameters.Add("toProductLevel", this.toProductLevel);
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








        
 

