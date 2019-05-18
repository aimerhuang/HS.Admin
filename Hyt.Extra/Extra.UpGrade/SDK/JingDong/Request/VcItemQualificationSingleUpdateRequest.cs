using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class VcItemQualificationSingleUpdateRequest : IJdRequest<VcItemQualificationSingleUpdateResponse>
{
		                                                                                                       
public   		string
   applyId  { get; set; }

                  
                                                                                                                                    
                                                                                                                                                                                                                                                                                                                                                            		public  		string
   type  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   applicant  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   qcCode  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   endDate  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         		public  		string
   fileKeyList  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.vc.item.qualification.single.update";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("apply_id", this.applyId);
			parameters.Add("type", this.type);
			parameters.Add("applicant", this.applicant);
			parameters.Add("qc_code", this.qcCode);
			parameters.Add("end_date", this.endDate);
			parameters.Add("file_key_list", this.fileKeyList);
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








        
 

