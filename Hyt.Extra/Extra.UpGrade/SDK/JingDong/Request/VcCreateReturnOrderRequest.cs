using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcCreateReturnOrderRequest : IJdRequest<VcCreateReturnOrderResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   fromDeliverCenterId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   toDeliverCenterId  { get; set; }

                  
                                                            
                                                          
public   		string
   purchaseErpCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   balanceType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   transportType  { get; set; }

                  
                                                            
                                                          
public   		string
   receiverName  { get; set; }

                  
                                                            
                                                          
public   		string
   receiverCell  { get; set; }

                  
                                                            
                                                          
public   		string
   pikerID  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   reservedPickUpDate  { get; set; }

                  
                                                            
                                                          
public   		string
   receiverAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   postCode  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                                                              		public  		string
   wareId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   returnNum  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.create.return.order";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("fromDeliverCenterId", this.fromDeliverCenterId);
			parameters.Add("toDeliverCenterId", this.toDeliverCenterId);
			parameters.Add("purchaseErpCode", this.purchaseErpCode);
			parameters.Add("balanceType", this.balanceType);
			parameters.Add("transportType", this.transportType);
			parameters.Add("receiverName", this.receiverName);
			parameters.Add("receiverCell", this.receiverCell);
			parameters.Add("pikerID", this.pikerID);
			parameters.Add("reservedPickUpDate", this.reservedPickUpDate);
			parameters.Add("receiverAddress", this.receiverAddress);
			parameters.Add("postCode", this.postCode);
			parameters.Add("wareId", this.wareId);
			parameters.Add("returnNum", this.returnNum);
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








        
 

