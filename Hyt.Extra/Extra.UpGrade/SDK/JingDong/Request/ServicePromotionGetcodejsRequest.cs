using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ServicePromotionGetcodejsRequest : IJdRequest<ServicePromotionGetcodejsResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   promotionType  { get; set; }

                  
                                                            
                                                          
public   		string
   materialId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   unionId  { get; set; }

                  
                                                            
                                                          
public   		string
   subUnionId  { get; set; }

                  
                                                            
                                                          
public   		string
   siteSize  { get; set; }

                  
                                                            
                                                          
public   		string
   siteId  { get; set; }

                  
                                                            
                                                          
public   		string
   channel  { get; set; }

                  
                                                            
                                                          
public   		string
   webId  { get; set; }

                  
                                                            
                                                          
public   		string
   extendId  { get; set; }

                  
                                                            
                                                          
public   		string
   ext1  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.service.promotion.getcodejs";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("promotionType", this.promotionType);
			parameters.Add("materialId", this.materialId);
			parameters.Add("unionId", this.unionId);
			parameters.Add("subUnionId", this.subUnionId);
			parameters.Add("siteSize", this.siteSize);
			parameters.Add("siteId", this.siteId);
			parameters.Add("channel", this.channel);
			parameters.Add("webId", this.webId);
			parameters.Add("extendId", this.extendId);
			parameters.Add("ext1", this.ext1);
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








        
 

