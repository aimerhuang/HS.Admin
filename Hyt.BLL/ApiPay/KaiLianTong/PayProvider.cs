using Hyt.BLL.QianDai.Extends;
using Hyt.BLL.Finance;
using Hyt.Model;
using Hyt.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using Hyt.BLL.ApiPay;
using Hyt.Util;

namespace Hyt.BLL.ApiPay.KaiLianTong
{
    public class PayProvider : IPayProvider
    {
         protected  KaiLianTongConfig config3 = BLL.Config.Config.Instance.KaiLianTongConfig();
         public string url = "http://opsweb.koolyun.cn/customs/mcht_customs_declare.do";
        public override Model.CommonEnum.PayCode Code
        {
            get { return Model.CommonEnum.PayCode.开通联; }
        }

        public override Model.Result ApplyToCustoms(Model.SoOrder order)
        {
            Result result = new Result();
            try
            {
                IList<FnOnlinePayment> list = FinanceBo.Instance.GetOnlinePaymentList(order.SysNo);
                var receiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                FnOnlinePayment payment=null;
                if (list.Count > 0)
                {
                    payment = list[0];
                }
                string param = "";
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("sign_type", "0");
                sParaTemp.Add("service_version", "1.0");
                sParaTemp.Add("input_charset", "UTF-8");
               
                sParaTemp.Add("request_id", payment != null ? payment.BusinessOrderSysNo : order.SysNo.ToString());
                sParaTemp.Add("notify_url", "http://admin.xrcmall.com");
                sParaTemp.Add("mcht_id", config3.MarId);
                sParaTemp.Add("mcht_customs_code", config3.EbcCode);
                sParaTemp.Add("mcht_customs_name", config3.EbcName);
                sParaTemp.Add("currency", "156");
                sParaTemp.Add("amount", ((int)(order.OrderAmount * 100)).ToString());
                sParaTemp.Add("customs_type", config3.CusCode);
                sParaTemp.Add("id_type", "01");
                sParaTemp.Add("id_no", receiveAddress.IDCardNo);
                sParaTemp.Add("id_name", receiveAddress.Name);
                sParaTemp.Add("is_split", "Y");
                sParaTemp.Add("sub_order_no", payment != null ? payment.BusinessOrderSysNo : order.SysNo.ToString());
                sParaTemp.Add("sub_order_time", DateTime.Now.ToString("yyyyMMddHHmmss"));

                foreach (var key in sParaTemp.Keys)
                {
                    if(!string.IsNullOrEmpty(param))
                    {
                        param += "&";
                    }
                    param += key + "=" + sParaTemp[key];
                }

                string md5Key = GetMD5(param + "&key=" + config3.MD5Key);
                param += "&sign_msg=" + md5Key;
                string txt= MyHttp.GetResponse(url, param, "UTF-8");
                return result;
            }
            catch
            {
                result.Status = false;
                return result;
            }
        }
        public string GetMD5(string sDataIn)
        {
            StringBuilder sb = new StringBuilder(32);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(sDataIn));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString().ToUpper();
        }
        public override Model.Result CustomsQuery(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
