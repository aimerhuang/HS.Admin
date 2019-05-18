using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class UnionOrderServiceQueryCommisionsRequest : IJdRequest<UnionOrderServiceQueryCommisionsResponse>
{
		                                                                                                       
public   		string
   time  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                          
public   		string
   unionId  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.UnionOrderService.queryCommisions";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("time", this.time);
			parameters.Add("pageIndex", this.pageIndex);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("unionId", this.unionId);
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








        
 

