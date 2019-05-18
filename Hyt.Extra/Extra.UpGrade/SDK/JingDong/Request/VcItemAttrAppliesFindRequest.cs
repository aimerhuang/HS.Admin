using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcItemAttrAppliesFindRequest : IJdRequest<VcItemAttrAppliesFindResponse>
{
		                                                                                                                                                                   
public   		string
   wareGroupId  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<int>
   category  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   beginApplyTime  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   endApplyTime  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<int>
   state  { get; set; }

                  
                                                            
                                                          
public   		string
   publicName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   offset  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.item.attr.applies.find";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ware_group_id", this.wareGroupId);
			parameters.Add("category", this.category);
			parameters.Add("begin_apply_time", this.beginApplyTime);
			parameters.Add("end_apply_time", this.endApplyTime);
			parameters.Add("state", this.state);
			parameters.Add("public_name", this.publicName);
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








        
 

