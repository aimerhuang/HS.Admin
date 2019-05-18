using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptJstoreOrderOutbillReturnRequest : IJdRequest<EptJstoreOrderOutbillReturnResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   outBillNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   storeId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   orderNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   sellerId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   outBillTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   skuNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   deliveryTime  { get; set; }

                  
                                                            
                                                          
public   		string
   length  { get; set; }

                  
                                                            
                                                          
public   		string
   width  { get; set; }

                  
                                                            
                                                          
public   		string
   height  { get; set; }

                  
                                                            
                                                          
public   		string
   weight  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.jstore.order.outbill.return";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("outBillNo", this.outBillNo);
			parameters.Add("storeId", this.storeId);
			parameters.Add("orderNo", this.orderNo);
			parameters.Add("sellerId", this.sellerId);
			parameters.Add("outBillTime", this.outBillTime);
			parameters.Add("skuId", this.skuId);
			parameters.Add("skuNum", this.skuNum);
			parameters.Add("deliveryTime", this.deliveryTime);
			parameters.Add("length", this.length);
			parameters.Add("width", this.width);
			parameters.Add("height", this.height);
			parameters.Add("weight", this.weight);
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








        
 

