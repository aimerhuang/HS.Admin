using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsPdjPriceUpdateRequest : IJdRequest<LogisticsPdjPriceUpdateResponse>
{
		                                                                                                       
public   		string
   outerSkuId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                            
                                                          
public   		string
   stationNo  { get; set; }

                  
                                                            
                                                          
public   		string
   outerStationNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   salePrice  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   marketPrice  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.pdj.price.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("outerSkuId", this.outerSkuId);
			parameters.Add("skuId", this.skuId);
			parameters.Add("stationNo", this.stationNo);
			parameters.Add("outerStationNo", this.outerStationNo);
			parameters.Add("salePrice", this.salePrice);
			parameters.Add("marketPrice", this.marketPrice);
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








        
 

