using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LasSpareZerostockServiceSearchRequest : IJdRequest<LasSpareZerostockServiceSearchResponse>
{
		                                                                                                                                  
public   		string
   begin  { get; set; }

                  
                                                            
                                                          
public   		string
   end  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   index  { get; set; }

                  
                                                            
                                                          
public   		string
   vc  { get; set; }

                  
                                                            
                                                          
public   		string
   token  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.las.spare.zerostock.service.search";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("begin", this.begin);
			parameters.Add("end", this.end);
			parameters.Add("index", this.index);
			parameters.Add("vc", this.vc);
			parameters.Add("token", this.token);
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








        
 

