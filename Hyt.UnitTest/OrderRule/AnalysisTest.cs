using System;
using System.Collections.Generic;
using System.Diagnostics;
using Hyt.Admin.Controllers;
using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Hyt.BLL.OrderRule;

namespace Hyt.UnitTest.OrderRule
{
    [TestClass]
    public class AnalysisTest
    {
        public AnalysisTest()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }

        [TestMethod]
        public void Test对内备注包含()
        { 
            string c = "对内备注包含:升舱订单、傻逼";
            var command = new BLL.OrderRule.Command_对内备注包含().Parse(c);
            var orderData = new BLL.OrderRule.OrderData();
            orderData.Order = new Model.SoOrder() { InternalRemarks = "升舱订单" };
            Assert.IsTrue(command.Result(orderData));
            orderData.Order.InternalRemarks = "傻逼";
            Assert.IsTrue(command.Result(orderData));
            orderData.Order.InternalRemarks = "XXX";
            Assert.IsFalse(command.Result(orderData));

            //测试订单InternalRemarks为null的情况
            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);
            Assert.IsFalse(command.Result(orderData));
        }

        [TestMethod]
        public void Test购买了商品()
        {
            string c = "购买了商品031130103001、031130314001";
            var command = new BLL.OrderRule.Command_购买了商品().Parse(c);
            var orderData = new BLL.OrderRule.OrderData();
            orderData.OrderItems = new List<SoOrderItem>();
            orderData.OrderItems.Add(new SoOrderItem()
                {
                    ProductSysNo = 2629//031130314001
                });
            Assert.IsTrue(command.Result(orderData));
            orderData.OrderItems.Clear();
            orderData.OrderItems.Add(new SoOrderItem()
            {
                ProductSysNo = 1//031130103001
            });
            Assert.IsTrue(command.Result(orderData));
            orderData.OrderItems.Clear();
            orderData.OrderItems.Add(new SoOrderItem()
            {
                ProductSysNo = 2730
            });
            Assert.IsFalse(command.Result(orderData));
        }

        [TestMethod]
        public void Test分销商城编号是()
        {
            string c = "分销商城编号是：1、3";
            //var command = new BLL.OrderRule.Command_分销商城编号是().Parse(c);
            var orderData = new BLL.OrderRule.OrderData();
            orderData.Order = new Model.SoOrder() {   OrderSource=80, OrderSourceSysNo=1};
            //Assert.IsTrue(command.Result(orderData));
            //orderData.Order.OrderSourceSysNo = 3;
            //Assert.IsTrue(command.Result(orderData));
            //orderData.Order.OrderSourceSysNo = 5;
            //Assert.IsFalse(command.Result(orderData));
        }

        [TestMethod]
        public void Test仓库名称是()
        {
            string c = "仓库名称是：广州海印总库";
            var command = new BLL.OrderRule.Command_仓库名称是().Parse(c);
            var orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);
            Assert.IsTrue(command.Result(orderData));
          
        }

        [TestMethod]
        public void Test分销商城名称是()
        {
            string c = "分销商城名称是：品胜数码旗舰店、QQ品胜旗舰店";
            //var command = new BLL.OrderRule.Command_分销商城名称是().Parse(c);
            var orderData = new BLL.OrderRule.OrderData();
            //orderData.Order = new Model.SoOrder() { OrderSource = 80, OrderSourceSysNo = 1 };
            //Assert.IsTrue(command.Result(orderData));
            //orderData.Order = new Model.SoOrder() { OrderSource = 80, OrderSourceSysNo = 3 };
            //Assert.IsTrue(command.Result(orderData));
            //orderData.Order = new Model.SoOrder() { OrderSource = 80, OrderSourceSysNo = 5 };
            //Assert.IsFalse(command.Result(orderData));
        }

        [TestMethod]
        public void Test只购买了商品()
        {
            string c = "只购买了商品：031130103001、031130314001";
            var command = new BLL.OrderRule.Command_只购买了商品().Parse(c);
            var orderData = new BLL.OrderRule.OrderData();
            orderData.OrderItems = new List<SoOrderItem>();
            orderData.OrderItems.Add(new SoOrderItem()
            {
                ProductSysNo = 2629//031130314001
            });
            Assert.IsTrue(command.Result(orderData));
            orderData.OrderItems.Add(new SoOrderItem()
            {
                ProductSysNo = 1//031130103001
            });
            Assert.IsTrue(command.Result(orderData));
            orderData.OrderItems.Add(new SoOrderItem()
            {
                ProductSysNo = 2730
            });
            Assert.IsFalse(command.Result(orderData));
        }
        
        

        [TestMethod]
        public void Test订单金额大于()
        {
            string c = "订单金额大于50.01";
            var command = new BLL.OrderRule.Command_订单金额大于().Parse(c);

            var orderData = new  BLL.OrderRule.OrderData();
            orderData.Order = new Model.SoOrder();
            orderData.Order.OrderAmount = 50.01m;

            Assert.IsFalse(command.Result(orderData));

            orderData.Order.OrderAmount = 50.02m;
            Assert.IsTrue(command.Result(orderData));

        }

        [TestMethod]
        public void Test收货地区()
        {
            string c = "收货地区省是：四川、北京、广东";
            var command = new BLL.OrderRule.Command_收货地区().Parse(c);

            var orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);

            Assert.IsTrue(command.Result(orderData));

            c = "收货地区市是：成都、北京、重庆";
            command = new BLL.OrderRule.Command_收货地区().Parse(c);
            Assert.IsFalse(command.Result(orderData));

            c = "收货地区区是:金牛区、北关区、武侯区";
            command = new BLL.OrderRule.Command_收货地区().Parse(c);
            Assert.IsFalse(command.Result(orderData));

            c = "收货地区省是：江苏、北京、广东";
            command = new BLL.OrderRule.Command_收货地区().Parse(c);
            Assert.IsTrue(command.Result(orderData));

            orderData.Order = SoOrderBo.Instance.GetEntity(1);

            Assert.IsFalse(command.Result(orderData));

        }

        [TestMethod]
        public void Test收货地区和仓库匹配()
        {
            string c = "收货地区和仓库匹配";
            var command = new BLL.OrderRule.Command_收货地区和仓库匹配().Parse(c);

            var orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);

            Assert.IsTrue(command.Result(orderData));

            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(1467123);

            Assert.IsFalse(command.Result(orderData));

            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(0);

            Assert.IsFalse(command.Result(orderData));
            
        }

        [TestMethod]
        public void Test第三方快递()
        {
            string c = "第三方快递";
            var command = new BLL.OrderRule.Command_第三方快递().Parse(c);

            var orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);

            Assert.IsFalse(command.Result(orderData));

            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746523);

            Assert.IsTrue(command.Result(orderData));

            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(0);

            Assert.IsFalse(command.Result(orderData));

        }

        [TestMethod]
        public void Test百城当日达()
        {
            string c = "百城当日达";
            var command = new BLL.OrderRule.Command_百城当日达().Parse(c);

            var orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);

            Assert.IsTrue(command.Result(orderData));

            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746523);

            Assert.IsFalse(command.Result(orderData));

            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(0);

            Assert.IsFalse(command.Result(orderData));

        }

        [TestMethod]
        public void Test仓库有库存()
        {
            string c = "仓库有库存";
            var command = new BLL.OrderRule.Command_仓库有库存().Parse(c);

            //测试所有商品数量都为1的情况（不超过）
            var orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);
            orderData.OrderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(746544);
            orderData.OrderItems.ForEach(x => x.Quantity = 1);
            Assert.IsTrue(command.Result(orderData));

            //测试所有商品数量都为1000的情况（超过）
            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);
            orderData.OrderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(746544);
            orderData.OrderItems.ForEach(x=>x.Quantity=1000);
            Assert.IsFalse(command.Result(orderData));

            //测试有一个商品数量为1000的情况
            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);
            orderData.OrderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(746544);
            var i = 0;
            orderData.OrderItems.ForEach(x => {
                                                  if (i != 0) return;
                                                  x.Quantity = 1000;
                                                  i++;
            });
            Assert.IsFalse(command.Result(orderData));

        }

        [TestMethod]
        public void TestParseCommands() {

            var list = Hyt.BLL.OrderRule.OrderEngine.Instance.ParseCommands("订单金额大于50.01,订单金额大于50,订单金额大于50 or 商品数量大于10");


        }
        [TestMethod()]
        public void HandlerUpGradesOrderTest()
        {
            var orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);
            orderData.OrderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(746544);
            Hyt.BLL.OrderRule.OrderEngine.Instance.HandlerUpGradesOrder(orderData);
            var rule = "[{Name:'升舱订单自动处理',Rule:\"{不自动审核:'',自动审核:'仓库有库存'}\"}]";
            Hyt.BLL.OrderRule.OrderEngine.Instance.handlerThings = Hyt.BLL.OrderRule.OrderEngine.Instance.GetThings(rule);
            Hyt.BLL.OrderRule.OrderEngine.Instance.HandlerUpGradesOrder(orderData);

            rule = "[{Name:'升舱订单自动处理',Rule:\"{不自动审核:'',自动审核:'对内备注包含升舱 and 购买了商品746544 or not(只购买了商品746544 and 订单金额大于50) and 商品数量大于10 and 分销商城名称是品胜数码旗舰店 and 分销商城编号是1、3 and 收货地区省是：四川、北京、广东'}\"}]";
            Hyt.BLL.OrderRule.OrderEngine.Instance.handlerThings=Hyt.BLL.OrderRule.OrderEngine.Instance.GetThings(rule);
            Hyt.BLL.OrderRule.OrderEngine.Instance.HandlerUpGradesOrder(orderData);

            rule = "[{Name:'升舱订单自动处理',Rule:\"{不自动审核:'not(对内备注包含升舱)',自动审核:'对内备注包含升舱 and 购买了商品746544 or not(只购买了商品746544 and 订单金额大于50) and 商品数量大于10 and 分销商城名称是品胜数码旗舰店 and 分销商城编号是1、3 and 收货地区省是：四川、北京、广东'}\"}]";
            Hyt.BLL.OrderRule.OrderEngine.Instance.handlerThings = Hyt.BLL.OrderRule.OrderEngine.Instance.GetThings(rule); 
            Hyt.BLL.OrderRule.OrderEngine.Instance.HandlerUpGradesOrder(orderData);
        }

        [TestMethod()]
        public void SyConfigHandlerUpGradesOrderTest()
        {
            var watch = new Stopwatch();
            //不自动审核订单
            watch.Start();
            var orderData = new BLL.OrderRule.OrderData();
            //orderData.Order = SoOrderBo.Instance.GetEntity(746544);
            //orderData.OrderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(746544);
            //Hyt.BLL.OrderRule.OrderEngine.Instance.HandlerUpGradesOrder(orderData);
            //watch.Stop();
            //System.Diagnostics.Debug.Write(string.Format("不自动审核订单{0}执行时间:" + watch.ElapsedMilliseconds+"毫秒", 746544));

            //自动审核订单
            watch.Start();
            orderData = new BLL.OrderRule.OrderData();
            orderData.Order = SoOrderBo.Instance.GetEntity(746544);
            orderData.OrderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(746544);
           Hyt.BLL.OrderRule.OrderEngine.Instance.HandlerUpGradesOrder(orderData);
            watch.Stop();
            System.Diagnostics.Debug.Write(string.Format("自动审核订单{0}执行时间:" + watch.ElapsedMilliseconds + "毫秒", 1465914));
        }

         [TestMethod()]
        public void GetThingsTest()
         {
             var strJosn = "[{Name:'升舱订单自动处理',Rule:'{}'}]";
             var list= Hyt.BLL.OrderRule.OrderEngine.Instance.GetThings(strJosn);
             Assert.IsNotNull(list);

             strJosn = "[{Name:\"升舱订单自动处理\",Rule:\"{不自动审核:'',自动审核:'订单金额大于50 or 商品数量大于10'}\"}]";
             list = Hyt.BLL.OrderRule.OrderEngine.Instance.GetThings(strJosn);
             Assert.IsNotNull(list);

             strJosn = "[{Name:\"升舱订单自动处理\",Rule:\"{不自动审核:'',自动审核:'对内备注包含升舱 and 购买了商品746544 or not(只购买了商品746544 and 订单金额大于50) and 商品数量大于10 and 分销商城名称是品胜数码旗舰店 and 分销商城编号是1、3 and 收货地区省是：四川、北京、广东'}\"}]";
             list = Hyt.BLL.OrderRule.OrderEngine.Instance.GetThings(strJosn);
             Assert.IsNotNull(list);
         }

       [TestMethod]
        public void TestToPostfixExpression() {
            //a+b*c+(d*e+f)*g转换成abc*+de*f+g*+
            // a or b and c or (d and e or f) and g转换成a b c and  or d e and f or g and  or 

            string command = "a or b and c or (d and e or f) and g";
            var list = Hyt.BLL.OrderRule.OrderEngine.Instance.ToPostfixExpression(command);
            Assert.AreEqual(list.Count, 13);
           Assert.AreEqual(list[0],"a");
           Assert.AreEqual(list[12], "or");
           Assert.AreEqual(list[3], "and");

        }
         [TestMethod]
        public void TestUpgradeOrderHandler()
         {
                new ApiController().UpgradeOrderHandler("746729");
         }
       [TestMethod]
       public void TestParseCommand() {
           var orderData = new BLL.OrderRule.OrderData();
           orderData.Order = new Model.SoOrder();
           orderData.Order.OrderAmount = 50
           orderData.Order.CustomerMessage = "请发圆通快递";
           orderData.OrderItems = new List<SoOrderItem>();
           orderData.OrderItems.Add(new SoOrderItem()
           {               
               Quantity = 3
           });
           orderData.OrderItems.Add(new SoOrderItem()
           {
               Quantity = 1
           });

           var command = Hyt.BLL.OrderRule.OrderEngine.Instance.ParseCommand(" 订单金额大于40 And 商品数量大于6");
           Assert.AreEqual(command.Result(orderData), false);
           command = Hyt.BLL.OrderRule.OrderEngine.Instance.ParseCommand("订单金额大于40 oR 商品数量大于6 ");
           Assert.AreEqual(command.Result(orderData), true);
           command = Hyt.BLL.OrderRule.OrderEngine.Instance.ParseCommand("nOt(订单金额大于40)");
           Assert.AreEqual(command.Result(orderData), false);

           

       }

    }
}
