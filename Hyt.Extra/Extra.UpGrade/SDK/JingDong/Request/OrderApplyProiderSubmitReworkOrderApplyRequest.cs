using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class OrderApplyProiderSubmitReworkOrderApplyRequest : IJdRequest<OrderApplyProiderSubmitReworkOrderApplyResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   afsServiceId  { get; set; }

                  
                                                            
                                                          
public   		string
   buId  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   operatorPin  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorNick  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorRemark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operatorDate  { get; set; }

                  
                                                            
                                                          
public   		string
   platformSrc  { get; set; }

                  
                                                            
                                                                                           
public   		string
   customerPin  { get; set; }

                  
                                                            
                                                          
public   		string
   receiptName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   provinceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   city  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   county  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   village  { get; set; }

                  
                                                            
                                                          
public   		string
   receiptAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   tel  { get; set; }

                  
                                                            
                                                          
public   		string
   applyDescription  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   wareId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   wareName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   wareQty  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   relationWareId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   relationWareType  { get; set; }
                                                                                                                                                                                                
public   		Nullable<int>
   deliveryCenterId  { get; set; }

                  
                                                            
                                                          
public   		string
   deliveryCenterName  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   storeId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.OrderApplyProider.submitReworkOrderApply";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("afsServiceId", this.afsServiceId);
			parameters.Add("buId", this.buId);
			parameters.Add("operatorPin", this.operatorPin);
			parameters.Add("operatorNick", this.operatorNick);
			parameters.Add("operatorRemark", this.operatorRemark);
			parameters.Add("operatorDate", this.operatorDate);
			parameters.Add("platformSrc", this.platformSrc);
			parameters.Add("customerPin", this.customerPin);
			parameters.Add("receiptName", this.receiptName);
			parameters.Add("provinceId", this.provinceId);
			parameters.Add("city", this.city);
			parameters.Add("county", this.county);
			parameters.Add("village", this.village);
			parameters.Add("receiptAddress", this.receiptAddress);
			parameters.Add("tel", this.tel);
			parameters.Add("applyDescription", this.applyDescription);
			parameters.Add("wareId", this.wareId);
			parameters.Add("wareName", this.wareName);
			parameters.Add("wareQty", this.wareQty);
			parameters.Add("relationWareId", this.relationWareId);
			parameters.Add("relationWareType", this.relationWareType);
			parameters.Add("deliveryCenterId", this.deliveryCenterId);
			parameters.Add("deliveryCenterName", this.deliveryCenterName);
			parameters.Add("storeId", this.storeId);
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








        
 

