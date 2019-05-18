using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsSkuAcceptServiceHandlerDeleteRequest : IJdRequest<JcloudWmsSkuAcceptServiceHandlerDeleteResponse>
{
		                                                                                                                                                                                                    
public   		string
   code  { get; set; }

                  
                                                            
                                                          
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                          
public   		string
   ownerNo  { get; set; }

                  
                                                            
                                                          
public   		string
   operateUser  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operateTime  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.SkuAcceptServiceHandler.delete";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("code", this.code);
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("ownerNo", this.ownerNo);
			parameters.Add("operateUser", this.operateUser);
			parameters.Add("operateTime", this.operateTime);
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








        
 

