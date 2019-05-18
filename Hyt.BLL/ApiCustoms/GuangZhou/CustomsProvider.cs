using Hyt.BLL.ApiPay;
using Hyt.Model;
using Hyt.Model.Order;
using Hyt.Model.WorkflowStatus;
using Hyt.Util.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hyt.BLL.ApiCustoms.GuangZhou
{
    /// <summary>
    /// 广州海关接口
    /// </summary>
    public class CustomsProvider : ICustomsProvider
    {
        #region 海关接口参数
        /// <summary>
        /// 报文类型 880020-电子订单
        /// </summary>
        private string MessageType =config.MessageType;//"880020";
        /// <summary>
        /// 信营企业备案号
        /// </summary>
        //private string XinYinSenderID = "IE150723865142";//"IE150831444711";
        private string XinYinSenderID =config.SenderID;// "IE150831444711";
        /// <summary>
        /// 信营海关FTP地址
        /// </summary>
        private string FtpUrl = config.FtpUrl; // "ftp://210.21.48.7:2312/";

        /// <summary>
        /// 信营海关FTP地址用户名
        /// </summary>
        //private string FtpUserName = "XYMY";// "XYGJ";
        private string FtpUserName = config.FtpUserName;//"XYGJ";
        /// <summary>
        /// 信营海关FTP地址密码
        /// </summary>
        //private string FtpPassword = "z233L7PK"; //"64g5vsVh";
        private string FtpPassword = config.FtpPassword;//"64g5vsVh";
        /// <summary>
        /// 版本号
        /// </summary>
        private string Version =config.Version;// "1.0";
        #endregion
        /// <summary>
        /// 海关内部代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public override CommonEnum.海关 Code
        {
            get { return CommonEnum.海关.广州机场海关; }
        }
        /// <summary>
        /// 上传订单至海关
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="customsLogInfo">海关日志</param>
        /// <param name="orderStatus">订单状态 S-订单新增，C-订单取消</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 杨浩 创建</remarks>
        private Result Upload(int orderId, int warehouseSysNo, SoCustomsOrderLog customsLogInfo, string orderStatus="S")
        {
            var result = new Result()
            {
                Status = false
            };
          
            //xml文件名
            string fileName = "";
            //获取当前时间
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            int Day = DateTime.Now.Day;
            int Hour = DateTime.Now.Hour;
            int Minute = DateTime.Now.Minute;
            int Second = DateTime.Now.Second;
            string SendTime = Year.ToString() + (Month < 10 ? "0" + Month.ToString() : Month.ToString()) + (Day < 10 ? "0" + Day.ToString() : Day.ToString());
            SendTime += (Hour < 10 ? "0" + Hour.ToString() : Hour.ToString()) + (Minute < 10 ? "0" + Minute.ToString() : Minute.ToString()) + (Second < 10 ? "0" + Second.ToString() : Second.ToString());
            //if (customsLogInfo == null)
            //{
                string MessageIDNo = BLL.Basic.ReceiptNumberBo.Instance.GetCustomsOrderNo();
                fileName = MessageType + SendTime + MessageIDNo;
            //}
            //else
            //{
            //    fileName = customsLogInfo.FileName;
            //}

            var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntityNoCache(orderId);
            var receiveInfo = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(orderInfo.ReceiveAddressSysNo);

            #region 上载数据

            string OrderId = orderId.ToString();
            string IEFlag = "I"; //进出口标识 I:进口；E:出口
            string OrderStatus = orderStatus;// "S";//订单状态 S-订单新增，C-订单取消
            string EntRecordNo = XinYinSenderID;//电商平台企业备案号（代码）
            //string EntRecordName = "深圳信营贸易有限公司";// "深圳市信营国际电子商务有限公司";//电商平台企业名称
            string EntRecordName = config.EntRecordName;// "深圳市信营国际电子商务有限公司";//电商平台企业名称
            string OrderName = receiveInfo.Name;  //订单人姓名
            string OrderDocType = "01"; //订单人证件类型01:身份证、02:护照、03:其他
            string OrderDocId = receiveInfo.IDCardNo;
            string OrderPhone = receiveInfo.MobilePhoneNumber;
            string OrderGoodTotal = (orderInfo.ProductAmount + orderInfo.ProductChangeAmount).ToString("F2"); 
            string OrderGoodTotalCurr = "142"; //订单商品总额币制，人民币(CNY/142)
            string Freight = orderInfo.FreightAmount.ToString("F2"); //运费
            string FreightCurr = "142";//运费币制
            string Tax = orderInfo.TaxFee.ToString("F2"); //行邮税
            string TaxCurr = "142";
            string Note = "";//备注
            string OrderDate = orderInfo.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");//订单日期，精确到秒 YYYY-MM-DD：HH：MM：SS


            var onlinePaymentFilter = new Hyt.Model.Parameter.ParaOnlinePaymentFilter()
            {
                OrderSysNo = orderId,
                Id = 1
            };
            var onlinePayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePayments(onlinePaymentFilter).Rows;

            if (onlinePayments == null || (onlinePayments != null && onlinePayments.Count() <= 0))
            {
                result.Status = false;
                result.Message = "订单没有支付单！";
                return result;
            }

            string orderNo = onlinePayments[0].BusinessOrderSysNo;

            if (string.IsNullOrEmpty(orderNo))
            {
                result.Status = false;
                result.Message = "订单编号异常！";
                return result;
            }


            string strxml = "";
            strxml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            strxml += "<Manifest>";
            strxml += "<Head>";
            strxml += "<MessageID>" + fileName + "</MessageID>";
            strxml += "<MessageType>" + MessageType + "</MessageType>";
            strxml += "<SenderID>" + XinYinSenderID + "</SenderID>";
            strxml += "<SendTime>" + SendTime + "</SendTime>";
            strxml += "<Version>" + Version + "</Version>";
            strxml += "</Head>";
            strxml += "<Declaration>";
            strxml += "<EOrder>";
            strxml += "<OrderId>" + orderNo + "</OrderId>";
            strxml += "<IEFlag>" + IEFlag + "</IEFlag>";
            strxml += "<OrderStatus>" + OrderStatus + "</OrderStatus>";
            strxml += "<EntRecordNo>" + EntRecordNo + "</EntRecordNo>";
            strxml += "<EntRecordName>" + EntRecordName + "</EntRecordName>";
            strxml += "<OrderName>" + OrderName + "</OrderName>";
            strxml += "<OrderDocType>" + OrderDocType + "</OrderDocType>";
            strxml += "<OrderDocId>" + OrderDocId + "</OrderDocId>";
            strxml += "<OrderPhone>" + OrderPhone + "</OrderPhone>";
            strxml += "<OrderGoodTotal>" + OrderGoodTotal + "</OrderGoodTotal>";
            strxml += "<OrderGoodTotalCurr>" + OrderGoodTotalCurr + "</OrderGoodTotalCurr>";
            strxml += "<Freight>" + Freight + "</Freight>";
            strxml += "<FreightCurr>" + FreightCurr + "</FreightCurr>";
            strxml += "<Tax>" + Tax + "</Tax>";
            strxml += "<TaxCurr>" + TaxCurr + "</TaxCurr>";
            strxml += "<Note>" + Note + "</Note>";
            strxml += "<OrderDate>" + OrderDate + "</OrderDate>";
            strxml += "</EOrder>";
            strxml += "<EOrderGoods>";

            var orderItemList = BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(orderId);

            foreach (var item in orderItemList)
            {
                var warehouseProductInfo = Hyt.BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(warehouseSysNo, item.ProductSysNo);
                if (warehouseProductInfo == null)
                {
                    result.Message = "订单中有产品(系统编号：" + item.ProductSysNo + ")在仓库中不存在！";
                    return result;
                }

                string customsNo = warehouseProductInfo.CustomsNo;
                if (customsNo == "")
                {
                    result.Message = "订单中有产品(系统编号：" + item.ProductSysNo + ")无海关备案号！";
                    return result;
                }

                strxml += "<EOrderGood>";
                strxml += "<GNo>" + item.SysNo + "</GNo>";
                strxml += "<ChildOrderNo></ChildOrderNo>";
                strxml += "<StoreRecordNo>" + EntRecordNo + "</StoreRecordNo>";
                strxml += "<StoreRecordName>" + EntRecordName + "</StoreRecordName>";
                strxml += "<CopGNo>" + item.ProductSysNo + "</CopGNo>";
                strxml += "<CustomsListNO>" + warehouseProductInfo.CustomsNo + "</CustomsListNO>"; //商品海关备案号
                strxml += "<DecPrice>" + item.SalesUnitPrice + "</DecPrice>";
                strxml += "<Unit>001</Unit>"; //计量单位(件/011)
                strxml += "<GQty>" + item.Quantity + "</GQty>";//商品数量
                strxml += "<DeclTotal>" + (item.SalesAmount+item.ChangeAmount).ToString("F2") + "</DeclTotal>";
                strxml += "<Notes></Notes>";
                strxml += "</EOrderGood>";
            }
            strxml += "</EOrderGoods>";
            strxml += "</Declaration>";
            strxml += "</Manifest>";

            #endregion
            var customsOrderLog = new SoCustomsOrderLog();

            if (customsLogInfo != null)
                customsOrderLog=customsLogInfo;

            customsOrderLog.Packets = strxml;

            strxml = Encrypt(strxml);
            //上传xml文件
            MemoryStream stream = new MemoryStream();
            byte[] buffer = Encoding.Default.GetBytes(strxml);
            string msg = "";
            string _ftpImageServer = FtpUrl + "UPLOAD/";

           
            customsOrderLog.StatusCode = "";
            customsOrderLog.StatusMsg = "处理中";           
            customsOrderLog.LastUpdateBy = 0;
            if (BLL.Authentication.AdminAuthenticationBo.Instance.IsLogin)
                customsOrderLog.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
            customsOrderLog.LastUpdateDate = DateTime.Now;
            customsOrderLog.CreateDate = DateTime.Now;
            customsOrderLog.CreatedBy = customsOrderLog.LastUpdateBy;
            customsOrderLog.ReceiptContent = "";
            customsOrderLog.FileName = fileName;
            customsOrderLog.OrderSysNo = orderId;
        
            FtpUtil ftp = new FtpUtil(_ftpImageServer, FtpUserName, FtpPassword);
            //上传xml文件
            ftp.UploadFile(_ftpImageServer, customsOrderLog.FileName + ".xml", buffer, out msg);
            //if (customsLogInfo != null)
            //{
            //    customsOrderLog.CreateDate = customsLogInfo.CreateDate;
            //    customsOrderLog.CreatedBy = customsLogInfo.CreatedBy;
            //    customsOrderLog.SysNo = customsLogInfo.SysNo;
            //    BLL.Order.SoCustomsOrderLogBo.Instance.UpdateCustomsOrderLog(customsOrderLog);
            //}
            if (customsLogInfo == null)
                BLL.Order.SoCustomsOrderLogBo.Instance.AddCustomsOrderLog(customsOrderLog);
            else
                BLL.Order.SoCustomsOrderLogBo.Instance.UpdateCustomsOrderLog(customsOrderLog);
            if (orderStatus=="S")
                BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.海关报关状态.处理中, 2, orderId);
            else
                BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.海关报关状态.作废, 2, orderId);

            result.Status = true;
            result.Message = "提交成功！";

            return result;
        }
        /// <summary>
        /// 锁对象
        /// </summary>
        private static object lockHelper = new object();
        private static object lockSearch = new object();
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2016-3-23 杨浩 创建</remarks>
        public override Result CancelOrder(int orderId, int warehouseSysNo)
        {
            lock (lockHelper)
            {
                var result = new Result()
                {
                    Status = false
                };

                try
                {
                    var customsLogInfo = BLL.Order.SoCustomsOrderLogBo.Instance.GetCustomsOrderLogInfo(orderId, (int)Code);
                    result = Upload(orderId, warehouseSysNo, customsLogInfo, "C");                  
                }
                catch (Exception ex)
                {
                    //显示错误信息  
                    result.Status = false;
                    result.Message = "系统错误！";
                    BLL.Log.LocalLogBo.Instance.Write(ex, "GZCancelOrder");
                }
                return result;
            }
        }
        /// <summary>
        /// 推送订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public override Result PushOrder(int orderId, int warehouseSysNo)
        {
            lock (lockHelper)
            {
                var result = new Result()
                {
                    Status = false
                };
                try
                {
                    var customsLogInfo = BLL.Order.SoCustomsOrderLogBo.Instance.GetCustomsOrderLogInfo(orderId, (int)Code);
                    //if (customsLogInfo != null && (customsLogInfo.StatusCode == "C01" || customsLogInfo.StatusCode == "B13"))
                    //{
                    //    result.Status = false;
                    //    result.Message = customsLogInfo.StatusMsg != "" ? "此订单已提交至广州海关,海关回执:" + customsLogInfo.StatusMsg : "此订单已提交至广州海关,请查询回执！";
                    //    return result;
                    //}

                    result = Upload(orderId, warehouseSysNo, customsLogInfo);
           
                }
                catch (Exception ex)
                {
                    //显示错误信息  
                    result.Status = false;
                    result.Message = "系统错误！";
                    BLL.Log.LocalLogBo.Instance.Write(ex, "GZPushOrder");
                }
                return result;
            }
        }

        /// <summary>
        /// 查询海关订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-29 杨浩 创建</remarks>
        public override Result SearchCustomsOrder(int orderId)
        {
            lock (lockSearch)
            {
                var result = new Result()
                {
                    Status = false
                };

                try
                {
                    OrderStatus.海关报关状态 status = OrderStatus.海关报关状态.成功;

                    var customsLogInfo = BLL.Order.SoCustomsOrderLogBo.Instance.GetCustomsOrderLogInfo(orderId, (int)Code);
                    if (customsLogInfo == null)
                    {
                        result.Message = "还未提交订单至海关！";
                        return result;
                    }

                    //原始文件（海关回执后缀为.txt的文件）
                    string oldFile = customsLogInfo.FileName + ".txt";

                    //失败回执文件名
                    string failureFileName = "0_" + oldFile;

                    //成功回执文件名
                    string successFileName = "1_" + oldFile;

                    string _ftpImageServer = FtpUrl + "DOWNLOAD/";

                    FtpUtil ftp = new FtpUtil(_ftpImageServer, FtpUserName, FtpPassword);
                    string msg = "";

                    //首先读取成功的文件
                    Stream stream = ftp.FileStream(_ftpImageServer + successFileName, ref msg);

                    if (msg == "get file success!")
                    {                     
                        result.Status = true;
                        status = OrderStatus.海关报关状态.成功;
                    }
                    else
                    {
                        msg = ""; 
                        //成功回执文件不存在则读取失败文件名称
                        stream = ftp.FileStream(_ftpImageServer + failureFileName, ref msg);
                        if (msg != "get file success!")
                        {
                            result.Status = false;
                            result.Message = "暂时海关没有生成回执文件，请稍后再试！";
                            return result;
                        }
                        status = OrderStatus.海关报关状态.失败;
                    }


                    //设置当前流的位置为流的开始，防止读取位置错误造成无法读取完整流的内容
                    stream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string xml = Decrypt(reader.ReadToEnd());

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xml);
                        XmlNode xmlNode = xmlDoc.SelectSingleNode("//ResponseMessage//ResponseBodyList//ResponseBody");
                        var customsOrderLog = new SoCustomsOrderLog();

                        customsOrderLog.StatusCode = xmlNode.SelectSingleNode("ReturnCode").InnerText;
                        customsOrderLog.StatusMsg = xmlNode.SelectSingleNode("ReturnInfo").InnerText;

                        //switch (code)
                        //{
                        //    case "A01": customsOrderLog.StatusMsg = "-缺少接入企业备案号"; break;
                        //    case "A02": customsOrderLog.StatusMsg = "-电商接入企业未备案或失效"; break;
                        //    case "A10": customsOrderLog.StatusMsg = "-缺少电商企业备案号"; break;
                        //    case "A11": customsOrderLog.StatusMsg = "-电商企业未备案或失效"; break;
                        //    case "A14": customsOrderLog.StatusMsg = "-退回修改"; break;
                        //    case "B10": customsOrderLog.StatusMsg = "-报文类型不正确"; break;
                        //    case "B11": customsOrderLog.StatusMsg = "-业务字段异常"; break;
                        //    case "B12": customsOrderLog.StatusMsg = "-重复报文"; break;
                        //    case "B13": customsOrderLog.StatusMsg = "-重复记录"; break;
                        //    case "C01": customsOrderLog.StatusMsg = "-入库成功"; break;
                        //    default: break;
                        //}                       
                        if(customsOrderLog.StatusMsg=="订单编号已经存在，不可重复插入。")
                            status = OrderStatus.海关报关状态.成功;
                        else if (customsOrderLog.StatusCode != "C01")
                            status = OrderStatus.海关报关状态.失败;


                        customsOrderLog.LastUpdateBy = 0;
                        if (BLL.Authentication.AdminAuthenticationBo.Instance.IsLogin)
                            customsOrderLog.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;

                        customsOrderLog.LastUpdateDate = DateTime.Now;
                        customsOrderLog.ReceiptContent = xml;
                        customsOrderLog.SysNo = customsLogInfo.SysNo;
                        customsOrderLog.FileName = customsLogInfo.FileName;
                        customsOrderLog.Packets = customsLogInfo.Packets;
                        BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)status, 2, orderId);

                        BLL.Order.SoCustomsOrderLogBo.Instance.UpdateCustomsOrderLog(customsOrderLog);

                        result.Message = customsOrderLog.StatusMsg;

                    }
                }
                catch (Exception ex)
                {
                    result.Message = "系统异常！";
                    result.Status = false;
                    BLL.Log.LocalLogBo.Instance.Write(ex, "GZSearchCustomsOrder");
                }
                return result;
            }
        }
    }
}