using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpPoAddPoSidDetailRequest : IJdRequest<EclpPoAddPoSidDetailResponse>
{
		                                                                                                                                  
public   		string
   poNo  { get; set; }

                  
                                                            
                                                          
public   		string
   version  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                             		public  		string
   deptGoodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   serialNo  { get; set; }
                                                                                                                                                                                                
public   		string
   pin  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.po.addPoSidDetail";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("poNo", this.poNo);
			parameters.Add("version", this.version);
			parameters.Add("deptGoodsNo", this.deptGoodsNo);
			parameters.Add("serialNo", this.serialNo);
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








        
 

