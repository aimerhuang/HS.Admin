using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptWarecenterWareAddRequest : IJdRequest<EptWarecenterWareAddResponse>
{
		                                                                                                                                  
public   		Nullable<int>
   categoryId  { get; set; }

                  
                                                            
                                                                                           
public   		Nullable<int>
   wareStatus  { get; set; }

                  
                                                            
                                                          
public   		string
   title  { get; set; }

                  
                                                            
                                                          
public   		string
   rfId  { get; set; }

                  
                                                            
                                                          
public   		string
   itemNum  { get; set; }

                  
                                                            
                                                          
public   		string
   hsCode  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   transportId  { get; set; }

                  
                                                            
                                                          
public   		string
   attributes  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   SupplyPrice  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   amountCount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   lockCount  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   lockStartTime  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   lockEndTime  { get; set; }

                  
                                                            
                                                          
public   		string
   imgByte  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   recommendTpid  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   customTpid  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   brandId  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   deliveryDays  { get; set; }

                  
                                                            
                                                          
public   		string
   keywords  { get; set; }

                  
                                                            
                                                          
public   		string
   description  { get; set; }

                  
                                                            
                                                          
public   		string
   packInfo  { get; set; }

                  
                                                            
                                                          
public   		string
   netWeight  { get; set; }

                  
                                                            
                                                          
public   		string
   weight  { get; set; }

                  
                                                            
                                                          
public   		string
   packLong  { get; set; }

                  
                                                            
                                                          
public   		string
   packWide  { get; set; }

                  
                                                            
                                                          
public   		string
   packHeight  { get; set; }

                  
                                                            
                                                          
public   		string
   upc  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.warecenter.ware.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("categoryId", this.categoryId);
			parameters.Add("wareStatus", this.wareStatus);
			parameters.Add("title", this.title);
			parameters.Add("rfId", this.rfId);
			parameters.Add("itemNum", this.itemNum);
			parameters.Add("hsCode", this.hsCode);
			parameters.Add("transportId", this.transportId);
			parameters.Add("attributes", this.attributes);
			parameters.Add("SupplyPrice", this.SupplyPrice);
			parameters.Add("amountCount", this.amountCount);
			parameters.Add("lockCount", this.lockCount);
			parameters.Add("lockStartTime", this.lockStartTime);
			parameters.Add("lockEndTime", this.lockEndTime);
			parameters.Add("imgByte", this.imgByte);
			parameters.Add("recommendTpid", this.recommendTpid);
			parameters.Add("customTpid", this.customTpid);
			parameters.Add("brandId", this.brandId);
			parameters.Add("deliveryDays", this.deliveryDays);
			parameters.Add("keywords", this.keywords);
			parameters.Add("description", this.description);
			parameters.Add("packInfo", this.packInfo);
			parameters.Add("netWeight", this.netWeight);
			parameters.Add("weight", this.weight);
			parameters.Add("packLong", this.packLong);
			parameters.Add("packWide", this.packWide);
			parameters.Add("packHeight", this.packHeight);
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








        
 

