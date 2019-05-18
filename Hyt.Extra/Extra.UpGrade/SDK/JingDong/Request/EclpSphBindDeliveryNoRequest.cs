using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EclpSphBindDeliveryNoRequest : IJdRequest<EclpSphBindDeliveryNoResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   serviceId  { get; set; }

                  
                                                            
                                                          
public   		string
   sourceId  { get; set; }

                  
                                                            
                                                          
public   		string
   deliveryCompany  { get; set; }

                  
                                                            
                                                          
public   		string
   deliveryNo  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.eclp.sph.bindDeliveryNo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("serviceId", this.serviceId);
			parameters.Add("sourceId", this.sourceId);
			parameters.Add("deliveryCompany", this.deliveryCompany);
			parameters.Add("deliveryNo", this.deliveryNo);
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








        
 

