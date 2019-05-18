using System;
using System.Collections.Generic;
using Extra.UpGrade.SDK.JingDong.Response;
using Extra.UpGrade.SDK.JingDong.Util;

namespace Extra.UpGrade.SDK.JingDong.Request
{
    public class OverseasOrderSopOutstorageRequest : IJdRequest<OverseasOrderSopOutstorageResponse>
    {

        public string orderId { get; set; }

        public string tradeNo { get; set; }


        private IDictionary<string, string> otherParameters;

        public string GetApiName()
        {
            return "360buy.overseas.order.sop.outstorage";
        }

        public IDictionary<string, string> GetParameters()
        {
            JdDictionary parameters = new JdDictionary();
            parameters.Add("order_id", this.orderId);
            parameters.Add("trade_no", this.tradeNo);
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











