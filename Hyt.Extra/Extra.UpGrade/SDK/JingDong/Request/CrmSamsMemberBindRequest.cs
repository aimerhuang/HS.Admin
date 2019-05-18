using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class CrmSamsMemberBindRequest : IJdRequest<CrmSamsMemberBindResponse>
{
		                                                                                                                                                                                                                                                                                                                                                                        		public  		string
   samCardNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   cardHolderNbr  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   cardHolderType  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   chTypeShortDesc  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   cardStatCd  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   currStatusCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   memberCode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   startDate  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   expireDate  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   fullName  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   birthDate  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   phoneNbr  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   phoneNbr2  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   emailAddress  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   certType  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   ctzidNbr  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   driverNbr  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   passportNbr  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   crmCardNo  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   crmPin  { get; set; }
                                                                                                                                      
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.crm.sams.member.bind";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("samCardNo", this.samCardNo);
			parameters.Add("cardHolderNbr", this.cardHolderNbr);
			parameters.Add("cardHolderType", this.cardHolderType);
			parameters.Add("chTypeShortDesc", this.chTypeShortDesc);
			parameters.Add("cardStatCd", this.cardStatCd);
			parameters.Add("currStatusCode", this.currStatusCode);
			parameters.Add("memberCode", this.memberCode);
			parameters.Add("startDate", this.startDate);
			parameters.Add("expireDate", this.expireDate);
			parameters.Add("fullName", this.fullName);
			parameters.Add("birthDate", this.birthDate);
			parameters.Add("phoneNbr", this.phoneNbr);
			parameters.Add("phoneNbr2", this.phoneNbr2);
			parameters.Add("emailAddress", this.emailAddress);
			parameters.Add("certType", this.certType);
			parameters.Add("ctzidNbr", this.ctzidNbr);
			parameters.Add("driverNbr", this.driverNbr);
			parameters.Add("passportNbr", this.passportNbr);
			parameters.Add("crmCardNo", this.crmCardNo);
			parameters.Add("crmPin", this.crmPin);
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








        
 

