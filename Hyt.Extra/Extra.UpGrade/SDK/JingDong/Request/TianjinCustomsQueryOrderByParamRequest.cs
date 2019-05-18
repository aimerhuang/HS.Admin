using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class TianjinCustomsQueryOrderByParamRequest : IJdRequest<TianjinCustomsQueryOrderByParamResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   beginDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   endDate  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   page  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   type  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.tianjin.customs.queryOrderByParam";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("beginDate", this.beginDate);
			parameters.Add("endDate", this.endDate);
			parameters.Add("page", this.page);
			parameters.Add("type", this.type);
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








        
 

