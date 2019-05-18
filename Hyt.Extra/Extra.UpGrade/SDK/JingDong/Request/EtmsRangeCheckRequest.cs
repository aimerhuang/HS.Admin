using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EtmsRangeCheckRequest : IJdRequest<EtmsRangeCheckResponse>
{
		                                                                                                                                                                                                    
public   		string
   salePlat  { get; set; }

                  
                                                            
                                                          
public   		string
   customerCode  { get; set; }

                  
                                                            
                                                          
public   		string
   orderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   goodsType  { get; set; }

                  
                                                            
                                                          
public   		string
   wareHouseCode  { get; set; }

                  
                                                            
                                                          
public   		string
   receiveAddress  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   senderProvinceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   senderCityId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   senderCountyId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   senderTownId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   receiverProvinceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   receiverCityId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   receiverCountyId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   receiverTownId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   sendTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   isCod  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.etms.range.check";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("salePlat", this.salePlat);
			parameters.Add("customerCode", this.customerCode);
			parameters.Add("orderId", this.orderId);
			parameters.Add("goodsType", this.goodsType);
			parameters.Add("wareHouseCode", this.wareHouseCode);
			parameters.Add("receiveAddress", this.receiveAddress);
			parameters.Add("senderProvinceId", this.senderProvinceId);
			parameters.Add("senderCityId", this.senderCityId);
			parameters.Add("senderCountyId", this.senderCountyId);
			parameters.Add("senderTownId", this.senderTownId);
			parameters.Add("receiverProvinceId", this.receiverProvinceId);
			parameters.Add("receiverCityId", this.receiverCityId);
			parameters.Add("receiverCountyId", this.receiverCountyId);
			parameters.Add("receiverTownId", this.receiverTownId);
			parameters.Add("sendTime", this.sendTime);
			parameters.Add("isCod", this.isCod);
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








        
 

