using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VisJosServiceUpdateInventoryRequest : IJdRequest<VisJosServiceUpdateInventoryResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                                                                                                                		public  		string
   JDSKU  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   VENDORCODE  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		Nullable<int>
   QUANTITY  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         		public  		string
   DELVCENTERID  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vis.jos.service.updateInventory";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("JD_SKU", this.JDSKU);
			parameters.Add("VENDOR_CODE", this.VENDORCODE);
			parameters.Add("QUANTITY", this.QUANTITY);
			parameters.Add("DELV_CENTER_ID", this.DELVCENTERID);
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








        
 

