using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopAfsRefundapplyOperatorRefundApplyRequest : IJdRequest<PopAfsRefundapplyOperatorRefundApplyResponse>
{
		                                                                                                                                        
public   		Nullable<long>
   raId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   state  { get; set; }

                  
                                                            
                                                          
public   		string
   remark  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.afs.refundapply.operatorRefundApply";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ra_id", this.raId);
			parameters.Add("state", this.state);
			parameters.Add("remark", this.remark);
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








        
 

