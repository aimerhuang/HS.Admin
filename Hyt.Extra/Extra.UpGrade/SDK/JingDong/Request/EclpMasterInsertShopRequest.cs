using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpMasterInsertShopRequest : IJdRequest<EclpMasterInsertShopResponse>
{
		                                                                                                                                  
public   		string
   isvShopNo  { get; set; }

                  
                                                            
                                                          
public   		string
   spSourceNo  { get; set; }

                  
                                                            
                                                          
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                          
public   		string
   spShopNo  { get; set; }

                  
                                                            
                                                          
public   		string
   shopName  { get; set; }

                  
                                                            
                                                          
public   		string
   contacts  { get; set; }

                  
                                                            
                                                          
public   		string
   phone  { get; set; }

                  
                                                            
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		string
   email  { get; set; }

                  
                                                            
                                                          
public   		string
   fax  { get; set; }

                  
                                                            
                                                          
public   		string
   afterSaleContacts  { get; set; }

                  
                                                            
                                                          
public   		string
   afterSaleAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   afterSalePhone  { get; set; }

                  
                                                            
                                                          
public   		string
   bdOwnerNo  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve1  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve2  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve3  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve4  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve5  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve6  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve7  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve8  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve9  { get; set; }

                  
                                                            
                                                          
public   		string
   reserve10  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.master.insertShop";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("isvShopNo", this.isvShopNo);
			parameters.Add("spSourceNo", this.spSourceNo);
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("spShopNo", this.spShopNo);
			parameters.Add("shopName", this.shopName);
			parameters.Add("contacts", this.contacts);
			parameters.Add("phone", this.phone);
			parameters.Add("address", this.address);
			parameters.Add("email", this.email);
			parameters.Add("fax", this.fax);
			parameters.Add("afterSaleContacts", this.afterSaleContacts);
			parameters.Add("afterSaleAddress", this.afterSaleAddress);
			parameters.Add("afterSalePhone", this.afterSalePhone);
			parameters.Add("bdOwnerNo", this.bdOwnerNo);
			parameters.Add("reserve1", this.reserve1);
			parameters.Add("reserve2", this.reserve2);
			parameters.Add("reserve3", this.reserve3);
			parameters.Add("reserve4", this.reserve4);
			parameters.Add("reserve5", this.reserve5);
			parameters.Add("reserve6", this.reserve6);
			parameters.Add("reserve7", this.reserve7);
			parameters.Add("reserve8", this.reserve8);
			parameters.Add("reserve9", this.reserve9);
			parameters.Add("reserve10", this.reserve10);
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








        
 

