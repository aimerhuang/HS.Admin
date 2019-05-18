using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionPropAddRequest : IJdRequest<SellerPromotionPropAddResponse>
{
		                                                                                                                                                                                                          		public  		Nullable<long>
   promoId  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   type  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   num  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   usedWay  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.prop.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("promo_id", this.promoId);
			parameters.Add("type", this.type);
			parameters.Add("num", this.num);
			parameters.Add("used_way", this.usedWay);
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








        
 

