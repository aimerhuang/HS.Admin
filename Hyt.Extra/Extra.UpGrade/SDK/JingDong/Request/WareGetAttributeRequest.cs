using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareGetAttributeRequest : IJdRequest<WareGetAttributeResponse>
{
		                                                                                                                                  
public   		string
   cid  { get; set; }

                  
                                                            
                                                          
public   		string
   isKeyProp  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   isSaleProp  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   aid  { get; set; }

                  
                                                            
                                                          
public   		string
   fields  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.ware.get.attribute";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("cid", this.cid);
			parameters.Add("is_key_prop", this.isKeyProp);
			parameters.Add("is_sale_prop", this.isSaleProp);
			parameters.Add("aid", this.aid);
			parameters.Add("fields", this.fields);
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








        
 

