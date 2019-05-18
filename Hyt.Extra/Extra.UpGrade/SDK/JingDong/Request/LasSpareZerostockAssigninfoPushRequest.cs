using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LasSpareZerostockAssigninfoPushRequest : IJdRequest<LasSpareZerostockAssigninfoPushResponse>
{
		                                                                                                                                  
public   		string
   afsNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   ordNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   sitNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   sitN  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   sitTel  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   actT  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.las.spare.zerostock.assigninfo.push";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("afs_no", this.afsNo);
			parameters.Add("ord_no", this.ordNo);
			parameters.Add("sit_no", this.sitNo);
			parameters.Add("sit_n", this.sitN);
			parameters.Add("sit_tel", this.sitTel);
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








        
 

