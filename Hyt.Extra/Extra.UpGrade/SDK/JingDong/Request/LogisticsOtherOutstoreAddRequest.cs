using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsOtherOutstoreAddRequest : IJdRequest<LogisticsOtherOutstoreAddResponse>
{
		                                                                                                                                  
public   		string
   outboundNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   joslWareNo  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   joslCarriersNo  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   expectDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   supplierName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   supplierNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   approver  { get; set; }

                  
                                                            
                                                          
public   		string
   outboundType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneeName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		string
   stationNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   stationName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   receivable  { get; set; }

                  
                                                            
                                                          
public   		string
   zipCode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   phone  { get; set; }

                  
                                                            
                                                          
public   		string
   mobile  { get; set; }

                  
                                                            
                                                          
public   		string
   email  { get; set; }

                  
                                                            
                                                          
public   		string
   buyerRemark  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   verifyRemark  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   returnConsigneeName  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   returnConsigneeAddress  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   returnConsigneeMobile  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   addressProvince  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   addressCity  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   addressCounty  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   addressTown  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   picker  { get; set; }

                  
                                                            
                                                          
public   		string
   pickerCell  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pikerId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   transportWay  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   orderMark  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             		public  		string
   joslGoodNo  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         		public  		string
   isvGoodNo  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   outQty  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   goodStatus  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.otherOutstore.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("outbound_no", this.outboundNo);
			parameters.Add("josl_ware_no", this.joslWareNo);
			parameters.Add("josl_carriers_no", this.joslCarriersNo);
			parameters.Add("expect_date", this.expectDate);
			parameters.Add("supplier_name", this.supplierName);
			parameters.Add("supplier_no", this.supplierNo);
			parameters.Add("approver", this.approver);
			parameters.Add("outbound_type", this.outboundType);
			parameters.Add("remark", this.remark);
			parameters.Add("consignee_name", this.consigneeName);
			parameters.Add("address", this.address);
			parameters.Add("station_no", this.stationNo);
			parameters.Add("station_name", this.stationName);
			parameters.Add("receivable", this.receivable);
			parameters.Add("zip_code", this.zipCode);
			parameters.Add("phone", this.phone);
			parameters.Add("mobile", this.mobile);
			parameters.Add("email", this.email);
			parameters.Add("buyer_remark", this.buyerRemark);
			parameters.Add("verify_remark", this.verifyRemark);
			parameters.Add("return_consignee_name", this.returnConsigneeName);
			parameters.Add("return_consignee_address", this.returnConsigneeAddress);
			parameters.Add("return_consignee_mobile", this.returnConsigneeMobile);
			parameters.Add("address_province", this.addressProvince);
			parameters.Add("address_city", this.addressCity);
			parameters.Add("address_county", this.addressCounty);
			parameters.Add("address_town", this.addressTown);
			parameters.Add("picker", this.picker);
			parameters.Add("picker_cell", this.pickerCell);
			parameters.Add("piker_id", this.pikerId);
			parameters.Add("transport_way", this.transportWay);
			parameters.Add("order_mark", this.orderMark);
			parameters.Add("josl_good_no", this.joslGoodNo);
			parameters.Add("isv_good_no", this.isvGoodNo);
			parameters.Add("out_qty", this.outQty);
			parameters.Add("good_status", this.goodStatus);
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








        
 

