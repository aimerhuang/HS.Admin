using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpRtwTransportRtwRequest : IJdRequest<EclpRtwTransportRtwResponse>
{
		                                                                                                                                  
public   		string
   eclpSoNo  { get; set; }

                  
                                                            
                                                          
public   		string
   eclpRtwNo  { get; set; }

                  
                                                            
                                                          
public   		string
   isvRtwNum  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		string
   reson  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.rtw.transportRtw";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("eclpSoNo", this.eclpSoNo);
			parameters.Add("eclpRtwNo", this.eclpRtwNo);
			parameters.Add("isvRtwNum", this.isvRtwNum);
			parameters.Add("warehouseNo", this.warehouseNo);
			parameters.Add("reson", this.reson);
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








        
 

