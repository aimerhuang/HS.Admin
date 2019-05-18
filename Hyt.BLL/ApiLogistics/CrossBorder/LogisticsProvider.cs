using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using System.ServiceModel;
using Hyt.BLL.CrossBorder;
using System.ServiceModel.Security;
using Hyt.Model.Transfer;
using Hyt.BLL.Order;

namespace Hyt.BLL.ApiLogistics.CrossBorder
{
    /// <summary>
    /// 东方之箭跨境物流WCF接口
    /// </summary>
    /// <remarks> 2018-1-9 廖移凤 创建</remarks>
    public class LogisticsProvider : ILogisticsProvider
    {
        const string UserName = "xinraocheng";
        const string Password = "202CB962AC59075B964B07152D234B70";
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.广州桥集拉德国际货运代理;}
        }

        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }
        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            throw new NotImplementedException();
        }
        public override Result GetOrderExpressno(string orderId)
        {
            CrossBorderLogisticsOrder mod = Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(Convert.ToInt32(orderId));
            if (mod != null)
            {
                return new Result()
                {
                    Status = true,
                    Message = mod.ExpressNo
                };
            }
            else
            {
                return new Result()
                {
                    Status = false,
                    Message = "未找到当前推送的物流信息"
                };
            }

        }
        public override Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        }

        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Result AddOrderTrade(int orderSysno)
        {
            Result result = new Result();
            var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);
            CrCustomer customer = Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(order.CustomerSysNo);

            WhWarehouse warehouseMod = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
            BsArea wareDistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(warehouseMod.AreaSysNo);
            BsArea wareCityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(wareDistrictEntity.ParentSysNo);
            BsArea wareProvinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(wareDistrictEntity.ParentSysNo);

            SoReceiveAddress srenity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(order.SysNo);
            BsArea DistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(srenity.AreaSysNo);
            BsArea CityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(DistrictEntity.ParentSysNo);
            BsArea ProvinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(CityEntity.ParentSysNo);

            var kuaidi = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetKuaidi(orderSysno);


            IList<CBSoOrderItem> datao = SoOrderBo.Instance.GetCBOrderItemsByOrderId(order.SysNo);
            order.OrderItemList = new List<SoOrderItem>();
            int ParcelNo=0;
           string GoodsName = "";
            foreach (CBSoOrderItem item in datao)
            {
                ParcelNo += item.Quantity;
                GoodsName = item.EasName;
                order.OrderItemList.Add(item);
            }
                
            CrossBorderParam cp = new CrossBorderParam();
            cp.Cost = order.FreightAmount;
            cp.OrderDateTime = order.CreateDate;
            cp.OrderNo = order.SysNo.ToString().Trim();
            cp.CustomerID = 689;
            cp.S_Address = warehouseMod.BackWarehouseName.Trim();
            cp.S_Province = wareProvinceEntity.AreaName.Trim();
            cp.S_City = wareCityEntity.AreaName.Trim();
            cp.S_County = wareDistrictEntity.AreaName.Trim();
            cp.S_Name = warehouseMod.WarehouseName.Trim();
            cp.S_Phone = warehouseMod.Phone.Trim();
            if (!string.IsNullOrEmpty(srenity.IDCardNo))
            {
                cp.R_IDNo = srenity.IDCardNo.Trim().ToUpper();
            }
            cp.R_Name = srenity.Name.Trim();
            cp.R_Phone = srenity.MobilePhoneNumber.Trim();
            cp.R_Address = srenity.StreetAddress.Trim();
            cp.R_Province = ProvinceEntity.AreaName.Trim();
            cp.R_City = CityEntity.AreaName.Trim();
            cp.R_County = DistrictEntity.AreaName.Trim();
            cp.Type = "4";
            cp.ParcelNo = ParcelNo;
            cp.GoodsName = GoodsName;
            cp.Mark = "300-012-005";
            cp.Batch = "114批";
            cp.DeliveryType = -1;
            cp.PrintSenderMsg = warehouseMod.WarehouseName.Trim();
            cp.PayType = -1;
            cp.Company = kuaidi.Company ;
            cp.Date = kuaidi.Date;
            cp.TrackingNo = kuaidi.TrackingNo;


            using (ChannelFactory<IExpress> channelFactory = new ChannelFactory<IExpress>("WSHttpBinding_IExpress"))
            {
                UserNamePasswordClientCredential credential = channelFactory.Credentials.UserName;
                credential.UserName = UserName;
                credential.Password = Password;
                IExpress calculator = channelFactory.CreateChannel();
              
                #region 参数
                T_Express express = new T_Express
                {
                    PicLocalNo = cp.PicLocalNo,
                    Batch = cp.Batch,
                    Type = cp.Type,
                    S_Name = cp.S_Name,
                    S_Phone = cp.S_Phone,
                    S_Province = cp.S_Province,
                    S_City = cp.S_City,
                    S_County = cp.S_County,
                    S_Address = cp.S_Address,
                    R_Name = cp.R_Name,
                    R_Phone = cp.R_Phone,
                    R_Province = cp.R_Province,
                    R_City = cp.R_City,
                    R_County = cp.R_County,
                    R_Address = cp.R_Address,
                    R_IDNo = cp.R_IDNo,
                    OrderNo = cp.OrderNo,
                    GoodsName = cp.GoodsName,
                    CollectionPayment = cp.CollectionPayment,
                   
                    Cost = cp.Cost,
                    OrderDateTime = cp.OrderDateTime,
                    Account = cp.Account,
                    BankAccount = cp.BankAccount,
                    PayType = cp.PayType,
                    DeliveryType = cp.DeliveryType,
                    Mark = cp.Mark,
                    PrintSenderMsg = cp.PrintSenderMsg,
                    ParcelNo = cp.ParcelNo.ToString(),
                    Date = DateTime.Parse(cp.Date),
                     Company = cp.Company,
                    TrackingNo = cp.TrackingNo,
                    CustomerID = cp.CustomerID

                    #region 测试数据
                    // //预留溯源码
                    //Batch = "114批",
                    ////已下订单,未受理
                    //Type = "4",
                    //S_Name = "货之家",
                    //S_Phone = "15802026993",
                    //S_Province = "广东省",
                    //S_City = "广州市",
                    //S_County = "南沙区",
                    //S_Address = "货之家保税仓",
                    //R_Name = "周恒",
                    //R_Phone = "13662656512",
                    //R_Province = "四川省",
                    //R_City = "达州市",
                    //R_County = "开江县",
                    //R_Address = "新宁镇接龙桥惠民社区5号楼三楼八号",
                    //R_IDNo = "440103698954523618",
                    //OrderNo = "Test_70151538959",
                    ////快递面单，底部信息
                    //GoodsName = "日用品",
                    //Company = "圆通",
                    //TrackingNo = "81257111826892",
                    //Cost = 0,
                    ////订单日期
                    //OrderDateTime = DateTime.Parse("2018-01-08 09:48:23"),
                    //PayType = -1,
                    //DeliveryType = -1,
                    ////三段码、大头笔
                    //Mark = "300-012-005",
                    ////覆盖详细的发件信息
                    //PrintSenderMsg = "货之家南沙保税仓",
                    //ParcelNo = "1",
                    ////快递单日期
                    //Date = DateTime.Parse("2018-01-08 09:48:23"),
                    ////新绕城,固定为689
                    //CustomerID = 689, 

                    //PicLocalNo = null,
                    //CollectionPayment = null,
                    //可空 = null,
                    //BankAccount = null,
                    #endregion
                };
                #endregion

                result.Status= calculator.ExpressSaveOrUpdate(express);

            }
            return result;
        }

    }

    /// <summary>
    /// 东方之箭跨境物流WCF接口参数
    /// </summary>
    /// <remarks> 2018-1-9 廖移凤 创建</remarks>
    public class CrossBorderParam
    {
        /// <summary>
        /// 打印广告图片号 可空
        /// </summary>
        public string PicLocalNo { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 物流状态
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public string S_Name { get; set; }
        /// <summary>
        /// 发件人手机
        /// </summary>
        public string S_Phone { get; set; }
        /// <summary>
        /// 发件人省
        /// </summary>
        public string S_Province { get; set; }
        /// <summary>
        /// 发件人市
        /// </summary>
        public string S_City { get; set; }      
        /// <summary>
        /// 发件人区
        /// </summary>
        public string S_County { get; set; }      
        /// <summary>
        /// 发件人地址
        /// </summary>
        public string S_Address { get; set; }      
        /// <summary>
        /// 收件人
        /// </summary>
        public string R_Name { get; set; }      
        /// <summary>
        /// 收件人手机
        /// </summary>
        public string R_Phone { get; set; }      
        /// <summary>
        /// 收件人省
        /// </summary>
        public string R_Province { get; set; }      
        /// <summary>
        /// 收件人市
        /// </summary>
        public string R_City { get; set; }
        /// <summary>
        /// 收件人区
        /// </summary>
        public string R_County { get; set; }
        /// <summary>
        /// 收件人身份证
        /// </summary>
        public string R_IDNo { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        public string R_Address { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 货物名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 代收货款 可空
        /// </summary>
        public string CollectionPayment { get; set; }
     
        /// <summary>
        /// 运费
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime OrderDateTime { get; set; }
        /// <summary>
        /// 代收货款_开户名 可空
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 代收货款_银行账号 可空
        /// </summary>
        public string BankAccount { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 货物方式
        /// </summary>
        public int DeliveryType { get; set; }
        /// <summary>
        /// 大头笔
        /// </summary>
        public string Mark { get; set; }
        /// <summary>
        /// 打印面单发件信息
        /// </summary>
        public string PrintSenderMsg { get; set; }
        /// <summary>
        /// 包裹数量 可空
        /// </summary>
        public int ParcelNo { get; set; }
   
        /// <summary>
        /// 客户编号
        /// </summary>
        public int CustomerID { get; set; }
        /// <summary>
        /// 快递单日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string TrackingNo { get; set; }
    }

}
