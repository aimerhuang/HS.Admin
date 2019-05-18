using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VenderAnnouncementListRequest : IJdRequest<VenderAnnouncementListResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   type  { get; set; }

                  
                                                            
                                                                                                                            
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   size  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vender.announcement.list";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("type", this.type);
			parameters.Add("page", this.page);
			parameters.Add("size", this.size);
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








        
 

