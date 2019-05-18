using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class PopAfterSaleReplyWorkOrderRequest : IJdRequest<PopAfterSaleReplyWorkOrderResponse>
{
		                                                                                                       
public   		string
   account  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   workId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   replyContent  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   workType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   work2Type  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.pop.afterSale.ReplyWorkOrder";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("account", this.account);
			parameters.Add("work_id", this.workId);
			parameters.Add("reply_content", this.replyContent);
			parameters.Add("work_type", this.workType);
			parameters.Add("work2_type", this.work2Type);
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








        
 

