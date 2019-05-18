using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WaresVendersellskuUpdateRequest : IJdRequest<WaresVendersellskuUpdateResponse>
{
		                                                                                                                                  
public   		string
   valueId  { get; set; }

                  
                                                            
                                                          
public   		string
   categoryId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   indexId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   attributeId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   attributeValue  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   features  { get; set; }

                  
                                                            
                                                          
public   		string
   status  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.wares.vendersellsku.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("valueId", this.valueId);
			parameters.Add("category_id", this.categoryId);
			parameters.Add("index_id", this.indexId);
			parameters.Add("attribute_id", this.attributeId);
			parameters.Add("attribute_value", this.attributeValue);
			parameters.Add("features", this.features);
			parameters.Add("status", this.status);
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








        
 

