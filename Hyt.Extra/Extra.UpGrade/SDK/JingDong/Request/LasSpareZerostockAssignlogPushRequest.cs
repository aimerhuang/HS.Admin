using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LasSpareZerostockAssignlogPushRequest : IJdRequest<LasSpareZerostockAssignlogPushResponse>
{
		                                                                                                                                  
public   		string
   afsNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   ordNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   traInf  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   actT  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.las.spare.zerostock.assignlog.push";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("afs_no", this.afsNo);
			parameters.Add("ord_no", this.ordNo);
			parameters.Add("tra_inf", this.traInf);
			parameters.Add("act_t", this.actT);
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








        
 

