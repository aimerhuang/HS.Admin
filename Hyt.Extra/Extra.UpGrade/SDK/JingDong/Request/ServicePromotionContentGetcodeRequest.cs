using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ServicePromotionContentGetcodeRequest : IJdRequest<ServicePromotionContentGetcodeResponse>
{
		                                                                                                                                                                   
public   		string
   releaseType  { get; set; }

                  
                                                            
                                                          
public   		string
   typeId  { get; set; }

                  
                                                            
                                                          
public   		string
   sortName  { get; set; }

                  
                                                            
                                                          
public   		string
   sort  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   unionId  { get; set; }

                  
                                                            
                                                          
public   		string
   subUnionId  { get; set; }

                  
                                                            
                                                          
public   		string
   webId  { get; set; }

                  
                                                            
                                                          
public   		string
   ext1  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   protocol  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   positionId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.service.promotion.content.getcode";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("releaseType", this.releaseType);
			parameters.Add("typeId", this.typeId);
			parameters.Add("sortName", this.sortName);
			parameters.Add("sort", this.sort);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("pageIndex", this.pageIndex);
			parameters.Add("unionId", this.unionId);
			parameters.Add("subUnionId", this.subUnionId);
			parameters.Add("webId", this.webId);
			parameters.Add("ext1", this.ext1);
			parameters.Add("protocol", this.protocol);
			parameters.Add("positionId", this.positionId);
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








        
 

