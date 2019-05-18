using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SkuReadSearchSkuListRequest : IJdRequest<SkuReadSearchSkuListResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    		public  		string
   wareId  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   skuStatuValue  { get; set; }
                                                                                                                                                                                                
public   		Nullable<long>
   maxStockNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   minStockNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endCreatedTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endModifiedTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startCreatedTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startModifiedTime  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                           		public  		string
   outId  { get; set; }
                                                                                                                                                                                                
public   		Nullable<int>
   colType  { get; set; }

                  
                                                            
                                                          
public   		string
   itemNum  { get; set; }

                  
                                                            
                                                          
public   		string
   wareTitle  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   orderFiled  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   orderType  { get; set; }
                                                                                                                                                                                                
public   		Nullable<int>
   pageNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                                                                                       
public   		string
   field  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.sku.read.searchSkuList";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wareId", this.wareId);
			parameters.Add("skuStatuValue", this.skuStatuValue);
			parameters.Add("maxStockNum", this.maxStockNum);
			parameters.Add("minStockNum", this.minStockNum);
			parameters.Add("endCreatedTime", this.endCreatedTime);
			parameters.Add("endModifiedTime", this.endModifiedTime);
			parameters.Add("startCreatedTime", this.startCreatedTime);
			parameters.Add("startModifiedTime", this.startModifiedTime);
			parameters.Add("outId", this.outId);
			parameters.Add("colType", this.colType);
			parameters.Add("itemNum", this.itemNum);
			parameters.Add("wareTitle", this.wareTitle);
			parameters.Add("orderFiled", this.orderFiled);
			parameters.Add("orderType", this.orderType);
			parameters.Add("pageNo", this.pageNo);
			parameters.Add("page_size", this.pageSize);
			parameters.Add("field", this.field);
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








        
 

