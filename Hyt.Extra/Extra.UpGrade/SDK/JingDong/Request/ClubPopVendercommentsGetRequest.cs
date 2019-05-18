using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ClubPopVendercommentsGetRequest : IJdRequest<ClubPopVendercommentsGetResponse>
{
		                                                                                                                                  
public   		string
   skuids  { get; set; }

                  
                                                            
                                                                                           
public   		string
   wareName  { get; set; }

                  
                                                            
                                                          
public   		string
   beginTime  { get; set; }

                  
                                                            
                                                          
public   		string
   endTime  { get; set; }

                  
                                                            
                                                          
public   		string
   score  { get; set; }

                  
                                                            
                                                          
public   		string
   content  { get; set; }

                  
                                                            
                                                          
public   		string
   pin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<bool>
   isVenderReply  { get; set; }

                  
                                                            
                                                          
public   		string
   cid  { get; set; }

                  
                                                            
                                                          
public   		string
   orderIds  { get; set; }

                  
                                                            
                                                          
public   		string
   page  { get; set; }

                  
                                                            
                                                          
public   		string
   pageSize  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.club.pop.vendercomments.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("skuids", this.skuids);
			parameters.Add("wareName", this.wareName);
			parameters.Add("beginTime", this.beginTime);
			parameters.Add("endTime", this.endTime);
			parameters.Add("score", this.score);
			parameters.Add("content", this.content);
			parameters.Add("pin", this.pin);
			parameters.Add("isVenderReply", this.isVenderReply);
			parameters.Add("cid", this.cid);
			parameters.Add("orderIds", this.orderIds);
			parameters.Add("page", this.page);
			parameters.Add("pageSize", this.pageSize);
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








        
 

