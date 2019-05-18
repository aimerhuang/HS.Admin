using System;
using System.Linq;
using Hyt.Infrastructure.Communication;
using Hyt.Service.Contract;
using Hyt.Service.Contract.B2CApp;
using Hyt.Service.Contract.MallSeller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.Service.Contract.LogisApp;
using Hyt.Model.LogisApp;
using System.Collections.Generic;
using Newtonsoft.Json;
using Hyt.BLL.Base;
using Hyt.Model.SystemPredefined;
using Hyt.BLL.ScheduledEvents;
using System.Threading;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using Grand.Platform.Api.Contract.DataContract;
using Hyt.BLL.Order;
using Hyt.Model;

namespace Hyt.UnitTest
{
    [TestClass]
    public class WcfTest
    {
        public WcfTest()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        [TestMethod]
        public void TestMethod1()
        {

            var request = new MemberSynchronizeRequest()

            {

                PageSize = 100,
                CurrnetPage = 1,
                LastSynchronizeDate = DateTime.Parse("2016-01-01"),
            };
            
            using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Api.Contract.IPosService>())
            {
                var response = service.Channel.MemberSynchronize(request);
                Assert.AreEqual(true, response.RmVipList.Items.Count > 0);
            }  
            //using (var service = new ServiceProxy<Grand.Platform.Api.Contract.IPosService>())
            //{
            //    string time = Convert.ToString(DateTime.Parse("2016-5-01"));
            //    var res = service.Channel.MemberSynchronize(time);
            //}

            //using (var service = new ServiceProxy<IAccount>())
            //{
            //    var res = service.Channel.GetRegistAgreement();
            //}
        }
        static Timer eventTimer;
        [TestMethod]
        public void ScheduledEventTest()
        {

            string url="http://127.0.0.1:8040/MallOrderService.svc";

            url = "http://server.singingwhale.cn/Task/TaskService.svc/ExecuteOrderToXinYeErp";

           // url = "http://server.singingwhale.cn/Task/TaskService.svc/ExecuteOrderToErp";
            Hyt.Util.WebUtil.PostJson(url, "");

            //if (eventTimer == null)
            //{
            //    EventLogs.LogFileName = Hyt.Util.WebUtil.GetMapPath(string.Format("cache/scheduleeventfaildlog.config"));
            //    EventManager.RootPath = Hyt.Util.WebUtil.GetMapPath("/");
            //    eventTimer = new Timer(new TimerCallback(ScheduledEventWorkCallback), null, 0, EventManager.TimerMinutesInterval * 60000);
            //}
            //else
            //{
            //    EventManager.UpdateTimeByKeyAndDealerSysNo("ClearJsTicketEvent",0);
            //}
        }
        /// <summary>
        /// 定时器回调方法
        /// </summary>
        /// <param name="sender"></param>
        /// <remarks>2016-5-4 杨浩 添加</remarks>
        private void ScheduledEventWorkCallback(object sender)
        {
            try
            {
                EventManager.Execute();
            }
            catch
            {
                EventLogs.WriteFailedLog("Failed ScheduledEventCallBack");
            }

        }
        /// <summary>
        /// 二次销售单元测试
        /// </summary>
        [TestMethod]
        public void CreateSoOrderToSettlement()
        {
            int ordsysno = 746566;//订单模板
            using (var service = new ServiceProxy<ILogistics>())
            {
              
               var rr= service.Channel.SelectProductLowestPrice("1");
               var rx = service.Channel.SelectProductPrice("1");
                var res=service.Channel.Login("natoop", "123456");
                service.Channel.ModifyPassword("123456", "123456");
                AppOrder order = new AppOrder();
                order.SoOrder = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(ordsysno);
                order.Products = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(ordsysno);
                order.SoReceiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.SoOrder.ReceiveAddressSysNo);

               var kkdd= service.Channel.CreateCustomer(new AppCBCrCustomer()
                {
                    AreaSysNo = order.SoReceiveAddress.AreaSysNo,
                     Name="tttttttt",
                     MobilePhoneNumber="12928914770",

                });
                AppOrder2 wcforder = new AppOrder2();
                wcforder.SoReceiveAddress = order.SoReceiveAddress;
                wcforder.OrderItems = order.Products.Select(m => new AppOrderItem()
                {
                    Price = m.SalesUnitPrice-0.05M,
                    ProductName = m.ProductName,
                    Quantity = m.Quantity+1,
                    SysNo = m.ProductSysNo
                }).ToList();

                wcforder.Order = new AppShopCartOrder()
                {
                    CustomerMessage = "二次销售客户留言",
                    InternalRemarks = "二次销售对内备注",
                    ContactBeforeDelivery = 1,
                    DeliveryRemarks = "二次销售配送备注",
                    PayTypeSysNo = PaymentType.现金预付,
                    DeliveryTypeSysNo = DeliveryType.普通百城当日达,
                    CustomerSysNo = order.SoOrder.CustomerSysNo
                };
                var json = JsonConvert.SerializeObject(wcforder);
               var rp= service.Channel.CreateSoOrderToSettlement(wcforder);
                //service.Channel.CreateSoOrderToAudit(wcforder);  
            }
        }
        private void AddDelaration(XmlDocument doc)
        {
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.InsertBefore(xmlDeclaration, doc.DocumentElement);
        }


        /// <summary>
        /// 任务计划测试
        /// </summary>
        [TestMethod]
        public void TaskTest()
        {
            #region 测试
            //Encoding encoding = Encoding.UTF8;
            ////byte[] data = encoding.GetBytes(postData);
            ////此处为为http请求url  
            //var uri = new Uri("http://127.0.0.1:8070/Task/TaskService.svc/ExecuteRebatesRecord");
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            ////用此方法可以添加标准或非标准http请求，诸如conten-type ,accept,range等  
            ////request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
            ////此处为C#实现的一些标准http请求头添加方法，用上面的方面也可以实现  
            //request.ContentType = "application/json";
            //request.Accept = "application/json";
            ////此处添加标准http请求方面  
            //request.Method = "POST";
            //System.IO.Stream sm = request.GetRequestStream();
            ////sm.Write(data, 0, data.Length);
            //sm.Close();
            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //Stream streamResponse = response.GetResponseStream();
            //StreamReader streamRead = new StreamReader(streamResponse, Encoding.UTF8);
            //Char[] readBuff = new Char[256];
            //int count = streamRead.Read(readBuff,0,256);
            ////content为http响应所返回的字符流  
            //String content = "";
            //while (count > 0)
            //{
            //    String outputData = new String(readBuff, 0, count);
            //    content += outputData;
            //    count = streamRead.Read(readBuff, 0, 256);
            //}
            //response.Close();
            #endregion
            //using (var service = new ServiceProxy<Hyt.Service.Contract.Task.ITaskService>())
            //{
            //    var result = service.Channel.ExecuteRebatesRecord();
            //}

            var orderInfo = Hyt.BLL.Web.SoOrderBo.Instance.GetEntity(4347);
            //var orderItem=Hyt.BLL.Web.SoOrderBo.Instance.GetOrderItemListByOrderSysNo(4347);
            orderInfo.ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(orderInfo.ReceiveAddressSysNo);
            orderInfo.Customer = Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(orderInfo.CustomerSysNo);

            orderInfo.OnlinePayment = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentBySourceSysNo(Model.WorkflowStatus.FinanceStatus.网上支付单据来源.销售单, orderInfo.SysNo);

            
            var json =Newtonsoft.Json.JsonConvert.SerializeObject(orderInfo);
           
         

            


            Encoding myEncode = Encoding.GetEncoding("UTF-8");
            //byte[] postBytes = Encoding.UTF8.GetBytes("{\"dd\":444,\"33\":6,\"333\":{\"w\":5}}");
            byte[] postBytes = Encoding.UTF8.GetBytes(json);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://api.singingwhale.com.cn/B2CApp/Orders.svc/AddOrder");
            req.Method = "POST";
            //req.ContentType = "application/json; charset=utf-8";

            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

            req.ContentLength = postBytes.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(postBytes, 0, postBytes.Length);
            }
            using (WebResponse res = req.GetResponse())
            {
                using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                {
                    string strResult = sr.ReadToEnd();

                }
            }
         
     
        }
        [TestMethod]
        public void SearchFromDataBase()
        {
            using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Api.Contract.IMallService>())
            {
                var request = new GetOrder100Request()
                {
                    CurrentPage = 1,
                    PageSize = 5,
                    FTransactionStatus = 38192,//已付款
                    FWebshopID = "SUNING_3"//"SUNING_3",
                };
                var response = service.Channel.GetOrder100(request);
                var ss = response.UseHasNext;

            } 
            //string ss= string.Join(",", null);
           // Hyt.Service.Implement.B2CApp.Product product = new Service.Implement.B2CApp.Product();
           // product.SearchFromDataBase("",212,"",48,1,false,1,false,0,10,CustomerLevel.初级,false,0,null,0,0,0,0);
            //using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.B2CApp.IProduct>())
            //{
            //    var response = service.Channel.SearchFromDataBase("", 212, "", 40, 1, false, 1, true, 0, 10, CustomerLevel.初级, false, 0, null, 0, 0, 0, 0);
            //    Assert.AreEqual(true, response.RecCount > 0);
            //}  
        }
        [TestMethod]
        public void Search()
        {
            var items = new List<FeProductItem>()
            {
                new FeProductItem()
                {
                    ProductSysNo=5241,
                },
            };

            var productSysNoList = new StringBuilder();

            foreach (var item in items)
            {
                if (productSysNoList.Length != 0)
                    productSysNoList.Append(",");

                productSysNoList.Append(item.ProductSysNo.ToString());
            }
            string p = productSysNoList.ToString();
            var product = new Service.Implement.B2CApp.Product();
            var productList = product.Search(0, "4338,4364,10", 10, CustomerLevel.初级);
            using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.B2CApp.IProduct>())
            {
               var response = service.Channel.Search(0, productSysNoList.ToString(), 10, CustomerLevel.初级);
                Assert.AreEqual(true,response.Data!=null&&response.Data.Count > 0);
            }  
        }
        [TestMethod]
        public void ImportERPOrder100()
        {
            BLL.Order.SoOrderBo.Instance.ImportERPOrder100();
        }
    }
}
;