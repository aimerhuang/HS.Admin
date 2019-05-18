using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopAfsSoaRefundapplyReplyRefundRequest : IJdRequest<PopAfsSoaRefundapplyReplyRefundResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   status  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   id  { get; set; }

                  
                                                            
                                                          
public   		string
   checkUserName  { get; set; }

                  
                                                            
                                                          
public   		string
   remark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   rejectType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   outWareStatus  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.afs.soa.refundapply.replyRefund";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("status", this.status);
			parameters.Add("id", this.id);
			parameters.Add("checkUserName", this.checkUserName);
			parameters.Add("remark", this.remark);
			parameters.Add("rejectType", this.rejectType);
			parameters.Add("outWareStatus", this.outWareStatus);
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








        
 

