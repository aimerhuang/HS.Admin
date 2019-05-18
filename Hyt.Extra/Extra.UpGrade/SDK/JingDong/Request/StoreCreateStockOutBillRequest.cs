using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class StoreCreateStockOutBillRequest : IJdRequest<StoreCreateStockOutBillResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   comId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   orgId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   whId  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                                                                                                                                                                                                                                                    		public  		string
   skuCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   num  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   remark  { get; set; }
                                                                                                                                                                                                        
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.store.createStockOutBill";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("com_id", this.comId);
			parameters.Add("org_id", this.orgId);
			parameters.Add("wh_id", this.whId);
			parameters.Add("sku_code", this.skuCode);
			parameters.Add("num", this.num);
			parameters.Add("remark", this.remark);
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








        
 

