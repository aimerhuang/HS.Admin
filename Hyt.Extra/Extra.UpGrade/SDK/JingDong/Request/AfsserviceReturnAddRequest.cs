using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class AfsserviceReturnAddRequest : IJdRequest<AfsserviceReturnAddResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   shipWay  { get; set; }

                  
                                                            
                                                          
public   		string
   shipWayName  { get; set; }

                  
                                                            
                                                          
public   		string
   waybill  { get; set; }

                  
                                                            
                                                          
public   		string
   partCodes  { get; set; }

                  
                                                            
                                                                                                                                                       
public   		Nullable<int>
   afsServiceId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   province  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   city  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   county  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   village  { get; set; }

                  
                                                            
                                                          
public   		string
   address  { get; set; }

                  
                                                            
                                                          
public   		string
   zipCode  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneeName  { get; set; }

                  
                                                            
                                                          
public   		string
   consigneeTel  { get; set; }

                  
                                                            
                                                          
public   		string
   applayRemark  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.afsservice.return.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("shipWay", this.shipWay);
			parameters.Add("shipWayName", this.shipWayName);
			parameters.Add("waybill", this.waybill);
			parameters.Add("partCodes", this.partCodes);
			parameters.Add("afsServiceId", this.afsServiceId);
			parameters.Add("province", this.province);
			parameters.Add("city", this.city);
			parameters.Add("county", this.county);
			parameters.Add("village", this.village);
			parameters.Add("address", this.address);
			parameters.Add("zipCode", this.zipCode);
			parameters.Add("consigneeName", this.consigneeName);
			parameters.Add("consigneeTel", this.consigneeTel);
			parameters.Add("applayRemark", this.applayRemark);
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








        
 

