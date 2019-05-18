using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class AfsReturnServiceProviderCreateReturnApplyRequest : IJdRequest<AfsReturnServiceProviderCreateReturnApplyResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   afsServiceId  { get; set; }

                  
                                                            
                                                          
public   		string
   buId  { get; set; }

                  
                                                            
                                                                                                                      
public   		string
   operatorPin  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorNick  { get; set; }

                  
                                                            
                                                          
public   		string
   operatorRemark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   operatorDate  { get; set; }

                  
                                                            
                                                          
public   		string
   platformSrc  { get; set; }

                  
                                                            
                                                                                           
public   		string
   consigneeName  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneeTel  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   provinceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   cityId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   countyId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   villageId  { get; set; }

                  
                                                            
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   repairState  { get; set; }

                  
                                                            
                                                          
public   		string
   applayRemark  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   shipWay  { get; set; }

                  
                                                            
                                                          
public   		string
   shipWayName  { get; set; }

                  
                                                            
                                                          
public   		string
   waybill  { get; set; }

                  
                                                            
                                                          
public   		string
   partCodes  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.AfsReturnServiceProvider.createReturnApply";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("afsServiceId", this.afsServiceId);
			parameters.Add("buId", this.buId);
			parameters.Add("operatorPin", this.operatorPin);
			parameters.Add("operatorNick", this.operatorNick);
			parameters.Add("operatorRemark", this.operatorRemark);
			parameters.Add("operatorDate", this.operatorDate);
			parameters.Add("platformSrc", this.platformSrc);
			parameters.Add("consigneeName", this.consigneeName);
			parameters.Add("consigneeTel", this.consigneeTel);
			parameters.Add("provinceId", this.provinceId);
			parameters.Add("cityId", this.cityId);
			parameters.Add("countyId", this.countyId);
			parameters.Add("villageId", this.villageId);
			parameters.Add("address", this.address);
			parameters.Add("repairState", this.repairState);
			parameters.Add("applayRemark", this.applayRemark);
			parameters.Add("shipWay", this.shipWay);
			parameters.Add("shipWayName", this.shipWayName);
			parameters.Add("waybill", this.waybill);
			parameters.Add("partCodes", this.partCodes);
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








        
 

