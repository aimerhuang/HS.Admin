using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WaresSearchRequest : IJdRequest<WaresSearchResponse>
{
		                                                                                                                                  
public   		string
   cid  { get; set; }

                  
                                                            
                                                          
public   		string
   startPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endPrice  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   title  { get; set; }

                  
                                                            
                                                          
public   		string
   orderBy  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   startTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   startModified  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endModified  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   wareStatus  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   fields  { get; set; }

                  
                                                            
                                                          
public   		string
   parentShopCategoryId  { get; set; }

                  
                                                            
                                                          
public   		string
   shopCategoryId  { get; set; }

                  
                                                            
                                                          
public   		string
   itemNum  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.wares.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("cid", this.cid);
			parameters.Add("start_price", this.startPrice);
			parameters.Add("end_price", this.endPrice);
			parameters.Add("page", this.page);
			parameters.Add("page_size", this.pageSize);
			parameters.Add("title", this.title);
			parameters.Add("order_by", this.orderBy);
			parameters.Add("start_time", this.startTime);
			parameters.Add("end_time", this.endTime);
			parameters.Add("start_modified", this.startModified);
			parameters.Add("end_modified", this.endModified);
			parameters.Add("ware_status", this.wareStatus);
			parameters.Add("fields", this.fields);
			parameters.Add("parentShopCategoryId", this.parentShopCategoryId);
			parameters.Add("shopCategoryId", this.shopCategoryId);
			parameters.Add("itemNum", this.itemNum);
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








        
 

