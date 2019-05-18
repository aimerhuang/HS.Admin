using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class EdiSpiMidandsmGetRequest : IJdRequest<EdiSpiMidandsmGetResponse>
{
		                                                                                                                                  
public   		string
   orgId  { get; set; }

                  
                                                            
                                                          
public   		string
   warehouseCode  { get; set; }

                  
                                                            
                                                          
public   		string
   jdSku  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   inTimeStart  { get; set; }

                  
                                                            
                                                          
public   		Nullable<DateTime>
   inTimeEnd  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageNum  { get; set; }

                  
                                                            
                                                          
public   		Nullable<int>
   pageSize  { get; set; }

                  
                                                            
                                                                  
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.edi.spi.midandsm.get";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("orgId", this.orgId);
			parameters.Add("warehouseCode", this.warehouseCode);
			parameters.Add("jdSku", this.jdSku);
			parameters.Add("inTimeStart", this.inTimeStart);
			parameters.Add("inTimeEnd", this.inTimeEnd);
			parameters.Add("pageNum", this.pageNum);
			parameters.Add("pageSize", this.pageSize);
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








        
 

