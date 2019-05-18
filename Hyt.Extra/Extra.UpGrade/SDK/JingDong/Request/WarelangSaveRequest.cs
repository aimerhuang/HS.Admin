using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WarelangSaveRequest : IJdRequest<WarelangSaveResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   langId  { get; set; }

                  
                                                            
                                                          
public   		string
   title  { get; set; }

                  
                                                            
                                                          
public   		string
   seoKeywords  { get; set; }

                  
                                                            
                                                          
public   		string
   extPackInfo  { get; set; }

                  
                                                            
                                                          
public   		string
   description  { get; set; }

                  
                                                            
                                                          
public   		string
   appDescription  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.warelang.save";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wareId", this.wareId);
			parameters.Add("langId", this.langId);
			parameters.Add("title", this.title);
			parameters.Add("seoKeywords", this.seoKeywords);
			parameters.Add("extPackInfo", this.extPackInfo);
			parameters.Add("description", this.description);
			parameters.Add("appDescription", this.appDescription);
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








        
 

