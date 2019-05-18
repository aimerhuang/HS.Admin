using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LogisticsOrderAddRequest : IJdRequest<LogisticsOrderAddResponse>
{
		                                                                                                                                  
public   		string
   channelsSellerNo  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   channelsOutboundNo  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   carriersId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   expectDate  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   orderNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   shopNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   consigneeName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   addressProvince  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   addressCity  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   addressCounty  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   addressTown  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		string
   zipCode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   phone  { get; set; }

                  
                                                            
                                                          
public   		string
   mobile  { get; set; }

                  
                                                            
                                                          
public   		string
   receivable  { get; set; }

                  
                                                            
                                                          
public   		string
   email  { get; set; }

                  
                                                            
                                                          
public   		string
   buyerRemark  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   verifyRemark  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   returnConsigneeAddress  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   returnConsigneeName  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   returnConsigneePhone  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   stationNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   stationName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   orderMark  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   shopOrderSource  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   shopOrderCreateTime  { get; set; }

                  
                                                                                                                                                                                    
                                                                                                                                                             
public   		string
   picker  { get; set; }

                  
                                                            
                                                          
public   		string
   pickerCall  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   pikerId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   packType  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     		public  		string
   goodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   skuId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   shopGoodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   qty  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   goodsStatus  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.logistics.order.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("channels_seller_no", this.channelsSellerNo);
			parameters.Add("channels_outbound_no", this.channelsOutboundNo);
			parameters.Add("warehouse_no", this.warehouseNo);
			parameters.Add("carriers_id", this.carriersId);
			parameters.Add("expect_date", this.expectDate);
			parameters.Add("order_no", this.orderNo);
			parameters.Add("shop_no", this.shopNo);
			parameters.Add("consignee_name", this.consigneeName);
			parameters.Add("address_province", this.addressProvince);
			parameters.Add("address_city", this.addressCity);
			parameters.Add("address_county", this.addressCounty);
			parameters.Add("address_town", this.addressTown);
			parameters.Add("address", this.address);
			parameters.Add("zip_code", this.zipCode);
			parameters.Add("phone", this.phone);
			parameters.Add("mobile", this.mobile);
			parameters.Add("receivable", this.receivable);
			parameters.Add("email", this.email);
			parameters.Add("buyer_remark", this.buyerRemark);
			parameters.Add("verify_remark", this.verifyRemark);
			parameters.Add("return_consignee_address", this.returnConsigneeAddress);
			parameters.Add("return_consignee_name", this.returnConsigneeName);
			parameters.Add("return_consignee_phone", this.returnConsigneePhone);
			parameters.Add("station_no", this.stationNo);
			parameters.Add("station_name", this.stationName);
			parameters.Add("order_mark", this.orderMark);
			parameters.Add("shop_order_source", this.shopOrderSource);
			parameters.Add("shop_order_create_time", this.shopOrderCreateTime);
			parameters.Add("picker", this.picker);
			parameters.Add("picker_call", this.pickerCall);
			parameters.Add("piker_id", this.pikerId);
			parameters.Add("pack_type", this.packType);
			parameters.Add("goods_no", this.goodsNo);
			parameters.Add("sku_id", this.skuId);
			parameters.Add("shopGoodsNo", this.shopGoodsNo);
			parameters.Add("qty", this.qty);
			parameters.Add("goods_status", this.goodsStatus);
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








        
 

