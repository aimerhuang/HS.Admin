using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class ServicePromotionWxsqGetCodeBySubUnionIdRequest : IJdRequest<ServicePromotionWxsqGetCodeBySubUnionIdResponse>
{
		                                                                                                                                                                   
public   		string
   proCont  { get; set; }

                  
                                                            
                                                                                                                                                                                                                                                                                                                                                           		public  		string
   materialIds  { get; set; }
                                                                                                                                                                                                
public   		string
   subUnionId  { get; set; }

                  
                                                            
                                                          
public   		string
   positionId  { get; set; }

                  
                                                            
                                 
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.service.promotion.wxsq.getCodeBySubUnionId";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("proCont", this.proCont);
			parameters.Add("materialIds", this.materialIds);
			parameters.Add("subUnionId", this.subUnionId);
			parameters.Add("positionId", this.positionId);
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








        
 

