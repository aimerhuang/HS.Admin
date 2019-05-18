using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VssPromotionQueryunitpromopagelistRequest : IJdRequest<VssPromotionQueryunitpromopagelistResponse>
{
		                                                                                                                                                                   
public   		Nullable<long>
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   promoId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   promoName  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   createTimeBegin  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   createTimeEnd  { get; set; }

                  
                                                                                                                                                            
                                                          
public   		Nullable<DateTime>
   beginTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<DateTime>
   endTime  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   promoState  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   auditState  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageIndex  { get; set; }

                  
                                                                                                                                    
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                                                                                                    
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vss.promotion.queryunitpromopagelist";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("ware_id", this.wareId);
			parameters.Add("promo_id", this.promoId);
			parameters.Add("promo_name", this.promoName);
			parameters.Add("create_time_begin", this.createTimeBegin);
			parameters.Add("create_time_end", this.createTimeEnd);
			parameters.Add("begin_time", this.beginTime);
			parameters.Add("end_time", this.endTime);
			parameters.Add("promo_state", this.promoState);
			parameters.Add("audit_state", this.auditState);
			parameters.Add("page_index", this.pageIndex);
			parameters.Add("page_size", this.pageSize);
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








        
 

