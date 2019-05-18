using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Extra.AliF2F
{
    public class Config
    {
        /*****************************生产环境的测试数据*****************************/
        public static string alipay_public_key = System.AppDomain.CurrentDomain.BaseDirectory + "/RSA/alipay_rsa_public_key.pem";
        //这里要配置没有经过PKCS8转换的原始私钥
        public static string merchant_private_key = System.AppDomain.CurrentDomain.BaseDirectory + "/RSA/rsa_private_key.pem";
        public static string merchant_public_key = System.AppDomain.CurrentDomain.BaseDirectory + "/RSA/rsa_public_key.pem";
        public static string appId = "2016092101938516";//"2016060501483725";
        public static string serverUrl = "https://openapi.alipay.com/gateway.do";
        public static string mapiUrl = "https://mapi.alipay.com/gateway.do";
        public static string monitorUrl = "http://mcloudmonitor.com/gateway.do";
        public static string pid = "2088221899598360";//"2088221193698609";
        /*****************************生产环境的测试数据*****************************/



        /*****************************开发环境的测试数据*****************************
              public static string     alipay_public_key = HttpRuntime.AppDomainAppPath.ToString() + "Demo\\alipay_rsa_public_key_dev.pem";
                   //这里要配置没有经过PKCS8转换的原始私钥
              public static string     merchant_private_key = HttpRuntime.AppDomainAppPath.ToString() + "Demo\\rsa_private_key_dev.pem";
              public static string     merchant_public_key = HttpRuntime.AppDomainAppPath.ToString() + "Demo\\rsa_public_key_dev.pem";
              public static string     appId = "2015042200550512";
              public static string     serverUrl = "http://openapi.d7165.alipay.net/gateway.do";
              public static string     mapiUrl = "http://mapi.stable.alipay.net/gateway.do";
              public static string     pid = "2088102146891244";
         *****************************开发环境的测试数据*****************************/


        public static string charset = "utf-8";//"utf-8";
        public static string sign_type = "RSA";
        public static string version = "1.0";
        public Config()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        public static string getMerchantPublicKeyStr()
        {
            StreamReader sr = new StreamReader(merchant_public_key);
            string pubkey = sr.ReadToEnd();
            sr.Close();
            if (pubkey != null)
            {
                pubkey = pubkey.Replace("-----BEGIN PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("-----END PUBLIC KEY-----", "");
                pubkey = pubkey.Replace("\r", "");
                pubkey = pubkey.Replace("\n", "");
            }
            return pubkey;
        }

    }

    
}
