using Hyt.BLL.Finance;
using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Common;
using Hyt.Model.Icp.GZNanSha;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Hyt.BLL.ApiPay.AliPay
{
    /// <summary>
    /// 支付宝报关接口 
    /// </summary>
    /// <remarks>2016-06-06 杨云奕 添加</remarks>
    public class PayProvider : IPayProvider
    {
        private PayConfig config = Hyt.BLL.Config.Config.Instance.GetPayConfig();
        private AlipayCustomsConfig customsConfig = Hyt.BLL.Config.Config.Instance.GetAlipayCustomsConfig();
        public override Model.CommonEnum.PayCode Code
        {
            get { return CommonEnum.PayCode.支付宝; }
        }
        /// <summary>
        /// 支付宝海关报关
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override Model.Result ApplyToCustoms(Model.SoOrder order)
        {
            IList<FnOnlinePayment> list = FinanceBo.Instance.GetOnlinePaymentList(order.SysNo);
            Result result = new Result();

            if (list.Count > 0)
            {
                result = AliAcquireCustomsBackData(list[0], order);
            }
            else
            {
                result.Status = false;
                result.Message = "未找到支付单，请核实支付订单号有效";
            }
            return result;
        }

        /// <summary>
        /// 支付宝报关实体
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="soorder"></param>
        /// <returns></returns>
        public Result AliAcquireCustomsBackData(FnOnlinePayment payment, SoOrder soorder)
        {
            Result result = new Result();
            try
            {

                // Hyt.Model.Common.PayConfig modelConfig = Hyt.BLL.Config.Config.Instance.GetPayConfig();

                ///支付宝报关实体
                AlipayCustomsMdl mdl = new AlipayCustomsMdl();
                mdl.service = customsConfig.service;
                mdl.partner = customsConfig.partner;
                mdl._input_charset = customsConfig.input_charset;
                mdl.sign_type = customsConfig.sign_type;
                mdl.out_request_no = payment.BusinessOrderSysNo;
                mdl.trade_no = payment.VoucherNo;
                mdl.merchant_customs_code = customsConfig.merchant_customs_code;
                mdl.merchant_customs_name = customsConfig.merchant_customs_name;
                mdl.amount = payment.Amount;
                mdl.customs_place = customsConfig.customs_place;

                string pushResult = "";

                string pushData = "";
                string outReportData = "";

                #region 海关报关

                ///创建支付宝报关签名
                CreateAlipayCustomsSign(mdl);
                ///生成支付宝连接
                string uriPath =GetAlipayAcquireCustoms(mdl);
                //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/AliAcquireCustoms.txt"), uriPath);
                ///执行http连接获取返回的xml内容

                pushData = uriPath;
                string xml = Hyt.Util.MyHttp.GetResponse(uriPath);
                outReportData = xml;

                //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/AliAcquireCustomsBack.txt"), xml);
                ///返回实体内容
                AliAcquireCustomsBack backMod = SaveAlipayAcquireCustomsBackData(xml);
                backMod.OutRequestNo = mdl.out_request_no;
                backMod.OrderSysNo = payment.SourceSysNo;
                backMod.Success = "海关：" + backMod.Success;
                //AliAcquireCustomsBackBo.Instance.InnerAcquireCustoms(backMod);
                //pushResult = "海关：" + backMod.Success;
                if (backMod.Success != "海关：T")
                {
                    pushResult = "海关支付报关失败-" + xml;
                }
                #endregion

                #region 商检
                if (!string.IsNullOrEmpty(customsConfig.merchant_commodity_code))
                {
                    mdl.out_request_no = "S" + soorder.OrderNo;
                    mdl.merchant_customs_code = customsConfig.merchant_commodity_code;
                    mdl.merchant_customs_name = customsConfig.merchant_commodity_name;
                    mdl.customs_place = customsConfig.commodity_place;
                    CreateAlipayCustomsSign(mdl);
                    ///生成支付宝连接
                    uriPath = GetAlipayAcquireCustoms(mdl);
                    //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/AliAcquireSJ.txt"), uriPath);
                    ///执行http连接获取返回的xml内容

                    pushData += "\r\n" + uriPath;
                    xml = Hyt.Util.MyHttp.GetResponse(uriPath);
                    outReportData += "\r\n" + xml;

                    //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/AliAcquireSJBack.txt"), xml);
                    ///返回实体内容
                    backMod = SaveAlipayAcquireCustomsBackData(xml);
                    backMod.OutRequestNo = mdl.out_request_no;
                    backMod.OrderSysNo = payment.SourceSysNo;
                    backMod.Success = "商检：" + backMod.Success;
                    if (backMod.Success != "商检：T")
                    {
                        pushResult += "商检支付报关失败-" + xml;
                    }
                }
                //AcquireCustomsBo.Instance.InnerAcquireCustoms(backMod);
                //pushResult = "商检：" + backMod.Success;
                #endregion

                #region 将支付宝保税报关反馈信息返回到订单中
                SoOrder sorder = SoOrderBo.Instance.GetEntity(backMod.OrderSysNo);
                //sorder.CustomsResult = pushResult;
                //SoOrderBo.Instance.UpdateOrder(sorder);
                //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/CustomsResult.txt"), sorder.CustomsResult);

                if (string.IsNullOrEmpty(pushResult))
                {
                    result.Message = "提交成功！";
                    result.Status = true;
                    //更新订单支付报关状态为处理中
                    Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.成功, 0, sorder.SysNo);
                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.前台, "订单编号：" + sorder.SysNo + ",支付信息报关提交成功！" + "回执：" + outReportData, LogStatus.系统日志目标类型.订单支付报关, sorder.SysNo, 0);
                }
                else
                {
                    result.Message = pushResult;
                    result.Status = false;
                    BLL.Log.LocalLogBo.Instance.Write("提交失败！" + pushResult, "EhkingCustomsLog");
                }
               

                result.Status = true;
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
                #endregion

            return result;

        }

        public override Model.Result CustomsQuery(int orderId)
        {
            string service = "alipay.overseas.acquire.customs.query";
            string partner = customsConfig.partner;
            string _input_charset = "UTF-8";
            string sign_type = "MD5";
            string sign = "";
            string out_request_nos = "So00000060";
            string _sign = "";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("_input_charset", _input_charset);
            dic.Add("out_request_nos", out_request_nos);
            dic.Add("partner", partner);
            dic.Add("service", service);
           // dic.Add("sign_type", sign_type);
           
            foreach (string key in dic.Keys)
            {
                if (!string.IsNullOrEmpty(sign))
                {
                    sign += "&";
                }
                sign += key + "=" + dic[key];
            }
            string param = sign;
            param += "&sign_type=" + sign_type;
            Hyt.Model.Common.PayConfig modelConfig = Hyt.BLL.Config.Config.Instance.GetPayConfig();
            sign += modelConfig.AliPaykey;
            //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/AliAcquireCustoms011.txt"), sign);
            if (sign_type == "MD5")
            {
                StringBuilder sb = new StringBuilder(32);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(sign));
                for (int i = 0; i < t.Length; i++)
                {
                    sb.Append(t[i].ToString("x").PadLeft(2, '0'));
                }
                sign = sb.ToString();
            }
            //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/AliAcquireCustoms012.txt"), sign);
            _sign = sign;
            string url = "https://mapi.alipay.com/gateway.do?" + param + "&sign=" + sign;
            string xml = Hyt.Util.MyHttp.GetResponse(url);
            return new Result() { Message=xml, Status=true };
        }

        #region 支付宝报关
        /// <summary>
        /// 生成支付宝报关签名
        /// </summary>
        /// <param name="mdl"></param>
        /// <returns></returns>
        /// <remarks>2016-06-06 杨云奕 添加</remarks>
        public static void CreateAlipayCustomsSign(AlipayCustomsMdl mdl)
        {
            string sign = "";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("_input_charset", mdl._input_charset);
            dic.Add("amount", mdl.amount.ToString());
            dic.Add("customs_place", mdl.customs_place);
            dic.Add("merchant_customs_code", mdl.merchant_customs_code);
            dic.Add("merchant_customs_name", mdl.merchant_customs_name);
            dic.Add("out_request_no", mdl.out_request_no);
            dic.Add("partner", mdl.partner);
            dic.Add("service", mdl.service);
            dic.Add("trade_no", mdl.trade_no);
            foreach (string key in dic.Keys)
            {
                if (!string.IsNullOrEmpty(sign))
                {
                    sign += "&";
                }
                sign += key + "=" + dic[key];
            }
            Hyt.Model.Common.PayConfig modelConfig = Hyt.BLL.Config.Config.Instance.GetPayConfig();
            sign += modelConfig.AliPaykey;
            //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/AliAcquireCustoms011.txt"), sign);
            if (mdl.sign_type == "MD5")
            {
                StringBuilder sb = new StringBuilder(32);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] t = md5.ComputeHash(Encoding.GetEncoding(mdl._input_charset).GetBytes(sign));
                for (int i = 0; i < t.Length; i++)
                {
                    sb.Append(t[i].ToString("x").PadLeft(2, '0'));
                }
                sign = sb.ToString();
            }
            //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/AliAcquireCustoms012.txt"), sign);
            mdl.sign = sign;
        }

        /// <summary>
        /// 生成海关报关连接代码
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-06-06 杨云奕 添加</remarks>
        public static string GetAlipayAcquireCustoms(AlipayCustomsMdl mdl)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("service", mdl.service);
            dic.Add("partner", mdl.partner);
            dic.Add("_input_charset", mdl._input_charset);
            dic.Add("sign_type", mdl.sign_type);
            dic.Add("sign", mdl.sign);
            dic.Add("out_request_no", mdl.out_request_no);
            dic.Add("trade_no", mdl.trade_no);
            dic.Add("merchant_customs_code", mdl.merchant_customs_code);
            dic.Add("merchant_customs_name", mdl.merchant_customs_name);
            dic.Add("amount", mdl.amount.ToString());
            dic.Add("customs_place", mdl.customs_place);
            //dic.Add("order_fee", mdl.order_fee);
            //dic.Add("product_fee", mdl.product_fee);
            //dic.Add("service_version", mdl.service_version);
            //dic.Add("transport_fee", mdl.transport_fee);
            //dic.Add("merchant_customs_name", mdl.merchant_customs_name);
            //dic.Add("is_split", mdl.is_split);
            //dic.Add("sub_out_biz_no", mdl.sub_out_biz_no);

            string htmlUrl = "";
            foreach (string key in dic.Keys)
            {
                if (!string.IsNullOrEmpty(htmlUrl))
                {
                    htmlUrl += "&";
                }
                htmlUrl += key + "=" + dic[key];
            }
            return "https://mapi.alipay.com/gateway.do?" + htmlUrl;
        }
        /// <summary>
        /// 设置海关报关数据
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        /// <remarks>2016-06-06 杨云奕 添加</remarks>
        public static AliAcquireCustomsBack SaveAlipayAcquireCustomsBackData(string xml)
        {
            AliAcquireCustomsBack backMod = new AliAcquireCustomsBack();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement rootElem = doc.DocumentElement;
            backMod.Success = rootElem.ChildNodes[0].InnerText;
            if (rootElem.ChildNodes.Count > 2)
            {
                XmlNode node = rootElem.ChildNodes[2];
                XmlNode alipayNode = node.ChildNodes[0];
                XmlNodeList list = alipayNode.ChildNodes;
                foreach (XmlNode aliNode in list)
                {
                    switch (aliNode.LocalName)
                    {
                        case "result_code":
                            backMod.Error = aliNode.InnerText;
                            break;
                        case "trade_no":
                            backMod.CustomsTradeNo = aliNode.InnerText;
                            break;
                        case "alipay_declare_no":
                            backMod.AlipayTradeNo = aliNode.InnerText;
                            break;
                        case "detail_error_code":
                            backMod.DetailErrorCode = aliNode.InnerText;
                            break;
                        case "detail_error_des":
                            backMod.DetailErrorCode = aliNode.InnerText;
                            break;

                    }
                }
            }
            else if (rootElem.ChildNodes.Count == 2)
            {
                backMod.Error = rootElem.ChildNodes[1].InnerText;
            }
            return backMod;
        }
        #endregion
    }

    /// <summary>
    /// 支付宝报关实体
    /// </summary>
    /// <remarks>2015-10-24 杨云奕 添加</remarks>
    public class AlipayCustomsMdl
    {
        #region 基本参数
        /// <summary>
        /// 接口名称
        /// </summary>
        public string service { get; set; }
        /// <summary>
        /// 合作者身份ID
        /// </summary>
        public string partner { get; set; }
        /// <summary>
        /// 参数编码字符集
        /// </summary>
        public string _input_charset { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        #endregion
        #region 业务参数
        /// <summary>
        /// 报关流水号，商户自己生成
        /// </summary>
        public string out_request_no { get; set; }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 商户海关备案编码
        /// </summary>
        public string merchant_customs_code { get; set; }
        /// <summary>
        /// 报关金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 海关编码
        /// </summary>
        public string customs_place { get; set; }
        /// <summary>
        /// 商户海关备案名称
        /// </summary>
        public string merchant_customs_name { get; set; }
        /// <summary>
        /// 是否拆单
        /// </summary>
        public string is_split { get; set; }
        /// <summary>
        /// 子订单号
        /// </summary>
        public string sub_out_biz_no { get; set; }

        #endregion
    }
}
