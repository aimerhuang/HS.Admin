using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareSkuAddRequest : IJdRequest<WareSkuAddResponse>
{
		                                                                                                                                  
public   		string
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   attributes  { get; set; }

                  
                                                            
                                                          
public   		string
   jdPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   stockNum  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   tradeNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   outerId  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.ware.sku.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ware_id", this.wareId);
			parameters.Add("attributes", this.attributes);
			parameters.Add("jd_price", this.jdPrice);
			parameters.Add("stock_num", this.stockNum);
			parameters.Add("trade_no", this.tradeNo);
			parameters.Add("outer_id", this.outerId);
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








        
 

