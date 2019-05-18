using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class MiniwmsOcOrderApplyOrderTaskRequest : IJdRequest<MiniwmsOcOrderApplyOrderTaskResponse>
{
		                                                                                                                                  
public   		string
   stationId  { get; set; }

                  
                                                            
                                                          
public   		string
   uuid  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   goodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   num  { get; set; }
                                                                                                                                                                                                        
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.miniwms.oc.order.applyOrderTask";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("stationId", this.stationId);
			parameters.Add("uuid", this.uuid);
			parameters.Add("goodsNo", this.goodsNo);
			parameters.Add("num", this.num);
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








        
 

