using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsO2oSkuStockOwnerUpdateRequest : IJdRequest<LogisticsO2oSkuStockOwnerUpdateResponse>
{
		                                                                                                       
public   		Nullable<long>
   spuId  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                                                                                                                                                                                                                                                    		public  		string
   skuId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   name  { get; set; }
                                                                                                                                                                                                
public   		string
   stockOwner  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.o2o.sku.stockOwner.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("spu_id", this.spuId);
			parameters.Add("sku_id", this.skuId);
			parameters.Add("name", this.name);
			parameters.Add("stock_owner", this.stockOwner);
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








        
 

