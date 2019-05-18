using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcItemAdApplyListRequest : IJdRequest<VcItemAdApplyListResponse>
{
		                                                                                                                                                                   
public   		string
   wid  { get; set; }

                  
                                                            
                                                          
public   		string
   name  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   cid1  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   brandId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   state  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   beginApplyTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endApplyTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   offset  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.item.ad.apply.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wid", this.wid);
			parameters.Add("name", this.name);
			parameters.Add("cid1", this.cid1);
			parameters.Add("brandId", this.brandId);
			parameters.Add("state", this.state);
			parameters.Add("beginApplyTime", this.beginApplyTime);
			parameters.Add("endApplyTime", this.endApplyTime);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("offset", this.offset);
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








        
 

