using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpSerialQueryPageSerialByOwnerNoAndConditionRequest : IJdRequest<EclpSerialQueryPageSerialByOwnerNoAndConditionResponse>
{
		                                                                                                                                  
public   		string
   billType  { get; set; }

                  
                                                            
                                                          
public   		string
   ownerNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   startTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageNo  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                                           
public   		string
   pin  { get; set; }

                  
                                                            

		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.serial.queryPageSerialByOwnerNoAndCondition";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("billType", this.billType);
			parameters.Add("ownerNo", this.ownerNo);
			parameters.Add("startTime", this.startTime);
			parameters.Add("endTime", this.endTime);
			parameters.Add("warehouseNo", this.warehouseNo);
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








        
 

