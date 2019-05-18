using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class DeleteVenderDeliveryCompanyRequest : IJdRequest<DeleteVenderDeliveryCompanyResponse>
{
		                                                                                                                                  
public   		string
   deliveryCompanyId  { get; set; }

                  
                                                                                                                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.delete.vender.delivery.company";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("delivery_company_id", this.deliveryCompanyId);
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








        
 

