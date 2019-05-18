using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class StoreDeleteStockInBill4SamRequest : IJdRequest<StoreDeleteStockInBill4SamResponse>
{
		                                                                                                       
public   		Nullable<long>
   samBillId  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<long>
   stockInBillId  { get; set; }

                  
                                                                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.store.deleteStockInBill4Sam";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("sam_bill_Id", this.samBillId);
			parameters.Add("stock_in_bill_id", this.stockInBillId);
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








        
 

