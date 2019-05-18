using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class StoreCreateStockInBillForSamRequest : IJdRequest<StoreCreateStockInBillForSamResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   samBillId  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<int>
   arrivalDay  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   clubId  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                                                                                                                                                                                                                                                    		public  		string
   itemId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   num  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   remark  { get; set; }
                                                                                                                                                                                                
public   		Nullable<int>
   samStoreType  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.store.createStockInBillForSam";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("sam_bill_id", this.samBillId);
			parameters.Add("arrivalDay", this.arrivalDay);
			parameters.Add("club_id", this.clubId);
			parameters.Add("item_id", this.itemId);
			parameters.Add("num", this.num);
			parameters.Add("remark", this.remark);
			parameters.Add("samStoreType", this.samStoreType);
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








        
 

