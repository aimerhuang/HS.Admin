using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareProductDetailSearchListGetRequest : IJdRequest<WareProductDetailSearchListGetResponse>
{
		                                                                      
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isLoadWareScore  { get; set; }

                  
                                                            
                                                          
public   		string
   client  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ware.product.detail.search.list.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("skuId", this.skuId);
			parameters.Add("isLoadWareScore", this.isLoadWareScore);
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








        
 

