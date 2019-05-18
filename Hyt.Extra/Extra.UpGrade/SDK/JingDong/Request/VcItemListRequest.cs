using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcItemListRequest : IJdRequest<VcItemListResponse>
{
		                                                                                                                                                                   
public   		string
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   brandId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   categoryId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   saleState  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   beginModifyTime  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   endModifyTime  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<int>
   orderType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   offset  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.item.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ware_id", this.wareId);
			parameters.Add("name", this.name);
			parameters.Add("brand_id", this.brandId);
			parameters.Add("category_id", this.categoryId);
			parameters.Add("sale_state", this.saleState);
			parameters.Add("begin_modify_time", this.beginModifyTime);
			parameters.Add("end_modify_time", this.endModifyTime);
			parameters.Add("order_type", this.orderType);
			parameters.Add("offset", this.offset);
			parameters.Add("page_size", this.pageSize);
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








        
 

