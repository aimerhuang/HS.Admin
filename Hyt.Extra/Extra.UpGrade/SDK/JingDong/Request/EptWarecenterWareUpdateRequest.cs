using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptWarecenterWareUpdateRequest : IJdRequest<EptWarecenterWareUpdateResponse>
{
		                                                                                                                                  
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                            
                                                                                           
public   		string
   title  { get; set; }

                  
                                                            
                                                          
public   		string
   itemNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<long>
   transportId  { get; set; }

                  
                                                            
                                                          
public   		string
   attributes  { get; set; }

                  
                                                            
                                                          
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
			return "jingdong.ept.warecenter.ware.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("wareId", this.wareId);
			parameters.Add("title", this.title);
			parameters.Add("itemNum", this.itemNum);
			parameters.Add("transportId", this.transportId);
			parameters.Add("attributes", this.attributes);
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








        
 

