using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class AfsserviceCompensateorderAddRequest : IJdRequest<AfsserviceCompensateorderAddResponse>
{
		                                                                                                                                                                   
public   		string
   customerPin  { get; set; }

                  
                                                            
                                                          
public   		string
   customerName  { get; set; }

                  
                                                            
                                                          
public   		string
   receiptName  { get; set; }

                  
                                                            
                                                          
public   		string
   receiptAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   zipcode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   province  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   city  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   county  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   village  { get; set; }

                  
                                                            
                                                          
public   		string
   tel  { get; set; }

                  
                                                            
                                                          
public   		string
   phone  { get; set; }

                  
                                                            
                                                          
public   		string
   applyDescription  { get; set; }

                  
                                                            
                                                          
public   		string
   orderRemark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   relationOrderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   afsServiceId  { get; set; }

                  
                                                            
                                                                                                                                                       
public   		Nullable<int>
   wareId  { get; set; }

                  
                                                            
                                                          
public   		string
   wareName  { get; set; }

                  
                                                            
                                                          
public   		string
   warePrice  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.afsservice.compensateorder.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("customerPin", this.customerPin);
			parameters.Add("customerName", this.customerName);
			parameters.Add("receiptName", this.receiptName);
			parameters.Add("receiptAddress", this.receiptAddress);
			parameters.Add("zipcode", this.zipcode);
			parameters.Add("province", this.province);
			parameters.Add("city", this.city);
			parameters.Add("county", this.county);
			parameters.Add("village", this.village);
			parameters.Add("tel", this.tel);
			parameters.Add("phone", this.phone);
			parameters.Add("applyDescription", this.applyDescription);
			parameters.Add("orderRemark", this.orderRemark);
			parameters.Add("relationOrderId", this.relationOrderId);
			parameters.Add("afsServiceId", this.afsServiceId);
			parameters.Add("wareId", this.wareId);
			parameters.Add("wareName", this.wareName);
			parameters.Add("warePrice", this.warePrice);
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








        
 

