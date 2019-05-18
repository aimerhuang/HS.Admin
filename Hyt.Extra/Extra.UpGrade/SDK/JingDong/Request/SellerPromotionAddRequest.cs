using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionAddRequest : IJdRequest<SellerPromotionAddResponse>
{
		                                                                                                                                                                   
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   type  { get; set; }

                  
                                                            
                                                          
public   		string
   beginTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   bound  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   member  { get; set; }

                  
                                                            
                                                          
public   		string
   slogan  { get; set; }

                  
                                                            
                                                          
public   		string
   comment  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   favorMode  { get; set; }

                  
                                                                                                                                    
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("name", this.name);
			parameters.Add("type", this.type);
			parameters.Add("begin_time", this.beginTime);
			parameters.Add("end_time", this.endTime);
			parameters.Add("bound", this.bound);
			parameters.Add("member", this.member);
			parameters.Add("slogan", this.slogan);
			parameters.Add("comment", this.comment);
			parameters.Add("favor_mode", this.favorMode);
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








        
 

