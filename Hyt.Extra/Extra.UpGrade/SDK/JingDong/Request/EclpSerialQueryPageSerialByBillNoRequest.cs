using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpSerialQueryPageSerialByBillNoRequest : IJdRequest<EclpSerialQueryPageSerialByBillNoResponse>
{
		                                                                                                                                  
public   		string
   billNo  { get; set; }

                  
                                                            
                                                          
public   		string
   billType  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                                           
public   		string
   pin  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.serial.queryPageSerialByBillNo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("billNo", this.billNo);
			parameters.Add("billType", this.billType);
			parameters.Add("pageNo", this.pageNo);
			parameters.Add("pageSize", this.pageSize);
			parameters.Add("pin", this.pin);
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








        
 

