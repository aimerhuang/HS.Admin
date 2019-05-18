using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopOrderModifyOrderAddrRequest : IJdRequest<PopOrderModifyOrderAddrResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                            
                                                                                           
public   		string
   customerName  { get; set; }

                  
                                                            
                                                          
public   		string
   customerPhone  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   provinceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   cityId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   countyId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   townId  { get; set; }

                  
                                                            
                                                          
public   		string
   detailAddr  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.order.modifyOrderAddr";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderId", this.orderId);
			parameters.Add("customerName", this.customerName);
			parameters.Add("customerPhone", this.customerPhone);
			parameters.Add("provinceId", this.provinceId);
			parameters.Add("cityId", this.cityId);
			parameters.Add("countyId", this.countyId);
			parameters.Add("townId", this.townId);
			parameters.Add("detailAddr", this.detailAddr);
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








        
 

