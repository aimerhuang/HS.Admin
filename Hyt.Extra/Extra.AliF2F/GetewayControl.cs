using Aop.Api.Util;
using Extra.AliF2F.ToAlipay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Extra.AliF2F
{
    public class GetewayControl
    {
        string txt = "";
        public string  InitGateway(AliServicePostDataMod aliMod,string privateKey ,string publicKey,string appId)
        {
            //log(GetUrlParam(getRequstParam(aliMod)));
            // log(getRequestString("service"));
            Config.merchant_private_key = privateKey;
            Config.merchant_public_key = publicKey;
            //Config.appId = appId;
            Game.Utils.FileManager.WriteFile(System.AppDomain.CurrentDomain.BaseDirectory + ("/PosGateway.txt"), aliMod.service + "\r\n" + aliMod.biz_content + "\r\n" + aliMod.charset + "\r\n" + aliMod.service + "\r\n" + aliMod.sign + "\r\n" + aliMod.sign_type);
            //验证网关
            if ("alipay.service.check".Equals(aliMod.service))
            {
               return  verifygw(aliMod);
            }
            return "";
        }

        public string verifygw(AliServicePostDataMod aliMod)
        {
            //  Request.Params;
            Dictionary<string, string> dict = getAlipayRequstParams(aliMod);
            //string biz_content = AlipaySignature.CheckSignAndDecrypt(dict, Config.alipay_public_key, Config.merchant_private_key, true, false);
            string biz_content = dict["biz_content"];
            if (!verifySignAlipayRequest(dict))
            {
                txt = verifygwResponse(false, Config.getMerchantPublicKeyStr());
            }
            if ("verifygw".Equals(getXmlNode(biz_content, "EventType")))
            {
                txt = verifygwResponse(true, Config.getMerchantPublicKeyStr());
            }
            return txt;
        }
        /// <summary>
        /// 获取xml中的事件类型
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public string getXmlNode(string xml, string node)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            //string EventType = xmlDoc.GetElementsByTagName("EventType").ToString();
            string EventType = xmlDoc.SelectSingleNode("//" + node).InnerText.ToString();
            //Response.Output.WriteLine("EventType:" + EventType);
            return EventType;
        }
        /// <summary>
        /// 验签支付宝请求
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool verifySignAlipayRequest(Dictionary<string, string> param)
        {
            bool result = AlipaySignature.RSACheckV2(param, Config.alipay_public_key, Config.charset);
            return result;
        }
        /// <summary>
        /// 验证网关，签名内容并返回给支付宝xml
        /// </summary>
        /// <param name="_success"></param>
        /// <param name="merchantPubKey"></param>
        /// <returns></returns>
        public string verifygwResponse(bool _success, string merchantPubKey)
        {
            //Response.ContentType = "text/xml";
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("GBK");
            //Response.Clear();

            XmlDocument xmlDoc = new XmlDocument(); //创建实例
            XmlDeclaration xmldecl = xmlDoc.CreateXmlDeclaration("1.0", "GBK", null);
            xmlDoc.AppendChild(xmldecl);


            XmlElement xmlElem = xmlDoc.CreateElement("alipay"); //新建元素

            xmlDoc.AppendChild(xmlElem); //添加元素


            XmlNode alipay = xmlDoc.SelectSingleNode("alipay");
            XmlElement response = xmlDoc.CreateElement("response");
            XmlElement success = xmlDoc.CreateElement("success");
            if (_success)
            {
                success.InnerText = "true";//设置文本节点 
                response.AppendChild(success);//添加到<Node>节点中 
            }
            else
            {
                success.InnerText = "false";//设置文本节点 
                response.AppendChild(success);//添加到<Node>节点中 
                XmlElement err = xmlDoc.CreateElement("error_code");
                err.InnerText = "VERIFY_FAILED";
                response.AppendChild(err);
            }

            XmlElement biz_content = xmlDoc.CreateElement("biz_content");
            biz_content.InnerText = merchantPubKey;
            response.AppendChild(biz_content);

            alipay.AppendChild(response);

            string _sign = AlipaySignature.RSASign(response.InnerXml, Config.merchant_private_key, Config.charset);

            XmlElement sign = xmlDoc.CreateElement("sign");
            sign.InnerText = _sign;
            alipay.AppendChild(sign);
            XmlElement sign_type = xmlDoc.CreateElement("sign_type");
            sign_type.InnerText = "RSA";
            alipay.AppendChild(sign_type);

            //Response.Output.Write(xmlDoc.InnerXml);
            //log(xmlDoc.InnerXml);
            //Response.End();

            return xmlDoc.InnerXml;
        }
        private Dictionary<string, string> getAlipayRequstParams(AliServicePostDataMod aliMod)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("service", aliMod.service);
            dict.Add("sign_type", aliMod.sign_type);
            dict.Add("charset", aliMod.charset);
            dict.Add("biz_content", aliMod.biz_content);
            dict.Add("sign", aliMod.sign);
            return dict;
        }
        
    }
}
