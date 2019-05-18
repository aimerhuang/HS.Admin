using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellercatAddRequest : IJdRequest<SellercatAddResponse>
{
		                                                                                                                                  
public   		string
   parentId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isOpen  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<bool>
   isHomeShow  { get; set; }

                  
                                                                                                                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.sellercat.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("parent_id", this.parentId);
			parameters.Add("name", this.name);
			parameters.Add("is_open", this.isOpen);
			parameters.Add("is_home_show", this.isHomeShow);
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








        
 

