using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class LasSpareZerostockConfirmRequest : IJdRequest<LasSpareZerostockConfirmResponse>
{
		                                                                                                                                  
public   		string
   afsNo  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   venCod  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   warDet  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.las.spare.zerostock.confirm";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("afs_no", this.afsNo);
			parameters.Add("ven_cod", this.venCod);
			parameters.Add("war_det", this.warDet);
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








        
 

