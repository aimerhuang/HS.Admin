using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class StockstateAreaStockStateRequest : IJdRequest<StockstateAreaStockStateResponse>
{
		                                                                                                       
public   		string
   ch  { get; set; }

                  
                                                            
                                                                                           
public   		string
   skuNum  { get; set; }

                  
                                                            
                                                          
public   		string
   area  { get; set; }

                  
                                                            
                                                          
public   		string
   coord  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.stockstate.areaStockState";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ch", this.ch);
			parameters.Add("skuNum", this.skuNum);
			parameters.Add("area", this.area);
			parameters.Add("coord", this.coord);
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








        
 

