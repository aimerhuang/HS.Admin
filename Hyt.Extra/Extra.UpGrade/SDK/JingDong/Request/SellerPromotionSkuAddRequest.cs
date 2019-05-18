using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
public class SellerPromotionSkuAddRequest : IJdRequest<SellerPromotionSkuAddResponse>
{
		                                                                                                                                                                                                          		public  		Nullable<long>
   promoId  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   skuIds  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   jdPrices  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   promoPrices  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   seq  { get; set; }
                                                                                                                                                                                                                                                                                                                         		public  		string
   num  { get; set; }
                                                                                                                                                                                                                                                                                                                                                                                                                 		public  		string
   bindType  { get; set; }
                                                                                                                                                                       
		private IDictionary<string, string> otherParameters;

		public string GetApiName()
        {
			return "jingdong.seller.promotion.sku.add";
        }

		public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
			parameters.Add("promo_id", this.promoId);
			parameters.Add("sku_ids", this.skuIds);
			parameters.Add("jd_prices", this.jdPrices);
			parameters.Add("promo_prices", this.promoPrices);
			parameters.Add("seq", this.seq);
			parameters.Add("num", this.num);
			parameters.Add("bind_type", this.bindType);
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








        
 

