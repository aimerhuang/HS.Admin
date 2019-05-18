using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpPoExtQueryPoOrderRequest : IJdRequest<EclpPoExtQueryPoOrderResponse>
{
		                                                                                                                                  
public   		string
   poOrderNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   queryItemFlag  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   queryBoxFlag  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   queryQcFlag  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.po.ext.queryPoOrder";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("poOrderNo", this.poOrderNo);
			parameters.Add("queryItemFlag", this.queryItemFlag);
			parameters.Add("queryBoxFlag", this.queryBoxFlag);
			parameters.Add("queryQcFlag", this.queryQcFlag);
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








        
 

