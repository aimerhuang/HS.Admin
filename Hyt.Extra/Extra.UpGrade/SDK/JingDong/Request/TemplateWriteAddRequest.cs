using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TemplateWriteAddRequest : IJdRequest<TemplateWriteAddResponse>
{
		                                                                                                                                                                                                                                                                                                                                  
public   		string
   bottomContent  { get; set; }

                  
                                                            
                                                          
public   		string
   headContent  { get; set; }

                  
                                                            
                                                          
public   		string
   name  { get; set; }

                  
                                                            
                                                                                           
public   		string
   mobileBottomContent  { get; set; }

                  
                                                            
                                                          
public   		string
   mobileHeadContent  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.template.write.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("bottomContent", this.bottomContent);
			parameters.Add("headContent", this.headContent);
			parameters.Add("name", this.name);
			parameters.Add("mobileBottomContent", this.mobileBottomContent);
			parameters.Add("mobileHeadContent", this.mobileHeadContent);
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








        
 

