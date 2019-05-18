using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class KuaicheAccountIncomexpenseSearchRequest : IJdRequest<KuaicheAccountIncomexpenseSearchResponse>
{
		                                                                                                       
public   		Nullable<long>
   inOutType  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<long>
   type  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   checkType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   pageIndex  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<long>
   pageSize  { get; set; }

                  
                                                                                                                                    

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.kuaiche.account.incomexpense.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("in_out_type", this.inOutType);
			parameters.Add("type", this.type);
			parameters.Add("check_type", this.checkType);
			parameters.Add("page_index", this.pageIndex);
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








        
 

