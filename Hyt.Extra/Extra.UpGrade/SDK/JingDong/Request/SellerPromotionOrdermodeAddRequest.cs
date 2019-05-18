using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionOrdermodeAddRequest : IJdRequest<SellerPromotionOrdermodeAddResponse>
{
		                                                                                                                                                                                                          		public  		Nullable<long>
   promoId  { get; set; }
                                                                                                                                                           		public  		Nullable<int>
   favorMode  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   quota  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   rate  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   plus  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   minus  { get; set; }
                                                                                                                                                           		public  		string
   link  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.ordermode.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("promo_id", this.promoId);
			parameters.Add("favor_mode", this.favorMode);
			parameters.Add("quota", this.quota);
			parameters.Add("rate", this.rate);
			parameters.Add("plus", this.plus);
			parameters.Add("minus", this.minus);
			parameters.Add("link", this.link);
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








        
 

