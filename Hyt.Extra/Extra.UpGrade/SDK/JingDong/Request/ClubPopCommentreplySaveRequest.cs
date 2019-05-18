using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ClubPopCommentreplySaveRequest : IJdRequest<ClubPopCommentreplySaveResponse>
{
		                                                                                                                                  
public   		string
   commentId  { get; set; }

                  
                                                            
                                                          
public   		string
   content  { get; set; }

                  
                                                            
                                                                                           
public   		string
   replyId  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.club.pop.commentreply.save";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("commentId", this.commentId);
			parameters.Add("content", this.content);
			parameters.Add("replyId", this.replyId);
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








        
 

