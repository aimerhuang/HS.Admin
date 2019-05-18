using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsMasterCategoryInfoUpdateRequest : IJdRequest<JcloudWmsMasterCategoryInfoUpdateResponse>
{
		                                                                                                                                                                                                    
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                          
public   		string
   categoryNo  { get; set; }

                  
                                                            
                                                          
public   		string
   categoryName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   sortNo  { get; set; }

                  
                                                            
                                                          
public   		string
   operateUser  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operateTime  { get; set; }

                  
                                                            
                                                          
public   		string
   memo  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.master.categoryInfo.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("categoryNo", this.categoryNo);
			parameters.Add("categoryName", this.categoryName);
			parameters.Add("sortNo", this.sortNo);
			parameters.Add("operateUser", this.operateUser);
			parameters.Add("operateTime", this.operateTime);
			parameters.Add("memo", this.memo);
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








        
 

