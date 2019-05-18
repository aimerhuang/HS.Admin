using Hyt.Model;
using Hyt.Model.Icp.GZNanSha;
using Hyt.Model.Icp.GZNanSha.Order;
using Hyt.Model.WorkflowStatus;
using Hyt.Util.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.BLL.Authentication;
using Hyt.DataAccess.Icp;
using Hyt.Model.Common;
using Hyt.BLL.ApiIcq;
using System.IO;
using Hyt.BLL.Order;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.ApiIcq.GZNanSha
{
    /// <summary>
    /// 南沙商检
    /// </summary>
    /// <remarks>2016-7-7 杨浩 添加注释</remarks>
    public class NanShaProvider : IIcqProvider
    {
        public NanShaProvider()
        { 
             dicUnit = new Dictionary<string, string>();
             dicUnit.Add("台", "001");
             dicUnit.Add("座", "002");
             dicUnit.Add("辆", "003");
             dicUnit.Add("艘", "004");
             dicUnit.Add("架", "005");
             dicUnit.Add("套", "006");
             dicUnit.Add("个", "007");
             dicUnit.Add("只", "008");
             dicUnit.Add("头", "009");
             dicUnit.Add("张", "010");
             dicUnit.Add("件", "011");
             dicUnit.Add("支", "012");
             dicUnit.Add("枝", "013");
             dicUnit.Add("根", "014");
             dicUnit.Add("条", "015");
             dicUnit.Add("把", "016");
             dicUnit.Add("块", "017");
             dicUnit.Add("卷", "018");
             dicUnit.Add("副", "019");
             dicUnit.Add("片", "020");
             dicUnit.Add("组", "021");
             dicUnit.Add("份", "022");
             dicUnit.Add("幅", "023");
             dicUnit.Add("双", "025");
             dicUnit.Add("对", "026");
             dicUnit.Add("棵", "027");
             dicUnit.Add("株", "028");
             dicUnit.Add("井", "029");
             dicUnit.Add("米", "030");
             dicUnit.Add("盘", "031");
             dicUnit.Add("其他", "999");
            	

        }
        /// <summary>
        /// 商检代码
        /// </summary>
        /// <remarks>2016-3-19 杨浩 创建</remarks>
        public override CommonEnum.商检 Code
        {
            get
            {
                return CommonEnum.商检.广州南沙;
            }
        }
        protected static IcpInfoConfig config = Hyt.BLL.Config.Config.Instance.GetIcqInfoConfig();
        /// <summary>
        /// 企业备案号
        /// </summary>
        string ERCI = config.NSIcpInfo.Sender;
        /// <summary>
        /// 组织机构代码
        /// </summary>
        string SLDW = "000069";

        #region 商品备案

        Dictionary<string, string> dicUnit = new Dictionary<string, string>();


        void SetCustomHead(string MessageID, Model.Icp.GZNanSha.Head head)
        {
            head.MessageID = MessageID;
            head.MessageType = config.NSIcpInfo.GoodsMessageType;
            head.Sender = config.NSIcpInfo.Sender;
            //测试使用
            //head.Sender = config.NSIcpInfo.Sender;
            head.Receiver = config.NSIcpInfo.Receiver;
            head.SendTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            head.Version = config.NSIcpInfo.Version;
            head.FunctionCode = config.NSIcpInfo.FunctionCode;
        }
        void SetCustomBodyData(Model.Icp.GZNanSha.Record record, Hyt.Model.Icp.GZNanSha.CommodityInspection mod)
        {
            record.CargoBcode = mod.CargoBcode;
            record.CbeComcode = ERCI;
            record.Ciqbcode = SLDW;
            record.Editccode = ERCI;
            record.OperType = (mod.Status == 0 ? "A" : mod.Status == 1 ? "M" : "");
        }
        void SetCustomBodyData(Model.Icp.GZNanSha.CARGO.CARGOLIST cargoList, IList<Hyt.Model.Icp.GZNanSha.CommodityInspectionLists> modLists)
        {
            foreach (Hyt.Model.Icp.GZNanSha.CommodityInspectionLists item in modLists)
            {
                string unitCode="";
                if(!dicUnit.ContainsKey(item.Unit))
                {
                    unitCode=dicUnit["其他"];
                }
                else{
                     unitCode=dicUnit[item.Unit];
                }
                cargoList.recordList.Add(new Hyt.Model.Icp.GZNanSha.Record1()
                {
                    Additiveflag = item.Additiveflag,
                    Brand = item.Brand,
                    AssemCountry = item.AssemCountry,
                    ComName = item.ComName,
                    Gcode = item.Gcode,
                    Gname = item.Gname,
                    GoodsBarcode = item.GoodsBarcode,
                    GoodsDesc = item.GoodsDesc,
                    Hscode = item.Hscode,
                    Ingredient = item.Ingredient,
                    Poisonflag = item.Poisonflag,
                    Remark = item.Remark,
                    Spec = item.Spec,
                    Unit = unitCode
                });
            }
        }
        /// <summary>
        /// 商品备案
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-21 王耀发 创建</remarks>
        public override Result PushGoods( string ProductSysNoList)
        {
            Result result = new Result();
            CIcp model = new CIcp();
            model.IcpType = (int)Model.CommonEnum.商检.广州南沙;
            List<CIcpGoodsItem> CIcpGoodsItemList = new List<CIcpGoodsItem>();
            string[] sArray = ProductSysNoList.Split(',');

            foreach (string i in sArray)
            {
                int ProductSysNo = int.Parse(i);
                IcpGZNanShaGoodsInfo Entity = IcpBo.Instance.GetIcpGZNanShaGoodsInfoEntityByPid(ProductSysNo);
                CIcpGoodsItemList.Add(new CIcpGoodsItem()
                {
                    IcpType = model.IcpType,
                    ProductSysNo = ProductSysNo,
                    EntGoodsNo = Entity.Gcode
                });
            }

            Model.Icp.GZNanSha.CommodityInspection mod = SetMod(model);
            List<Hyt.Model.Icp.GZNanSha.CommodityInspectionLists> modLists = SetModLists(CIcpGoodsItemList);
            Hyt.Model.Icp.GZNanSha.Root root = new Hyt.Model.Icp.GZNanSha.Root();
            root.head = new Hyt.Model.Icp.GZNanSha.Head();
            root.body = new Hyt.Model.Icp.GZNanSha.Body();
            root.body.goodSrecord = new Model.Icp.GZNanSha.GOODSRECORD();
            root.body.goodSrecord.record = new Model.Icp.GZNanSha.Record();
            root.body.goodSrecord.record.cargoList = new Model.Icp.GZNanSha.CARGO.CARGOLIST();
            root.body.goodSrecord.record.cargoList.recordList = new List<Hyt.Model.Icp.GZNanSha.Record1>();

            //生成最大流水号
            string MaxSerialNumber = GetMaxSerialNumberByMType(model.IcpType.ToString(), config.NSIcpInfo.GoodsMessageType);
            DateTime now = DateTime.Now;
            string strDate = now.ToString("yyyyMMddHHmmssfff");
            string strDateMid = now.ToString("yyyyMMddHHmmss");

            //string fileName = config.NSIcpInfo.GoodsMessageType + "_" + strDate + MaxSerialNumber;
            string MessageID = "ICIP" + strDateMid;
            string fileName = config.NSIcpInfo.GoodsMessageType + "_" + strDate;

            SetCustomHead(MessageID, root.head);
            SetCustomBodyData(root.body.goodSrecord.record, mod);
            SetCustomBodyData(root.body.goodSrecord.record.cargoList, modLists);
            string str = Hyt.Util.Serialization.SerializationUtil.XmlSerialize<Model.Icp.GZNanSha.Root>(root);
            str = str.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");
            FtpUtil ftp = new FtpUtil(config.NSIcpInfo.FtpUrl, config.NSIcpInfo.FtpName, config.NSIcpInfo.FtpPassword);
            string msg = "";
            //string MessageID = fileName;
            fileName = fileName + ".xml";
            try
            {
                ftp.UploadFile(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBCARGOBACK.REPORT/in", fileName, Encoding.UTF8.GetBytes(str), out msg);
                //新增商检表信息
                int UserSysNo = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                model.SourceSysNo = 0;
                model.MessageID = MessageID;
                model.MessageType = config.NSIcpInfo.GoodsMessageType;
                model.SerialNumber = MaxSerialNumber;
                model.XmlContent = str;
                model.CreatedBy = UserSysNo;
                model.CreatedDate = DateTime.Now;
                model.Status = (int)IcpStatus.商品商检推送状态.已推送;
                model.LastUpdateBy = AdminAuthenticationBo.Instance.Current.Base.SysNo;
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
                result.Message = root.head.MessageID;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
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
                    SerialNumber = "00" + intSerialNumber.ToString();
                    return SerialNumber;
                }
                if (intSerialNumber < 100)
                {
                    SerialNumber = "0" + intSerialNumber.ToString();
                    return SerialNumber;
                }
                if (intSerialNumber == 100)
                {
                    SerialNumber = "001";
                    return SerialNumber;
                }
            }
            else
            {
                SerialNumber = "001";
            }

            return SerialNumber;
        }

        Hyt.Model.Icp.GZNanSha.CommodityInspection SetMod(CIcp model)
        {
            Model.Icp.GZNanSha.CommodityInspection mod = new Model.Icp.GZNanSha.CommodityInspection();
            mod.CargoBcode = "ICPI" + DateTime.Now.ToString("yyyyMMddHHmmss");
            mod.Status = 0;
            return mod;
        }

        List<Hyt.Model.Icp.GZNanSha.CommodityInspectionLists> SetModLists(List<CIcpGoodsItem> CIcpGoodsItemList)
        {
            List<Hyt.Model.Icp.GZNanSha.CommodityInspectionLists> modLists = new List<Hyt.Model.Icp.GZNanSha.CommodityInspectionLists>();
            foreach (CIcpGoodsItem item in CIcpGoodsItemList)
            {
                IcpGZNanShaGoodsInfo Entity = IcpBo.Instance.GetIcpGZNanShaGoodsInfoEntityByPid(item.ProductSysNo);
                if (Entity != null)
                {
                    modLists.Add(new CommodityInspectionLists()
                    {
                        Additiveflag = Entity.GoodsDesc == null ? "无" : Entity.GoodsDesc,
                        Brand = Entity.Brand,
                        AssemCountry = Entity.AssemCountry,
                        ComName = Entity.ComName == null ? "" : Entity.ComName,
                        Gcode = Entity.Gcode,
                        Gname = Entity.Gname,
                        GoodsBarcode = Entity.GoodsBarcode == null ? "" : Entity.GoodsBarcode,
                        GoodsDesc = Entity.GoodsDesc == null ? "" : Entity.GoodsDesc,
                        Hscode = Entity.HSCode,
                        Ingredient = Entity.Ingredient == null ? "无" : Entity.Ingredient,
                        Poisonflag = Entity.Poisonflag == null ? "无" : Entity.Poisonflag,
                        Remark = Entity.Remark,
                        Spec = Entity.Spec,
                        Unit = Entity.Unit

                    });
                }
            }
            return modLists;
        }


        #endregion

        /// <summary>
        /// 推送订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-21 王耀发 创建</remarks>
        public override Result PushOrder(int soOrderSysNo)
        {
            Result result = new Result();
            int IcpType = (int)Model.CommonEnum.商检.广州南沙;
            Hyt.Model.Manual.SoOrderMods order = SoOrderBo.Instance.GetSoOrderMods(soOrderSysNo);
            order.ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress2(order.ReceiveAddressSysNo);
            order.ReceiverProvince = ((Hyt.Model.Manual.SoReceiveAddressMod)order.ReceiveAddress).ReceiverProvince.Trim();
            order.ReceiverCity = ((Hyt.Model.Manual.SoReceiveAddressMod)order.ReceiveAddress).ReceiverCity.Trim();
            order.ReceiverArea = ((Hyt.Model.Manual.SoReceiveAddressMod)order.ReceiveAddress).ReceiverArea.Trim();
            order.OrderItemList = SoOrderBo.Instance.GetOrderItemsByOrderId(order.SysNo);
            order.OrderInvoice = SoOrderBo.Instance.GetFnInvoice(order.InvoiceSysNo);
            order.ReceiptVoucher = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherByOrder(soOrderSysNo);
            order.ReceiptVoucherItemList = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(order.ReceiptVoucher.SysNo);

            IList<int> proIdList = new List<int>();
            List<Hyt.Model.Manual.SoOrderItemByPro> soProList = new List<Model.Manual.SoOrderItemByPro>();
            foreach (var item in order.OrderItemList)
            {
                proIdList.Add(item.ProductSysNo);
            }

            IList<IcpGZNanShaGoodsInfo> proList = Hyt.BLL.Product.PdProductBo.Instance.GetIcpGZNanShaGoodsInfoList(proIdList);

            if (order.OrderItemList.Count != proList.Count)
            {
                return new Result() { Message = "订单明细有尚未备案的商品，推送失败", Status = false };
            }
            foreach (var proItem in proList)
            {
                SoOrderItem item = order.OrderItemList.First(p => p.ProductSysNo == proItem.ProductSysNo);

                Hyt.Model.Manual.SoOrderItemByPro mod = new Model.Manual.SoOrderItemByPro();
                mod.GCode = proItem.Gcode;
                mod.ProductSysNo = proItem.ProductSysNo;
                mod.Hscode = proItem.HSCode;
                mod.CiqGoodsNo = proItem.CIQGoodsNo;
                mod.ProductName = proItem.Gname;
                mod.Brand = proItem.Brand;
                mod.Spec = proItem.Spec;
                mod.Origin = proItem.AssemCountry;
                mod.QtyUnit = proItem.Unit;
                mod.Qty = item.Quantity.ToString();
                mod.DecPrice = item.SalesUnitPrice;
                mod.DecTotal = item.SalesAmount;
                mod.SellWebSite = proItem.SellWebSite;
                soProList.Add(mod);
            }

            ////测试
            //foreach (var proItem in order.OrderItemList)
            //{
            //    var product = Hyt.BLL.Product.PdProductBo.Instance.GetProduct(proItem.ProductSysNo);
            //    //获取启邦商品备案信息
            //    var IcpQiBang = Hyt.BLL.ApiIcq.IcpBo.Instance.GetIcpQiBangGoodsInfoEntityByPid(proItem.ProductSysNo);

            //    Hyt.Model.Manual.SoOrderItemByPro mod = new Model.Manual.SoOrderItemByPro();
            //    mod.GCode = IcpQiBang.item_code;
            //    mod.ProductSysNo = proItem.ProductSysNo;
            //    mod.Hscode = IcpQiBang.item_id;
            //    mod.CiqGoodsNo = IcpQiBang.ciqgoodsno;
            //    mod.ProductName = IcpQiBang.item_name;
            //    mod.Brand = Hyt.BLL.Product.PdBrandBo.Instance.GetEntity(product.BrandSysNo).Name;
            //    mod.Spec = IcpQiBang.item_spec;
            //    mod.Origin = IcpQiBang.origincountry;
            //    mod.QtyUnit = "克";
            //    mod.Qty = proItem.Quantity.ToString();
            //    mod.DecPrice = proItem.SalesUnitPrice;
            //    mod.DecTotal = proItem.SalesAmount;
            //    mod.SellWebSite = "http://www.gaopin999.com/";
            //    soProList.Add(mod);

            //}


            Hyt.Model.Icp.GZNanSha.Order.Root root = new Hyt.Model.Icp.GZNanSha.Order.Root();
            root.head = new Model.Icp.GZNanSha.Head();
            root.body = new Model.Icp.GZNanSha.Order.OrderBody();
            root.body.record = new Model.Icp.GZNanSha.Order.OrderBodyRecord();
            root.body.record.cusOrderMod = new Model.Icp.GZNanSha.Order.CustomOrderMod();
            root.body.record.cusOrderMod.orderGoodsList = new Model.Icp.GZNanSha.Order.OrderGoodsList();

            //生成最大流水号
            string MaxSerialNumber = GetMaxSerialNumberByMType(IcpType.ToString(), config.NSIcpInfo.OrderMessageType);
            DateTime now = DateTime.Now;
            string strDate = now.ToString("yyyyMMddHHmmssfff");
            string strDateMid = now.ToString("yyyyMMddHHmmss");
            //string fileName = config.NSIcpInfo.OrderMessageType + "_" + strDate + MaxSerialNumber;
            string fileName = config.NSIcpInfo.OrderMessageType + "_" + strDate;
            string MessageID = "ICIP" + strDateMid;

            SetCustomOrderHead(MessageID, root.head);
            BindCusOrderData(order, root.body.record.cusOrderMod);
            BindCusOrderGoodsData(soProList, root.body.record.cusOrderMod.orderGoodsList);

            string str = Hyt.Util.Serialization.SerializationUtil.XmlSerialize<Model.Icp.GZNanSha.Order.Root>(root);
            str = str.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");
            str = str.Replace("Root", "ROOT");
            FtpUtil ftp = new FtpUtil(config.NSIcpInfo.FtpUrl, config.NSIcpInfo.FtpName, config.NSIcpInfo.FtpPassword);
            string msg = "";
            fileName = fileName + ".xml";
            try
            {
                ftp.UploadFile(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBEBTRADE.REPORT/in", fileName, Encoding.UTF8.GetBytes(str), out msg);
                //新增商检表信息
                int UserSysNo = AdminAuthenticationBo.Instance.Current.Base.SysNo;
                CIcp model = new CIcp();
                model.SourceSysNo = soOrderSysNo;
                model.IcpType = Hyt.Model.CommonEnum.商检.广州南沙.GetHashCode();
                model.MessageID = MessageID;
                model.MessageType = config.NSIcpInfo.OrderMessageType;
                model.SerialNumber = MaxSerialNumber;
                model.XmlContent = str;
                model.Status = (int)IcpStatus.商品商检推送状态.已推送;
                model.CreatedBy = UserSysNo;
                model.CreatedDate = DateTime.Now;
                model.LastUpdateBy = UserSysNo;
                model.LastUpdateDate = DateTime.Now;
                model.SysNo = IcpDao.Instance.Insert(model);
                //插入明细
                foreach (var proItem in proList)
                {
                    var m = new CIcpGoodsItem
                    {
                        SourceSysNo = soOrderSysNo,
                        IcpType = model.IcpType,
                        MessageID = MessageID,
                        IcpGoodsSysNo = model.SysNo,
                        ProductSysNo = proItem.ProductSysNo,
                        EntGoodsNo = "None",
                        CreatedBy = UserSysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = UserSysNo,
                        LastUpdateDate = DateTime.Now
                    };
                    IcpDao.Instance.InsertIcpGoodsItem(m);
                }
                //foreach (var proItem in order.OrderItemList)
                //{
                //    var m = new CIcpGoodsItem
                //    {
                //        SourceSysNo = soOrderSysNo,
                //        IcpType = model.IcpType,
                //        MessageID = MessageID,
                //        IcpGoodsSysNo = model.SysNo,
                //        ProductSysNo = proItem.ProductSysNo,
                //        EntGoodsNo = "None",
                //        CreatedBy = UserSysNo,
                //        CreatedDate = DateTime.Now,
                //        LastUpdateBy = UserSysNo,
                //        LastUpdateDate = DateTime.Now
                //    };
                //    IcpDao.Instance.InsertIcpGoodsItem(m);
                //}
                //更新订单的商检推送状态
                SoOrderBo.Instance.UpdateOrderNsStatus(soOrderSysNo, (int)OrderStatus.商检状态.已推送);
                result.Status = true;
                result.Message = root.head.MessageID;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }

        #region 海关电商交易订单

        void SetCustomOrderHead(string MessageID, Model.Icp.GZNanSha.Head head)
        {
            head.MessageID = MessageID;
            head.MessageType = config.NSIcpInfo.OrderMessageType;
            head.Sender = config.NSIcpInfo.Sender;
            //测试使用
            //head.Sender = "1000000139";
            head.Receiver = config.NSIcpInfo.Receiver;
            head.SendTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            head.FunctionCode = config.NSIcpInfo.FunctionCode;
            head.Version = config.NSIcpInfo.Version;
        }

        /// <summary>
        /// 绑定订单信息
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderMod"></param>
        void BindCusOrderData(SoOrder order, Model.Icp.GZNanSha.Order.CustomOrderMod orderMod)
        {
            CBFnOnlinePayment payment = Hyt.BLL.Finance.FinanceBo.Instance.GetOnPaymentBySourceSysNo(order.SysNo);
            orderMod.EntInsideNo = payment.BusinessOrderSysNo;//order.SysNo.ToString();//"SO"+order.SysNo.ToString().PadLeft(8,'0');
            orderMod.Ciqbcode = SLDW;
            orderMod.CbeComcode = ERCI;
            orderMod.CbepComcode = ERCI;
            orderMod.OrderStatus = "S";
            orderMod.ReceiveName = order.ReceiveAddress.Name;
            orderMod.ReceiveAddr = (order as Hyt.Model.Manual.SoOrderMods).ReceiverProvince + " " +
                                  (order as Hyt.Model.Manual.SoOrderMods).ReceiverCity + " " +
                                  (order as Hyt.Model.Manual.SoOrderMods).ReceiverArea + " " +
                                  order.ReceiveAddress.StreetAddress;

            orderMod.ReceiveNo = order.ReceiveAddress.IDCardNo;
            orderMod.ReceivePhone = order.ReceiveAddress.MobilePhoneNumber;
            orderMod.FCY = order.ProductAmount;
            orderMod.Fcode = "CNY";
            orderMod.Editccode = ERCI;
            orderMod.DrDate = order.CreateDate.ToString("yyyyMMddHHmmss");
        }

        void BindCusOrderGoodsData(List<Hyt.Model.Manual.SoOrderItemByPro> proList, Model.Icp.GZNanSha.Order.OrderGoodsList goodsLists)
        {
            int indx = 1;
            foreach (Hyt.Model.Manual.SoOrderItemByPro item in proList)
            {
                string unitCode = "";
                if (!dicUnit.ContainsKey(item.QtyUnit))
                {
                    unitCode = dicUnit["其他"];
                }
                else
                {
                    unitCode = dicUnit[item.QtyUnit];
                }
                CustomOrderGoodsMod mod = new CustomOrderGoodsMod();
                mod.Brand = item.Brand;
                mod.CiqGoodsNo = item.CiqGoodsNo;
                mod.CopGName = item.ProductName;
                mod.EntGoodsNo = indx.ToString().PadLeft(6, '0');
                mod.Gcode = item.GCode;
                mod.Hscode = item.Hscode;
                mod.Spec = item.Spec;
                mod.Origin = item.Origin;
                mod.Qty = item.Qty;
                mod.QtyUnit = unitCode;
                mod.DecPrice = item.DecPrice;
                mod.DecTotal = item.DecTotal;
                mod.SellWebSite = item.SellWebSite;
                mod.Nots = item.Nots;
                indx++;
                goodsLists.list.Add(mod);
            }
        }
        #endregion

        #region 商品推送后回执报文

        /// <summary>
        /// 海关商检反馈信息
        /// </summary>
        /// <returns></returns>
        public override string[] GetCustomsOutResult(Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型 type)
        {
            FtpUtil ftp = new FtpUtil(config.NSIcpInfo.FtpUrl, config.NSIcpInfo.FtpName, config.NSIcpInfo.FtpPassword);
            string[] inflist = null;
            switch (type)
            {
                case Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品检查:
                    inflist = ftp.GetFileList(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBCARGOBACK.REPORT/out");
                    break;
                case Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品订单:
                    inflist = ftp.GetFileList(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBEBTRADE.REPORT/out");
                    break;
                case Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品审核报文:
                    inflist = ftp.GetFileList(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBCARGOBACK.AUDIT/out");
                    break;
            }
            return inflist;
        }


        /// <summary>
        /// 下载报文
        /// </summary>
        /// <param name="type"></param>
        /// <param name="localDir"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public override string DownResultFileData(OrderStatus.商检反馈报文类型 type, string localDir, string filePath)
        {
            FtpUtil ftp = new FtpUtil(config.NSIcpInfo.FtpUrl, config.NSIcpInfo.FtpName, config.NSIcpInfo.FtpPassword);
            string msg = "";
            switch (type)
            {
                case Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品检查:
                    ftp.DownloadFile(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBCARGOBACK.REPORT/out/" + filePath, localDir, out msg);
                    //ftp.DeleteFile(ftpUri + "4200.IMPBA.SWBCARGOBACK.REPORT/out/" + filePath);
                    break;
                case Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品订单:
                    ftp.DownloadFile(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBEBTRADE.REPORT/out/" + filePath, localDir, out msg);
                    //ftp.DeleteFile(ftpUri + "4200.IMPBA.SWBEBTRADE.REPORT/out/" + filePath);
                    break;
                case Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品审核报文:
                    ftp.DownloadFile(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBCARGOBACK.AUDIT/out/" + filePath, localDir, out msg);
                    //ftp.DeleteFile(ftpUri + "4200.IMPBA.SWBCARGOBACK.AUDIT/out/" + filePath);
                    break;
            }
            return msg;
        }
        public override Result GetGoodsRec()
        {
            Result result = new Result();
            Result result2 = new Result();
            result2 = GetRecByType(OrderStatus.商检反馈报文类型.商品检查);
            result.Message = result2.Message;
            result2 = GetRecByType(OrderStatus.商检反馈报文类型.商品审核报文);
            result.Status = true;
            result.Message = result.Message + "," + result2.Message;
            return result;
        }
        /// <summary>
        /// 获取订单回执
        /// </summary>
        /// <returns></returns>
        public override Result GetOrderRec()
        {
            Result result = new Result();
            try
            {
                FtpUtil ftp = new FtpUtil(config.NSIcpInfo.FtpUrl, config.NSIcpInfo.FtpName, config.NSIcpInfo.FtpPassword);
                string[] fileList = GetCustomsOutResult(OrderStatus.商检反馈报文类型.商品订单);
                if (fileList != null)
                {
                    foreach (string fileTxt in fileList)
                    {
                        string msg = "";
                        Stream stream = ftp.FileStream(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBEBTRADE.REPORT/" + fileTxt, ref msg);
                        //设置当前流的位置为流的开始，防止读取位置错误造成无法读取完整流的内容
                        stream.Seek(0, SeekOrigin.Begin);
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string txt = reader.ReadToEnd();
                                                            //接收回执
                            if (fileTxt.Contains("DOCREC_"))
                            {
                                Model.Icp.GZNanSha.CustomsResult.Commodity.ROOT root = Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<Model.Icp.GZNanSha.CustomsResult.Commodity.ROOT>(txt);
                                //更新商检回执信息
                                if (root.declaration.OrgMessageType == "661101")
                                {
                                    CIcp Icp = IcpBo.Instance.GetEntityByMessageIDType(root.declaration.OrgMessageType, root.declaration.OrgMessageID);
                                    if (Icp != null)
                                    {
                                        if (Icp.Status == (int)OrderStatus.商检状态.已推送)
                                        {
                                            IcpBo.Instance.UpdatePlatDocRecByMessageID(root.declaration.OrgMessageID, txt, root.declaration.Status);

                                            //Status: 10 入库成功,20 出错
                                            if (root.declaration.Status == "10")
                                            {
                                                //更新订单的商检推送状态为已通过
                                                IcpBo.Instance.UpdateStatus(root.declaration.OrgMessageID, (int)IcpStatus.商品商检推送状态.已接收);
                                                SoOrderBo.Instance.UpdateOrderNsStatus(Icp.SourceSysNo, (int)OrderStatus.商检状态.已通过);
                                            }
                                            if (root.declaration.Status == "20")
                                            {
                                                //更新订单的商检推送状态为未推送
                                                IcpBo.Instance.UpdateStatus(root.declaration.OrgMessageID, (int)IcpStatus.商品商检推送状态.申报失败);
                                                SoOrderBo.Instance.UpdateOrderNsStatus(Icp.SourceSysNo, (int)OrderStatus.商检状态.未推送);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    result.Status = true;
                    result.Message = "商品订单回执获取成功";
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
        /// 获取商品回执
        /// </summary>
        /// <returns></returns>
        public  Result GetRecByType(OrderStatus.商检反馈报文类型 type)
        {
            string strType = "";
            if (type == Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品检查)
            {
                strType = "商品检查";
            }
            if (type == Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品审核报文)
            {
                strType = "商品审核";
            }
            Result result = new Result();
            try
            {
                FtpUtil ftp = new FtpUtil(config.NSIcpInfo.FtpUrl, config.NSIcpInfo.FtpName, config.NSIcpInfo.FtpPassword);
                string[] fileList = GetCustomsOutResult(type);
                if (fileList != null)
                {
                    if (type == Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品检查)
                    {                     
                        foreach (string fileTxt in fileList)
                        {
                            string msg = "";
                            Stream stream = ftp.FileStream(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBCARGOBACK.REPORT/" + fileTxt, ref msg);
                            //设置当前流的位置为流的开始，防止读取位置错误造成无法读取完整流的内容
                            stream.Seek(0, SeekOrigin.Begin);
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string txt = reader.ReadToEnd();
                                //接收回执
                                if (fileTxt.Contains("DOCREC_"))
                                {
                                    Model.Icp.GZNanSha.CustomsResult.Commodity.ROOT root = Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<Model.Icp.GZNanSha.CustomsResult.Commodity.ROOT>(txt);
                                    //更新商检回执信息
                                    if (root.declaration.OrgMessageType == "661105")
                                    {
                                        CIcp Icp = IcpBo.Instance.GetEntityByMessageIDType(root.declaration.OrgMessageType, root.declaration.OrgMessageID);
                                        if (Icp != null)
                                        {
                                            if (Icp.Status == (int)OrderStatus.商检状态.已推送)
                                            {
                                                IcpBo.Instance.UpdatePlatDocRecByMessageID(root.declaration.OrgMessageID, txt, root.declaration.Status);
                                                if (root.declaration.Status == "10")
                                                {
                                                    IcpBo.Instance.UpdateStatus(root.declaration.OrgMessageID, (int)IcpStatus.商品商检推送状态.已接收);
                                                }
                                                if (root.declaration.Status == "20")
                                                {
                                                    IcpBo.Instance.UpdateStatus(root.declaration.OrgMessageID, (int)IcpStatus.商品商检推送状态.申报失败);
                                                    IcpBo.Instance.UpdateEntGoodsNoByMessageID(root.declaration.OrgMessageID, "None");
                                                }
                                            }
                                        }                                     
                                    }                                   
                                }
                            }
                        }
                    }
                    if (type == Hyt.Model.WorkflowStatus.OrderStatus.商检反馈报文类型.商品审核报文)
                    {
                        foreach (string fileTxt in fileList)
                        {
                            string msg = "";
                            Stream stream = ftp.FileStream(config.NSIcpInfo.FtpUrl + "4200.IMPBA.SWBCARGOBACK.AUDIT/out/" + fileTxt, ref msg);
                            //设置当前流的位置为流的开始，防止读取位置错误造成无法读取完整流的内容
                            stream.Seek(0, SeekOrigin.Begin);
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string txt = reader.ReadToEnd();
                                Hyt.Model.Icp.GZNanSha.CustomsResult.CommodityAudit.ROOT root = Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<Model.Icp.GZNanSha.CustomsResult.CommodityAudit.ROOT>(txt);
                                List<Hyt.Model.Icp.GZNanSha.CustomsResult.CommodityAudit.Record> RecordList = root.declaration.GoodsRegRecList.RecordList;
                                foreach (Hyt.Model.Icp.GZNanSha.CustomsResult.CommodityAudit.Record item in RecordList)
                                {
                                    //审核通过,更新检验检疫商品备案编号
                                    if (item.RegStatus == "10")
                                    {
                                        IcpBo.Instance.UpdateNSCIQGoodsNo(item.Gcode, item.CIQGoodsNO);
                                    }    
                                }
                            }
                        }
                    }
                    result.Status = true;
                    result.Message = strType + "回执获取成功";
                }
                else
                {
                    result.Status = true;
                    result.Message = strType + "暂未生成相应的回执";
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
            filter.MessageType = "661105";
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
            filter.MessageType = "661101";
            return IcpDao.Instance.OrderQuery(filter);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2016-3-23 王耀发 创建</remarks>
        public override Pager<CBIcpGoodsItem> IcpGoodsItemQuery(ParaIcpGoodsFilter filter)
        {
            filter.MessageType = "661101";
            return IcpDao.Instance.IcpGoodsItemQuery(filter);
        }



        #endregion

        #region 商检审核通过后返回的审核备案号

        #endregion
    }
}
