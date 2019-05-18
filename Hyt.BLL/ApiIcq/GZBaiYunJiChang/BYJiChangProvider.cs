using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Util.Net;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Common;
using Hyt.BLL.ApiIcq;
using Hyt.BLL.Authentication;
using Hyt.DataAccess.Icp;
using System.IO;
using Hyt.BLL.Order;
using Hyt.Model.Parameter;
using Hyt.BLL.CRM;
using Hyt.Model.Transfer;

namespace Hyt.BLL.ApiIcq.GZBaiYunJiChang
{
    /// <summary>
    /// 广州白云机场商检
    /// </summary>
    /// <remarks>2016-3-19 杨浩 创建</remarks>
    public class BYJiChangProvider : IIcqProvider
    {
       
        /// <summary>
        /// 商检代码
        /// </summary>
        /// <remarks>2016-3-19 杨浩 创建</remarks>
        public override CommonEnum.商检 Code 
        { 
            get
            {
                return CommonEnum.商检.广州白云机场;
            }
        }
        /// <summary>
        /// 商检配置
        /// </summary>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        protected static IcpInfoConfig config = Hyt.BLL.Config.Config.Instance.GetIcqInfoConfig();

        #region 商品备案
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IcpType"></param>
        /// <param name="ProductSysNoList"></param>
        /// <returns></returns>
        /// <remarks>2016-4-1 王耀发 创建</remarks>
        public override Result PushGoods(string ProductSysNoList)
        {
            Result result = new Result();

            CIcp model = new CIcp();
            model.IcpType = (int)Model.CommonEnum.商检.广州白云机场;
            List<CIcpGoodsItem> CIcpGoodsItemList = new List<CIcpGoodsItem>();
            string[] sArray = ProductSysNoList.Split(',');

            foreach (string i in sArray)
            {
                int ProductSysNo = int.Parse(i);
                IcpBYJiChangGoodsInfo Entity = IcpBo.Instance.GetIcpBYJiChangGoodsInfoEntityByPid(ProductSysNo);
                CIcpGoodsItemList.Add(new CIcpGoodsItem()
                {
                    IcpType = model.IcpType,
                    ProductSysNo = ProductSysNo,
                    EntGoodsNo = Entity.EntGoodsNo
                });
            }
            Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.InternationalTrade internationaltrade = new Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.InternationalTrade();
            internationaltrade.Head = new Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.Head();
            internationaltrade.Declaration = new Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.Declaration();
            internationaltrade.Declaration.GoodsRegHead = new Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsRegHead();
            internationaltrade.Declaration.GoodsRegList = new Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsRegList();
            internationaltrade.Declaration.GoodsRegList.GoodsContentList = new List<Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsContent>();
            //生成最大流水号
            string MaxSerialNumber = GetMaxSerialNumberByMType(model.IcpType.ToString(), config.GZJCIcpInfoTrade.GoodsMessageType);
            string fileName = config.GZJCIcpInfoTrade.GoodsMessageType + "_" + config.GZJCIcpInfoTrade.Sender + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MaxSerialNumber;

            internationaltrade.Head = SetIcpGoodsHead(fileName, config.GZJCIcpInfoTrade.GoodsMessageType);
            internationaltrade.Declaration.GoodsRegHead = SetIcpGoodsHeadData();

            List<Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsContent> GoodsContentList = SetIcpGoodsBodyData(CIcpGoodsItemList);

            if (GoodsContentList.Count != 0)
            {
                internationaltrade.Declaration.GoodsRegList.GoodsContentList = SetIcpGoodsBodyData(CIcpGoodsItemList);
                string str = Hyt.Util.Serialization.SerializationUtil.XmlSerialize<Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.InternationalTrade>(internationaltrade);
                str = str.Replace("encoding=\"utf-16\"", "encoding=\"UTF-8\"");

                FtpUtil ftp = new FtpUtil(config.GZJCIcpInfoTrade.FtpUrl, config.GZJCIcpInfoTrade.FtpName, config.GZJCIcpInfoTrade.FtpPassword);
                string msg = "";
                string MessageID = fileName;
                fileName = fileName + ".xml";
                try
                {
                    ftp.UploadFile(config.GZJCIcpInfoTrade.FtpUrl + "in", fileName, Encoding.UTF8.GetBytes(str), out msg);
                    //新增商检表信息
                    int UserSysNo = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    model.SourceSysNo = 0;
                    model.MessageID = MessageID;
                    model.MessageType = config.GZJCIcpInfoTrade.GoodsMessageType;
                    model.SerialNumber = MaxSerialNumber;
                    model.XmlContent = str;
                    model.CreatedBy = UserSysNo;
                    model.CreatedDate = DateTime.Now;
                    model.Status = (int)IcpStatus.商品商检推送状态.已推送;
                    model.LastUpdateBy = UserSysNo;
                    model.LastUpdateDate = DateTime.Now;
                    model.SysNo = IcpDao.Instance.Insert(model);
                    if (model.SysNo > 0) //
                    {
                        if (CIcpGoodsItemList != null)
                        {
                            foreach (var item in CIcpGoodsItemList)
                            {
                                var m = new CIcpGoodsItem
                                {
                                    SourceSysNo = 0,                    
                                    IcpType = model.IcpType,
                                    MessageID = MessageID,
                                    IcpGoodsSysNo = model.SysNo,
                                    ProductSysNo = item.ProductSysNo,
                                    EntGoodsNo = item.EntGoodsNo,
                                    CreatedBy = UserSysNo,
                                    CreatedDate = DateTime.Now,
                                    LastUpdateBy = UserSysNo,
                                    LastUpdateDate = DateTime.Now
                                };
                                IcpDao.Instance.InsertIcpGoodsItem(m);
                            }
                        }
                    }
                    result.Status = true;
                    result.Message = internationaltrade.Head.MessageID;
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                }
            }
            else
            {
                result.Status = false;
                result.Message = "商品备案信息无效";
            }
            return result;
        }
        /// <summary>
        /// 获取指定类型的最大三位流水号
        /// </summary>
        /// <param name="name">报文类型</param>
        /// <returns>最大流水号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public string GetMaxSerialNumberByMType(string IcpType, string MessageType)
        {
            CIcp entity = IcpDao.Instance.GetEntityByMType(IcpType, MessageType);
            string SerialNumber = "";
            if (entity != null)
            {
                SerialNumber = entity.SerialNumber;
                int intSerialNumber = int.Parse(SerialNumber) + 1;
                if (intSerialNumber < 10)
                {
                    SerialNumber = "0000" + intSerialNumber.ToString();
                    return SerialNumber;
                }
                if (intSerialNumber < 100)
                {
                    SerialNumber = "000" + intSerialNumber.ToString();
                    return SerialNumber;
                }
                if (intSerialNumber < 1000)
                {
                    SerialNumber = "00" + intSerialNumber.ToString();
                    return SerialNumber;
                }
                if (intSerialNumber < 10000)
                {
                    SerialNumber = "0" + intSerialNumber.ToString();
                    return SerialNumber;
                }
                if (intSerialNumber == 10000)
                {
                    SerialNumber = "00001";
                    return SerialNumber;
                }
            }
            else
            {
                SerialNumber = "00001";
            }

            return SerialNumber;
        }

        /// <summary>
        /// 设置XML头部值
        /// </summary>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        /// <remarks>2016-3-21 王耀发 添加</remarks>
        Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.Head SetIcpGoodsHead(string MessageID, string MessageType)
        {
            Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.Head head = new Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.Head();
            head.MessageID = MessageID;
            head.MessageType = MessageType;
            head.Sender = config.GZJCIcpInfoTrade.Sender;
            head.Receiver = config.GZJCIcpInfoTrade.Receiver;
            head.SendTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            head.FunctionCode = "";
            head.SignerInfo = "";
            head.Version = config.GZJCIcpInfoTrade.Version;
            return head;
        }
        /// <summary>
        /// 设置商品备案头部信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsRegHead SetIcpGoodsHeadData()
        {
            Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsRegHead GoodsRegHead = new Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsRegHead();
            //申报企业编号
            GoodsRegHead.DeclEntNo = config.GZJCIcpInfoTrade.DeclEntNo;
            //申报企业名称
            GoodsRegHead.DeclEntName = config.GZJCIcpInfoTrade.DeclEntName;
            //电商企业编号
            GoodsRegHead.EBEntNo = config.GZJCIcpInfoTrade.EBEntNo;
            //电商企业名称
            GoodsRegHead.EBEntName = config.GZJCIcpInfoTrade.EBEntName;
            //操作方式
            GoodsRegHead.OpType = config.GZJCIcpInfoTrade.OpType;
            //主管海关代码
            GoodsRegHead.CustomsCode = config.GZJCIcpInfoTrade.CustomsCode;
            //检验检疫机构代码
            GoodsRegHead.CIQOrgCode = config.GZJCIcpInfoTrade.CIQOrgCode;
            //电商平台企业编号可空
            GoodsRegHead.EBPEntNo = config.GZJCIcpInfoTrade.EBPEntNo;
            //电商平台名称 可空
            GoodsRegHead.EBPEntName = config.GZJCIcpInfoTrade.EBPEntName;
            //物流企业编号 可空
            GoodsRegHead.EHSEntNo = config.GZJCIcpInfoTrade.EHSEntNo;
            //物流企业名称 可空
            GoodsRegHead.EHSEntName = config.GZJCIcpInfoTrade.EHSEntName;
            //币制代码
            GoodsRegHead.CurrCode = config.GZJCIcpInfoTrade.CurrCode;
            //跨境业务类型
            GoodsRegHead.BusinessType = config.GZJCIcpInfoTrade.BusinessType;
            //录入日期
            GoodsRegHead.InputDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            //申请备案时
            GoodsRegHead.DeclDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            //进出境标志
            GoodsRegHead.IeFlag = config.GZJCIcpInfoTrade.IeFlag;
            GoodsRegHead.Notes = "";
            return GoodsRegHead;
        }
        /// <summary>
        /// 设置商品备案内部信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        List<Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsContent> SetIcpGoodsBodyData(List<CIcpGoodsItem> CIcpGoodsItemList)
        {
            List<Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsContent> GoodsContentList = new List<Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsContent>();
            int i = 0;
            foreach (CIcpGoodsItem item in CIcpGoodsItemList)
            {
                i++;
                IcpBYJiChangGoodsInfo Entity = IcpBo.Instance.GetIcpBYJiChangGoodsInfoEntityByPid(item.ProductSysNo);
                if (Entity != null)
                {
                    GoodsContentList.Add(new Hyt.Model.Icp.GZBaiYunJiChang.Goods.Goods.GoodsContent()
                    {
                        Seq = i.ToString(),
                        EntGoodsNo = Entity.EntGoodsNo,
                        EPortGoodsNo = "",
                        CIQGoodsNo = "",
                        CusGoodsNo = "",
                        EmsNo = "",
                        ItemNo = "",
                        ShelfGName = Entity.ShelfGName,
                        NcadCode = Entity.NcadCode,
                        PostTariffName = Entity.PostTariffName,
                        BarCode = (Entity.BarCode == null ? "" : Entity.BarCode),
                        HSCode = (Entity.HSCode == null ? "" : Entity.HSCode),
                        GoodsName = Entity.GoodsName,
                        GoodsStyle = Entity.GoodsStyle,
                        Brand = Entity.Brand,
                        GUnit = Entity.GUnit,
                        StdUnit = Entity.StdUnit,
                        //SecUnit = (Entity.SecUnit == null ? "" : Entity.SecUnit),
                        RegPrice = Entity.RegPrice.ToString(),
                        CurrCode = Entity.CurrCode,
                        GiftFlag = Entity.GiftFlag.ToString(),
                        OriginCountry = Entity.OriginCountry,
                        Quality = Entity.Quality,
                        QualityCertify = (Entity.QualityCertify == null ? "" : Entity.QualityCertify),
                        Manufactory = (Entity.Manufactory == null ? "" : Entity.Manufactory),
                        NetWt = Entity.NetWt.ToString(),
                        GrossWt = Entity.GrossWt.ToString(),
                        GNote = Entity.GNote,
                        ValidDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                        EndDate = "20651207164852",
                        Notes = ""
                    });
                }
            }
            return GoodsContentList;
        }


        #endregion

        #region 电子订单
        /// <summary>
        /// 订单备案
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-21 王耀发 添加</remarks>
        public override Result PushOrder(int OrderSysNo)
        {
            Result result = new Result();
            int IcpType = (int)Model.CommonEnum.商检.广州白云机场;
            Hyt.Model.Icp.GZBaiYunJiChang.Order.InternationalTrade internationaltrade = new Hyt.Model.Icp.GZBaiYunJiChang.Order.InternationalTrade();
            internationaltrade.Head = new Hyt.Model.Icp.GZBaiYunJiChang.Order.Head();
            internationaltrade.Declaration = new Hyt.Model.Icp.GZBaiYunJiChang.Order.Declaration();
            internationaltrade.Declaration.OrderHead = new Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderHead();
            internationaltrade.Declaration.OrderList = new Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderList();
            internationaltrade.Declaration.OrderList.OrderContentList = new List<Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderContent>();

            //生成最大流水号
            string MaxSerialNumber = GetMaxSerialNumberByMType(IcpType.ToString(), config.GZJCIcpInfoTrade.OrderMessageType);
            string fileName = config.GZJCIcpInfoTrade.OrderMessageType + "_" + config.GZJCIcpInfoTrade.Sender + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + MaxSerialNumber;

            internationaltrade.Head = SetIcpOrderHead(fileName, config.GZJCIcpInfoTrade.OrderMessageType);
            internationaltrade.Declaration.OrderHead = SetIcpOrderHeadData();
            internationaltrade.Declaration.OrderList.OrderContentList.Add(SetOrderContent(OrderSysNo));

            string str = Hyt.Util.Serialization.SerializationUtil.XmlSerialize<Hyt.Model.Icp.GZBaiYunJiChang.Order.InternationalTrade>(internationaltrade);
            str = str.Replace("encoding=\"utf-16\"", "encoding=\"UTF-8\"");

            FtpUtil ftp = new FtpUtil(config.GZJCIcpInfoTrade.FtpUrl, config.GZJCIcpInfoTrade.FtpName, config.GZJCIcpInfoTrade.FtpPassword);
            string msg = "";
            string MessageID = fileName;
            fileName = fileName + ".xml";

            try
            {
                ftp.UploadFile(config.GZJCIcpInfoTrade.FtpUrl + "in", fileName, Encoding.UTF8.GetBytes(str), out msg);

                //新增商检表信息
                int UserSysNo = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                CIcp model = new CIcp();
                model.SourceSysNo = OrderSysNo;
                model.IcpType = IcpType;
                model.MessageID = MessageID;
                model.MessageType = config.GZJCIcpInfoTrade.OrderMessageType;
                model.SerialNumber = MaxSerialNumber;
                model.XmlContent = str;
                model.CreatedBy = UserSysNo;
                model.CreatedDate = DateTime.Now;
                model.Status = (int)IcpStatus.商品商检推送状态.已推送;
                model.LastUpdateBy = UserSysNo;
                model.LastUpdateDate = DateTime.Now;
                model.SysNo = IcpDao.Instance.Insert(model);

                var soItems = SoOrderBo.Instance.GetOrderItemsByOrderId(OrderSysNo);
                foreach (SoOrderItem item in soItems)
                {
                    var m = new CIcpGoodsItem
                    {
                        SourceSysNo = OrderSysNo,
                        IcpType = model.IcpType,
                        MessageID = MessageID,
                        IcpGoodsSysNo = model.SysNo,
                        ProductSysNo = item.ProductSysNo,
                        EntGoodsNo = "None",
                        CreatedBy = UserSysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = UserSysNo,
                        LastUpdateDate = DateTime.Now
                    };
                    IcpDao.Instance.InsertIcpGoodsItem(m);
                }
                //更新订单的商检推送状态
                SoOrderBo.Instance.UpdateOrderGZJCStatus(OrderSysNo,(int)OrderStatus.销售单推送状态.已推送);               
                result.Status = true;
                result.Message = internationaltrade.Head.MessageID;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }
        #region 海关电商交易订单
        /// <summary>
        /// 设置XML头部值
        /// </summary>
        /// <param name="MessageID"></param>
        /// <returns></returns>
        /// <remarks>2016-3-21 王耀发 添加</remarks>
        Hyt.Model.Icp.GZBaiYunJiChang.Order.Head SetIcpOrderHead(string MessageID, string MessageType)
        {
            Hyt.Model.Icp.GZBaiYunJiChang.Order.Head head = new Hyt.Model.Icp.GZBaiYunJiChang.Order.Head();
            head.MessageID = MessageID;
            head.MessageType = MessageType;
            head.Sender = config.GZJCIcpInfoTrade.Sender;
            head.Receiver = config.GZJCIcpInfoTrade.Receiver;
            head.SendTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            head.FunctionCode = "";
            head.SignerInfo = "";
            head.Version = config.GZJCIcpInfoTrade.Version;
            return head;
        }

        /// <summary>
        /// 设置订单头部信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-1-15 王耀发 创建</remarks>
        Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderHead SetIcpOrderHeadData()
        {
            Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderHead OrderHead = new Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderHead();
            //申报企业编号
            OrderHead.DeclEntNo = config.GZJCIcpInfoTrade.DeclEntNo;
            //申报企业名称
            OrderHead.DeclEntName = config.GZJCIcpInfoTrade.DeclEntName;
            //报送者名称（个人）
            OrderHead.DeclPerson = config.GZJCIcpInfoTrade.DeclPerson;
            //报送者证件号码（个人）
            OrderHead.DeclPerNumber = config.GZJCIcpInfoTrade.DeclPerNumber;
            //报送者证件类型代码（个人）
            OrderHead.DeclPerTypeCode = config.GZJCIcpInfoTrade.DeclPerTypeCode;
            //电商企业编号
            OrderHead.EBEntNo = config.GZJCIcpInfoTrade.EBEntNo;
            //电商企业名称
            OrderHead.EBEntName = config.GZJCIcpInfoTrade.EBEntName;
            //电商平台企业编号可空
            OrderHead.EBPEntNo = config.GZJCIcpInfoTrade.EBPEntNo;
            //电商平台互联网域名
            OrderHead.InternetDomainName = config.GZJCIcpInfoTrade.InternetDomainName;
            //电商平台名称 可空
            OrderHead.EBPEntName = config.GZJCIcpInfoTrade.EBPEntName;
            //操作方式
            OrderHead.OpType = config.GZJCIcpInfoTrade.OpType;
            //申请备案时
            OrderHead.DeclTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //进出口标示
            OrderHead.IeFlag = config.GZJCIcpInfoTrade.IeFlag;
            //主管海关代码
            OrderHead.CustomsCode = config.GZJCIcpInfoTrade.CustomsCode;
            //检验检疫机构代码
            OrderHead.CIQOrgCode = config.GZJCIcpInfoTrade.CIQOrgCode;
            return OrderHead;
        }
        #endregion

        Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderContent SetOrderContent(int OrderSysNo)
        {
            Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderContent OrderContent = new Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderContent();
            OrderContent.OrderDetail = SetOrderDetail(OrderSysNo);
            OrderContent.OrderWaybillRel = SetOrderWaybillRel(OrderSysNo);
            OrderContent.OrderPaymentRel = SetOrderPaymentRel(OrderSysNo);
            return OrderContent;
        }
        /// <summary>
        /// 设置订单明细信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-4-12 王耀发 创建</remarks>
        Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderDetail SetOrderDetail(int OrderSysNo)
        {
            Hyt.Model.Manual.SoOrderMods OrderEntity = SoOrderBo.Instance.GetSoOrderMods(OrderSysNo);
            OrderEntity.Customer = CrCustomerBo.Instance.GetModel(OrderEntity.CustomerSysNo);
            OrderEntity.ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress2(OrderEntity.ReceiveAddressSysNo);
            OrderEntity.ReceiverCountry = ((Hyt.Model.Manual.SoReceiveAddressMod)OrderEntity.ReceiveAddress).ReceiverCountry.Trim();
            OrderEntity.ReceiverProvince = ((Hyt.Model.Manual.SoReceiveAddressMod)OrderEntity.ReceiveAddress).ReceiverProvince.Trim();
            OrderEntity.ReceiverCity = ((Hyt.Model.Manual.SoReceiveAddressMod)OrderEntity.ReceiveAddress).ReceiverCity.Trim();
            OrderEntity.ReceiverArea = ((Hyt.Model.Manual.SoReceiveAddressMod)OrderEntity.ReceiveAddress).ReceiverArea.Trim();
            OrderEntity.OrderItemList = SoOrderBo.Instance.GetOrderItemsByOrderId(OrderEntity.SysNo);
            OrderEntity.OrderInvoice = SoOrderBo.Instance.GetFnInvoice(OrderEntity.InvoiceSysNo);

            Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderDetail OrderDetail = new Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderDetail();
            //获取电子订单编号
            FnOnlinePayment OnPayment = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentBySourceSysNo(OrderSysNo);            
            //企业电子订单编号
            OrderDetail.EntOrderNo = OnPayment.BusinessOrderSysNo;
            //电子订单状态 0-订单确认、1-订单完成、2-订单取消
            OrderDetail.OrderStatus = "1";
            //支付状态 0-已付款、1-未付款
            OrderDetail.PayStatus = "0";
            //订单商品总额
            OrderDetail.OrderGoodTotal = OrderEntity.OrderAmount.ToString();
            //订单商品总额币制
            OrderDetail.OrderGoodTotalCurr = "142";
            //订单运费
            OrderDetail.Freight = OrderEntity.FreightAmount.ToString();
            //运费币制
            OrderDetail.FreightCurr = "142";
            //税款
            OrderDetail.Tax = OrderEntity.TaxFee.ToString();
            //税款币制
            OrderDetail.TaxCurr = "142";
            //抵付金额
            OrderDetail.OtherPayment = "";
            //币制代码
            OrderDetail.OtherPayCurr = "";
            //抵付说明
            OrderDetail.OtherPayNotes = "";
            //其它费用
            OrderDetail.OtherCharges = "";
            //币制代码
            OrderDetail.OtherChargesCurr = "";
            //实际支付金额
            OrderDetail.ActualAmountPaid = OrderEntity.CashPay.ToString();
            //实际支付币制
            OrderDetail.ActualCurr = "142";
            //收货人名称
            OrderDetail.RecipientName = OrderEntity.ReceiveAddress.Name;
            //收货人地址
            OrderDetail.RecipientAddr = (OrderEntity as Hyt.Model.Manual.SoOrderMods).ReceiverProvince + " " +
                                        (OrderEntity as Hyt.Model.Manual.SoOrderMods).ReceiverCity + " " +
                                        (OrderEntity as Hyt.Model.Manual.SoOrderMods).ReceiverArea + " " +
                                        OrderEntity.ReceiveAddress.StreetAddress;
            //收货人电话
            OrderDetail.RecipientTel = OrderEntity.ReceiveAddress.MobilePhoneNumber;
            //收货人所在国
            if (OrderEntity.ReceiverCountry == "中国")
                OrderDetail.RecipientCountry = "142";
            //下单人账户
            OrderDetail.OrderDocAcount = OrderEntity.Customer.Account;
            //下单人姓名
            OrderDetail.OrderDocName = OrderEntity.Customer.Account;
            //下单人证件类型
            OrderDetail.OrderDocType = "01";
            //下单人证件号
            OrderDetail.OrderDocId = OrderEntity.ReceiveAddress.IDCardNo;
            //下单人电话
            OrderDetail.OrderDocTel = OrderEntity.ReceiveAddress.MobilePhoneNumber;
            //订单日期
            OrderDetail.OrderDate = OrderEntity.CreateDate.ToString("yyyyMMddHHmmss");
            //备注
            OrderDetail.Notes = "";
            OrderDetail.GoodsList = SetGoodsList(OrderSysNo);
            return OrderDetail;
        }
        Hyt.Model.Icp.GZBaiYunJiChang.Order.GoodsList SetGoodsList(int OrderSysNo)
        {
            Hyt.Model.Icp.GZBaiYunJiChang.Order.GoodsList GoodsList = new Hyt.Model.Icp.GZBaiYunJiChang.Order.GoodsList();
            GoodsList.OrderGoodsListList = SetOrderGoodsListList(OrderSysNo);
            return GoodsList;
        }

        List<Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderGoodsList> SetOrderGoodsListList(int OrderSysNo)
        {
            //获取电子订单编号
            FnOnlinePayment OnPayment = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentBySourceSysNo(OrderSysNo);     

            List<Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderGoodsList> OrderGoodsListList = new List<Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderGoodsList>();
            IList<SoOrderItem> OrderItem = SoOrderBo.Instance.GetOrderItemsByOrderId(OrderSysNo);
            int i = 0;
            foreach (SoOrderItem item in OrderItem)
            {
                i++;
                IcpBYJiChangGoodsInfo Entity = IcpBo.Instance.GetIcpBYJiChangGoodsInfoEntityByPid(item.ProductSysNo);
                if (Entity != null)
                {
                    OrderGoodsListList.Add(new Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderGoodsList()
                    {
                        Seq = i.ToString(),
                        LogisticsOrderNo = OnPayment.BusinessOrderSysNo,
                        EntGoodsNo = Entity.EntGoodsNo,
                        EPortGoodsNo = Entity.EPortGoodsNo,
                        CIQGoodsNo = Entity.CIQGoodsNo,
                        CusGoodsNo = Entity.CusGoodsNo,
                        HSCode = Entity.HSCode,
                        GoodsName = Entity.GoodsName,
                        GoodsStyle = Entity.GoodsStyle,
                        BarCode = Entity.BarCode,
                        Brand = Entity.Brand,
                        Qty = item.Quantity.ToString(),
                        Unit = Entity.GUnit,
                        Price = item.SalesUnitPrice.ToString(),
                        Total = item.SalesAmount.ToString(),
                        CurrCode = Entity.CurrCode,
                        Notes = Entity.Notes
                    });
                }
            }

            //OrderGoodsListList.Add(new Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderGoodsList()
            //{
            //    Seq = "1",
            //    LogisticsOrderNo = "A20150915007",
            //    EntGoodsNo = "123",
            //    EPortGoodsNo = "",
            //    CIQGoodsNo = "1500096155",
            //    CusGoodsNo = "GDO5141507080000560",
            //    HSCode = "1901900000",
            //    GoodsName = "日本尤妮佳拉拉裤男用XXL26片",
            //    GoodsStyle = "XXL码|26片/包",
            //    BarCode = "",
            //    Brand = "尤妮佳",
            //    Qty = "1",
            //    Unit = "125",
            //    Price = "100.00",
            //    Total = "100.00",
            //    CurrCode = "142",
            //    Notes = ""
            //});
            return OrderGoodsListList;
        }

        Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderWaybillRel SetOrderWaybillRel(int OrderSysNo)
        {
            //获取物流反馈回来的
            Model.CrossBorderLogisticsOrder LogisticsEntity = Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(OrderSysNo);

            Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderWaybillRel OrderWaybillRel = new Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderWaybillRel();
            OrderWaybillRel.EHSEntNo = "C000101000470027";
            OrderWaybillRel.EHSEntName = "广州市有信达国际货运代理有限公司";
            OrderWaybillRel.WaybillNo = LogisticsEntity.LogisticsOrderId;
            OrderWaybillRel.Notes = "";
            return OrderWaybillRel;
        }

        Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderPaymentRel SetOrderPaymentRel(int OrderSysNo)
        {
            FnReceiptVoucher ReceiptVoucher = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherByOrder(OrderSysNo);
            List<CBFnReceiptVoucherItem> ReceiptVoucherItemList = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(ReceiptVoucher.SysNo);
            string PayEntName = "";              
            string PayNo = "";
            foreach (CBFnReceiptVoucherItem item in ReceiptVoucherItemList)
            {
                if(PayEntName == "")
                {
                    PayEntName = item.PaymentTypeName;
                }
                else
                {
                    PayEntName += "," + item.PaymentTypeName;
                }
                if (PayNo == "")
                {
                    PayNo = item.VoucherNo;
                }
                else
                {
                    PayNo += "," + item.VoucherNo;
                }
            }

            Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderPaymentRel OrderPaymentRel = new Hyt.Model.Icp.GZBaiYunJiChang.Order.OrderPaymentRel();
            //支付企业代码
            OrderPaymentRel.PayEntNo = "C010000000677460";
            //支付企业名称
            OrderPaymentRel.PayEntName = PayEntName;
            //支付交易类型
            OrderPaymentRel.PayType = "M";
            //支付交易编号
            OrderPaymentRel.PayNo = PayNo;
            //备注
            OrderPaymentRel.Notes = "";
            return OrderPaymentRel;
        }
        #endregion

        /// <summary>
        /// 海关商检反馈信息
        /// </summary>
        /// <returns></returns>
        public override string[] GetIcpOutResult()
        {
            FtpUtil ftp = new FtpUtil(config.GZJCIcpInfoTrade.FtpUrl, config.GZJCIcpInfoTrade.FtpName, config.GZJCIcpInfoTrade.FtpPassword);
            string[] inflist = null;
            inflist = ftp.GetFileList(config.GZJCIcpInfoTrade.FtpUrl + "out");
            return inflist;
        }

        /// <summary>
        /// 下载报文
        /// </summary>
        /// <param name="type"></param>
        /// <param name="localDir"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public override string DownResultFileData(string localDir, string filePath)
        {
            FtpUtil ftp = new FtpUtil(config.GZJCIcpInfoTrade.FtpUrl, config.GZJCIcpInfoTrade.FtpName, config.GZJCIcpInfoTrade.FtpPassword);
            string msg = "";
            ftp.DownloadFile(config.GZJCIcpInfoTrade.FtpUrl + "out/" + filePath, localDir, out msg);
            return msg;
        }

        /// <summary>
        /// 获取商品商检回执
        /// </summary>
        /// <returns></returns>
        public override Result GetGoodsRec()
        {
            Result result = new Result();
            try
            {

                FtpUtil ftp = new FtpUtil(config.GZJCIcpInfoTrade.FtpUrl, config.GZJCIcpInfoTrade.FtpName, config.GZJCIcpInfoTrade.FtpPassword);

                string[] fileList = GetIcpOutResult();

                if (fileList != null)
                {
                    foreach (string fileTxt in fileList)
                    {

                        string msg = "";
                        Stream stream = ftp.FileStream(config.GZJCIcpInfoTrade.FtpUrl + fileTxt, ref msg);
                        //设置当前流的位置为流的开始，防止读取位置错误造成无法读取完整流的内容
                        stream.Seek(0, SeekOrigin.Begin);
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string txt = reader.ReadToEnd();

                            //接收回执
                            if (fileTxt.Contains("KJDOCREC_"))
                            {
                                Hyt.Model.Icp.GZBaiYunJiChang.Goods.DOCREC.DocRec DocRec = Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<Hyt.Model.Icp.GZBaiYunJiChang.Goods.DOCREC.DocRec>(txt);
                                //更新平台回执信息
                                if (DocRec.Declaration.OrgMessageType == "KJ881101")
                                {
                                    IcpBo.Instance.UpdatePlatDocRecByMessageID(DocRec.Declaration.OrgMessageID, txt, DocRec.Declaration.Status);
                                    if (DocRec.Declaration.Status != "S")
                                    {
                                        IcpBo.Instance.UpdateEntGoodsNoByMessageID(DocRec.Declaration.OrgMessageID, "None");
                                    }
                                }
                                //更新商检回执信息
                                if (DocRec.Declaration.OrgMessageType == "881101")
                                {
                                    IcpBo.Instance.UpdateCiqDocRecByMessageID(DocRec.Declaration.OrgMessageID, txt, DocRec.Declaration.Status);
                                    if (DocRec.Declaration.Status != "S")
                                    {
                                        IcpBo.Instance.UpdateEntGoodsNoByMessageID(DocRec.Declaration.OrgMessageID, "None");
                                    }
                                }
                            }
                            //国检审核回执
                            if (fileTxt.Contains("KJ881101CIQREC_"))
                            {
                                Hyt.Model.Icp.GZBaiYunJiChang.Goods.CIQREC.InternationalTrade InternationalTrade = Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<Hyt.Model.Icp.GZBaiYunJiChang.Goods.CIQREC.InternationalTrade>(txt);
                                List<Hyt.Model.Icp.GZBaiYunJiChang.Goods.CIQREC.GoodsRegRecList> GoodsRegRecList = InternationalTrade.Declaration.GoodsRegRecList;
                                foreach (Hyt.Model.Icp.GZBaiYunJiChang.Goods.CIQREC.GoodsRegRecList item in GoodsRegRecList)
                                {
                                    IcpBo.Instance.UpdateIcpGoodsItemCIQ((int)Model.CommonEnum.商检.广州白云机场,item.EntGoodsNo, item.CIQGRegStatus, item.CIQNotes);
                                    //审核通过,更新检验检疫商品备案编号
                                    if(item.CIQGRegStatus == "C")
                                    {
                                        IcpBo.Instance.UpdateCIQGoodsNo(item.EntGoodsNo, item.CIQGoodsNo);
                                    }                                  
                                }
                            }
                            //海关审核回执
                            if (fileTxt.Contains("KJ881101CUSREC_"))
                            {
                                Hyt.Model.Icp.GZBaiYunJiChang.Goods.CUSREC.InternationalTrade InternationalTrade = Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<Hyt.Model.Icp.GZBaiYunJiChang.Goods.CUSREC.InternationalTrade>(txt);
                                List<Hyt.Model.Icp.GZBaiYunJiChang.Goods.CUSREC.GoodsRegRecList> GoodsRegRecList = InternationalTrade.Declaration.GoodsRegRecList;
                                foreach (Hyt.Model.Icp.GZBaiYunJiChang.Goods.CUSREC.GoodsRegRecList item in GoodsRegRecList)
                                {
                                    IcpBo.Instance.UpdateIcpGoodsItemCUS((int)Model.CommonEnum.商检.广州白云机场, item.EntGoodsNo, item.OpResult, item.CustomsNotes);
                                    //审核通过,更新海关正式备案编号
                                    if (item.OpResult == "C")
                                    {
                                        IcpBo.Instance.UpdateCusGoodsNo(item.EntGoodsNo, item.CusGoodsNo);
                                    }
                                }
                            }
                        }
                    }
                    result.Status = true;
                    result.Message = "获取成功";
                }
                else
                {
                    result.Status = true;
                    result.Message = "暂未生成相应的回执";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取订单商检回执
        /// </summary>
        /// <returns></returns>
        public override Result GetOrderRec()
        {
            Result result = new Result();
            try
            {

                FtpUtil ftp = new FtpUtil(config.GZJCIcpInfoTrade.FtpUrl, config.GZJCIcpInfoTrade.FtpName, config.GZJCIcpInfoTrade.FtpPassword);

                string[] fileList = GetIcpOutResult();

                if (fileList != null)
                {
                    foreach (string fileTxt in fileList)
                    {

                        string msg = "";
                        Stream stream = ftp.FileStream(config.GZJCIcpInfoTrade.FtpUrl + fileTxt, ref msg);
                        //设置当前流的位置为流的开始，防止读取位置错误造成无法读取完整流的内容
                        stream.Seek(0, SeekOrigin.Begin);
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string txt = reader.ReadToEnd();

                            //接收回执
                            if (fileTxt.Contains("KJDOCREC_"))
                            {
                                Hyt.Model.Icp.GZBaiYunJiChang.Goods.DOCREC.DocRec DocRec = Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<Hyt.Model.Icp.GZBaiYunJiChang.Goods.DOCREC.DocRec>(txt);
                                //更新平台回执信息
                                if (DocRec.Declaration.OrgMessageType == "KJ881111")
                                {
                                    IcpBo.Instance.UpdatePlatDocRecByMessageID(DocRec.Declaration.OrgMessageID, txt, DocRec.Declaration.Status);
                                }
                                //更新商检回执信息
                                if (DocRec.Declaration.OrgMessageType == "881111")
                                {
                                    IcpBo.Instance.UpdateCiqDocRecByMessageID(DocRec.Declaration.OrgMessageID, txt, DocRec.Declaration.Status);
                                }
                            }
                        }
                    }
                    result.Status = true;
                    result.Message = "获取成功";
                }
                else
                {
                    result.Status = true;
                    result.Message = "暂未生成相应的回执";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override Pager<CIcp> GetGoodsPagerList(ParaIcpGoodsFilter filter)
        {
            filter.MessageType = "KJ881101";
            return IcpDao.Instance.GoodsQuery(filter);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override Pager<CIcp> GetOrderPagerList(ParaIcpGoodsFilter filter)
        {
            filter.MessageType = "KJ881111";
            return IcpDao.Instance.OrderQuery(filter);
        }
    }
}
