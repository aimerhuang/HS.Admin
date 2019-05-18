using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsPoAddRequest : IJdRequest<LogisticsPoAddResponse>
{
		                                                                                                                                  
public   		string
   channelsSellerNo  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   poNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   expectDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   supplierName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   supplierNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   approver  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     		public  		string
   goodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   expectedQty  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   goodsStatus  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.po.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("channels_seller_no", this.channelsSellerNo);
			parameters.Add("po_no", this.poNo);
			parameters.Add("warehouse_no", this.warehouseNo);
			parameters.Add("expect_date", this.expectDate);
			parameters.Add("supplier_name", this.supplierName);
			parameters.Add("supplier_no", this.supplierNo);
			parameters.Add("approver", this.approver);
			parameters.Add("goods_no", this.goodsNo);
			parameters.Add("expected_qty", this.expectedQty);
			parameters.Add("goods_status", this.goodsStatus);
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








        
 

