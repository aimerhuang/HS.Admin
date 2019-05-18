using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsSerialtracDetailQueryRequest : IJdRequest<JcloudWmsSerialtracDetailQueryResponse>
{
		                                                                                                                                                                                                    
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   billType  { get; set; }

                  
                                                            
                                                          
public   		string
   orderNo  { get; set; }

                  
                                                            
                                                          
public   		string
   skuNo  { get; set; }

                  
                                                            
                                                          
public   		string
   serialNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   id  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.serialtrac.detail.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("billType", this.billType);
			parameters.Add("orderNo", this.orderNo);
			parameters.Add("skuNo", this.skuNo);
			parameters.Add("serialNo", this.serialNo);
			parameters.Add("id", this.id);
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








        
 

