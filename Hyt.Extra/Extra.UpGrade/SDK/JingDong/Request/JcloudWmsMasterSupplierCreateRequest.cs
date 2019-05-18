using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsMasterSupplierCreateRequest : IJdRequest<JcloudWmsMasterSupplierCreateResponse>
{
		                                                                                                                                                                                                    
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                          
public   		string
   supplierNo  { get; set; }

                  
                                                            
                                                          
public   		string
   supplierName  { get; set; }

                  
                                                            
                                                          
public   		string
   contact  { get; set; }

                  
                                                            
                                                          
public   		string
   contactMobile  { get; set; }

                  
                                                            
                                                          
public   		string
   contactPhone  { get; set; }

                  
                                                            
                                                          
public   		string
   province  { get; set; }

                  
                                                            
                                                          
public   		string
   city  { get; set; }

                  
                                                            
                                                          
public   		string
   district  { get; set; }

                  
                                                            
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		string
   contactEmail  { get; set; }

                  
                                                            
                                                          
public   		string
   returnAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   memo  { get; set; }

                  
                                                            
                                                          
public   		string
   operateUser  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operateTime  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.master.supplier.create";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("supplierNo", this.supplierNo);
			parameters.Add("supplierName", this.supplierName);
			parameters.Add("contact", this.contact);
			parameters.Add("contactMobile", this.contactMobile);
			parameters.Add("contactPhone", this.contactPhone);
			parameters.Add("province", this.province);
			parameters.Add("city", this.city);
			parameters.Add("district", this.district);
			parameters.Add("address", this.address);
			parameters.Add("contactEmail", this.contactEmail);
			parameters.Add("returnAddress", this.returnAddress);
			parameters.Add("memo", this.memo);
			parameters.Add("operateUser", this.operateUser);
			parameters.Add("operateTime", this.operateTime);
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








        
 

