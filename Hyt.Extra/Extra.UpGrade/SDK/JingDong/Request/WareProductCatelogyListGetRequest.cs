using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareProductCatelogyListGetRequest : IJdRequest<WareProductCatelogyListGetResponse>
{
		                                                                      
public   		Nullable<int>
   catelogyId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   level  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isIcon  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isDescription  { get; set; }

                  
                                                            
                                                          
public   		string
   client  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ware.product.catelogy.list.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("catelogyId", this.catelogyId);
			parameters.Add("level", this.level);
			parameters.Add("isIcon", this.isIcon);
			parameters.Add("isDescription", this.isDescription);
			parameters.Add("client", this.client);
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








        
 

