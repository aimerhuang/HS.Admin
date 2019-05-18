using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerCouponReadGetCouponListRequest : IJdRequest<SellerCouponReadGetCouponListResponse>
{
		                                                                                                                                                                                                    
public   		string
   ip  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   port  { get; set; }

                  
                                                            
                                                                                                                                                                                        
public   		Nullable<long>
   couponId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   type  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   grantType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   bindType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   grantWay  { get; set; }

                  
                                                            
                                                          
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		string
   createMonth  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   creatorType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   closed  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   page  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.coupon.read.getCouponList";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ip", this.ip);
			parameters.Add("port", this.port);
			parameters.Add("couponId", this.couponId);
			parameters.Add("type", this.type);
			parameters.Add("grantType", this.grantType);
			parameters.Add("bindType", this.bindType);
			parameters.Add("grantWay", this.grantWay);
			parameters.Add("name", this.name);
			parameters.Add("createMonth", this.createMonth);
			parameters.Add("creatorType", this.creatorType);
			parameters.Add("closed", this.closed);
			parameters.Add("page", this.page);
			parameters.Add("pageSize", this.pageSize);
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








        
 

