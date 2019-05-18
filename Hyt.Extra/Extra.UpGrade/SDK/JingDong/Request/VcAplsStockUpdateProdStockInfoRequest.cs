using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcAplsStockUpdateProdStockInfoRequest : IJdRequest<VcAplsStockUpdateProdStockInfoResponse>
{
		                                                                                                                                  
public   		string
   vendorCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   companyId  { get; set; }

                  
                                                            
                                                          
public   		string
   stockRfId  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                            		public  		Nullable<long>
   skuid  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		Nullable<int>
   stockNum  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.apls.stock.updateProdStockInfo";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("vendorCode", this.vendorCode);
			parameters.Add("companyId", this.companyId);
			parameters.Add("stockRfId", this.stockRfId);
			parameters.Add("skuid", this.skuid);
			parameters.Add("stockNum", this.stockNum);
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








        
 

