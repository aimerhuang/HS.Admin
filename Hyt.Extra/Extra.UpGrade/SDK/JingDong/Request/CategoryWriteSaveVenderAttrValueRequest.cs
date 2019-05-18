using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CategoryWriteSaveVenderAttrValueRequest : IJdRequest<CategoryWriteSaveVenderAttrValueResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                   
public   		Nullable<long>
   valueId  { get; set; }

                  
                                                            
                                                          
public   		string
   attValue  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   attributeId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   categoryId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   indexId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   key  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   value  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.category.write.saveVenderAttrValue";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("valueId", this.valueId);
			parameters.Add("attValue", this.attValue);
			parameters.Add("attributeId", this.attributeId);
			parameters.Add("categoryId", this.categoryId);
			parameters.Add("indexId", this.indexId);
			parameters.Add("key", this.key);
			parameters.Add("value", this.value);
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








        
 

