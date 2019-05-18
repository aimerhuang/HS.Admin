using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WarePromoinfoGetRequest : IJdRequest<WarePromoinfoGetResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                       		public  		string
   skuIds  { get; set; }
                                                                                                                                                                                                
public   		string
   webSite  { get; set; }

                  
                                                            
                                                          
public   		string
   origin  { get; set; }

                  
                                                            
                                                          
public   		string
   areaId  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ware.promoinfo.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("skuIds", this.skuIds);
			parameters.Add("webSite", this.webSite);
			parameters.Add("origin", this.origin);
			parameters.Add("areaId", this.areaId);
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








        
 

