using System;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Util.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.Model.UpGrade;
using Newtonsoft.Json.Linq;
using Extra.UpGrade.Model;
using System.Security.Cryptography;
using System.IO;
using Hyt.Model;
using Extra.UpGrade.Provider;
using System.Collections.Generic;
using System.Linq;
using Hyt.BLL.Base;
namespace Hyt.UnitTest
{
    [TestClass]
    public class UtilTest
    {

        public UtilTest()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        [TestMethod]
        public void TestEnumToList()
        {


            string xmlStr = System.IO.File.ReadAllText("推单请求报文.xml");
            Hyt.Util.Xml.XmlDocumentExtender xms = new Hyt.Util.Xml.XmlDocumentExtender();
            xms.LoadXml(xmlStr);

            var order = new UpGradeOrder();                
           
            
            //商城订单明细
            order.UpGradeOrderItems = new System.Collections.Generic.List<UpGradeOrderItem>();
           
            var orders=xms.SelectNodes("/HipacPush/Body/OrderItemList/OrderItem");
            for (int i = 0; i < orders.Count; i++)
            {
                var item = orders[i];
                var code = item["itemSupplyNo"].InnerText;

                int specNum = int.Parse(item["specNum"].InnerText);//规格数
                decimal tariffRate = decimal.Parse(item["tariffRate"].InnerText);//关税
                decimal exciseRate = decimal.Parse(item["exciseRate"].InnerText);//消费税
                decimal addTaxRate = decimal.Parse(item["addTaxRate"].InnerText);//增值税

                decimal itemTotalTax = decimal.Parse(item["itemTotalTax"].InnerText);//总税款
                decimal itemTotal = decimal.Parse(item["itemTotal"].InnerText);//商品总价

                order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                {
                    MallProductName = item["itemName"].InnerText,
                    MallProductCode = code,               
                    MallPrice = decimal.Parse(item["itemPrice"].InnerText),
                    MallAmount = itemTotal,
                    Quantity = int.Parse(item["itemQuantity"].InnerText) * specNum,           
                });

            }

            var receiveInfo = xms.SelectSingleNode("/HipacPush/Body/Customer");

            //订单收货信息   
            order.MallOrderReceive = new MallOrderReceiveInfo()
            {
                City = receiveInfo["custCity"].InnerText,
                Province = receiveInfo["custProvice"].InnerText,
                District = receiveInfo["custArea"].InnerText,
                ReceiveAddress = receiveInfo["custAddress"].InnerText,
                ReceiveContact = receiveInfo["custName"].InnerText,
                Mobile = receiveInfo["custPhone"].InnerText,
                IdCard = receiveInfo["custIdNum"].InnerText,
            };
          

         
           var payInfo= xms["HipacPush"]["Body"]["PayInfo"];
           var _order = xms["HipacPush"]["Body"]["Order"];



           
           order.HytOrderDealer = new HytOrderDealerInfo()
           {
               //第三方订单编号              
               HytPayType = Convert.ToInt32(payInfo["payType"].InnerText),//支付类型（微信支付，支付宝，盛付通）
               HytPayTime = DateTime.Parse(payInfo["payTime"].InnerText),
               HytPayment= decimal.Parse(_order["totalPayAmount"].InnerText),
               DealerSysNo=0,
               DealerMallSysNo=0,
           };          
              

   
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {               
                AlipayNo = payInfo["payNo"].InnerText,
                PayTime = DateTime.Parse(payInfo["payTime"].InnerText),            
                Payment = decimal.Parse(_order["totalPayAmount"].InnerText),
                PostFee = decimal.Parse(_order["logisticsAmount"].InnerText),
                TotalTaxAmount = decimal.Parse(_order["totalTaxAmount"].InnerText),
            };
     


      

         //   string dd = DateTime.Now.AddDays(-1 * 14).ToString();
            //var list = EnumUtil.ToDictionary(typeof(BasicStatus.配送方式));

            //var name = EnumUtil.GetDescription(typeof(BasicStatus.配送方式), 2);

            //var description = BasicStatus.配送方式.第三方快递.GetDescription();
        }


        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>

        private string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));

            string outString = "";
            for (int i = 0; i < result.Length; i++)
            {
                outString += result[i].ToString("x2");
            }

            return outString;

        }
        [TestMethod]
        public void SendDelivery()
        {
            var param = new DeliveryParameters() { MallOrderId="200008" };
            var auth = new AuthorizationParameters()
            {
                MallType = (int)DistributionStatus.商城类型预定义.海拍客,
            };

            int dealerMallSysNo = 25;
            var mallInfo = BLL.Distribution.DsDealerMallBo.Instance.GetEntity(dealerMallSysNo);
          
            var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(mallInfo.DealerAppSysNo);


            var instance = UpGradeProvider.GetInstance(auth.MallType);

            auth.DealerApp = appInfo;

            var result = instance.SendDelivery(param,auth);
        }
        [TestMethod]
        public void HipacOrderReceipt()
        {

            var param = new OrderParameters();
            var auth = new AuthorizationParameters()
            {
                MallType = (int)DistributionStatus.商城类型预定义.海拍客,
            };

            string sendId = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100000,999999).ToString();
            string sign = "";
            string appKey="";
           
            param.Xml =System.IO.File.ReadAllText("推单请求报文.xml");;

            //return Content("");
          

         
           
            //var sr = new StreamReader(Request.InputStream);
            //string responseStr = sr.ReadToEnd();
            //param.Xml = responseStr;

            BLL.Log.LocalLogBo.Instance.Write(param.Xml, "HipacOrderReceiptLog");
            int dealerMallSysNo = 25;
            var entity = new DsDealerLog();
            entity.MallTypeSysNo = auth.MallType;
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = 0;
            entity.LastUpdateBy = 0;
            entity.LastUpdateDate = DateTime.Now;

            try
            {
                var instance = UpGradeProvider.GetInstance(auth.MallType);
                var result = instance.GetUpGradedWaitSend(param, auth);
                if (result.Status)
                {
                    var mallInfo = BLL.Distribution.DsDealerMallBo.Instance.GetEntity(dealerMallSysNo);
                    var dic = result.Data.First().UpGradeOrderItems.ToDictionary(x => x.MallProductCode);
                    var erpCodeList = dic.Keys.ToList();
                    var productList = BLL.Product.PdProductBo.Instance.GetProductListByErpCode(erpCodeList);
                    var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(mallInfo.DealerAppSysNo);

                    var upGradeOrder = result.Data.First();
                    upGradeOrder.HytOrderDealer.DealerMallSysNo = dealerMallSysNo;
                    upGradeOrder.HytOrderDealer.DealerSysNo = mallInfo.DealerSysNo;
                    upGradeOrder.HytOrderDealer.IsSelfSupport = mallInfo.IsSelfSupport;
                    var orderInfo = result.Data.First();
                    appKey = appInfo.AppKey;



                    string signStr = "appKey=" + appInfo.AppKey + "&sendID=" + sendId + "&key=" + appInfo.AppSecret;
                    sign = MD5Encrypt(signStr).ToUpper();

                    //var _result = BLL.Order.SoOrderBo.Instance.ImportMallOrder(new List<UpGradeOrder>() { upGradeOrder }, productList);

                    //if (!_result.Status)
                    //{

                    //    entity.MallOrderId = orderInfo.MallOrderBuyer.MallOrderId;
                    //    //sendID,appKey,orderNum,service,custName,payNo
                    //    entity.LogContent = _result.Message;
                    //    entity.Status = 10;
                    //    BLL.MallSeller.DsDealerLogBo.Instance.Insert(entity);
                    //}
                }
                else
                {
                    entity.MallOrderId = result.Data.First().MallOrderBuyer.MallOrderId;
                    entity.LogContent = result.Message;
                    entity.Status = 10;
                    BLL.MallSeller.DsDealerLogBo.Instance.Insert(entity);
                }
            }
            catch
            {
                entity.LogContent = param.Xml;
                entity.Status = 10;
                BLL.MallSeller.DsDealerLogBo.Instance.Insert(entity);
            }

            string content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> ";
            content += "<HipacPush>";
            content += "<Head>";
            content += "  <version>1.0</version>";
            content += "  <service>pushOrderInfo</service>";
            content += "  <sendID>" + sendId + "</sendID>";
            content += "  <appKey>" + appKey + "</appKey> ";
            content += " <retCode>SUCCESS</retCode> ";
            content += "  <sign>" + sign + "</sign>  ";
            content += "</Head>";
            content += "<Body>";
            content += "   <bizCode>SUCCESS</bizCode>";
            content += "   <retMsg>成功</retMsg>";
            content += " </Body>  ";
            content += "</HipacPush>";

             
        
        }
    }
}
