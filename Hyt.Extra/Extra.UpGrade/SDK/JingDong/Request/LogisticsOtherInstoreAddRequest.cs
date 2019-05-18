using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsOtherInstoreAddRequest : IJdRequest<LogisticsOtherInstoreAddResponse>
{
		                                                                                                                                  
public   		string
   instoreType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   poNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   expectedDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   approver  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     		public  		string
   goodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         		public  		string
   isvGoodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   expectedQty  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   goodsStatus  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.otherInstore.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("instore_type", this.instoreType);
			parameters.Add("po_no", this.poNo);
			parameters.Add("expected_date", this.expectedDate);
			parameters.Add("approver", this.approver);
			parameters.Add("warehouse_no", this.warehouseNo);
			parameters.Add("goods_no", this.goodsNo);
			parameters.Add("isv_goods_no", this.isvGoodsNo);
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








        
 

