//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Hyt.Model;
//using Hyt.Model.LiJiaNew;
//using System.Web.Http;
//using System.Net;
//using Newtonsoft.Json.Linq;
//using System.Web.Mvc;
//using System.IO;

//namespace Hyt.BLL.LiJia
//{

//    /// <summary>
//    /// 利嘉API接口
//    /// </summary>
//    /// <remarks>2017-9-18 杨大鹏 创建</remarks>
//    public class LiJiaNewProvider : BOBase<LiJiaNewProvider>
//    {
//        private static int timestamp = 0;

//        private string sign = "";

//        private readonly static LiJiaConfig config = Config.Config.Instance.GetLiJiaConfig();


//        #region 签名通用方法

//        /// <summary>
//        /// 得到签名
//        /// </summary>
//        /// <returns></returns>
//        public string GetSign(string jiekouUrl)
//        {

//            DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);
//            timestamp = Convert.ToInt32((DateTime.Now - DateStart).TotalSeconds);
//            string qianming = ("post" + config.ApiUrl + jiekouUrl + config.AppSecret + timestamp).ToLower();
//            sign = Extra.Erp.Helper.MD5Encrypt(qianming).ToUpper();
//            return sign;
//        }
//        #endregion


//        #region 利嘉商品增加

//        /// <summary>
//        /// 利嘉商品增加推送
//        /// </summary>
//        /// <param name="lm"></param>
//        /// <returns></returns>
//        public string ProductAdd(LiJiaModel lm)
//        {
//            string jiekouUrl = config.ApiJieKouUrl;
//            string sign = new LiJiaNewProvider().GetSign(jiekouUrl);
//            string Url = config.ApiUrl + config.ApiJieKouUrl + "?" + "appKey=" + config.AppKey + "&" + "sign=" + sign + "&" + "timestamp=" + timestamp;
//            #region json添加字段

//            var dic = new Dictionary<string, string>();
//            dic.Add("FirstCategory", lm.FirstCategory);
//            dic.Add("SecondCategory", lm.SecondCategory);
//            dic.Add("ThirdCategory", lm.ThirdCategory);
//            dic.Add("BrandName", lm.BrandName);
//            dic.Add("ExternallNo", lm.ExternallNo);
//            dic.Add("UPCNo", lm.UPCNo);


//            dic.Add("ProductName", lm.ProductName);
//            dic.Add("MultiBatchFlag", lm.MultiBatchFlag);
//            dic.Add("ValidDate", lm.ValidDate);
//            dic.Add("Origin", lm.Origin);
//            dic.Add("Specific", lm.Specific);
//            dic.Add("PackingQty", lm.PackingQty);

//            dic.Add("Length", lm.Length);
//            dic.Add("Width", lm.Width);
//            dic.Add("Height", lm.Height);
//            dic.Add("DimUnit", lm.DimUnit);
//            dic.Add("ProductDescription", lm.ProductDescription);
//            #endregion
//            var objectJson = JObject.Parse(new Extra.UpGrade.Api.WebUtils().DoPost(Url, dic));
//            return objectJson.ToString();

//        }
//        #endregion


//        #region 利嘉采购单同步

//        /// <summary>
//        /// 采购单同步推送
//        /// </summary>
//        /// <returns></returns>
//        public string GetPrPurchase(LiJiaCaiGouDanOrder order)
//        {

//            string jiekouUrl = config.ApiCaiGouDan;
//            string sign = new LiJiaNewProvider().GetSign(jiekouUrl);
//            string Url = config.ApiUrl + config.ApiCaiGouDan + "?" + "appKey=" + config.AppKey + "&" + "sign=" + sign + "&" + "timestamp=" + timestamp;
//            string data = Hyt.Util.Serialization.JsonUtil.ToJson(order);//序列化json
//            var result = JObject.Parse(Post(Url, data, sign));//post请求
//            Log.LocalLogBo.Instance.WriteReturn("利嘉采购单同步：" + data + "/n处理结果：" + result, "D:/Log/" + DateTime.Now + "");   //本地日志

//            return result.ToString();


//        }

//        #endregion


//        #region Post请求

//        public static string Post(string url, string strData, string auth) //加一个Head参数
//        {
//            try
//            {
//                string result = "";
//                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
//                req.Method = "POST";
//                req.ContentType = "application/json";
//                #region 添加Post 参数
//                byte[] data = Encoding.UTF8.GetBytes(strData);  //json转换为UTF-8格式
//                req.ContentLength = data.Length;  //获取这个字节数组的长度，把长度放到req这个对象里面的。
//                //req.Headers.Add("sign", auth);
//                using (Stream reqStream = req.GetRequestStream())   //节省资源
//                {
//                    reqStream.Write(data, 0, data.Length);   //http协议头
//                    reqStream.Close();
//                }
//                #endregion

//                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
//                Stream stream = resp.GetResponseStream();
//                //获取响应内容
//                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
//                {
//                    result = reader.ReadToEnd();
//                }
//                return result;
//            }
//            catch (WebException ex)
//            {
//                if (ex.Status == WebExceptionStatus.ProtocolError)
//                {

//                    HttpWebResponse err = ex.Response as HttpWebResponse;
//                    if (err != null)
//                    {
//                        string htmRespose = new StreamReader(err.GetResponseStream()).ReadToEnd();
//                        string msg = string.Format("{0},{1}", err.StatusDescription, htmRespose);
//                    }

//                }


//                return ex.Message;
//            }

//        }

//        #endregion

//        #region 往来单位接口
//        /// <summary>
//        /// 新增往来单位
//        /// </summary>
//        /// <param name="lp"></param>
//        /// <returns>string</returns>
//        public string PartnerAdd(LiJiaPartner lp)
//        {
//            string jiekouUrl = config.ApiParnter;
//            string sign = new LiJiaNewProvider().GetSign(jiekouUrl);
//            string Url = config.ApiUrl + config.ApiParnter + "?" + "appKey=" + config.AppKey + "&" + "sign=" + sign + "&" + "timestamp=" + timestamp;
//            var dic = new Dictionary<string, string>();
//            #region 字典添加字段

//            dic.Add("PartnerName", lp.PartnerName);
//            dic.Add("PartnerType", lp.PartnerType);
//            dic.Add("ShortTitle", lp.ShortTitle);
//            dic.Add("Country", lp.Country);
//            dic.Add("Province", lp.Province);
//            dic.Add("City", lp.City);
//            dic.Add("District", lp.District);
//            dic.Add("ZipCode", lp.ZipCode);
//            dic.Add("StreetLine", lp.StreetLine);
//            dic.Add("ContactName", lp.ContactName);
//            dic.Add("PhoneNumber", lp.PhoneNumber);
//            dic.Add("EMail", lp.EMail);
//            dic.Add("BankName", lp.BankName);
//            dic.Add("BankAccount", lp.BankAccount);
//            dic.Add("Remark", lp.Remark);
//            #endregion
//            var objectJson = JObject.Parse(new Extra.UpGrade.Api.WebUtils().DoPost(Url, dic));

//            return objectJson.ToString();
//        }
//        #endregion

//        #region 采购退货单接口

//        /// <summary>
//        /// 采购退货单接口
//        /// </summary>
//        /// <returns>接口处理结果json</returns>
//        public string PurchaseAddReturn(LiJiaCaiGouDanOrder order)
//        {
//            string jiekouUrl = config.ApiPurchase;
//            string sign = new LiJiaNewProvider().GetSign(jiekouUrl);
//            string Url = config.ApiUrl + config.ApiPurchase + "?" + "appKey=" + config.AppKey + "&" + "sign=" + sign + "&" + "timestamp=" + timestamp;
//            string data = Hyt.Util.Serialization.JsonUtil.ToJson(order);//序列化json
//            string result = Post(Url, data, sign);
//            Log.LocalLogBo.Instance.WriteReturn("利嘉退货单同步：" + data + "/n处理结果：" + result, "D:/Log/" + DateTime.Now + "");   //本地日志
//            return result;
//        }

//        #endregion

//        #region 销售单同步
//        /// <summary>
//        /// 销售单同步
//        /// </summary>
//        /// <param name="sales">销售单实体</param>
//        /// <returns>接口处理结果json</returns>
//        public string SalesAdd(LiJiaSales sales)
//        {

//            string jiekouUrl = config.ApiSales;
//            string sign = new LiJiaNewProvider().GetSign(jiekouUrl);
//            string Url = config.ApiUrl + config.ApiSales + "?" + "appKey=" + config.AppKey + "&" + "sign=" + sign + "&" + "timestamp=" + timestamp;
//            string data = Hyt.Util.Serialization.JsonUtil.ToJson(sales);//序列化json
//            string result = Post(Url, data, sign);//post请求
//            Log.LocalLogBo.Instance.WriteReturn("利嘉销售单：" + data + "/n处理结果：" + result, "D:/Log/" + DateTime.Now + "");   //本地日志
//            return result;
//        }

//        #endregion

//        #region 销售退货单
//        /// <summary>
//        ///销售退货单
//        /// </summary>
//        /// <param name="sales">销售单实体</param>
//        /// <returns>接口处理结果json</returns>
//        public string SalesReturn(LiJiaSales sales)
//        {
//            string jiekouUrl = config.ApiSalesReturn;
//            string sign = new LiJiaNewProvider().GetSign(jiekouUrl);
//            string Url = config.ApiUrl + config.ApiSalesReturn + "?" + "appKey=" + config.AppKey + "&" + "sign=" + sign + "&" + "timestamp=" + timestamp;
//            string data = Hyt.Util.Serialization.JsonUtil.ToJson(sales);//序列化json
//            string result = Post(Url, data, sign);
//            Log.LocalLogBo.Instance.WriteReturn("利嘉销售退货单同步：" + data + "/n处理结果：" + result, "D:/Log/" + DateTime.Now + "");   //本地日志
//            return result;
//        }

//        #endregion


//    }
//}
