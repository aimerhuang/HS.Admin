using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class MarketServiceQtReportUpdateRequest : IJdRequest<MarketServiceQtReportUpdateResponse>
{
		                                                                                                                                  
public   		string
   serviceItemCode  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		string
   qtCode  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   qtName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   qtType  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   qtStandard  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   isPassed  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   spName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   message  { get; set; }

                  
                                                            
                                                          
public   		string
   submitTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   reportTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   expiryTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   itemUrl  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   itemDesc  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   reportUrl  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   extAttr  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   numIid  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   status  { get; set; }

                  
                                                            
                                                          
public   		string
   pin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   itemSku  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   itemTag  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   itemWashingMark  { get; set; }

                  
                                                                                                                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.market.service.qt.report.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("service_item_code", this.serviceItemCode);
			parameters.Add("qt_code", this.qtCode);
			parameters.Add("qt_name", this.qtName);
			parameters.Add("qt_type", this.qtType);
			parameters.Add("qt_standard", this.qtStandard);
			parameters.Add("is_passed", this.isPassed);
			parameters.Add("sp_name", this.spName);
			parameters.Add("message", this.message);
			parameters.Add("submit_time", this.submitTime);
			parameters.Add("report_time", this.reportTime);
			parameters.Add("expiry_time", this.expiryTime);
			parameters.Add("item_url", this.itemUrl);
			parameters.Add("item_desc", this.itemDesc);
			parameters.Add("report_url", this.reportUrl);
			parameters.Add("ext_attr", this.extAttr);
			parameters.Add("num_iid", this.numIid);
			parameters.Add("status", this.status);
			parameters.Add("pin", this.pin);
			parameters.Add("item_sku", this.itemSku);
			parameters.Add("item_tag", this.itemTag);
			parameters.Add("item_washing_mark", this.itemWashingMark);
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








        
 

