using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ServicePromotionBatchGetcodeRequest : IJdRequest<ServicePromotionBatchGetcodeResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                                                                                                                     		public  		string
   id  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   url  { get; set; }
                                                                                                                                                                                                
public   		Nullable<long>
   unionId  { get; set; }

                  
                                                            
                                                          
public   		string
   subUnionId  { get; set; }

                  
                                                            
                                                          
public   		string
   channel  { get; set; }

                  
                                                            
                                                          
public   		string
   webId  { get; set; }

                  
                                                            
                                                          
public   		string
   ext1  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   protocol  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.service.promotion.batch.getcode";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("id", this.id);
			parameters.Add("url", this.url);
			parameters.Add("unionId", this.unionId);
			parameters.Add("subUnionId", this.subUnionId);
			parameters.Add("channel", this.channel);
			parameters.Add("webId", this.webId);
			parameters.Add("ext1", this.ext1);
			parameters.Add("protocol", this.protocol);
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








        
 

