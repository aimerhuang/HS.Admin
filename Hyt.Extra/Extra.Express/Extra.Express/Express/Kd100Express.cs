using Extra.Express.Provider;
using Extra.Express.Public;
using Hyt.BLL.Config;
using Hyt.Model.ExpressList;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Extra.Express.Express
{
   public class Kd100Express : IKd100Express
    {
        #region 快递100电子面单 2017-12-13 廖移凤
        /// <summary>
        /// 快递100电子面单
        /// </summary>
        /// <param name="pms">参数</param>
        /// <returns></returns>
        /// <remarks> 2017-12-13 廖移凤</remarks>
        public override KdOrderNums OrderTracesSubByJson(KdOrderParam pms)
        {
            string pm = SendData.ObjectToJson(pms);
            long epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;//时间戳
            Dictionary<string, string> param = new Dictionary<string, string>();
            string dataSign = SendData.Encrypt(pm, epoch, Config.Kd100.Key, Config.Kd100.Secret, "UTF-8");
            param.Add("sign", HttpUtility.UrlEncode(dataSign, Encoding.UTF8));
            param.Add("key", Config.Kd100.Key);
            param.Add("t", epoch.ToString());
            param.Add("param", pm);
            string result = SendData.SendPost(Config.Kd100.ReqURL, param);
            JObject jo = (JObject)JsonConvert.DeserializeObject(result);//解析JSON字符串
            var result1 = jo["data"];
            var templateurl = result1[0]["templateurl"];
            KdOrderNums kn = new KdOrderNums()
            {
                destCode = result1[0]["destCode"].ToString(),
                destSortingCode = result1[0]["destSortingCode"].ToString(),
                expressCode = result1[0]["expressCode"].ToString(),
                kdOrderNum = result1[0]["kdOrderNum"].ToString(),
                kuaidinum = result1[0]["kuaidinum"].ToString(),
                payaccount = result1[0]["payaccount"].ToString()
            };
            if (templateurl != null)
            {
                kn.templateurl = templateurl.ToString();
            }
            return kn;
        }
        #endregion

    }
}
