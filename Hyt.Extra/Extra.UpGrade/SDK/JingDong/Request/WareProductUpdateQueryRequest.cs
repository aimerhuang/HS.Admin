using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareProductUpdateQueryRequest : IJdRequest<WareProductUpdateQueryResponse>
{
		                                                                      
public   		string
   skuStatus  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   startSaleDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endSaleDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   thirdCid  { get; set; }

                  
                                                            
                                                          
public   		string
   scrollId  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ware.product.update.query";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("sku_status", this.skuStatus);
			parameters.Add("start_SaleDate", this.startSaleDate);
			parameters.Add("end_SaleDate", this.endSaleDate);
			parameters.Add("thirdCid", this.thirdCid);
			parameters.Add("scrollId", this.scrollId);
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








        
 

