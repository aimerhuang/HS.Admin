using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TemplateReadFindTemplateByIdRequest : IJdRequest<TemplateReadFindTemplateByIdResponse>
{
		                                                                                                                                                                                                                                                                                                       
public   		Nullable<long>
   templateId  { get; set; }

                  
                                                            
                                                                                      
public   		string
   field  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.template.read.findTemplateById";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("templateId", this.templateId);
			parameters.Add("field", this.field);
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








        
 

