using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcItemQualificationListFindRequest : IJdRequest<VcItemQualificationListFindResponse>
{
		                                                                                                                                                                   
public   		string
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   categoryId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   brandId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   beginAuditTime  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   endAuditTime  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<int>
   state  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   offset  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.item.qualification.list.find";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ware_id", this.wareId);
			parameters.Add("category_id", this.categoryId);
			parameters.Add("name", this.name);
			parameters.Add("brand_id", this.brandId);
			parameters.Add("begin_audit_time", this.beginAuditTime);
			parameters.Add("end_audit_time", this.endAuditTime);
			parameters.Add("state", this.state);
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








        
 

