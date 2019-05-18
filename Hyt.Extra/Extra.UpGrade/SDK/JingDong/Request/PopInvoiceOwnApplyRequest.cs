using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopInvoiceOwnApplyRequest : IJdRequest<PopInvoiceOwnApplyResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                                                                                                                		public  		string
   productId  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   productName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   num  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   price  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   spec  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   unit  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   taxRate  { get; set; }
                                                                                                                                                                                                                                                            
public   		string
   orderId  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		string
   receiverTaxNo  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   receiverName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   invoiceCode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   invoiceNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   ivcTitle  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   totalPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   invoiceTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pdfInfo  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.invoice.own.apply";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("product_id", this.productId);
			parameters.Add("product_name", this.productName);
			parameters.Add("num", this.num);
			parameters.Add("price", this.price);
			parameters.Add("spec", this.spec);
			parameters.Add("unit", this.unit);
			parameters.Add("tax_rate", this.taxRate);
			parameters.Add("order_id", this.orderId);
			parameters.Add("receiver_tax_no", this.receiverTaxNo);
			parameters.Add("receiver_name", this.receiverName);
			parameters.Add("invoice_code", this.invoiceCode);
			parameters.Add("invoice_no", this.invoiceNo);
			parameters.Add("ivc_title", this.ivcTitle);
			parameters.Add("total_price", this.totalPrice);
			parameters.Add("invoice_time", this.invoiceTime);
			parameters.Add("pdf_info", this.pdfInfo);
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








        
 

