using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CategoryReadFindAttrByIdRequest : IJdRequest<CategoryReadFindAttrByIdResponse>
{
		                                                                                                                                                                                                                                                                                                       
public   		Nullable<long>
   attrId  { get; set; }

                  
                                                            
                                                                                      
public   		string
   field  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.category.read.findAttrById";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("attrId", this.attrId);
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








        
 

