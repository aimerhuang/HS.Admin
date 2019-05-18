using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcGetdetailbyorderidRequest : IJdRequest<VcGetdetailbyorderidResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   orderId  { get; set; }

                  
                                                                                                                                    
                                                                                           
public   		string
   sortFiled  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   sortMode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<bool>
   isPage  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.getdetailbyorderid";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("order_id", this.orderId);
			parameters.Add("sort_filed", this.sortFiled);
			parameters.Add("sort_mode", this.sortMode);
			parameters.Add("page_index", this.pageIndex);
			parameters.Add("page_size", this.pageSize);
			parameters.Add("is_page", this.isPage);
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








        
 

