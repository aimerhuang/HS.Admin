using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpIbAddOutsideMainRequest : IJdRequest<EclpIbAddOutsideMainResponse>
{
		                                                                                                                                  
public   		string
   deptNo  { get; set; }

                  
                                                            
                                                          
public   		string
   isvOutsideNo  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNoOut  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNoIn  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                                                             		public  		string
   goodsNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		Nullable<int>
   planNum  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.ib.addOutsideMain";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("deptNo", this.deptNo);
			parameters.Add("isvOutsideNo", this.isvOutsideNo);
			parameters.Add("warehouseNoOut", this.warehouseNoOut);
			parameters.Add("warehouseNoIn", this.warehouseNoIn);
			parameters.Add("goodsNo", this.goodsNo);
			parameters.Add("planNum", this.planNum);
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








        
 

