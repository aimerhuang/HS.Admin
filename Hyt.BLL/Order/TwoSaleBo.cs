using Hyt.BLL.Basic;
using Hyt.BLL.Logistics;
using Hyt.BLL.Product;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Order;
using Hyt.Model;
using Hyt.Model.LogisApp;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 二次销售调价实体
    /// </summary>
    internal class AdjustTwoSale
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 实际销售金额
        /// </summary>
        public decimal Price { get; set; }
    }
    /// <summary>
    /// 二次销售相关方法
    /// </summary>
    /// <remarks>2014-9-17  朱成果 创建</remarks>
    public class TwoSaleBo : BOBase<TwoSaleBo>
    {
        /// <summary>
        /// 解析调价数据
        /// </summary>
        /// <param name="productPrice">产品价格.[{SysNo:111,Price:xxx},SysNo:111,Price:xxx}]</param>
        /// <returns></returns>
        /// <remarks>2014-9-17  朱成果 创建</remarks>
        public Dictionary<int, decimal> AnalyzeAdjustTwoSaleData(string productPrice)
        {
            if (string.IsNullOrEmpty(productPrice))
            {
                return null;
            }
            Dictionary<int, decimal> lstp = new Dictionary<int, decimal>();
            var lst=JsonConvert.DeserializeObject<List<AdjustTwoSale>>(productPrice);//解析
            if(lst!=null )
            {
                foreach(var item in lst)
                {
                    lstp.Add(item.SysNo, item.Price);
                }
            }
            return lstp;
        }

        /// <summary>
        /// 添加业务员二次销售收款记录
        /// </summary>
        /// <param name="model">模型</param>
        /// <remarks>2014-9-17  朱成果 创建</remarks>
        public void InsertTwoSaleCashHistory(Rp_业务员二次销售 model)
        {
            ITwoSaleDao.Instance.InsertTwoSaleCashHistory(model);
        }

          /// <summary>
          /// 订单对象转换
          /// </summary>
          /// <param name="order">wcf 传入订单对象</param>
          /// <returns></returns>
        /// <remarks>2014-9-17  朱成果 创建</remarks>
        public AppOrder MapToAppOrder(AppOrder2 order)
        {
            AppOrder neworder = new AppOrder();
            int levelSysNo=0;//客户等级
            if (order != null)
            {
                if(order.SoReceiveAddress != null) neworder.SoReceiveAddress = order.SoReceiveAddress;
                if(order.Order!=null)
                {
                    #region 订单和发票
                    var oo=order.Order;
                    neworder.SoOrder=new SoOrder(){
                        CustomerSysNo=oo.CustomerSysNo,
                        PayTypeSysNo=oo.PayTypeSysNo,
                        DeliveryTypeSysNo=oo.DeliveryTypeSysNo,
                        DeliveryRemarks=oo.DeliveryRemarks,
                        DeliveryTime= oo.DeliveryTime,
                        ContactBeforeDelivery=oo.ContactBeforeDelivery,
                        InternalRemarks= oo.InternalRemarks,
                        CustomerMessage=oo.CustomerMessage,
                        CreateDate=DateTime.Now,
                        SalesType=(int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.普通订单
                        
                         
                    };
                    if(oo.Invoice!=null)
                    {
                        neworder.Invoice=new FnInvoice()
                        {
                            InvoiceTypeSysNo=oo.Invoice.InvoiceTypeSysNo,
                            InvoiceTitle= oo.Invoice.InvoiceTitle,
                            InvoiceRemarks= oo.Invoice.InvoiceRemarks
                        };
                    }
                    #endregion

                    var cr=Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(order.Order.CustomerSysNo);//客户信息
                    if(cr!=null)
                    {
                        levelSysNo=cr.LevelSysNo;//客户等级
                        neworder.SoOrder.LevelSysNo = levelSysNo;//下单时候的客户等级

                    }
                }
                 if(order.OrderItems!=null)
                 {
                    #region 订单明细
                     decimal amount=0;
                     neworder.Products = new List<SoOrderItem>();
                     foreach(var p in order.OrderItems)
                     {
                         if(!CheckTwoSalePrice(p.SysNo,p.Price))
                         {
                             throw new HytException(string.Format("商品（{0}）销售价格不允许少于限定价格{1}.",p.ProductName,p.Price));
                         }
                         var pitem = new SoOrderItem()
                         {
                              Quantity=p.Quantity,
                              ProductSysNo=p.SysNo,
                              OriginalPrice=p.Price,
                              SalesUnitPrice=p.Price,
                              ProductName=p.ProductName,
                              ProductSalesType =  (int)CustomerStatus.商品销售类型.普通,
                              SalesAmount=p.Price*p.Quantity,
                         };
                         if (levelSysNo>0)
                         {
                             var originalPrice = GetProductPriceByCustomerLevel(pitem.ProductSysNo, levelSysNo);//商城原单价
                             pitem.OriginalPrice = originalPrice > 0 ? originalPrice : p.Price;//商城原单价
                             pitem.SalesUnitPrice = originalPrice > 0 ? originalPrice : p.Price;//商城销售单价;
                             pitem.SalesAmount = pitem.SalesUnitPrice * pitem.Quantity;//销售总额
                             pitem.ChangeAmount = p.Price * p.Quantity - pitem.SalesAmount;//调价金额
                             if (originalPrice != p.Price && neworder.SoOrder!=null)
                             {
                                 neworder.SoOrder.ImgFlag = MallTypeFlag.二次销售并调价;
                             }
                         }
                         neworder.Products.Add(pitem);
                         amount+=pitem.SalesAmount;
                     }
                     if(neworder.SoOrder!=null)
                     {
                         neworder.SoOrder.ProductAmount = neworder.SoOrder.OrderAmount = neworder.Products.Sum(m => m.SalesAmount);//总金额
                         neworder.SoOrder.ProductChangeAmount = neworder.Products.Sum(m => m.ChangeAmount);//调价金额
                         neworder.SoOrder.CashPay=neworder.SoOrder.OrderAmount+ neworder.SoOrder.ProductChangeAmount;//支付金额

                     }
                     #endregion
                 } 
            }
            return neworder;
        }

        /// <summary>
        /// 获取产品会员等级价格
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="levelSysNo">客户等级编号</param>
        /// <returns></returns>
        /// <remarks>2014-9-17  朱成果 创建</remarks>
        private decimal GetProductPriceByCustomerLevel(int productSysNo, int levelSysNo)
        {
            var productPrices = PdPriceBo.Instance.GetProductPrice(productSysNo,new ProductStatus.产品价格来源[] { ProductStatus.产品价格来源.会员等级价 });
            var firstOrDefault = productPrices.FirstOrDefault(p => p.SourceSysNo == levelSysNo);
            return firstOrDefault == null ? 0M : firstOrDefault.Price;
        }

        /// <summary>
        /// 检查调价是否在限定范围内
        /// </summary>
        /// <param name="prices">调价信息[产品编号,产品价格]</param>
        /// <returns></returns>
        /// <remarks>2014-9-17  朱成果 创建</remarks>
       public bool CheckTwoSalePrice(Dictionary<int,decimal> prices)
       {
           bool flg = true;
           if(prices!=null)
           {
               foreach(var item in prices)
               {
                   flg = CheckTwoSalePrice(item.Key, item.Value);
                   if (!flg) break;
               }
           }
           return flg;
        }


        /// <summary>
       /// 检查调价是否在限定范围内
        /// </summary>
        /// <param name="productsysno">订单编号</param>
        /// <param name="price">产品价格</param>
        /// <returns></returns>
       /// <remarks>2014-9-17  朱成果 创建</remarks>
       private bool  CheckTwoSalePrice(int productsysno,decimal price)
       {
           bool flg = true;
           var pitem = PdPriceBo.Instance.GetProductPrice(productsysno, new ProductStatus.产品价格来源[] { ProductStatus.产品价格来源.业务员销售限价 }).FirstOrDefault();
           flg = pitem == null || pitem.Price <= price;
           return flg;
       }


        /// <summary>
        /// 验证订单数据
        /// </summary>
        /// <param name="order">订单</param>
       ///<param name="user">配送员</param>
       /// <remarks>2014-9-17  朱成果 创建</remarks>
       private void CheckAppOrder(AppOrder order,SyUser user)
       {
           if (order == null || order.SoOrder == null) throw new HytException("订单信息不能为空");
           if (order.Products == null || order.Products.Count < 1) throw new HytException("订购商品信息不能为空");
           if (order.SoReceiveAddress == null) throw new HytException("收货地址不能为空");
           if (user == null) throw new HytException("配送员信息不能为空");
           if (order.SoOrder.CustomerSysNo < 1) throw new HytException("购买客户信息不能为空");
       }

        /// <summary>
        /// 创建二次销售订单
        /// </summary>
        /// <param name="order">订单信息</param>
       /// <param name="user">用户信息</param>
       /// <param name="hasDelivery">是否配送员立即配送</param>
        /// <returns></returns>
       /// <remarks>2014-9-17  朱成果 创建</remarks>
       public AppOrder CreateTwoSaleSoOrder(AppOrder order, SyUser user, bool hasDelivery = true)
       {
           CheckAppOrder(order, user);//验证订单数据

           #region 创建销售单
           var payType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(order.SoOrder.PayTypeSysNo);
           order.SoOrder.OnlineStatus = payType.PaymentType == (int)BasicStatus.支付方式类型.到付 ? Constant.OlineStatusType.待审核 : Constant.OlineStatusType.待支付;
           order.SoOrder.Status = (int)OrderStatus.销售单状态.待审核;
           order.SoOrder.PayStatus = (int)OrderStatus.销售单支付状态.未支付;
           order.SoOrder.OrderCreatorSysNo = order.SoOrder.LastUpdateBy = user.SysNo;
           order.SoOrder.CreateDate = order.SoOrder.LastUpdateDate = DateTime.Now;
           order.SoOrder.DefaultWarehouseSysNo = BLL.Warehouse.WhWarehouseBo.Instance.GetDeliveryUserWarehouseSysNo(user.SysNo);//获取仓库信息
           order.SoOrder.OrderSource =(int) OrderStatus.销售单来源.业务员下单;
           order.SoOrder.OrderSourceSysNo = user.SysNo;
           if (order.SoOrder.DefaultWarehouseSysNo<1)
           {
               throw new HytException("配送员仓库信息获取失败");
           }
           var whinfo = WhWarehouseBo.Instance.GetWarehouse(order.SoOrder.DefaultWarehouseSysNo);//仓库信息
           if (hasDelivery)//马上配送
           {
               order.SoOrder.DeliveryTypeSysNo = DeliveryType.普通百城当日达;
               if(payType.PaymentType == (int)BasicStatus.支付方式类型.到付)//此情况不允许下到付订单
               {
                   throw new HytException("此流程必须先收款。");
               }
           }
           else//不必立刻配送
           {
               bool Isdrd = false;//是否满足当日达
               if (whinfo != null)
               {
                   var zb = Hyt.BLL.Map.BaiduMapAPI.Instance.Geocoder(order.SoReceiveAddress.StreetAddress, whinfo.CityName);//仓库所在城市->收货地址
                   if (zb != null)
                   {
                       Isdrd = LgDeliveryScopeBo.Instance.IsInScope(whinfo.CitySysNo, new Coordinate() { X = zb.Lng, Y = zb.Lat });//是否支持当日达
                   }
               }
               order.SoOrder.DeliveryTypeSysNo = Isdrd?DeliveryType.普通百城当日达:DeliveryType.第三方快递;
           }
           ISoReceiveAddressDao.Instance.InsertEntity(order.SoReceiveAddress);//收货地址
           order.SoOrder.ReceiveAddressSysNo = order.SoReceiveAddress.SysNo;//收货地址
           if (order.Invoice != null)//发票信息
           {
               order.Invoice.Status = FinanceStatus.发票状态.待开票.GetHashCode();
               order.Invoice.CreatedBy = order.Invoice.LastUpdateBy = user.SysNo;
               order.Invoice.CreatedDate = order.Invoice.LastUpdateDate = DateTime.Now;
               IFnInvoiceDao.Instance.InsertEntity(order.Invoice);
               order.SoOrder.InvoiceSysNo = order.Invoice.SysNo;
           }
           ISoOrderDao.Instance.InsertEntity(order.SoOrder);//创建订单
           order.SoOrder = SoOrderBo.Instance.GetEntity(order.SoOrder.SysNo);//获取订单
           foreach (var item in order.Products)//创建订单明细
           {
               item.OrderSysNo = order.SoOrder.SysNo;
               item.TransactionSysNo = order.SoOrder.TransactionSysNo;
               item.SysNo = ISoOrderItemDao.Instance.Insert(item);
           }
           SoOrderBo.Instance.WriteSoTransactionLog(order.SoOrder.TransactionSysNo, string.Format(Constant.ORDER_TRANSACTIONLOG_CREATE, order.SoOrder.SysNo), user.UserName);//创建订单日志
           #endregion

           #region 订单收款单
           decimal shoupay= SoOrderBo.Instance.SynchronousOrderAmount(order.SoOrder.SysNo, true);//同步订单价格
           order.SoOrder.CashPay = shoupay;
           order.SoOrder.ProductChangeAmount = order.Products.Sum(m => m.ChangeAmount);//记录调价总额
           Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(order.SoOrder);//创建订单收款单
           if (payType.PaymentType == (int)BasicStatus.支付方式类型.预付)
           {
               var easinfo = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetFnReceiptTitleAssociation(order.SoOrder.DefaultWarehouseSysNo, payType.SysNo).OrderByDescending(m => m.IsDefault).FirstOrDefault();//eas信息
               FnReceiptVoucherItem fvitem = new FnReceiptVoucherItem()
               {
                   Amount = order.SoOrder.CashPay,
                   CreatedDate = DateTime.Now,
                   PaymentTypeSysNo = order.SoOrder.PayTypeSysNo,
                   TransactionSysNo = order.SoOrder.TransactionSysNo,
                   Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效,
                   EasReceiptCode = easinfo==null?string.Empty:easinfo.EasReceiptCode,//eas收款科目
                   ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.仓库,//收款方来源
                   ReceivablesSideSysNo = order.SoOrder.DefaultWarehouseSysNo,
                   CreatedBy=user.SysNo
               };
               Finance.FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(order.SoOrder.SysNo, fvitem);//插入收款单明细
               Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.AutoConfirmReceiptVoucher(order.SoOrder.SysNo, user);//收现金自动确认收款单
               if (order.SoOrder.PayTypeSysNo == PaymentType.现金预付)
               {
                   InsertTwoSaleCashHistory(
                       new Rp_业务员二次销售()
                       {
                           CreateDate = order.SoOrder.CreateDate,
                           DeliveryUserSysNo = user.SysNo,
                           DeliveryUserName = user.UserName,
                           StockSysNo = whinfo.SysNo,
                           StockName = whinfo.WarehouseName,
                           OrderSysNo = order.SoOrder.SysNo,
                           OrderAmount = order.SoOrder.CashPay
                       }
                   );
               }//二次销售收款记录
           }
           #endregion
           //同步支付时间的到订单主表
           ISoOrderDao.Instance.UpdateOrderPayDteById(order.SoOrder.SysNo);
           #region 创建审单任务（可选)
           if (!hasDelivery)//未配送
           {
               SyJobPoolPublishBo.Instance.OrderAuditBySysNo(order.SoOrder.SysNo);//创建审单任务
               SyJobDispatcherBo.Instance.WriteJobLog(string.Format("二次销售下单成功创建订单审核任务，销售单编号:{0}", order.SoOrder.SysNo), order.SoOrder.SysNo, null, 0);
           }
           #endregion

           return order;
       }

        /// <summary>
       /// 创建业务员配送单（但是未结算)
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="user">业务员信息</param>
       /// <remarks>2014-9-17  朱成果 创建</remarks>
       public void DeliveryTwoSaleSoOrder(AppOrder order, SyUser user)
       {
           bool flg = SoOrderBo.Instance.AuditSoOrder(order.SoOrder.SysNo, user.SysNo, false);//审核订单
           if (flg)
           {
               foreach(var pitem in order.Products)
               {
                   pitem.RealStockOutQuantity = pitem.Quantity;////如果不设置分摊会出错
               }
               var outStock = SoOrderBo.Instance.CreateOutStock(order.Products, order.SoOrder.DefaultWarehouseSysNo, user);//订单出库
               if (outStock != null)
               {
                   DeliveryTwoSaleSoOrder(order, user, outStock);//创建业务员配送单（但是未结算)
               }
           }
       }
        /// <summary>
        /// 创建业务员配送单（但是未结算)
        /// </summary>
       /// <param name="order">订单信息</param>
       /// <param name="user">业务员信息</param>
       /// <param name="outStock">出库单信息</param>
       /// <remarks>2014-9-17  朱成果 创建</remarks>
       private void DeliveryTwoSaleSoOrder(AppOrder order, SyUser user, WhStockOut outStock)
       {
          if(order.Invoice!=null)
          {
              order.Invoice.LastUpdateBy = user.SysNo;
              order.Invoice.LastUpdateDate = DateTime.Now;     
              order.Invoice.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.发票状态.已开票;
              Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(order.Invoice); //更新发票 余勇 修改 改为调用业务层方法
          }
          outStock.Status = (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.配送中;
          outStock.StockOutBy = user.SysNo;
          outStock.StockOutDate = DateTime.Now;
          Hyt.DataAccess.Warehouse.IOutStockDao.Instance.Update(outStock);//更新出库单状态
          GenerateOtherDataForTwoSale(order, user, outStock);//创建业务员配送单
          Hyt.BLL.Warehouse.WhWarehouseBo.Instance.UpdateErpProductNumber(outStock.SysNo);//修改EAS库存
       }

      /// <summary>
      /// 构建业务员配送单信息
      /// </summary>
      /// <param name="order">订单</param>
      /// <param name="user">业务员</param>
      /// <param name="outStock">出库单</param>
       /// <remarks>2014-9-17  朱成果 创建</remarks>
       private void GenerateOtherDataForTwoSale(AppOrder order, SyUser user, WhStockOut outStock)
       {
           var payType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(order.SoOrder.PayTypeSysNo);
           bool isNeedPay = payType.PaymentType == (int)BasicStatus.支付方式类型.预付;//是否预付
           var delivery = new LgDelivery()
           {
               DeliveryUserSysNo = user.SysNo,
               DeliveryTypeSysNo = outStock.DeliveryTypeSysNo,
               CreatedBy = user.SysNo,
               CreatedDate = DateTime.Now,
               Status = (int)LogisticsStatus.配送单状态.配送在途,
               StockSysNo = outStock.WarehouseSysNo,
               IsEnforceAllow = 0,
               PaidAmount = isNeedPay ? order.SoOrder.CashPay : 0,
               CODAmount = isNeedPay ? 0 : order.SoOrder.CashPay,
           };
           var deliverySysNo = Hyt.DataAccess.Logistics.ILgDeliveryDao.Instance.CreateLgDelivery(delivery);//创建配送单
           var deliveryItem = new LgDeliveryItem()
           {
               DeliverySysNo = deliverySysNo,
               NoteType = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型.出库单,
               NoteSysNo = outStock.SysNo,
               Status = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单明细状态.待签收,
               AddressSysNo = outStock.ReceiveAddressSysNo,
               TransactionSysNo = outStock.TransactionSysNo,
               StockOutAmount = outStock.StockOutAmount,
               PaymentType = payType.PaymentType,
               Receivable = !isNeedPay ? order.SoOrder.CashPay : 0,
               IsCOD = !isNeedPay ? (int)Hyt.Model.WorkflowStatus.LogisticsStatus.是否到付.是 : (int)Hyt.Model.WorkflowStatus.LogisticsStatus.是否到付.否,
               Remarks = "【二次销售自动创建】",
               CreatedBy = user.SysNo,
               CreatedDate = DateTime.Now
           };
           Hyt.DataAccess.Logistics.ILgDeliveryDao.Instance.AddDeliveryItem(deliveryItem);//配送单明细
           Order.SoOrderBo.Instance.WriteSoTransactionLog(outStock.TransactionSysNo,
                                                        "出库单" + outStock.SysNo +
                                                        "分配配送成功，配送单<span style='color:red'>" +
                                                        deliverySysNo + "</span>",
                                                        user.UserName);//日志
           string msg = "出库单" + outStock.SysNo + "由百城当日达配送员<span style='color:red'>{0}</span>配送中，送货人联系电话<span style='color:red'>{1}</span>";
           msg = string.Format(msg, user.UserName, user.MobilePhoneNumber);
           Order.SoOrderBo.Instance.WriteSoTransactionLog(outStock.TransactionSysNo,msg,user.UserName);//日志
       }

       
    }
}
