using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsReturnorderAddRequest : IJdRequest<LogisticsReturnorderAddResponse>
{
		                                                                                                                                  
public   		string
   sellerNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   inboundNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   joslOutboundNo  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   expectedDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   isvSource  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   approver  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     		public  		string
   goodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   expectedQty  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   stockMark  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.returnorder.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("seller_no", this.sellerNo);
			parameters.Add("warehouse_no", this.warehouseNo);
			parameters.Add("inbound_no", this.inboundNo);
			parameters.Add("josl_outbound_no", this.joslOutboundNo);
			parameters.Add("expected_date", this.expectedDate);
			parameters.Add("isv_source", this.isvSource);
			parameters.Add("approver", this.approver);
			parameters.Add("goods_no", this.goodsNo);
			parameters.Add("expected_qty", this.expectedQty);
			parameters.Add("stock_mark", this.stockMark);
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








        
 

