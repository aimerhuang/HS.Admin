using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class AuditCommonProviderUpdateAfsServiceQuestionTypeRequest : IJdRequest<AuditCommonProviderUpdateAfsServiceQuestionTypeResponse>
{
		                                                                      
public   		string
   afsServiceIds  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   questionTypeCid1  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   questionTypeCid2  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   customerExpect  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   operatorPin  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorNick  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorRemark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operatorDate  { get; set; }

                  
                                                            
                                                          
public   		string
   platformSrc  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.AuditCommonProvider.updateAfsServiceQuestionType";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("afsServiceIds", this.afsServiceIds);
			parameters.Add("questionTypeCid1", this.questionTypeCid1);
			parameters.Add("questionTypeCid2", this.questionTypeCid2);
			parameters.Add("customerExpect", this.customerExpect);
			parameters.Add("operatorPin", this.operatorPin);
			parameters.Add("operatorNick", this.operatorNick);
			parameters.Add("operatorRemark", this.operatorRemark);
			parameters.Add("operatorDate", this.operatorDate);
			parameters.Add("platformSrc", this.platformSrc);
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








        
 

