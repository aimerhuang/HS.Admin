using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EptOrderGetorderIdsbyqueryRequest : IJdRequest<EptOrderGetorderIdsbyqueryResponse>
{
		                                                                                                                                                                   
public   		Nullable<int>
   orderStatus  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   locked  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   disputed  { get; set; }

                  
                                                            
                                                          
public   		string
   bookTimeBegin  { get; set; }

                  
                                                            
                                                          
public   		string
   bookTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		string
   userPin  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   startRow  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.ept.order.getorderIdsbyquery";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orderStatus", this.orderStatus);
			parameters.Add("locked", this.locked);
			parameters.Add("disputed", this.disputed);
			parameters.Add("bookTimeBegin", this.bookTimeBegin);
			parameters.Add("bookTimeEnd", this.bookTimeEnd);
			parameters.Add("userPin", this.userPin);
			parameters.Add("startRow", this.startRow);
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








        
 

