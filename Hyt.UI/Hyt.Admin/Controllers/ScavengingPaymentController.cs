using Extra.Express.Model;
using Extra.Express.Provider;
using Hyt.BLL.Convergence;
using Hyt.BLL.YTO;
using Hyt.Model.Convergence;
using Hyt.Model.TYO;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 汇聚支付
    /// </summary>
    /// <remarks>2017-12-18 廖移凤 创建</remarks>
    public class ScavengingPaymentController : Controller
    {
        //
        // GET: /ScavengingPayment/

        public ActionResult ScavengingPayment()
        {
            return View();
        }
        /// <summary>
        /// 汇聚扫码支付
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-12-21 廖移凤</remarks>
        public ActionResult AjaxScanPayment(int SysNo )  {

            var SP = ScanParamBo.Instance.GetScanParam(SysNo);
            int pd=0;
            if (SP == null) {
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            ScanParam param = new ScanParam();
            #region 扫码支付 测试参数
            param.P2_OrderNo = SysNo +"_"+ DateTime.Now.ToString("yyyyMMddhhmm");
            param.P3_Amount = SP.P3_Amount;

            param.P5_ProductName = SP.P5_ProductName.Replace("&","_");
            param.P0_Version = "1.0";
            param.P1_MerchantNo = "888100050941685";
            param.P4_Cur = 1;
            param.P9_NotifyUrl = "http://testapi.com/pay/wx_call_back";
            if ("微信支付".Equals(SP.Q1_FrpCode))
            {
                param.Q1_FrpCode = "WEIXIN_NATIVE";
                pd=1;
            }
            else if ("支付宝".Equals(SP.Q1_FrpCode))
            {
                param.Q1_FrpCode = "ALIPAY_NATIVE";
                pd = 2;
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

            param.Q4_IsShowPic = 1;
            #endregion
            var json = ScanPayment(param);//支付
            var data = new { json = json, pd = pd };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 汇聚推送海关
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-12-21 廖移凤</remarks>
        public ActionResult AjaxPush()
        {
            PushParam param = new PushParam();
            #region 推送海关 测试参数
            param.Pc09_PayerName = "廖移凤";
            param.Pc11_PayerIdNo = "430523199501298014";
            param.Pc12_PayerTel = "13265552415";

            param.Pc01_MerchantNo = "888100050941685";
            param.Pc02_OrderNo = "405_201712211649";
            param.Pc03_CustomsCode = "1000";
            param.Pc04_FunctionCode = "CUS";
            param.Pc06_DomainName = "http://testapi.com/pay/wx_call_back";
            param.Pc07_TmallCode = "123456";
            param.Pc08_TmallName = "测试";
            param.Pc10_PayerIdType = 1;
            #endregion
            var json = Push(param);//推送
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        #region 汇聚扫码支付接口
        /// <summary>
        /// 汇聚扫码支付 
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>json</returns>
        /// <remarks> 2017-12-18 廖移凤</remarks>
        public static string ScanPayment(ScanParam param)
        {
            // string secret = "3f67fd97162d20e6fe27748b5b372509";//888100000002340测试密钥
            string secret = "68d13cf26c4b4f4f932e3eff990093ba";//888100050941685测试密钥
            string url = "https://www.joinpay.com/trade/uniPayApi.action";//请求地址
            StringBuilder sb = new StringBuilder();//签名字符串+密钥
            sb.Append(param.P0_Version);
            sb.Append(param.P1_MerchantNo);
            sb.Append(param.P2_OrderNo);
            sb.Append(param.P3_Amount);
            sb.Append(param.P4_Cur);
            sb.Append(param.P5_ProductName);
            sb.Append(param.P9_NotifyUrl);
            sb.Append(param.Q1_FrpCode);
            sb.Append(param.Q4_IsShowPic);
            string request = url + "?p0_Version=" + param.P0_Version + "&p1_MerchantNo=" + param.P1_MerchantNo + "&p2_OrderNo=" + param.P2_OrderNo + "&p3_Amount=" + param.P3_Amount + "&p4_Cur=" + param.P4_Cur + "&p5_ProductName=" +
                        param.P5_ProductName + "&p9_NotifyUrl=" + param.P9_NotifyUrl + "&q1_FrpCode=" + param.Q1_FrpCode + "&q4_IsShowPic=" + param.Q4_IsShowPic + "&hmac=" + md5hex(sb.ToString() + secret);
            string result = Get(request);


           
            return result;
        } 
        #endregion

        #region 汇聚推送海关接口
        /// <summary>
        /// 汇聚推送海关
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns>json</returns>
        /// <remarks> 2017-12-21 廖移凤</remarks>
        public static string Push(PushParam param)
        {

            string result = "";
            string secret = "68d13cf26c4b4f4f932e3eff990093ba";
            string url = "https://www.joinpay.com/trade/pushedApi.action";
            StringBuilder sb = new StringBuilder();
            sb.Append(param.Pc01_MerchantNo);
            sb.Append(param.Pc02_OrderNo);
            sb.Append(param.Pc03_CustomsCode);
            sb.Append(param.Pc04_FunctionCode);
            sb.Append(param.Pc06_DomainName);
            sb.Append(param.Pc07_TmallCode);
            sb.Append(param.Pc08_TmallName);
            sb.Append(param.Pc09_PayerName);
            sb.Append(param.Pc10_PayerIdType);
            sb.Append(param.Pc11_PayerIdNo);
            sb.Append(param.Pc12_PayerTel);
            string requesturl = url + "?pc01_MerchantNo=" + param.Pc01_MerchantNo + "&pc02_OrderNo=" + param.Pc02_OrderNo + "&pc03_CustomsCode=" + param.Pc03_CustomsCode + "&pc04_FunctionCode=" + param.Pc04_FunctionCode + "&pc06_DomainName=" + param.Pc06_DomainName + "&pc07_TmallCode=" +
                param.Pc07_TmallCode + "&pc08_TmallName=" + param.Pc08_TmallName + "&pc09_PayerName=" + param.Pc09_PayerName + "&pc10_PayerIdType=" + param.Pc10_PayerIdType + "&pc11_PayerIdNo=" + param.Pc11_PayerIdNo + "&pc12_PayerTel=" + param.Pc12_PayerTel + "&hmac=" + md5hex(sb.ToString() + secret);
            result = Get(requesturl);
            return result;
        } 
        #endregion

        #region 公用
        /// <summary>
        /// md5
        /// </summary>
        /// <param name="password"></param>
        /// <returns>JSON字符串</returns>d
        ///  <remarks> 2017-12-18 廖移凤</remarks>
        public static string md5hex(string json)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            bs = x.ComputeHash(bs);
            StringBuilder s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            json = s.ToString();
            return json;
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string Get(string Url)
        {
            //请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            //响应
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
            return retString;
        } 
        #endregion



    }
}
