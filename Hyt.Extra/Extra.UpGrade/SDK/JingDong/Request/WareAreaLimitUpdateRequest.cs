using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class WareAreaLimitUpdateRequest : IJdRequest<WareAreaLimitUpdateResponse>
{
		                                                                                                                                  
public   		string
   levs  { get; set; }

                  
                                                            
                                                          
public   		string
   areaIds  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   areaFids  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   wareId  { get; set; }

                  
                                                                                                                                    
                                                          
public   		string
   type  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "360buy.ware.area.limit.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("levs", this.levs);
			parameters.Add("area_ids", this.areaIds);
			parameters.Add("area_fids", this.areaFids);
			parameters.Add("ware_id", this.wareId);
			parameters.Add("type", this.type);
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








        
 

