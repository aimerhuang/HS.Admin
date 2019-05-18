using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EtmsWaybillSendRequest : IJdRequest<EtmsWaybillSendResponse>
{
		                                                                                                                                                                   
public   		string
   deliveryId  { get; set; }

                  
                                                            
                                                          
public   		string
   salePlat  { get; set; }

                  
                                                            
                                                          
public   		string
   customerCode  { get; set; }

                  
                                                            
                                                          
public   		string
   orderId  { get; set; }

                  
                                                            
                                                          
public   		string
   thrOrderId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   selfPrintWayBill  { get; set; }

                  
                                                            
                                                          
public   		string
   pickMethod  { get; set; }

                  
                                                            
                                                          
public   		string
   packageRequired  { get; set; }

                  
                                                            
                                                          
public   		string
   senderName  { get; set; }

                  
                                                            
                                                          
public   		string
   senderAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   senderTel  { get; set; }

                  
                                                            
                                                          
public   		string
   senderMobile  { get; set; }

                  
                                                            
                                                          
public   		string
   senderPostcode  { get; set; }

                  
                                                            
                                                          
public   		string
   receiveName  { get; set; }

                  
                                                            
                                                          
public   		string
   receiveAddress  { get; set; }

                  
                                                            
                                                          
public   		string
   province  { get; set; }

                  
                                                            
                                                          
public   		string
   city  { get; set; }

                  
                                                            
                                                          
public   		string
   county  { get; set; }

                  
                                                            
                                                          
public   		string
   town  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   provinceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   cityId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   countyId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   townId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   siteType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   siteId  { get; set; }

                  
                                                            
                                                          
public   		string
   siteName  { get; set; }

                  
                                                            
                                                          
public   		string
   receiveTel  { get; set; }

                  
                                                            
                                                          
public   		string
   receiveMobile  { get; set; }

                  
                                                            
                                                          
public   		string
   postcode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   packageCount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   weight  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   vloumLong  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   vloumWidth  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   vloumHeight  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   vloumn  { get; set; }

                  
                                                            
                                                          
public   		string
   description  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   collectionValue  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   collectionMoney  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   guaranteeValue  { get; set; }

                  
                                                            
                                                          
public   		Nullable<double>
   guaranteeValueAmount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   signReturn  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   aging  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   transType  { get; set; }

                  
                                                            
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   goodsType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   orderType  { get; set; }

                  
                                                            
                                                          
public   		string
   shopCode  { get; set; }

                  
                                                            
                                                          
public   		string
   orderSendTime  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   areaProvId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   areaCityId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   shipmentStartTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   shipmentEndTime  { get; set; }

                  
                                                            
                                                          
public   		string
   extendField1  { get; set; }

                  
                                                            
                                                          
public   		string
   extendField2  { get; set; }

                  
                                                            
                                                          
public   		string
   extendField3  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   extendField4  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   extendField5  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.etms.waybill.send";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deliveryId", this.deliveryId);
			parameters.Add("salePlat", this.salePlat);
			parameters.Add("customerCode", this.customerCode);
			parameters.Add("orderId", this.orderId);
			parameters.Add("thrOrderId", this.thrOrderId);
			parameters.Add("selfPrintWayBill", this.selfPrintWayBill);
			parameters.Add("pickMethod", this.pickMethod);
			parameters.Add("packageRequired", this.packageRequired);
			parameters.Add("senderName", this.senderName);
			parameters.Add("senderAddress", this.senderAddress);
			parameters.Add("senderTel", this.senderTel);
			parameters.Add("senderMobile", this.senderMobile);
			parameters.Add("senderPostcode", this.senderPostcode);
			parameters.Add("receiveName", this.receiveName);
			parameters.Add("receiveAddress", this.receiveAddress);
			parameters.Add("province", this.province);
			parameters.Add("city", this.city);
			parameters.Add("county", this.county);
			parameters.Add("town", this.town);
			parameters.Add("provinceId", this.provinceId);
			parameters.Add("cityId", this.cityId);
			parameters.Add("countyId", this.countyId);
			parameters.Add("townId", this.townId);
			parameters.Add("siteType", this.siteType);
			parameters.Add("siteId", this.siteId);
			parameters.Add("siteName", this.siteName);
			parameters.Add("receiveTel", this.receiveTel);
			parameters.Add("receiveMobile", this.receiveMobile);
			parameters.Add("postcode", this.postcode);
			parameters.Add("packageCount", this.packageCount);
			parameters.Add("weight", this.weight);
			parameters.Add("vloumLong", this.vloumLong);
			parameters.Add("vloumWidth", this.vloumWidth);
			parameters.Add("vloumHeight", this.vloumHeight);
			parameters.Add("vloumn", this.vloumn);
			parameters.Add("description", this.description);
			parameters.Add("collectionValue", this.collectionValue);
			parameters.Add("collectionMoney", this.collectionMoney);
			parameters.Add("guaranteeValue", this.guaranteeValue);
			parameters.Add("guaranteeValueAmount", this.guaranteeValueAmount);
			parameters.Add("signReturn", this.signReturn);
			parameters.Add("aging", this.aging);
			parameters.Add("transType", this.transType);
			parameters.Add("remark", this.remark);
			parameters.Add("goodsType", this.goodsType);
			parameters.Add("orderType", this.orderType);
			parameters.Add("shopCode", this.shopCode);
			parameters.Add("orderSendTime", this.orderSendTime);
			parameters.Add("warehouseCode", this.warehouseCode);
			parameters.Add("areaProvId", this.areaProvId);
			parameters.Add("areaCityId", this.areaCityId);
			parameters.Add("shipmentStartTime", this.shipmentStartTime);
			parameters.Add("shipmentEndTime", this.shipmentEndTime);
			parameters.Add("extendField1", this.extendField1);
			parameters.Add("extendField2", this.extendField2);
			parameters.Add("extendField3", this.extendField3);
			parameters.Add("extendField4", this.extendField4);
			parameters.Add("extendField5", this.extendField5);
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








        
 

