using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareListingGetRequest : IJdRequest<WareListingGetResponse>
{
		                                                                                                                                  
public   		string
   cid  { get; set; }

                  
                                                            
                                                          
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   endModified  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   startModified  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   fields  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.ware.listing.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("cid", this.cid);
			parameters.Add("page", this.page);
			parameters.Add("page_size", this.pageSize);
			parameters.Add("end_modified", this.endModified);
			parameters.Add("start_modified", this.startModified);
			parameters.Add("fields", this.fields);
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








        
 

