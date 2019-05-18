using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;
using Newtonsoft.Json.Linq;
using com.ehking.utils;
using com.ekhing.Web;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Common;
using Hyt.Model.Logis.XinYi;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json;
using Hyt.BLL.EPayServiceReference;

namespace Hyt.BLL.ApiPay.EPay
{
    /// <summary>
    /// 易票联支付报关接口
    /// </summary>
    /// <remarks> 2016-12-12 杨浩 创建</remarks>
    public class PayProvider : IPayProvider
    {
        #region 属性字段
        private PayConfig payConfig = Hyt.BLL.Config.Config.Instance.GetPayConfig();
        private CustomsConfig customsConfig = Hyt.BLL.Config.Config.Instance.GetCustomsConfig();
        private IcpInfoConfig icpConfig = Hyt.BLL.Config.Config.Instance.GetIcqInfoConfig();
        
        public PayProvider() { }

        public override CommonEnum.PayCode Code
        {
            get { return CommonEnum.PayCode.易票联; }
        }

        private static object lockHelper = new object();
        #endregion

        #region 接口方法

        #region 支付报关推送
        /// <summary>
        /// 支付报关推送
        /// </summary>
        /// <param name="order">订单实体</param>
        public override Result ApplyToCustoms(SoOrder order)
        {
            Result result = new Result()
            {
                Status = false,
                StatusCode = 0,
                Message = "未知错误"
            };

            //防止并发操作
            lock (lockHelper)
            {
                try
                {
                    if (order != null)
                    {
                        var onlinePaymentFilter = new ParaOnlinePaymentFilter()
                        {
                            OrderSysNo = order.SysNo,
                            Id = 1
                        };

                        var onlinePayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePayments(onlinePaymentFilter).Rows;
                        var receiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);

                        DateTime dtNow = DateTime.Now;

                        //广州电子口岸
                        string xmlTemplte = "<VnbMessage>"
                                          +     "<MessageHead>"
                                          +         "<MessageCode>VNB3PARTY_GZEPORT</MessageCode>"
                                          +         "<MessageID>" + dtNow.ToString("yyyyMMddHHmmss") + "0001" + "</MessageID>"
                                          +         "<SenderID>" + payConfig.TLPaySenderID + "</SenderID>"
                                          +         "<SendTime>" + dtNow.ToString("yyyy-MM-dd HH:mm:ss") + "</SendTime>"
                                          +         "<ReceiptUrl>" + payConfig.TLPayReceiptUrl + "</ReceiptUrl>"
                                          +         "<Sign>{0}</Sign>"
                                          +     "</MessageHead>"
                                          +     "<MessageBodyList>"
                                          +         "<MessageBody>"
                                          +             "<customICP>" + customsConfig.SenderID + "</customICP>"
                                          +             "<customName>" + customsConfig.EbcName + "</customName>"
                                          +             "<ciqType>00</ciqType>"
                                          +             "<cbepComCode>C000000000001</cbepComCode>"
                                          +             "<orderNo>" + onlinePayments.First().BusinessOrderSysNo + "</orderNo>"
                                          +             "<payTransactionNo>" + onlinePayments.First().VoucherNo + "</payTransactionNo>"
                                          +             "<payChnlID>" + (order.OrderSource == (int)Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 ? "02" : "01") + "</payChnlID>"
                                          +             "<payTime>" + onlinePayments.First().CreatedDate.ToString("yyyy-MM-dd HH:mm:ss") + "</payTime>"
                                          +             "<payGoodsAmount>" + order.ProductAmount.ToString("F2") + "</payGoodsAmount>"
                                          +             "<payTaxAmount>" + order.TaxFee.ToString("F2") + "</payTaxAmount>"
                                          +             "<freight>" + order.FreightAmount.ToString("F2") + "</freight>"
                                          +             "<payCurrency>142</payCurrency>"
                                          +             "<payerName>" + receiveAddress.Name + "</payerName>"
                                          +             "<payerDocumentType>01</payerDocumentType>"
                                          +             "<payerDocumentNumber>" + receiveAddress.IDCardNo + "</payerDocumentNumber>"
                                          +         "</MessageBody>"
                                          +     "</MessageBodyList>"
                                          + "</VnbMessage>";

                        string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(string.Format(xmlTemplte, "") + payConfig.TLPaySignKey, "MD5").ToUpper();
                        string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + string.Format(xmlTemplte, sign);

                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "通联支付报关推送报文：" + xmlString, LogStatus.系统日志目标类型.支付方式, order.SysNo, 0);

                        HttpClient client = new HttpClient(payConfig.TLPayCustomsUrl);
                        string back = client.Post(xmlString);

                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "通联支付报关接口响应报文：" + back, LogStatus.系统日志目标类型.支付方式, order.SysNo, 0);

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(back);

                        XmlNode xmlNodeHead = xmlDoc.SelectSingleNode("//VnbMessage//MessageHead");
                        string CommCode = xmlNodeHead.SelectSingleNode("CommCode").InnerText;   //通讯状态码(请求方不填, 响应方返回)
                        string BizStatus = xmlNodeHead.SelectSingleNode("BizStatus").InnerText; //业务状态码(请求方不填, 响应方返回)

                        if (CommCode == "000000")
                        {
                            if (BizStatus != "BZ0001")
                            {
                                XmlNode xmlNodeBody = xmlDoc.SelectSingleNode("//VnbMessage//MessageBodyList//MessageBody");
                                string retInfo = xmlNodeBody.SelectSingleNode("retInfo").InnerText;
                                result.Message = retInfo;
                            }
                            else
                            {
                                result.Message = "请求已受理";
                                result.Status = true;

                                //更新订单支付报关状态为处理中
                                Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.处理中, 0, order.SysNo);
                                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "TLPay推送接口调试成功！", LogStatus.系统日志目标类型.支付方式, order.SysNo, 0);
                            }
                        }
                        else
                        {
                            string retInfo = "通讯失败";
                            switch (CommCode)
                            {
                                case "HD0001":
                                    retInfo = "无效的内容长度"; break;
                                case "HD0002":
                                    retInfo = "请求报文为空"; break;
                                case "HD0003":
                                    retInfo = "报文头格式错误"; break;
                                case "HD0004":
                                    retInfo = "报文头必填字段为空"; break;
                                case "HD0005":
                                    retInfo = "无效的报文消息码"; break;
                                case "HD0006":
                                    retInfo = "无效的接入系统代码"; break;
                                case "HD0007":
                                    retInfo = "无效的接入主机IP"; break;
                                case "HD0008":
                                    retInfo = "签名验签错误，报文签名域不正确"; break;
                                default: break;
                            }
                            result.Message = retInfo;
                        }
                    }
                    else
                    {
                        result.Message = "该订单不存在！";
                    }
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "通联支付报关推送异常：" + ex.Message, ex);
                }
            }

            return result;
        }

        #endregion

        #region 支付报关查询
        /// <summary>
        /// 支付报关查询
        /// </summary>
        /// <param name="orderId">订单id</param>
        public override Result CustomsQuery(int orderId)
        {
            Result result = new Result()
            {
                Status = false,
                StatusCode = 0,
                Message = "未知错误"
            };

            //防止并发操作
            lock (lockHelper)
            {
                try
                {
                    var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderId);
                    if (order != null)
                    {
                        var onlinePaymentFilter = new ParaOnlinePaymentFilter()
                        {
                            OrderSysNo = order.SysNo,
                            Id = 1
                        };

                        var onlinePayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePayments(onlinePaymentFilter).Rows;

                        DateTime dtNow = DateTime.Now;

                        string xmlTemplte = "<VnbMessage>"
                                          +     "<MessageHead>"
                                          +         "<MessageCode>VNB3PARTY_CUSTOMSQUERY</MessageCode>"
                                          +         "<MessageID>" + dtNow.ToString("yyyyMMddHHmmss") + "0000" + "</MessageID>"
                                          +         "<SenderID>" + payConfig.TLPaySenderID + "</SenderID>"
                                          +         "<SendTime>" + dtNow.ToString("yyyy-MM-dd HH:mm:ss") + "</SendTime>"
                                          +         "<Sign>{0}</Sign>"
                                          +     "</MessageHead>"
                                          +     "<MessageBodyList>"
                                          +         "<MessageBody>"
                                          +             "<orderNo>" + onlinePayments.First().BusinessOrderSysNo + "</orderNo>"
                                          +             "<payDate>" + onlinePayments.First().CreatedDate.ToString("yyyy-MM-dd") + "</payDate>"
                                          +         "</MessageBody>"
                                          +     "</MessageBodyList>"
                                          + "</VnbMessage>";

                        string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(string.Format(xmlTemplte, "") + payConfig.TLPaySignKey, "MD5").ToUpper();
                        string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + string.Format(xmlTemplte, sign);

                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "通联支付报关查询报文：" + xmlString, LogStatus.系统日志目标类型.支付方式, order.SysNo, 0);

                        HttpClient client = new HttpClient(payConfig.TLPayCustomsUrl);
                        string back = client.Post(xmlString);

                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "通联支付报关查询接口响应报文：" + back, LogStatus.系统日志目标类型.支付方式, order.SysNo, 0);

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(back);

                        XmlNode xmlNodeHead = xmlDoc.SelectSingleNode("//VnbMessage//MessageHead");
                        string CommCode = xmlNodeHead.SelectSingleNode("CommCode").InnerText;   //通讯状态码(请求方不填, 响应方返回)
                        string BizStatus = xmlNodeHead.SelectSingleNode("BizStatus").InnerText; //业务状态码(请求方不填, 响应方返回)

                        if (CommCode == "000000")
                        {
                            XmlNode xmlNodeBody = xmlDoc.SelectSingleNode("//VnbMessage//MessageBodyList//MessageBody");
                            if (BizStatus != "BZ0001")
                            {
                                string retInfo = xmlNodeBody.SelectSingleNode("retInfo").InnerText;
                                result.Message = retInfo;
                            }
                            else
                            {
                                string returnCode = xmlNodeBody.SelectSingleNode("returnCode").InnerText;
                                string returnInfo = xmlNodeBody.SelectSingleNode("returnInfo").InnerText;

                                result.Message = returnInfo;

                                if (returnCode == "C01")
                                {
                                    result.Status = true;

                                    //更新订单支付报关状态为处理中
                                    Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.成功, 0, order.SysNo);
                                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "TLPay查询接口调试成功！", LogStatus.系统日志目标类型.支付方式, order.SysNo, 0);
                                }
                                else
                                {
                                    string customsStatus = xmlNodeBody.SelectSingleNode("customsStatus").InnerText;
                                    string customsStatusInfo = "未上送";

                                    switch (customsStatus)
                                    { 
                                        case "1":
                                            customsStatusInfo = "上送中"; break;
                                        case "2":
                                            customsStatusInfo = "上送完成"; break;
                                        case "3":
                                            customsStatusInfo = "已获取回执"; break;
                                        case "4":
                                            customsStatusInfo = "反馈成功"; break;
                                        case "5":
                                            customsStatusInfo = "反馈失败"; break;
                                        case "6":
                                            customsStatusInfo = "上送失败"; break;
                                        case "7":
                                            customsStatusInfo = "订单不存在"; break;
                                        default: break;
                                    }
                                    result.Message = customsStatusInfo;
                                }
                            }
                        }
                        else
                        {
                            string retInfo = "通讯失败";
                            switch (CommCode)
                            {
                                case "HD0001":
                                    retInfo = "无效的内容长度"; break;
                                case "HD0002":
                                    retInfo = "请求报文为空"; break;
                                case "HD0003":
                                    retInfo = "报文头格式错误"; break;
                                case "HD0004":
                                    retInfo = "报文头必填字段为空"; break;
                                case "HD0005":
                                    retInfo = "无效的报文消息码"; break;
                                case "HD0006":
                                    retInfo = "无效的接入系统代码"; break;
                                case "HD0007":
                                    retInfo = "无效的接入主机IP"; break;
                                case "HD0008":
                                    retInfo = "签名验签错误，报文签名域不正确"; break;
                                default: break;
                            }
                            result.Message = retInfo;
                        }
                    }
                    else
                    {
                        result.Message = "订单不存在！";
                    }
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "通联支付报关查询异常：" + ex.Message, ex);
                }
            }

            return result;
        }

        #endregion

        #region 批量支付

       
        private bool isTest = true;
        /// <summary>
        /// 终端密码
        /// </summary>
        /// <remarks>2017-2-8 杨浩 创建</remarks>
        private string TerminalPwd
        {
            get
            {
                if (isTest)
                    return "888888";
                return "";
            }
        }
 
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="param">需要加密的参数</param>
        /// <returns></returns>
        /// <remarks>2017-2-8 杨浩 创建</remarks>
        private string MD5(string param)
        {
            string md5Str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(param, "MD5").ToLower();
            return md5Str;
        }
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="data">需要签名的数据</param>
        /// <returns></returns>
        /// <remarks>2017-2-8 杨浩 创建</remarks>
        private string GetSign(string data)
        {
            string mac = MD5(data + MD5(TerminalPwd));
            return mac;
        }
   
        #endregion
        #endregion

        #region 报文实体
        private class Reg:Account
        {
            /// <summary>
            /// 签名
            /// </summary>
            public string Mac { get; set; }
        }
        #endregion
    }
}