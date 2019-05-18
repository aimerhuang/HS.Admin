using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpSerialQueryInStockSIDBySkuRequest : IJdRequest<EclpSerialQueryInStockSIDBySkuResponse>
{
		                                                                                                                                  
public   		string
   goodsNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                                           
public   		string
   pin  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.serial.queryInStockSIDBySku";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("goodsNo", this.goodsNo);
			parameters.Add("pageNo", this.pageNo);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("pin", this.pin);
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








        
 

