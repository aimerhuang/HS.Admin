using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptStockWareAddRequest : IJdRequest<EptStockWareAddResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   spuId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                            
                                                          
public   		string
   rfId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   amountCount  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.stock.ware.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("spuId", this.spuId);
			parameters.Add("skuId", this.skuId);
			parameters.Add("rfId", this.rfId);
			parameters.Add("amountCount", this.amountCount);
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








        
 

