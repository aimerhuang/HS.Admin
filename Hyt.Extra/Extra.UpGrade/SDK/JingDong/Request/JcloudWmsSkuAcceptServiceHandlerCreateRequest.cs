using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class JcloudWmsSkuAcceptServiceHandlerCreateRequest : IJdRequest<JcloudWmsSkuAcceptServiceHandlerCreateResponse>
{
		                                                                                                                                                                                                    
public   		string
   code  { get; set; }

                  
                                                            
                                                          
public   		string
   tenantId  { get; set; }

                  
                                                            
                                                          
public   		string
   specification  { get; set; }

                  
                                                            
                                                          
public   		string
   model  { get; set; }

                  
                                                            
                                                          
public   		string
   ownerNo  { get; set; }

                  
                                                            
                                                          
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		string
   foreignName  { get; set; }

                  
                                                            
                                                          
public   		string
   categoryCode  { get; set; }

                  
                                                            
                                                          
public   		string
   brand  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   barcodeType  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   barcodeArr  { get; set; }
                                                                                                                                                                                                
public   		string
   sizeType  { get; set; }

                  
                                                            
                                                          
public   		string
   weight  { get; set; }

                  
                                                            
                                                          
public   		string
   length  { get; set; }

                  
                                                            
                                                          
public   		string
   width  { get; set; }

                  
                                                            
                                                          
public   		string
   height  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   isShelfLife  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   shelfLife  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   isSerial  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   isHighValue  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   isLuxury  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   isFragile  { get; set; }

                  
                                                            
                                                          
public   		string
   memo  { get; set; }

                  
                                                            
                                                          
public   		string
   operateUser  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operateTime  { get; set; }

                  
                                                            
                                                          
public   		string
   dangerLevel  { get; set; }

                  
                                                            
                                                          
public   		string
   manufactureSkuNo  { get; set; }

                  
                                                            
                                                          
public   		string
   erpSkuNo  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.jcloud.wms.SkuAcceptServiceHandler.create";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("code", this.code);
			parameters.Add("tenantId", this.tenantId);
			parameters.Add("specification", this.specification);
			parameters.Add("model", this.model);
			parameters.Add("ownerNo", this.ownerNo);
			parameters.Add("name", this.name);
			parameters.Add("foreignName", this.foreignName);
			parameters.Add("categoryCode", this.categoryCode);
			parameters.Add("brand", this.brand);
			parameters.Add("barcodeType", this.barcodeType);
			parameters.Add("barcodeArr", this.barcodeArr);
			parameters.Add("sizeType", this.sizeType);
			parameters.Add("weight", this.weight);
			parameters.Add("length", this.length);
			parameters.Add("width", this.width);
			parameters.Add("height", this.height);
			parameters.Add("isShelfLife", this.isShelfLife);
			parameters.Add("shelfLife", this.shelfLife);
			parameters.Add("isSerial", this.isSerial);
			parameters.Add("isHighValue", this.isHighValue);
			parameters.Add("isLuxury", this.isLuxury);
			parameters.Add("isFragile", this.isFragile);
			parameters.Add("memo", this.memo);
			parameters.Add("operateUser", this.operateUser);
			parameters.Add("operateTime", this.operateTime);
			parameters.Add("dangerLevel", this.dangerLevel);
			parameters.Add("manufactureSkuNo", this.manufactureSkuNo);
			parameters.Add("erpSkuNo", this.erpSkuNo);
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








        
 

