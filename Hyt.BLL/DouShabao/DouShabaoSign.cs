
using Hyt.Model;
using Hyt.Model.DouShabaoModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Hyt.BLL.DouShabao
{
    /// <summary>
    /// 豆沙包接口实现
    /// </summary>
    /// <remarks>2017-07-07 罗熙 创建</remarks>
    public class DouShabaoSign : BOBase<DouShabaoSign>
    {
        //获取签名
        public string GetSign(string orderNo, string totalAmount, string buyerName, string buyerMobile)
        {
            DouShabaoConfig config = Config.Config.Instance.GetDouShabaoConfig();
            string source = config.Source;
            string key = config.Key;
            var sign = "orderNo=" + orderNo + "&totalAmount=" + totalAmount + "&buyerName=" + buyerName + "&buyerMobile=" + buyerMobile + "&source=" + source + "&key=" + key;
            sign = Encrypt(sign).ToUpper();  //转大写
            return sign;
        }
        //MD5加密，UTF8字符集
        public static string Encrypt(string value)
        {
            var md5 = new MD5CryptoServiceProvider();
            var fromData = Encoding.UTF8.GetBytes(value);
            var targetData = md5.ComputeHash(fromData);
            var byte2String = targetData.Aggregate("", (current, t) => current + t.ToString("x2"));
            return byte2String.ToLower();
        }

        public Resuldt DoushabaoRealize(string orderNo, string expressChannel, string totalAmount, string buyerName, string buyerMobile, string idCard, string expressNo, string transitLine, string orderTime, double totalWeight, string sign, string purchasOrderNo, string purchasOrderAmount, string purchasOrderTime, ProductList productlist)
        {
            DouShabaoConfig config = Config.Config.Instance.GetDouShabaoConfig();
            string source = config.Source;
            string TotalWeight = totalWeight.ToString();
            string url = config.ApiUrl;      //ApiUrlTest测试路径,ApiUrl正式路径
            string Productlist = "[{\"name\": \"" + productlist.ProductName + "\",\"category\": \"" + productlist.ProductCategory + "\",\"num\": " + productlist.ProductNum + ",\"price\": " + productlist.ProductPrice + ",\"brand\": \"" + productlist.ProductBrand + "\"}]";
            string InsuranceProductList = "[{\"productId\": \"67\",\"insuranceAmount\": \"1000\",\"period\":\"60\",\"premium\": \"1\"}]";   //保险费暂定1元  67境内发货1000最高赔款金额  68国外发货400最高赔款金额  
            string data = "{\"orderNo\":\"" + orderNo + "\",\"expressChannel\":\"" + expressChannel + "\",\"totalAmount\":\"" + totalAmount + "\",\"buyerName\":\"" + buyerName + "\",\"buyerMobile\":\"" + buyerMobile + "\",\"idCard\": \"" + idCard + "\",\"expressNo\": \"" + expressNo + "\",\"transitLine\": \"EMS清关路线\",\"orderTime\": \"" + orderTime + "\",\"expressTime\": \"" + purchasOrderTime + "\",\"isReinforce\": false, \"prescription\": 1,\"totalWeight\": " + TotalWeight + ",\"source\": \"" + source + "\",\"sign\": \"" + sign + "\",\"purchasOrderNo\": \"" + purchasOrderNo + "\",\"shoppingSite\": \"信营\",\"purchasOrderAmount\": \"" + purchasOrderAmount + "\",\"destinationCity\": \"" + productlist.DestinationCity + "\",\"purchasOrderTime\": \"" + purchasOrderTime + "\",\"productList\":" + Productlist + ",\"insuranceProductList\":" + InsuranceProductList + ",\"purchasOrderNo\":\"" + purchasOrderNo + "\"}";
            string retData = Post(url, data, sign);  //接口处理
            Log.LocalLogBo.Instance.WriteReturn("豆沙包数据：" + data + "/n处理结果：" + retData, "D:/Log/" + DateTime.Now + "");   //本地日志
            return null;
        }

        //接口处理
        public static string Post(string url, string strData, string auth) //加一个Head参数
        {
            try
            {
                string result = "";
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";

                #region 添加Post 参数
                byte[] data = Encoding.UTF8.GetBytes(strData);  //json转换为UTF-8格式
                req.ContentLength = data.Length;  //获取这个字节数组的长度，把长度放到req这个对象里面的。
                //req.Headers.Add("sign", auth);
                using (Stream reqStream = req.GetRequestStream())   //节省资源
                {
                    reqStream.Write(data, 0, data.Length);   //http协议头
                    reqStream.Close();
                }
                #endregion

                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();  
                }
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message; 
            }

        }
    }
}
