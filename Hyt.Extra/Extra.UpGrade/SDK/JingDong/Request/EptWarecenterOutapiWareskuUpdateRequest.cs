using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptWarecenterOutapiWareskuUpdateRequest : IJdRequest<EptWarecenterOutapiWareskuUpdateResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   skuId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                          
public   		string
   rfId  { get; set; }

                  
                                                            
                                                          
public   		string
   attributes  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   supplyPrice  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   amountCount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   lockCount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   lockStartTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   lockEndTime  { get; set; }

                  
                                                            
                                                          
public   		string
   hsCode  { get; set; }

                  
                                                            
                                                          
public   		string
   upc  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.warecenter.outapi.waresku.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("skuId", this.skuId);
			parameters.Add("wareId", this.wareId);
			parameters.Add("rfId", this.rfId);
			parameters.Add("attributes", this.attributes);
			parameters.Add("supplyPrice", this.supplyPrice);
			parameters.Add("amountCount", this.amountCount);
			parameters.Add("lockCount", this.lockCount);
			parameters.Add("lockStartTime", this.lockStartTime);
			parameters.Add("lockEndTime", this.lockEndTime);
			parameters.Add("hsCode", this.hsCode);
			parameters.Add("upc", this.upc);
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








        
 

