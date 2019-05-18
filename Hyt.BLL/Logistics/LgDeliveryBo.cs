using Extra.UpGrade.Model;
using Extra.UpGrade.Provider;
using Hyt.BLL.Authentication;
using Hyt.BLL.Distribution;
using Hyt.BLL.Log;
using Hyt.BLL.MallSeller;
using Hyt.BLL.Product;
using Hyt.BLL.Sys;
using Hyt.BLL.Web;
using Hyt.DataAccess.Logistics;
using Hyt.DataAccess.MallSeller;
using Hyt.DataAccess.Warehouse;
using Hyt.Infrastructure.Communication;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SoOrderBo = Hyt.BLL.Order.SoOrderBo;
using WhWarehouseBo = Hyt.BLL.Warehouse.WhWarehouseBo;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 物流类
    /// </summary>
    /// <remarks>2013-06-09 沈强 创建</remarks>
    public class LgDeliveryBo : BOBase<LgDeliveryBo>, ILgDeliveryBo
    {
        #region 创建配送单

        #region 创建配送单
        /// <summary>
        /// 创建配送单
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliverUserSysNo">配送员系统编号.</param>
        /// <param name="deliverTypeSysno">配送物流方式系统编号.</param>
        /// <param name="operateUserSysNo">创建人系统编号.</param>
        /// <param name="items">配送单明细.</param>
        /// <param name="isForce">是否在配送信用额度不足时放行</param>
        /// <param name="userIp">访问者ip</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-06 何方 创建
        /// </remarks>
        public int CreateLgDelivery(int warehouseSysNo, int deliverUserSysNo, CBLgDeliveryType delivertType, int operateUserSysNo,
                                    List<LgDeliveryItem> items, bool isForce, string userIp = null)
        {
            int deliverySysNo = 0;

            //保存配送时消息发送、日志写入信息
            List<DeliveryMsg> deliveryMsgs = new List<DeliveryMsg>();

            //操作人信息
            var operationUser = SyUserBo.Instance.GetSyUser(operateUserSysNo);

            //配送员信息
            var deliveryUser = SyUserBo.Instance.GetSyUser(deliverUserSysNo);



            //配送员可用信用额度
            CBLgDeliveryUserCredit userCredit = null;
            if (delivertType.IsThirdPartyExpress == 0)
            {
                userCredit = ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(deliverUserSysNo,
                                                                                       warehouseSysNo);
                if (userCredit == null)
                {
                    throw new Exception("配送员未添加配货信用额度");
                }
            }

            var orderSysNoList = new List<int>();
            List<KeyValuePair<int, string>> tabobaoOrderExpress = new List<KeyValuePair<int, string>>();//淘宝运单号
            //保存出库单系统编号集合
            var stockSysNos = new List<int>();


            var delivery = new LgDelivery()
            {
                DeliveryUserSysNo = deliverUserSysNo,
                DeliveryTypeSysNo = delivertType.SysNo,
                CreatedBy = operateUserSysNo,
                CreatedDate = DateTime.Now,
                Status =
                    (int)LogisticsStatus.配送单状态.配送在途,
                //(delivertType.IsThirdPartyExpress == 1
                //     ? LogisticsStatus.配送单状态.已结算
                //     : LogisticsStatus.配送单状态.配送在途),
                StockSysNo = warehouseSysNo,
                IsEnforceAllow = isForce ? 1 : 0
            };
            deliverySysNo = ILgDeliveryDao.Instance.CreateLgDelivery(delivery);
            delivery.SysNo = deliverySysNo;


            //保存取件单系统编号集合
            var pickUpSysNos = new List<int>();

            var tmpItems = new List<LgDeliveryItem>();

            //循环
            foreach (var item in items)
            {
                item.DeliverySysNo = deliverySysNo;

                var r = CanAddToDeliery(delivertType, item.NoteType, item.NoteSysNo,
                                        delivertType.IsThirdPartyExpress, true,
                                        deliverUserSysNo, warehouseSysNo);
                if (!r.Status)
                {
                    throw new Exception(r.Message);
                }

                var deliveryItem = new LgDeliveryItem
                {
                    DeliverySysNo = deliverySysNo,
                    NoteType = item.NoteType,
                    NoteSysNo = item.NoteSysNo,
                    Status = (int)
                             (delivertType.IsThirdPartyExpress == 1
                                  ? LogisticsStatus.配送单明细状态.已签收
                                  : LogisticsStatus.配送单明细状态.待签收),
                    ExpressNo = item.ExpressNo,
                    CreatedDate = DateTime.Now,
                    CreatedBy = operateUserSysNo
                };

                if (item.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                {
                    #region

                    WhStockOut stockOut = WhWarehouseBo.Instance.Get(item.NoteSysNo);
                    orderSysNoList.Add(stockOut.OrderSysNO);
                    SoOrder soOrder = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);
                    //第三方快递的升仓订单
                    if (delivertType.IsThirdPartyExpress == 1 && soOrder != null && soOrder.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱)
                    {
                        #region 一个快递单号只能给一个升舱订单使用
                        if (!string.IsNullOrEmpty(item.ExpressNo)) //快递单号不为空
                        {
                            foreach (var itx in tabobaoOrderExpress)
                            {
                                if (itx.Value == item.ExpressNo && itx.Key != soOrder.SysNo)//订单号不同，运单号相同不允许
                                {
                                    throw new Exception("一个快递单号只能给一个升舱订单使用.");
                                }
                            }
                            tabobaoOrderExpress.Add(new KeyValuePair<int, string>(soOrder.SysNo, item.ExpressNo));
                        }
                        #endregion
                    }
                    CrCustomer crCustomer = CRM.CrCustomerBo.Instance.GetCrCustomerItem(soOrder.CustomerSysNo);
                    //收货地址
                    SoReceiveAddress soReceive =
                        Order.SoOrderBo.Instance.GetOrderReceiveAddress(soOrder.ReceiveAddressSysNo);
                    BsPaymentType bsPaymentType = Basic.PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo);

                    //deliveryItem.PaymentType = soOrder.PayTypeSysNo;
                    deliveryItem.PaymentType = bsPaymentType.PaymentType;
                    deliveryItem.AddressSysNo = stockOut.ReceiveAddressSysNo;
                    //deliveryItem.PaymentType = stockOut.IsCOD == (int) LogisticsStatus.是否到付.是 ? 20 : 10;//TODO缺少枚举;
                    deliveryItem.Receivable = stockOut.Receivable;
                    deliveryItem.IsCOD = stockOut.IsCOD;
                    deliveryItem.TransactionSysNo = stockOut.TransactionSysNo;
                    deliveryItem.StockOutAmount = stockOut.StockOutAmount;

                    //保存出库单系统编号
                    stockSysNos.Add(item.NoteSysNo);
                    switch (stockOut.IsCOD)
                    {
                        case 1:
                            delivery.CODAmount += stockOut.Receivable;
                            break;
                        case 0:
                            //delivery.PaidAmount += stockOut.StockOutAmount;
                            delivery.PaidAmount += soOrder.CashPay;
                            break;
                    }

                    //得到手机号
                    string mobilePhoneNum = string.IsNullOrEmpty(soReceive.MobilePhoneNumber)
                                                ? crCustomer.MobilePhoneNumber
                                                : soReceive.MobilePhoneNumber;


                    Order.SoOrderBo.Instance.WriteSoTransactionLog(stockOut.TransactionSysNo,
                                                                   "出库单" + stockOut.SysNo +
                                                                   "分配配送成功，配送单<span style='color:red'>" +
                                                                   deliverySysNo + "</span>",
                                                                   operationUser.UserName);
                    //非三方快递订单日志记录
                    if (delivertType.IsThirdPartyExpress == 0)
                    {
                        string msg = "出库单" + stockOut.SysNo + "由百城当日达配送员<span style='color:red'>{0}</span>配送中，送货人联系电话<span style='color:red'>{1}</span>";
                        msg = string.Format(msg, deliveryUser.UserName, deliveryUser.MobilePhoneNumber);
                        Order.SoOrderBo.Instance.WriteSoTransactionLog(stockOut.TransactionSysNo,
                                                                       msg,
                                                                       operationUser.UserName);
                    }
                    //三方快递订单日志记录
                    else if (delivertType.IsThirdPartyExpress == 1)
                    {

                        string msg = "你的出库单" + stockOut.SysNo + "由<span style='color:red'>{0}</span>发出,快递单号:<span style='color:red'>{1}</span> 查询地址:<a href='{2}' target='_blank'><span style='color:red'>{3}</span></a>";

                        msg = string.Format(msg, delivertType.DeliveryTypeName, item.ExpressNo,
                                            delivertType.TraceUrl, delivertType.TraceUrl);
                        Order.SoOrderBo.Instance.WriteSoTransactionLog(stockOut.TransactionSysNo,
                                                                       msg,
                                                                       operationUser.UserName);
                    }

                    #region 保存待发送的信息
                    //保存待发送的信息
                    var deliveryMsg = new DeliveryMsg()
                    {
                        CustomerEmailAddress = crCustomer.EmailAddress,
                        CustomerSysNo = crCustomer.SysNo,
                        DeliveryTypeName = delivertType.DeliveryTypeName,
                        ExpressNo = item.ExpressNo,
                        IsThirdPartyExpress = delivertType.IsThirdPartyExpress,
                        MobilePhoneNum = mobilePhoneNum,
                        OrderSysNo = soOrder.SysNo,
                        StockOutSysNo = stockOut.SysNo,
                        StockOutTransactionSysNo = stockOut.TransactionSysNo,
                        OperationUserName = operationUser.UserName
                    };
                    if (delivertType.IsThirdPartyExpress == 0)
                    {
                        SyUser syUser = Sys.SyUserBo.Instance.GetSyUser(delivery.DeliveryUserSysNo);
                        deliveryMsg.UserName = syUser.UserName;
                        deliveryMsg.UserMobilePhoneNum = syUser.MobilePhoneNumber;
                    }
                    #endregion

                    deliveryMsgs.Add(deliveryMsg);

                    #endregion
                }

                if (item.NoteType == (int)LogisticsStatus.配送单据类型.取件单)
                {
                    pickUpSysNos.Add(item.NoteSysNo);
                    LgPickUp lgPick = LgPickUpBo.Instance.GetPickUp(item.NoteSysNo);
                    deliveryItem.AddressSysNo = lgPick.PickupAddressSysNo;
                }
                tmpItems.Add(deliveryItem);

                //调用添加配送单明细方法
                AddDeliveryItem(deliveryItem, operationUser);

                string tmp = item.NoteType == (int)LogisticsStatus.配送单据类型.出库单 ? "开始配送" : "开始取件";

                LogStatus.系统日志目标类型 enumTmp = item.NoteType == (int)LogisticsStatus.配送单据类型.出库单
                                                 ? LogStatus.系统日志目标类型.出库单
                                                 : LogStatus.系统日志目标类型.取件单;

                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, tmp,
                                         enumTmp, item.NoteSysNo, operateUserSysNo);
            }

            if (delivertType.IsThirdPartyExpress == 0)
            {
                //更新配送员可用信用额度
                var credit = Instance.GetDeliveryCredit(stockSysNos.ToArray());
                var pickUpAmount = Instance.GetLgPickUpTotalCount(pickUpSysNos.ToArray());
                var sum = credit + pickUpAmount;
                //userCredit.RemainingDeliveryCredit -= sum;
                //ILgDeliveryUserCreditDao.Instance.Update(userCredit);
                DeliveryUserCreditBo.Instance.UpdateRemaining(warehouseSysNo, deliverUserSysNo,
                                                              -sum, 0, "创建配送单，单号：" + deliverySysNo);
            }
            // 计算总金额
            ILgDeliveryDao.Instance.Update(delivery);

            //第三方快递方的配送单创建以后
            if (delivertType.IsThirdPartyExpress == 1)
            {
                //自动创建结算单，状态：已结算
                //结算单明细状态：已签收
                //CreateSettlement
                var cbCreateSettlement = new CBCreateSettlement
                {
                    DeliverySysNos = new[] { delivery.SysNo },
                    OperatorSysNo = operateUserSysNo
                };

                var stockOutInfos = new List<StockOutInfo>();
                foreach (var lgDeliveryItem in tmpItems)
                {
                    if (lgDeliveryItem.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                    {
                        var stockOutInfo = new StockOutInfo
                        {
                            StockOutSysNo = lgDeliveryItem.NoteSysNo,
                            Status = lgDeliveryItem.Status
                        };
                        stockOutInfos.Add(stockOutInfo);
                    }
                }

                cbCreateSettlement.StockOutInfos = stockOutInfos;
                var result = LgSettlementBo.Instance.CreateSettlement(cbCreateSettlement, userIp, false);
                if (!result.Status)
                {
                    throw new Exception(result.Message);
                }

                // 更新第三方快递前端状态显示为"已发货"
                foreach (var orderId in orderSysNoList)
                {
                    SoOrderBo.Instance.UpdateOnlineStatusByOrderID(orderId, "已发货");
                }
            }

            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "配送单创建",
                                     LogStatus.系统日志目标类型.配送单, deliverySysNo, operateUserSysNo);



            #region 发送相关短消息

            //发送相关消息
            /* 2015-11-10 王耀发 注释 
            foreach (var msg in deliveryMsgs)
            {
                //Order.SoOrderBo.Instance.WriteSoTransactionLog(msg.StockOutTransactionSysNo,
                //                                                      "出库单" + msg.StockOutSysNo + "已配送，待结算",
                //                                                      msg.OperationUserName);
                if (msg.IsThirdPartyExpress == 0)
                {
                    new BLL.Extras.SmsBO().发送自建物流发货通知短信(msg.MobilePhoneNum, msg.OrderSysNo.ToString(),
                                                       msg.UserName, msg.UserMobilePhoneNum);
                    new BLL.Extras.EmailBo().发送百城当日达发货邮件(msg.CustomerEmailAddress, msg.CustomerSysNo.ToString(),
                                                        msg.OrderSysNo.ToString(), msg.UserName,
                                                        msg.UserMobilePhoneNum);
                }

                if (msg.IsThirdPartyExpress == 1)
                {
                    new BLL.Extras.SmsBO().发送第三方物流发货通知短信(msg.MobilePhoneNum, msg.OrderSysNo.ToString());
                    new BLL.Extras.EmailBo().发送第三方物流发货邮件(msg.CustomerEmailAddress, msg.CustomerSysNo.ToString(),
                                                         msg.OrderSysNo.ToString(), msg.DeliveryTypeName, msg.ExpressNo);
                }
            }
            */
            #endregion



            #region 出库单出库的时候不再减EAS库存，此处减EAS库存 2014-04-09 朱成果

            foreach (var osno in stockSysNos)
            {
                // todo:减EAS库存
                Hyt.BLL.Warehouse.WhWarehouseBo.Instance.UpdateErpProductNumber(osno);
            }
            #endregion

            //返回配送单号
            return deliverySysNo;
        }

        #endregion

        #region 新版创建配送单




        /// <summary>
        /// 创建配送单
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliverUserSysNo">配送员系统编号.</param>
        /// <param name="deliverTypeSysno">配送物流方式系统编号.</param>
        /// <param name="operateUserSysNo">创建人系统编号.</param>
        /// <param name="items">配送单明细.</param>
        /// <param name="isForce">是否在配送信用额度不足时放行</param>
        /// <param name="DeliveryMsg">配送时消息发送、日志写入信息</param>
        /// <param name="userIp">访问者ip</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-06 何方 创建
        /// 2014-05-09 杨浩 新建方法 何永东 杨文兵修改第三方调用
        /// </remarks>
        public int NewCreateLgDelivery(int warehouseSysNo, int deliverUserSysNo, CBLgDeliveryType delivertType, int operateUserSysNo,
                                    List<LgDeliveryItem> items, bool isForce, ref List<DeliveryMsg> deliveryMsgs, string userIp = null)
        {
            int deliverySysNo = 0;

            //保存配送时消息发送、日志写入信息
            deliveryMsgs = new List<DeliveryMsg>();

            //操作人信息
            var operationUser = SyUserBo.Instance.GetSyUser(operateUserSysNo);
            if (operationUser == null)
            {
                operationUser = new SyUser();
            }
            //配送员信息
            var deliveryUser = SyUserBo.Instance.GetSyUser(deliverUserSysNo);



            //配送员可用信用额度
            //CBLgDeliveryUserCredit userCredit = null;
            if (delivertType.IsThirdPartyExpress == 0)
            {
                //userCredit = ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(deliverUserSysNo,
                //                                                                       warehouseSysNo);
                //if (userCredit == null)
                //{
                //    throw new Exception("配送员未添加配货信用额度");
                //}
            }

            var orderList = new Dictionary<int, SoOrder>();
            List<KeyValuePair<int, string>> tabobaoOrderExpress = new List<KeyValuePair<int, string>>();//淘宝运单号
            //保存出库单系统编号集合
            var stockSysNos = new List<int>();


            var delivery = new LgDelivery()
            {
                DeliveryUserSysNo = deliverUserSysNo,
                DeliveryTypeSysNo = delivertType.SysNo,
                CreatedBy = operateUserSysNo,
                CreatedDate = DateTime.Now,
                Status =
                   (int)LogisticsStatus.配送单状态.配送在途,
                //(delivertType.IsThirdPartyExpress == 1
                //     ? (int)LogisticsStatus.配送单状态.已结算
                //     : (int)LogisticsStatus.配送单状态.配送在途),
                StockSysNo = warehouseSysNo,
                IsEnforceAllow = isForce ? 1 : 0
            };
            deliverySysNo = ILgDeliveryDao.Instance.CreateLgDelivery(delivery);
            delivery.SysNo = deliverySysNo;


            //保存取件单系统编号集合
            var pickUpSysNos = new List<int>();

            var tmpItems = new List<LgDeliveryItem>();

            //循环
            foreach (var item in items)
            {
                item.DeliverySysNo = deliverySysNo;

                var r = CanAddToDeliery(delivertType, item.NoteType, item.NoteSysNo,
                                        delivertType.IsThirdPartyExpress, true,
                                        deliverUserSysNo, warehouseSysNo);
                if (!r.Status)
                {
                    throw new Exception(r.Message);
                }

                var deliveryItem = new LgDeliveryItem
                {
                    DeliverySysNo = deliverySysNo,
                    NoteType = item.NoteType,
                    NoteSysNo = item.NoteSysNo,
                    Status = (int)
                             (delivertType.IsThirdPartyExpress == 1
                                  ? LogisticsStatus.配送单明细状态.已签收
                                  : LogisticsStatus.配送单明细状态.待签收),
                    ExpressNo = item.ExpressNo,
                    CreatedDate = DateTime.Now,
                    CreatedBy = operateUserSysNo
                };

                if (item.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                {
                    #region  出库单

                    WhStockOut stockOut = WhWarehouseBo.Instance.Get(item.NoteSysNo);
                    //orderSysNoList.Add(stockOut.OrderSysNO);
                    SoOrder soOrder = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);

                    if (!orderList.ContainsKey(soOrder.SysNo))
                    {
                        orderList.Add(soOrder.SysNo, soOrder);
                    }

                    //第三方快递的升仓订单
                    if (delivertType.IsThirdPartyExpress == 1 && soOrder != null)
                    {
                        if (!string.IsNullOrEmpty(item.ExpressNo)) //快递单号不为空
                        {
                            #region 一个快递单号只能给一个升舱订单使用
                            if (soOrder.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱)
                            {
                                foreach (var itx in tabobaoOrderExpress)
                                {
                                    if (itx.Value == item.ExpressNo && itx.Key != soOrder.SysNo)//订单号不同，运单号相同不允许
                                    {
                                        throw new Exception("一个快递单号只能给一个升舱订单使用.");
                                    }
                                }
                                tabobaoOrderExpress.Add(new KeyValuePair<int, string>(soOrder.SysNo, item.ExpressNo));
                            }
                            #endregion
                        }
                        else
                        {
                            throw new Exception("第三方快递的订单快递单号不能为空"); //增加校验第三方快递的订单快递单号不能为空 余勇 2014-07-24
                        }

                    }
                    CrCustomer crCustomer = CRM.CrCustomerBo.Instance.GetCrCustomerItem(soOrder.CustomerSysNo);
                    //收货地址
                    SoReceiveAddress soReceive =
                        Order.SoOrderBo.Instance.GetOrderReceiveAddress(soOrder.ReceiveAddressSysNo);
                    BsPaymentType bsPaymentType = Basic.PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo);
                    if (bsPaymentType == null)
                    {
                        deliveryItem.PaymentType = 10; //海带订单支付方式默认预存
                    }
                    else
                    {
                        deliveryItem.PaymentType = bsPaymentType.PaymentType;
                    }
                    //deliveryItem.PaymentType = soOrder.PayTypeSysNo;
                    deliveryItem.AddressSysNo = stockOut.ReceiveAddressSysNo;
                    //deliveryItem.PaymentType = stockOut.IsCOD == (int) LogisticsStatus.是否到付.是 ? 20 : 10;//TODO缺少枚举;
                    deliveryItem.Receivable = stockOut.Receivable;
                    deliveryItem.IsCOD = stockOut.IsCOD;
                    deliveryItem.TransactionSysNo = stockOut.TransactionSysNo;
                    deliveryItem.StockOutAmount = stockOut.StockOutAmount;

                    //保存出库单系统编号
                    stockSysNos.Add(item.NoteSysNo);
                    switch (stockOut.IsCOD)
                    {
                        case 1:
                            delivery.CODAmount += stockOut.Receivable;
                            break;
                        case 0:
                            //delivery.PaidAmount += stockOut.StockOutAmount;
                            delivery.PaidAmount += soOrder.CashPay;
                            break;
                    }

                    //得到手机号
                    string mobilePhoneNum = string.IsNullOrEmpty(soReceive.MobilePhoneNumber)
                                                ? crCustomer.MobilePhoneNumber
                                                : soReceive.MobilePhoneNumber;


                    Order.SoOrderBo.Instance.WriteSoTransactionLog(stockOut.TransactionSysNo,
                                                                   "出库单" + stockOut.SysNo +
                                                                   "分配配送成功，配送单<span style='color:red'>" +
                                                                   deliverySysNo + "</span>",
                                                                   operationUser.UserName);
                    //非三方快递订单日志记录
                    if (delivertType.IsThirdPartyExpress == 0)
                    {
                        string msg = "出库单" + stockOut.SysNo + "由配送员<span style='color:red'>{0}</span>配送中，送货人联系电话<span style='color:red'>{1}</span>";
                        msg = string.Format(msg, deliveryUser.UserName, deliveryUser.MobilePhoneNumber);
                        Order.SoOrderBo.Instance.WriteSoTransactionLog(stockOut.TransactionSysNo,
                                                                       msg,
                                                                       operationUser.UserName);
                    }
                    //三方快递订单日志记录
                    else if (delivertType.IsThirdPartyExpress == 1)
                    {

                        string msg = "你的出库单" + stockOut.SysNo + "由<span style='color:red'>{0}</span>发出,快递单号:<span style='color:red'>{1}</span> 查询地址:<a href='{2}' target='_blank'><span style='color:red'>{3}</span></a>";

                        msg = string.Format(msg, delivertType.DeliveryTypeName, item.ExpressNo,
                                            delivertType.TraceUrl, delivertType.TraceUrl);
                        Order.SoOrderBo.Instance.WriteSoTransactionLog(stockOut.TransactionSysNo,
                                                                       msg,
                                                                       operationUser.UserName);
                        //发送出库短信 王耀发 2015-11-06 创建,自己创建配送单
                        /*
                        if (soOrder.SendStatus == 0)
                        {
                            Hyt.BLL.Extras.SmsBO obj = new Hyt.BLL.Extras.SmsBO();
                            obj.SendMsg(mobilePhoneNum, msg, DateTime.Now);
                        }
                        */
                    }

                    #region 保存待发送的信息
                    //保存待发送的信息
                    var deliveryMsg = new DeliveryMsg()
                    {
                        CustomerEmailAddress = crCustomer.EmailAddress,
                        CustomerSysNo = crCustomer.SysNo,
                        DeliveryTypeName = delivertType.DeliveryTypeName,
                        ExpressNo = item.ExpressNo,
                        IsThirdPartyExpress = delivertType.IsThirdPartyExpress,
                        TraceUrl = delivertType.TraceUrl,
                        MobilePhoneNum = mobilePhoneNum,
                        OrderSysNo = soOrder.SysNo,
                        StockOutSysNo = stockOut.SysNo,
                        StockOutTransactionSysNo = stockOut.TransactionSysNo,
                        OperationUserName = operationUser.UserName
                    };
                    if (delivertType.IsThirdPartyExpress == 0)
                    {
                        SyUser syUser = Sys.SyUserBo.Instance.GetSyUser(delivery.DeliveryUserSysNo);
                        deliveryMsg.UserName = syUser.UserName;
                        deliveryMsg.UserMobilePhoneNum = syUser.MobilePhoneNumber;
                    }
                    #endregion

                    deliveryMsgs.Add(deliveryMsg);

                    #endregion
                }

                if (item.NoteType == (int)LogisticsStatus.配送单据类型.取件单)
                {
                    pickUpSysNos.Add(item.NoteSysNo);
                    LgPickUp lgPick = LgPickUpBo.Instance.GetPickUp(item.NoteSysNo);
                    deliveryItem.AddressSysNo = lgPick.PickupAddressSysNo;
                }
                tmpItems.Add(deliveryItem);

                //调用添加配送单明细方法
                AddDeliveryItem(deliveryItem, operationUser);

                string tmp = item.NoteType == (int)LogisticsStatus.配送单据类型.出库单 ? "开始配送" : "开始取件";

                LogStatus.系统日志目标类型 enumTmp = item.NoteType == (int)LogisticsStatus.配送单据类型.出库单
                                                 ? LogStatus.系统日志目标类型.出库单
                                                 : LogStatus.系统日志目标类型.取件单;

                Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, tmp,
                                         enumTmp, item.NoteSysNo, operateUserSysNo);
            }

            if (delivertType.IsThirdPartyExpress == 0)
            {
                //更新配送员可用信用额度
                //var credit = Instance.GetDeliveryCredit(stockSysNos.ToArray());
                //var pickUpAmount = Instance.GetLgPickUpTotalCount(pickUpSysNos.ToArray());
                //var sum = credit + pickUpAmount;
                //userCredit.RemainingDeliveryCredit -= sum;
                //ILgDeliveryUserCreditDao.Instance.Update(userCredit);
                //DeliveryUserCreditBo.Instance.UpdateRemaining(warehouseSysNo, deliverUserSysNo,
                // -sum, 0, "创建配送单，单号：" + deliverySysNo);
            }
            // 计算总金额
            ILgDeliveryDao.Instance.Update(delivery);

            //第三方快递方的配送单创建以后
            if (delivertType.IsThirdPartyExpress == 1)
            {
                //自动创建结算单，状态：已结算
                //结算单明细状态：已签收
                //CreateSettlement
                var cbCreateSettlement = new CBCreateSettlement
                {
                    DeliverySysNos = new[] { delivery.SysNo },
                    OperatorSysNo = operateUserSysNo
                };
                var expressItems = new List<rp_第三方快递发货量>();
                var stockOutInfos = new List<StockOutInfo>();
                foreach (var lgDeliveryItem in tmpItems)
                {
                    if (lgDeliveryItem.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                    {
                        var stockOutInfo = new StockOutInfo
                        {
                            StockOutSysNo = lgDeliveryItem.NoteSysNo,
                            Status = lgDeliveryItem.Status
                        };
                        stockOutInfos.Add(stockOutInfo);

                        //写入第三方快递单发货记录 余勇 2014-09-23
                        if (!expressItems.Exists(x => x.ExpressNo == lgDeliveryItem.ExpressNo))
                        {
                            expressItems.Add(new rp_第三方快递发货量
                            {
                                StockSysNo = delivery.StockSysNo,
                                CreateDate = DateTime.Now,
                                ExpressNo = lgDeliveryItem.ExpressNo,
                                CompanyName = delivertType.DeliveryTypeName,
                                Remarks = lgDeliveryItem.NoteSysNo.ToString()
                            });
                        }
                        else
                        {
                            expressItems.FirstOrDefault(x => x.ExpressNo == lgDeliveryItem.ExpressNo).Remarks +=
                                "," + lgDeliveryItem.NoteSysNo;
                        }

                    }
                }
                expressItems.ForEach(this.CreateExpressLgDelivery);

                cbCreateSettlement.StockOutInfos = stockOutInfos;
                var result = LgSettlementBo.Instance.CreateSettlement(cbCreateSettlement, userIp, false);
                if (!result.Status)
                {
                    throw new Exception(result.Message);
                }

                #region 更新订单状态
                // 更新第三方快递前端状态显示为"已发货"
                foreach (var key in orderList.Keys)
                {
                    var orderInfo = orderList[key];
                    if (orderInfo.Status == (int)OrderStatus.销售单状态.已创建出库单)
                    {
                        if (SoOrderBo.Instance.IsAllShip(key))
                        {
                            int orderStatus = (int)OrderStatus.销售单状态.出库待接收;
                            string onlineStatus = "已发货";
                            SoOrderBo.Instance.UpdateOrderStatus(key, orderStatus);
                            SoOrderBo.Instance.UpdateOnlineStatusByOrderID(key, onlineStatus);
                        }
                    }
                }
                #endregion

            }

            Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "配送单创建",
                                     LogStatus.系统日志目标类型.配送单, deliverySysNo, operateUserSysNo);

            #region 出库单出库的时候不再减EAS库存，此处减EAS库存 因暂无对接erp所以注释

            foreach (var osno in stockSysNos)
            {
                // todo:减EAS库存
                Hyt.BLL.Warehouse.WhWarehouseBo.Instance.UpdateErpProductNumber(osno);
            }
            #endregion

            //返回配送单号
            return deliverySysNo;
        }
        #endregion

        /// <summary>
        /// 根据订单编号获取配送单
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-7-16 杨浩 创建</remarks>
        public List<LgDeliveryItem> GetDeliveryItemByOrderSysNo(int sysNo)
        {
            return ILgDeliveryDao.Instance.GetDeliveryItemByOrderSysNo(sysNo);
        }

        /// <summary>
        /// 调用快递100的订阅服务（异表）
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="warehouseSysNo">The warehouse sys no.</param>
        /// <param name="delivertType">Type of the delivert.</param>
        public void CallKuaiDi100(List<LgDeliveryItem> items, int warehouseSysNo, CBLgDeliveryType delivertType)
        {
            #region 调用快递100的订阅服务


            if (delivertType.IsThirdPartyExpress == 1)  //是否为第三方快递
            {
                foreach (var item in items)
                {
                    //根据配送方式系统编号获取配送物流公司编码信息
                    string from = "";
                    string to = "";
                    var lgDeliveryCompanyCode =
                                LgDeliveryCompanyCodeBo.Instance.GetLgDeliveryCompanyCode(delivertType.SysNo);
                    if (lgDeliveryCompanyCode == null) continue;
                    if (string.IsNullOrEmpty(lgDeliveryCompanyCode.CompanyCode)) continue;

                    //发货仓库地址(城市)
                    var warehouse = WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo);
                    var fromCity = BsAreaBo.Instance.GetAreaDetail(warehouse.AreaSysNo);
                    if (fromCity != null) from = fromCity.Province + fromCity.City;

                    //收货地址(城市)
                    if (item.NoteType != (int)LogisticsStatus.配送单据类型.出库单) continue;
                    var whStockOut = WhWarehouseBo.Instance.Get(item.NoteSysNo);
                    var address = SoOrderBo.Instance.GetOrderReceiveAddress(whStockOut.ReceiveAddressSysNo);
                    var toCity = BsAreaBo.Instance.GetAreaDetail(address.AreaSysNo);
                    if (toCity != null) to = toCity.Province + toCity.City;

                    //签名字符,取9位随机数字
                    var salt = Hyt.Util.WebUtil.Number(9, false);
                    Pisen.Service.Dispatch.Result<string> res;
                    //调用快递100的订阅服务
                    using (var service = new Pisen.Service.Dispatch.ServiceProxy<Pisen.Service.Kuaidi.IKuaidiService>())
                    {
                        res = service.Channel.Subscribe(lgDeliveryCompanyCode.CompanyCode, item.ExpressNo.Trim(),
                           from, to, salt);
                    }
                    // if (!res.Status) continue; // StatusCode=700 不支持的快递公司

                    //保存出库单中的订单系统编号
                    int orderSysNo = whStockOut.OrderSysNO;

                    //获取订单实体
                    SoOrder order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);
                    var lgExpressModel = new LgExpressInfo()
                    {
                        TransactionSysNo = order.TransactionSysNo,      //订单事务编号
                        CompanyName = lgDeliveryCompanyCode.CompanyName,
                        CompanyCode = lgDeliveryCompanyCode.CompanyCode.Trim(),
                        ExpressNo = item.ExpressNo.Trim(),
                        AddressSysNo = address.AreaSysNo,
                        FromCity = from,
                        ToCity = to,
                        ExpressStatus = 0,
                        PostResultStatus = !string.IsNullOrEmpty(res.Data) ? int.Parse(res.Data) : 0,
                        PostNumber = 1,
                        PostTime = DateTime.Now,
                        Salt = salt
                    };
                    LgExpressBo.Instance.Insert(lgExpressModel);
                }
            }

            #endregion
        }


        //用于保存创建配送单时的消息发送、日志写入
        public class DeliveryMsg
        {
            /// <summary>
            /// 操作人名称
            /// </summary>
            public string OperationUserName { get; set; }
            /// <summary>
            /// 出库单系统编号
            /// </summary>
            public int StockOutSysNo { get; set; }
            /// <summary>
            /// 出库单事务编号
            /// </summary>
            public string StockOutTransactionSysNo { get; set; }

            /// <summary>
            /// 客户收货手机号
            /// </summary>
            public string MobilePhoneNum { get; set; }

            /// <summary>
            /// 是否为第三方快递
            /// </summary>
            public int IsThirdPartyExpress { get; set; }

            /// <summary>
            /// 订单系统编号
            /// </summary>
            public int OrderSysNo { get; set; }

            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// 用户手机号
            /// </summary>
            public string UserMobilePhoneNum { get; set; }

            /// <summary>
            /// 客户邮件地址
            /// </summary>
            public string CustomerEmailAddress { get; set; }

            /// <summary>
            /// 客户系统编号
            /// </summary>
            public int CustomerSysNo { get; set; }

            /// <summary>
            /// 配送方式名称
            /// </summary>
            public string DeliveryTypeName { get; set; }

            /// <summary>
            /// 快递单号
            /// </summary>
            public string ExpressNo { get; set; }

            /// <summary>
            /// 物流跟踪查询Url
            /// </summary>
            public string TraceUrl { get; set; }
        }

        /// <summary>
        /// 添加配送单明细信息
        /// </summary>
        /// <param name="deliveryItem">配送单明细实体</param>
        /// <param name="operatorUser">操作人</param>
        /// <returns>
        /// 返回配送单明细部分页面
        /// </returns>
        /// <remarks>
        /// 2013-06-26 沈强 创建
        /// </remarks>
        public int AddDeliveryItem(LgDeliveryItem deliveryItem, SyUser operatorUser)
        {
            switch ((LogisticsStatus.配送单据类型)deliveryItem.NoteType)
            {
                case LogisticsStatus.配送单据类型.出库单:
                    WhWarehouseBo.Instance.UpdateStockOutStatus(deliveryItem.NoteSysNo, WarehouseStatus.出库单状态.配送中,
                                                                operatorUser.SysNo);
                    //Order.SoOrderBo.Instance.WriteSoTransactionLog(deliveryItem.TransactionSysNo,
                    //                                               "您的货物已分配配送，配送单:" +
                    //                                               deliveryItem.DeliverySysNo,
                    //                                               operatorUser.UserName);
                    break;
                case LogisticsStatus.配送单据类型.取件单:
                    LgPickUpBo.Instance.UpdatePickUpStatus(deliveryItem.NoteSysNo, LogisticsStatus.取件单状态.取件中,
                                                           operatorUser.SysNo);
                    break;
            }

            return ILgDeliveryDao.Instance.AddDeliveryItem(deliveryItem);
        }

        /// <summary>
        /// 检查待添加的单据是否能添加到配送单中
        /// </summary>
        /// <param name="cbLgDeliveryType">配送方式</param>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteSysNo">单据编号</param>
        /// <param name="isThirdPartyExpress">是否为三方快递（是[1],否[0]）</param>
        /// <param name="isForce">是否强制扫描(1:强制扫描；0:非强制扫描)</param>
        /// <param name="deliverymanSysno">配送员系统编号（为三方快递时，此参数可不填）</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>
        /// 2013-07-04 沈强 创建
        /// </remarks> 
        private Result CanAddToDeliery(CBLgDeliveryType cbLgDeliveryType, int noteType, int noteSysNo, int isThirdPartyExpress,
                                       bool isForce, int deliverymanSysno, int warehouseSysNo)
        {
            //用于返回结果
            var result = new Result();

            ////获取配送方式
            //CBLgDeliveryType cbLgDeliveryType = DeliveryTypeBo.Instance.GetDeliveryType(deliveryTypeSysNo);
            isThirdPartyExpress = cbLgDeliveryType.IsThirdPartyExpress;

            #region 检查出库/取件单是否在其他有效配送单明细中

            if (NoteInDeliveryItem((LogisticsStatus.配送单据类型)noteType
                                   , noteSysNo))
            {
                result.Status = false;
                result.Message = "单据 " + noteSysNo + " 已出现在其他有效配送单中，不可重复添加！";
                return result;
            }

            #endregion

            #region 出库单检查

            if (noteType == (int)LogisticsStatus.配送单据类型.出库单)
            {
                WhStockOut whStockOut = WhWarehouseBo.Instance.Get(noteSysNo);

                if (whStockOut == null)
                {
                    result.Status = false;
                    result.Message = "有不存在的出库单！";
                    return result;
                }

                if (whStockOut.WarehouseSysNo != warehouseSysNo)
                {
                    result.Status = false;
                    result.Message = "出库单 " + whStockOut.SysNo + " 所在仓库与指定的仓库不匹配！";
                    return result;
                }

                //检查出库单配送类型是否一致
                if (whStockOut.DeliveryTypeSysNo != cbLgDeliveryType.SysNo)
                {
                    CBLgDeliveryType tmp = DeliveryTypeBo.Instance.GetDeliveryType(whStockOut.DeliveryTypeSysNo);
                    result.Status = false;
                    result.Message = "出库单 " + whStockOut.SysNo + " 配送方式为 " + tmp.DeliveryTypeName +
                                     " 与创建配送单时指定的不一致，不能添加！";
                    return result;
                }

                if (whStockOut.Status != (int)WarehouseStatus.出库单状态.待配送)
                {
                    result.Status = false;
                    result.Message = "编号为 " + whStockOut.SysNo + " 的出库单不是待配送状态，无法添加！";
                    return result;
                }

                //保存出库单中的订单系统编号
                int orderSysNo = whStockOut.OrderSysNO;

                //获取订单实体
                SoOrder order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);

                if (order.PayStatus == (int)OrderStatus.销售单支付状态.支付异常)
                {
                    throw new Exception("订单：" + orderSysNo + OrderStatus.销售单支付状态.支付异常.ToString());
                }

                //非三方快递时的检查
                if (isThirdPartyExpress == 0 && order.PayStatus == (int)OrderStatus.销售单支付状态.未支付)
                {
                    #region  出库单拆单: 检查出库单表中是否有相同订单系统编号的出库单

                    IList<WhStockOut> whStockOuts = WhWarehouseBo.Instance.GetWhStockOutListByOrderID(orderSysNo);
                    int count = whStockOuts.Count;

                    #region 出库单数量大于1时的处理方式

                    if (count > 1)
                    {
                        //检查出库单集合中是否有待配送、配送中、已签收的出库单
                        int statusCount = whStockOuts.Count(w =>
                            {
                                return (
                                           w.Status == (int)WarehouseStatus.出库单状态.待配送
                                           || w.Status == (int)WarehouseStatus.出库单状态.配送中
                                           || w.Status == (int)WarehouseStatus.出库单状态.已签收
                                       );
                            });

                        if (statusCount > 0)
                        {
                            //如果多个出库单中没有已签收,则判断是否存在配送中的出库单
                            if (whStockOuts.All(s => s.Status != (int)WarehouseStatus.出库单状态.已签收))
                            {
                                //检查出库单状态是否都处于配送中
                                if (whStockOuts.Any(s => s.Status == (int)WarehouseStatus.出库单状态.配送中))
                                {
                                    result.Status = false;
                                    result.Message = "系统编号为 " + whStockOut.SysNo + " 的出库单所在的订单中，第一个出库单未签收，因此不能添加此出库单：" +
                                                     whStockOut.SysNo + "到配送单中！";
                                    return result;
                                }
                            }
                        }
                    }

                    #endregion

                    #endregion
                }

                //三方快递时的检查
                if (isThirdPartyExpress == 1)
                {
                    if (whStockOut.Receivable != 0m)
                    {
                        result.Status = false;
                        result.Message = "编号为 " + whStockOut.SysNo + " 的出库单应收款不为零，不能使用第三方快递配送！";
                        return result;
                    }

                    if (order.PayStatus != (int)OrderStatus.销售单支付状态.已支付)
                    {
                        result.Status = false;
                        result.Message = "编号为 " + whStockOut.SysNo + " 的出库单所在订单还未支付，不能使用第三方快递配送！";
                        return result;
                    }
                }

                #region 是否强制扫描 取消该功能

                //检测出库单创建时间是否已经超过两个星期，
                //并且收货地址是否与配送员覆盖地区相同
                //if (whStockOut.CreatedDate.Date.AddDays(14) < DateTime.Now.Date)
                //{
                //    //超过两个星期或收货地址与配送员覆盖地区不同，检测是否允许强制扫描
                //    if (!isForce)
                //    {
                //        result.Status = false;
                //        result.Message = "系统编号为 " + whStockOut.SysNo + " 的出库单创建时间已超过两个星期，请选择强制扫描再添加！";
                //        return result;
                //    }
                //}

                //非三方快递时的检查
                //if (IsThirdPartyExpress == 0)
                //{
                //    //获取订单收货地址
                //    SoReceiveAddress soReceiveAddress = Order.SoOrderBo.Instance.GetOrderReceiveAddress(whStockOut.ReceiveAddressSysNo);
                //    if (!IsInDeliveryUserAreas(soReceiveAddress.AreaSysNo, deliverymanSysno))
                //    {
                //        result.Status = false;
                //        result.Message = "系统编号为 " + whStockOut.SysNo + " 的出库单收货地址与配送员覆盖地区不符，请选择强制扫描再添加！";
                //        return result;
                //    }
                //}

                #endregion
            }

            #endregion

            #region 取件单检查

            if (noteType == (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型.取件单)
            {
                if (isThirdPartyExpress == 1)
                {
                    result.Status = false;
                    result.Message = "配送方式为第三方快递不能添加取件单！";
                    return result;
                }
                LgPickUp lgPick = LgPickUpBo.Instance.GetPickUp(noteSysNo);
                if (lgPick == null)
                {
                    result.Status = false;
                    result.Message = "找不到编号为： " + noteSysNo + " 的取件单！";
                    return result;
                }
                if (lgPick.WarehouseSysNo != warehouseSysNo)
                {
                    result.Status = false;
                    result.Message = "取件单 " + lgPick.SysNo + " 所在仓库与指定的仓库不匹配！";
                    return result;
                }
                if (lgPick.Status != (int)LogisticsStatus.取件单状态.待取件)
                {
                    result.Status = false;
                    result.Message = "取件单 " + lgPick.SysNo + " 不是待取件状态！";
                    return result;
                }
            }

            #endregion

            result.Status = true;
            result.Message = "检查通过";
            return result;
        }

        #endregion

        /// <summary>
        /// 读取配送单
        /// </summary>
        /// <param name="sysNo">配送单sys no.</param>
        /// <returns>配送单 实体</returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
        public LgDelivery GetDelivery(int sysNo)
        {
            //return ILgDeliveryDao.Instance.GetDelivery(sysNo);

            return GetDeliveryList(new int[] { sysNo }).FirstOrDefault();
        }

        /// <summary>
        /// 读取多个配送单
        /// </summary>
        /// <param name="sysNos">配送单系统编号集合.</param>
        /// <returns>配送单集合</returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
        public IList<LgDelivery> GetDeliveryList(int[] sysNos)
        {
            return ILgDeliveryDao.Instance.GetLgDeliveryList(sysNos);
        }

        /// <summary>
        /// 作废配送单
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="operatorSysNo">The operator sys no.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-19 何方 创建
        /// 2013-06-27 沈强 修改
        /// </remarks>
        public Result CancelDelivery(int deliverySysNo, int operatorSysNo)
        {
            Result result = new Result();

            var delivery = GetDelivery(deliverySysNo);

            if (delivery.Status == (int)LogisticsStatus.配送单状态.已结算)
            {
                result.Message = string.Format("配送单{0}已结算,不能进行作废", deliverySysNo);
                return result;
            }

            if (delivery.Status == (int)LogisticsStatus.配送单状态.作废)
            {
                result.Message = string.Format("配送单{0}已经作废,不能进行作废", deliverySysNo);
                return result;
            }

            //获取配送单明细
            var itemList = GetDeliveryItemList(deliverySysNo);

            //保存出库单系统编号
            var stockSysNos = new List<int>();
            //保存取件单系统编号
            var pickUpSysNos = new List<int>();

            foreach (var item in itemList)
            {
                if (item.Status != (int)LogisticsStatus.配送单明细状态.待签收)
                {
                    result.Message = string.Format("配送单中{0}:{1}的状态已是 {2},配送单不能作废",
                                                   (LogisticsStatus.配送单据类型)item.NoteType, item.NoteSysNo,
                                                   (LogisticsStatus.配送单明细状态)item.Status);
                    return result;
                }

                if (item.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                {
                    stockSysNos.Add(item.NoteSysNo);
                }
                if (item.NoteType == (int)LogisticsStatus.配送单据类型.取件单)
                {
                    pickUpSysNos.Add(item.NoteSysNo);
                }
            }

            #region 获取配送单信息，配送方式信息

            //获取配送单信息
            var cbLgDelivery = GetDelivery(deliverySysNo);

            //获取配送方式信息
            var deliveryType = DeliveryTypeBo.Instance.GetDeliveryType(cbLgDelivery.DeliveryTypeSysNo);

            #endregion

            //配送方式不为第三方快递时，为配送员添加信用额度
            if (deliveryType.IsThirdPartyExpress == 0)
            {
                //更新配送员可用信用额度
                //var userCredit =
                //    ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(cbLgDelivery.DeliveryUserSysNo,
                //                                                              cbLgDelivery.StockSysNo);
                //if (userCredit != null)
                //{
                var credit = Instance.GetDeliveryCredit(stockSysNos.ToArray());
                var pickUpAmount = Instance.GetLgPickUpTotalCount(pickUpSysNos.ToArray());
                var sum = credit + pickUpAmount;
                //userCredit.RemainingDeliveryCredit += sum;
                //ILgDeliveryUserCreditDao.Instance.Update(userCredit);
                DeliveryUserCreditBo.Instance.UpdateRemaining(cbLgDelivery.StockSysNo, cbLgDelivery.DeliveryUserSysNo,
                                                          sum, 0, "作废配送单，单号：" + deliverySysNo);
                //}
            }
            result.Status = ILgDeliveryDao.Instance.UpdateStatus(deliverySysNo, LogisticsStatus.配送单状态.作废);
            if (result.Status)
            {
                result.Message = "作废成功。";
                var rrr = CreateInStockByStockSysNos(stockSysNos, operatorSysNo, "配送单" + deliverySysNo + "作废");//创建入库单
                if (rrr.Status)
                {
                    result.Message += ",请注意完成入库单" + rrr.Message + "的入库操作";
                }
                var deliveryItemList = GetDeliveryItemList(deliverySysNo);
                //将该出库单所有涉及的出库单取件单状态修改为待配送/待取件
                foreach (var item in deliveryItemList)
                {
                    //更新配送单明细状态
                    item.Status = (int)LogisticsStatus.配送单明细状态.作废;
                    item.LastUpdateDate = DateTime.Now;
                    item.LastUpdateBy = operatorSysNo;
                    UpdateDeliveryItem(item);

                    switch ((LogisticsStatus.配送单据类型)item.NoteType)
                    {
                        case LogisticsStatus.配送单据类型.出库单:
                            WhWarehouseBo.Instance.UpdateStockOutStatus(item.NoteSysNo, WarehouseStatus.出库单状态.待配送,
                                                                        operatorSysNo);
                            break;
                        case LogisticsStatus.配送单据类型.取件单:
                            LgPickUpBo.Instance.UpdatePickUpStatus(item.NoteSysNo, LogisticsStatus.取件单状态.待取件,
                                                                   operatorSysNo);
                            break;
                        default:
                            throw new Exception("修改配送单明细状态失败:配送单明细单据类型错误");
                    }
                }
            }
            else
            {
                result.Message = "作废失败";
            }


            return result;
        }

        /// <summary>
        /// 根据出库单创建入库单
        /// </summary>
        /// <param name="stockSysNos">出库单编号</param>
        /// <param name="operatorSysNo">操作人</param>
        /// <param name="remarks">备注</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-04-11 朱成果 创建
        /// </remarks> 
        public Result CreateInStockByStockSysNos(List<int> stockSysNos, int operatorSysNo, string remarks)
        {
            Result r = new Result() { Status = false };
            if (stockSysNos != null)
            {
                foreach (var sno in stockSysNos)
                {
                    r.Status = true;
                    var stockOut = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.Get(sno);
                    if (stockOut == null) continue;

                    //创建入库单
                    if (!string.IsNullOrEmpty(r.Message))
                    {
                        r.Message += ",";
                    }
                    r.Message += CreateInStockByStockOut(stockOut, operatorSysNo, remarks).ToString();
                }
            }
            return r;
        }

        /// <summary>
        /// 根据出库单创建入库单
        /// </summary>
        /// <param name="stockOut">出库单包括明细</param>
        /// <param name="operatorSysNo">操作人</param>
        /// <param name="remarks">备注</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-04-11 朱成果 创建
        /// </remarks> 
        public int CreateInStockByStockOut(WhStockOut stockOut, int operatorSysNo, string remarks)
        {

            var whStockIn = new WhStockIn
             {
                 WarehouseSysNo = stockOut.WarehouseSysNo,
                 CreatedBy = operatorSysNo,
                 CreatedDate = DateTime.Now,
                 LastUpdateBy = operatorSysNo,
                 LastUpdateDate = DateTime.Now,
                 DeliveryType = (int)WarehouseStatus.入库物流方式.作废出库,
                 IsPrinted = 0,
                 SourceSysNO = stockOut.SysNo,
                 SourceType = (int)LogisticsStatus.配送单据类型.出库单,
                 Status = (int)WarehouseStatus.入库单状态.待入库,
                 Remarks = remarks
             };

            var whStockInItems = new List<WhStockInItem>();
            foreach (var item in stockOut.Items)
            {
                if (item.Status == WarehouseStatus.出库单明细状态.有效.GetHashCode())
                {
                    var wh = new WhStockInItem
                    {
                        CreatedBy = operatorSysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = operatorSysNo,
                        LastUpdateDate = DateTime.Now,
                        ProductName = item.ProductName,
                        ProductSysNo = item.ProductSysNo,
                        StockInQuantity = item.ProductQuantity,
                        SourceItemSysNo = item.SysNo //记录入库单明细来源单号（出库单明细编号)
                    };
                    whStockInItems.Add(wh);
                }
            }
            whStockIn.ItemList = whStockInItems;
            //创建入库单
            return Hyt.BLL.Warehouse.InStockBo.Instance.CreateStockIn(whStockIn);

        }

        /// <summary>
        /// 根据调拨出库单（WhInventoryOut）创建入库单
        /// </summary>
        /// <param name="inventoryOut"></param>
        /// <param name="operatorSysNo"></param>
        /// <param name="remarks"></param>
        /// <returns></returns>
        /// <remarks>2016-07-01 陈海裕 创建</remarks>
        public int CreateInStockByInventoryOut(WhInventoryOut inventoryOut, int operatorSysNo, string remarks)
        {
            var tempInvenOut = BLL.Warehouse.WhInventoryOutBo.Instance.GetWhInventoryOut(inventoryOut.SysNo);
            var entity = BLL.Warehouse.AtAllocationBo.Instance.GetAtAllocationEntity(tempInvenOut.SourceSysNO);
            var whStockIn = new WhStockIn
            {
                WarehouseSysNo = entity.EnterWarehouseSysNo,
                CreatedBy = operatorSysNo,
                CreatedDate = DateTime.Now,
                LastUpdateBy = operatorSysNo,
                LastUpdateDate = DateTime.Now,
                DeliveryType = (int)WarehouseStatus.入库物流方式.调拨出库,
                IsPrinted = 0,
                SourceSysNO = inventoryOut.SysNo,// entity.SysNo,
                SourceType = (int)WarehouseStatus.入库单据类型.调拨出库单,
                Status = (int)WarehouseStatus.入库单状态.待入库,
                Remarks = remarks
            };

            var whStockInItems = new List<WhStockInItem>();
            foreach (var item in inventoryOut.ItemList)
            {
                var wh = new WhStockInItem
                {
                    CreatedBy = operatorSysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysNo,
                    LastUpdateDate = DateTime.Now,
                    ProductName = item.ProductName,
                    ProductSysNo = item.ProductSysNo,
                    StockInQuantity = item.StockOutQuantity,  //RealStockOutQuantity
                    SourceItemSysNo = item.SysNo,
                    RealStockInQuantity = item.StockOutQuantity //新增已入库数量与入库数量相同
                };
                whStockInItems.Add(wh);
            }
            whStockIn.ItemList = whStockInItems;
            //创建入库单
            return Hyt.BLL.Warehouse.InStockBo.Instance.CreateStockIn(whStockIn);
        }

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单sys no.</param>
        /// <returns>配送单列表列表</returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
        public IList<LgDeliveryItem> GetDeliveryItemList(int deliverySysNo)
        {
            return ILgDeliveryDao.Instance.GetDeliveryItems(deliverySysNo);
        }

        /// <summary>
        /// 获取配送单集合
        /// </summary>
        /// <param name="pager">分页对象</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
        public void GetLgDelivery(ref Pager<CBLgDelivery> pager, SysAuthorization currentUser = null)
        {
            //todo:
            //var    SyGroupUserBo.Instance.GroupContainsUser(UserGroup.包含所有仓库的用户组, currentUser);
            var currentUserSysNo = currentUser == null ? 0 : currentUser.Base.SysNo;
            var hasAllWarehouse = SyUserGroupBo.Instance.IsHasAllWarehouse(currentUserSysNo);

            ILgDeliveryDao.Instance.GetLogisticsDeliveryItems(ref pager, currentUserSysNo, hasAllWarehouse);
        }

        /// <summary>
        /// 获取配送单集合
        /// </summary>
        /// <param name="pagerFilter">分页对象</param>
        /// <param name="currentUserSysNo">当前用户系统编号</param>
        /// <param name="hasAllWarehouse">是否拥有所有仓库</param>
        /// <returns>返回分页配送单集合</returns>
        /// <remarks>
        /// 2013/6/17 何方 创建
        /// </remarks>
        public Pager<CBLgDelivery> GetLgDelivery(Pager<Hyt.Model.Parameter.ParaLogisticsFilter> pagerFilter, int currentUserSysNo, bool hasAllWarehouse)
        {
            return ILgDeliveryDao.Instance.GetLogisticsDeliveryItems(pagerFilter, currentUserSysNo, hasAllWarehouse);
        }

        /// <summary>
        /// 通过订单号读取配送单列表
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>配送单列表</returns>
        /// <remarks>
        /// 2013-12-06 余勇 创建
        /// </remarks>
        public List<CBLgDelivery> GetDeliveryListByOrderSysNo(int orderSysNo)
        {
            return ILgDeliveryDao.Instance.GetDeliveryListByOrderSysNo(orderSysNo);
        }

        /// <summary>
        /// 读取配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单sys no.</param>
        /// <returns>配送单明细组合实体列表</returns>
        /// <remarks>
        /// 2013-06-19 沈强 创建
        /// </remarks>
        public IList<CBLgDeliveryItem> GetCbDeliveryItems(int deliverySysNo)
        {
            return ILgDeliveryDao.Instance.GetCBDeliveryItems(deliverySysNo);
        }

        /// <summary>
        /// 更新单个配送单明细
        /// </summary>
        /// <param name="lgDeliveryItem">配送单明细实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-20 沈强 创建
        /// </remarks>
        public bool UpdateDeliveryItem(LgDeliveryItem lgDeliveryItem)
        {
            return ILgDeliveryDao.Instance.UpdateDeliveryItem(lgDeliveryItem);
        }

        /// <summary>
        /// 更新单个配送单明细状态
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="noteType">配送单明细单据类型.</param>
        /// <param name="noteSysNo">配送单明细单据系统编号.</param>
        /// <param name="status">配送单明细状态.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-11 何方 创建
        /// </remarks>
        public bool UpdateDeliveryItemStatus(int deliverySysNo, LogisticsStatus.配送单据类型 noteType, int noteSysNo,
                                             LogisticsStatus.配送单明细状态 status, int operatorSysNo = 0)
        {
            operatorSysNo = operatorSysNo == 0 ? (Authentication.AdminAuthenticationBo.Instance.Current == null ? 12 : AdminAuthenticationBo.Instance.Current.Base.SysNo) : operatorSysNo;
            return ILgDeliveryDao.Instance.UpdateDeliveryItemStatus(deliverySysNo, noteType, noteSysNo, status, operatorSysNo);
        }

        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="deliveryItemSysNo">配送单明细系统编号.</param>
        /// <returns>配送单明细实体</returns>
        /// <remarks>
        /// 2013-06-20 沈强 创建
        /// </remarks>
        public LgDeliveryItem GetDeliveryItem(int deliveryItemSysNo)
        {
            return ILgDeliveryDao.Instance.GetDeliveryItem(deliveryItemSysNo);
        }

        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="noteSysNo">配送单明细单据编号.</param>
        /// <param name="noteType">配送单明细单据类型.</param>
        /// <returns>
        /// 配送单明细实体
        /// </returns>
        /// <remarks>
        /// 2013-08-20 何方 创建
        /// </remarks>
        public LgDeliveryItem GetDeliveryItem(int deliverySysNo, int noteSysNo, LogisticsStatus.配送单据类型 noteType)
        {
            return ILgDeliveryDao.Instance.GetDeliveryItem(deliverySysNo, noteSysNo, noteType);
        }
        /// <summary>
        /// 读取单个配送单明细
        /// </summary>
        /// <param name="noteSysNo">配送明细单据系统编号.</param>
        /// <param name="noteType">配送明细单据类型.</param>
        /// <returns>
        /// 配送单明细实体
        /// </returns>
        /// <remarks>
        /// 2016-08-02 杨浩 创建
        /// </remarks>
        public LgDeliveryItem GetDeliveryItem(int noteSysNo, LogisticsStatus.配送单据类型 noteType)
        {
            return ILgDeliveryDao.Instance.GetDeliveryItem(noteSysNo, noteType);
        }
        /// <summary>
        /// 获取当天指定的配送单列表
        /// </summary>
        /// <param name="sysNosExcluded">要排除的配送单系统编号集合</param>
        /// <param name="userSysno">用户编号</param>
        /// <returns>当天指定的配送单列表</returns>
        /// <remarks>2013-06-21 黄伟 创建</remarks>
        public Result<List<CBWCFLgDelivery>> GetDeliveryList(List<int> sysNosExcluded, int userSysno)
        {
            return ILgDeliveryDao.Instance.GetDeliveryList(sysNosExcluded, userSysno);
        }

        /// <summary>
        /// 根据配送单编号获取配送单明细
        /// </summary>
        /// <param name="sysNo">配送单系统编号</param>
        /// <returns>配送单明细列表</returns>
        /// <remarks>2013-12-30 黄伟 创建</remarks>
        public Result<List<LogisticsDeliveryItem>> GetItemListByDeliverySysNo(int sysNo)
        {
            var lstLogisticsDeliveryItem = new List<LogisticsDeliveryItem>();

            try
            {
                var lstDeliveryItem = ILgDeliveryDao.Instance.GetItemListByDeliverySysNo(sysNo);

                lstDeliveryItem.ForEach(i =>
                    {
                        var soOrder = WhWarehouseBo.Instance.GetSoOrder(i.NoteSysNo);
                        var soReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(soOrder.ReceiveAddressSysNo);
                        //context.Sql("select * from SoReceiveAddress where sysno=:0", i.AddressSysNo)
                        //       .QuerySingle<SoReceiveAddress>();

                        var delUser = DeliveryUserLocationBo.Instance.GetLocationByUserSysNo(i.DeliverySysNo);

                        //获取商品
                        var lstProducts = new List<Item>();
                        var stockOutItem = new List<WhStockOutItem>();
                        var lgpickupItem = new List<LgPickUpItem>();
                        int orderSysNo = 0;
                        decimal totalAmount = 0;

                        var lgAppSignStatus =
                            ILgSettlementDao.Instance.GetAppSignStatusforDeliveryItem(i.DeliverySysNo, i.NoteSysNo, i.NoteType);
                        var appSignStatus = lgAppSignStatus == null ? 0 : lgAppSignStatus.Status;
                        var lstLgAppSignItem = new List<LgAppSignItem>();
                        if (lgAppSignStatus != null)
                        {
                            lstLgAppSignItem =
                                ILgSettlementDao.Instance.GetAppSignItem(lgAppSignStatus.SysNo).ToList();
                        }

                        var urlSignImage = "";
                        //出库单
                        if (i.NoteType == 10)
                        {
                            var stockOut = WhWarehouseBo.Instance.Get(i.NoteSysNo);
                            totalAmount = stockOut.StockOutAmount;
                            orderSysNo = soOrder.SysNo;
                            stockOutItem = WhWarehouseBo.Instance.GetWhStockOutItemList(i.NoteSysNo).ToList();
                            stockOutItem.ForEach(e =>
                                {
                                    var signQty = 0;
                                    if (appSignStatus == LogisticsStatus.App签收状态.部分签收.GetHashCode())
                                    {
                                        signQty =
                                            lstLgAppSignItem.Single(si => si.NoteItemSysNo == e.SysNo)
                                                            .SignQuantity;
                                    }
                                    else if (appSignStatus == LogisticsStatus.App签收状态.已签收.GetHashCode())
                                    {
                                        signQty = e.ProductQuantity;
                                    }
                                    lstProducts.Add(new Item
                                        {
                                            ProductSysNo = e.ProductSysNo,
                                            ProductName = e.ProductName,
                                            OriginalPrice = double.Parse(e.OriginalPrice + ""),
                                            Quantity = e.ProductQuantity,
                                            OrderItemSysNo = e.OrderItemSysNo,
                                            StockItemSysNo = e.SysNo,
                                            SignQuantity = signQty
                                        });
                                }
                                );
                            urlSignImage = stockOut.SignImage;
                            if (!string.IsNullOrWhiteSpace(urlSignImage))
                            {
                                urlSignImage = string.Format("{0}App\\Logistics\\{1}\\{2}\\{3}", Config.Config.Instance.GetAttachmentConfig().FileServer, urlSignImage.Substring(0, 1), urlSignImage.Substring(1, 2), urlSignImage);

                            }
                        }
                        else
                        {
                            lgpickupItem = LgPickUpBo.Instance.GetLgPickUpItem(i.NoteSysNo);
                            lgpickupItem.ForEach(e => lstProducts.Add(new Item
                                {
                                    ProductSysNo = e.ProductSysNo,
                                    ProductName = e.ProductName,
                                    OriginalPrice = double.Parse(PdPriceBo.Instance.GetPrice(e.PickUpSysNo).Price + ""),
                                    //context.Sql("select * from pdprice where productsysno=:0", e.PickUpSysNo)
                                    //       .QuerySingle<double>(),
                                    Quantity = e.ProductQuantity,
                                    SignQuantity = e.ProductQuantity
                                }));
                        }

                        #region 获取完整收货地址

                        //优化 var area = IBsAreaDao.Instance.GetArea(soReceiveAddress.AreaSysNo);
                        //优化 var city = IBsAreaDao.Instance.GetArea(area.ParentSysNo);
                        //优化 var province = IBsAreaDao.Instance.GetArea(city.ParentSysNo);
                        var area = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(soReceiveAddress.AreaSysNo);
                        var city = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(area.ParentSysNo);
                        var province = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(city.ParentSysNo);
                        var receiveAddrFull = string.Format("{0}{1}{2}{3}", province.AreaName, city.AreaName,
                                                            area.AreaName, soReceiveAddress.StreetAddress);

                        #endregion

                        var isPartialSignAllowed = false;//是否允许部分签收
                        var delType = GetDelivery(sysNo).DeliveryTypeSysNo;
                        if (i.IsCOD == 1 && i.Receivable > 0 && (delType == DeliveryType.普通百城当日达 || delType == DeliveryType.定时百城当日达 || delType == DeliveryType.加急百城当日达))
                        {
                            isPartialSignAllowed = true;
                        }

                        lstLogisticsDeliveryItem.Add(new LogisticsDeliveryItem
                            {
                                SysNo = i.SysNo,
                                NoteType = i.NoteType, //== 10 ? "出库单" : "取件单",
                                NoteSysNo = i.NoteSysNo,
                                AddrName = soReceiveAddress.Name,
                                StreetAddress = receiveAddrFull,
                                MobilePhoneNumber = soReceiveAddress.MobilePhoneNumber,
                                PhoneNumber = soReceiveAddress.PhoneNumber,
                                Longitude = delUser == null ? 0 : delUser.Longitude,
                                Latitude = delUser == null ? 0 : delUser.Latitude,
                                //状态：待签收（10）、拒收（20）、未送达（30）、已签
                                Status = i.Status,
                                // == 10 ? "待签收" : (i.Status == 20 ? "拒收" : (i.Status == 30 ? "未送达" : "已签")),
                                PaymentType = i.PaymentType,
                                PaymentAmount = double.Parse(i.Receivable + ""), //支付金额
                                Items = lstProducts,
                                OrderSysNo = orderSysNo,
                                TotalAmount = totalAmount,
                                AppStatus = appSignStatus,
                                UrlSignImage = urlSignImage,
                                IsPartialSignAllowed = isPartialSignAllowed

                            });
                    });
            }
            catch (Exception ex)
            {
                return new Result<List<LogisticsDeliveryItem>> { Status = false, Message = "获取配送单明细出错,请联系管理员." };
            }
            return new Result<List<LogisticsDeliveryItem>>
                {
                    Status = true,
                    Message = "获取配送单明细成功.",
                    Data = lstLogisticsDeliveryItem
                };
        }

        /// <summary>
        /// 获取所有配送单集合
        /// </summary>
        /// <returns>配送单集合</returns>
        /// <remarks>
        /// 2013-12-30 黄伟 创建
        /// </remarks>   
        public Result<List<CBWCFLgDelivery>> GetLgDeliveryListAll(int userSysNo)
        {
            List<CBWCFLgDelivery> lstDel;
            try
            {
                lstDel = ILgDeliveryDao.Instance.GetLgDeliveryListAll(userSysNo);
            }
            catch (Exception ex)
            {
                return new Result<List<CBWCFLgDelivery>> { Status = false, Message = "获取配送单出错,请联系管理员." };
            }
            return new Result<List<CBWCFLgDelivery>> { Status = true, Message = "获取配送单成功.", Data = lstDel };
        }

        /// <summary>
        /// 更新单据状态=================================================================
        /// </summary>
        /// <param name="lstStatus">需要更新的单据编号及状态集合</param>
        /// <param name="user">当前登录用户</param>
        /// <returns>封装的响应实体(状态,状态编码,消息)</returns>
        /// <remarks>2013-06-24 黄伟 创建</remarks>
        /// <remarks>2014-01-15 沈强 修改</remarks>
        /// <remarks>2014-03-20 周唐炬 修改</remarks>
        public Result UpdateStatus(List<CBWCFStatusUpdate> lstStatus, SyUser user)
        {
            foreach (var item in lstStatus)
            {
                //检查该单据记录数
                var counter = ILgDeliveryDao.Instance.GetAppSignCount(item.DeliverySysNo, item.NoteSysNo);

                //单据记录数>0，表示该单据已经处理过，异常跳出
                if (counter > 0) throw new HytException("无法完成该操作！");

                if (item.Status != LogisticsStatus.App签收状态.部分签收.GetHashCode()) continue;
                if (item.CbwcfStatusItems == null || !item.CbwcfStatusItems.Any())
                {
                    return new Result()
                        {
                            Status = false,
                            Message = "部分签收缺少签收明细！"
                        };
                }

                var num = item.CbwcfStatusItems.Sum(c => c.SignQuantity);
                if (num <= 0)
                {
                    return new Result()
                        {
                            Status = false,
                            Message = "部分签收签收数量不能为0！"
                        };
                }
            }

            var result = ILgDeliveryDao.Instance.UpdateStatus(lstStatus, user);
            foreach (var statu in lstStatus)
            {
                var noteType = statu.NoteType == LogisticsStatus.配送单据类型.出库单.GetHashCode()
                                   ? LogisticsStatus.配送单据类型.出库单
                                   : LogisticsStatus.配送单据类型.取件单;

                var itemStatus = (LogisticsStatus.App签收状态)statu.Status;
                //对APP配送项添加说明
                if (itemStatus == LogisticsStatus.App签收状态.未送达 || itemStatus == LogisticsStatus.App签收状态.拒收)
                {
                    var item = ILgDeliveryDao.Instance.GetDeliveryItem(statu.DeliverySysNo, statu.NoteSysNo, noteType);
                    if (item != null && !string.IsNullOrWhiteSpace(statu.Remarks))
                    {
                        item.Remarks = statu.Remarks;
                        ILgDeliveryDao.Instance.UpdateDeliveryItem(item);
                    }
                }
                var log = string.IsNullOrWhiteSpace(statu.Remarks)
                              ? string.Format("通过App签收{0}", itemStatus)
                              : string.Format("通过App签收{0},{1}", itemStatus, statu.Remarks);

                SysLog.Instance.Info(LogStatus.系统日志来源.物流App, log, (LogStatus.系统日志目标类型)statu.NoteType,
                                     statu.NoteSysNo, user.SysNo);

                if (noteType == LogisticsStatus.配送单据类型.出库单)
                {
                    var stockOut = IOutStockDao.Instance.GetStockOutInfo(statu.NoteSysNo);
                    SoOrderBo.Instance.WriteSoTransactionLog(stockOut.TransactionSysNo, log, user.UserName);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据指定仓库系统编号和配送员系统编号获取配送单系统编号
        /// </summary>
        /// <param name="warehouseSysno">仓库系统编号</param>
        /// <param name="userSysno">配送员系统编号</param>
        /// <param name="status">配送单状态</param>
        /// <returns>没有返回0；有则返回当前配送单系统编号</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        public int GetDeliveryNoteSysNo(int warehouseSysno, int userSysno,
                                        Hyt.Model.WorkflowStatus.LogisticsStatus.配送单状态 status)
        {
            return ILgDeliveryDao.Instance.GetDeliveryNoteSysNo(warehouseSysno, userSysno, status);
        }

        /// <summary>
        /// 指定类型的单据是否在其他有效配送单明细中
        /// </summary>
        /// <param name="deliverSysno">配送单系统编号</param>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <returns>在有效配送单明细中：true；否则：false</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        public bool IsNoteInDeliveryItem(int deliverSysno, LogisticsStatus.配送单据类型 noteType, int noteNumber)
        {
            return ILgDeliveryDao.Instance.IsNoteInDeliveryItem(deliverSysno, noteType, noteNumber);
        }

        /// <summary>
        /// 指定类型的单据是否在其他有效配送单明细中，并且不是未送达或者作废状态
        /// </summary>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <returns>在有效配送单明细中并且不是未送达或者作废状态：true ；否则：false</returns>
        /// <remarks>
        /// 2013-06-25 沈强 创建
        /// </remarks>
        private bool NoteInDeliveryItem(LogisticsStatus.配送单据类型 noteType, int noteNumber)
        {
            var lgDeliveryItems = ILgDeliveryDao.Instance.GetLgDeliveryItemList(noteType, noteNumber);

            return lgDeliveryItems != null &&
                   lgDeliveryItems.Any(
                       l =>
                       l.Status != (int)LogisticsStatus.配送单明细状态.作废 && l.Status != (int)LogisticsStatus.配送单明细状态.未送达);
        }

        /// <summary>
        /// 更新指定系统编号的配送单明细信息（仅更新支付类型、支付单号、配送单明细状态）
        /// </summary>
        /// <param name="sysNo">配送单明细系统编号</param>
        /// <param name="paymentType">支付类型</param>
        /// <param name="payNo">支付单号</param>
        /// <param name="status">配送单明细状态</param>
        /// <returns>更新结果</returns>
        /// <remarks>2013-06-26 沈强 创建</remarks>
        public Result UpdateDeliveryItem(int sysNo, int paymentType, string payNo, int status)
        {
            string message = string.Empty;
            bool returnResult = false;
            try
            {
                Model.LgDeliveryItem lgDeliveryItem = LgDeliveryBo.Instance.GetDeliveryItem(sysNo);
                lgDeliveryItem.PaymentType = paymentType;
                lgDeliveryItem.PayNo = payNo;
                lgDeliveryItem.Status = status;

                returnResult = LgDeliveryBo.Instance.UpdateDeliveryItem(lgDeliveryItem);
                message = "更新成功";
                returnResult = true;
            }
            catch
            {
                message = "更新失败";
                returnResult = false;
            }

            Result result = new Result();
            result.Message = message;
            result.Status = returnResult;
            return result;
        }

        /// <summary>
        /// 根据指定系统编号删除配送单明细
        /// </summary>
        /// <param name="deliveryItemSysno">配送单明细系统编号</param>
        /// <returns>删除结果</returns>
        /// <remarks>
        /// 2013-06-27 沈强 创建
        /// </remarks> 
        public Result DeleteDeliveryItem(int deliveryItemSysNo)
        {
            Result result = new Result();

            result.Status = ILgDeliveryDao.Instance.DeleteDeliveryItem(deliveryItemSysNo);
            if (result.Status)
                result.Message = "删除成功！";
            else
                result.Message = "删除失败！";

            return result;
        }

        /// <summary>
        /// 通过出库单系统编号组合配送单明细
        /// </summary>
        /// <param name="stockOutSysNos">出库单sys no.</param>
        /// <returns>配送单明细组合实体列表</returns>
        /// <remarks>
        /// 2013-06-19 沈强 创建
        /// </remarks>
        private IList<CBLgDeliveryItemJson> getCbDeliveryItemsByStockOutSysNo(int[] stockOutSysNos)
        {
            var whStockOuts = WhWarehouseBo.Instance.GetWhStockOutListBySysNos(stockOutSysNos);

            List<CBLgDeliveryItemJson> items = new List<CBLgDeliveryItemJson>();
            foreach (var item in whStockOuts)
            {
                SoReceiveAddress soReceiveAddress =
                    Order.SoOrderBo.Instance.GetOrderReceiveAddress(item.ReceiveAddressSysNo);

                CBLgDeliveryItemJson json = new CBLgDeliveryItemJson();
                json.NoteSysNo = item.SysNo;
                json.NoteType = (int)LogisticsStatus.配送单据类型.出库单;
                json.Name = soReceiveAddress.Name;
                json.CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                json.MobilePhoneNumber = soReceiveAddress.MobilePhoneNumber;
                json.Receivable = item.Receivable.ToString("C");
                json.IsCOD = item.IsCOD;
                json.StockOutAmount = item.StockOutAmount.ToString("C");
                json.AddressSysNo = item.ReceiveAddressSysNo;
                items.Add(json);
            }

            return items;
        }

        /// <summary>
        /// 获取页面用的取件单
        /// </summary>
        /// <param name="pickUpSysNos">取件单系统编号.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/6 何方 创建
        /// </remarks>
        private IList<CBLgDeliveryItemJson> GetCbDeliveryItemsByPickUpSysNo(int[] pickUpSysNos)
        {
            var pickUps = LgPickUpBo.Instance.GetWhStockOutListBySysNos(pickUpSysNos);

            var items = new List<CBLgDeliveryItemJson>();
            foreach (var pickUp in pickUps)
            {
                var crReceive = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(pickUp.PickupAddressSysNo);
                if (crReceive == null)
                {
                    throw new Exception("取件单：" + pickUp.SysNo + "未添加收货地址");
                }
                var item = new CBLgDeliveryItemJson
                    {
                        AddressSysNo = pickUp.PickupAddressSysNo,
                        CreatedDate = pickUp.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                        IsCOD = 0,
                        MobilePhoneNumber = crReceive.MobilePhoneNumber,
                        Name = crReceive.Name,
                        NoteSysNo = pickUp.SysNo,
                        NoteType = (int)LogisticsStatus.配送单据类型.取件单,
                        //Receivable = "￥0.00",
                        //StockOutAmount = "￥0.00",
                        Receivable = (0.00m).ToString("C"),
                        StockOutAmount = (0.00m).ToString("C"),
                        TotalAmount = pickUp.TotalAmount.ToString()
                    };
                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// 检查并获取组合配送单明细集合
        /// </summary>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteSysNo">单据编号数组</param>
        /// <param name="deliveryTypeSysNo">配送类型编号</param>
        /// <param name="isThirdPartyExpress">是否为三方快递（是[1],否[0]）</param>
        /// <param name="deliveryUserSysNo">配送员编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/6 何方 
        /// </remarks>
        public Result<IList<CBLgDeliveryItemJson>> GetCBLgDeliveryItems(int noteType, int[] noteSysNo,
                                                                        int deliveryTypeSysNo, int isThirdPartyExpress,
                                                                        int deliveryUserSysNo, int warehouseSysNo)
        {
            var result = new Result<IList<CBLgDeliveryItemJson>>();

            //保存不能添加的单据编号
            List<int> notAddNoteSysNos = new List<int>();

            //保存返回的消息
            string messages = string.Empty;

            //配送方式  
            var delivertType = DeliveryTypeBo.Instance.GetDeliveryType(deliveryTypeSysNo);

            #region 检查所有输入单据是否符合要求

            foreach (var item in noteSysNo)
            {
                Result tmpResult = this.CanAddToDeliery(delivertType, noteType, item, isThirdPartyExpress, true,
                                                        deliveryUserSysNo, warehouseSysNo);
                //如果不能添加到配送单,直接返回不能配送原因
                if (!tmpResult.Status)
                {
                    notAddNoteSysNos.Add(item);
                    messages += "_" + tmpResult.Message;
                }
            }

            #endregion

            //获取可以添加的单据编号
            int[] addNoteSysNos = noteSysNo.Where(n => !notAddNoteSysNos.Contains(n)).ToArray();

            result.Message = string.IsNullOrEmpty(messages) ? "" : messages.Substring(1);
            if (addNoteSysNos.Length == 0)
            {
                result.Status = false;
                return result;
            }

            #region 出库单处理

            if (noteType == (int)LogisticsStatus.配送单据类型.出库单)
            {
                result.Data = getCbDeliveryItemsByStockOutSysNo(addNoteSysNos);
                result.Status = true;
            }

            #endregion

            #region 取件单处理

            if (noteType == (int)LogisticsStatus.配送单据类型.取件单)
            {
                result.Data = GetCbDeliveryItemsByPickUpSysNo(addNoteSysNos);
                result.Status = true;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 根据配送单系统编号数组，获取配送单明细集合
        /// </summary>
        /// <param name="deliverySysNos">配送单系统编号数组</param>
        /// <returns>配送单明细集合</returns>
        /// <remarks>
        /// 2013-07-04 沈强 创建
        /// </remarks>   
        public IList<LgDeliveryItem> GetDeliveryItemList(int[] deliverySysNos)
        {
            return ILgDeliveryDao.Instance.GetLgDeliveryItemList(deliverySysNos);
        }

        /// <summary>
        /// 更新配送单状态
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="status">LogisticsStatus.配送单状态</param>
        /// <returns>
        /// 作废结果
        /// </returns>
        /// <remarks>
        /// 2013-07-14 何方 创建
        /// </remarks>
        public bool UpdateStatus(int deliverySysNo, LogisticsStatus.配送单状态 status)
        {
            return ILgDeliveryDao.Instance.UpdateStatus(deliverySysNo, status);
        }

        /// <summary>
        /// 根据用户编号来获取配送中的配送单数量
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <returns>配送中的配送单数量</returns>
        /// <remarks>2013-08-27 周瑜 创建</remarks>
        public int GetDeliveryingCount(int customerSysNo)
        {
            return ILgDeliveryDao.Instance.GetDeliveryingCount(customerSysNo);
        }

        /// <summary>
        /// 通过事务编号获取配送单列表
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>配送单</returns>
        /// <remarks>2013-09-18 周唐炬 创建</remarks>
        public IList<LgDelivery> GetLgDeliveryList(string transactionSysNo)
        {
            return ILgDeliveryDao.Instance.GetLgDeliveryList(transactionSysNo);
        }

        /// <summary>
        /// 通过来源单据类型跟编号获取配送单
        /// </summary>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteNumber">单据编号</param>
        /// <remarks>2013-09-18 周唐炬 创建</remarks>
        /// <returns>配送单列表</returns>
        public LgDelivery GetDelivery(LogisticsStatus.配送单据类型 noteType, int noteNumber)
        {
            return ILgDeliveryDao.Instance.GetDelivery(noteType, noteNumber);
        }

        /// <summary>
        /// 根据出库单编号集合获取配送员使用的配送额度
        /// </summary>
        /// <param name="stockout">出库单实体</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-10-24 沈强 创建
        /// </remarks>
        public decimal GetDeliveryCredit(WhStockOut stockout)
        {
            return stockout.StockOutAmount > stockout.Receivable ? stockout.StockOutAmount : stockout.Receivable;
        }

        /// <summary>
        /// 根据出库单编号集合获取配送员使用的配送额度
        /// </summary>
        /// <param name="stockOutSysNos">出库单系统编号</param>
        /// <param name="status">出库单状态</param>
        /// <returns></returns>
        /// <remarks>2013-10-24 沈强 创建</remarks>
        public decimal GetDeliveryCredit(int[] stockOutSysNos, WarehouseStatus.出库单状态? status = null)
        {
            var whStockOuts = WhWarehouseBo.Instance.GetWhStockOutListBySysNos(stockOutSysNos);
            var credits = 0m;
            if (status != null)
            {
                var whStockOutList = whStockOuts.Where(w => w.Status == (int)status);
                credits = whStockOutList.Select(w => w.StockOutAmount > w.Receivable ? w.StockOutAmount : w.Receivable).Sum();
            }
            else
            {
                credits = whStockOuts.Select(w => w.StockOutAmount > w.Receivable ? w.StockOutAmount : w.Receivable).Sum();
            }
            return credits;
        }

        /// <summary>
        /// 根据取件单系统编号集合获取取件单金额总和
        /// </summary>
        /// <param name="pickUpSysNos">取件单系统编号集合</param>
        /// <returns>返回总金额</returns>
        /// <remarks>2013-11-25 沈强 创建</remarks>
        public decimal GetLgPickUpTotalCount(int[] pickUpSysNos)
        {
            if (pickUpSysNos == null || pickUpSysNos.Length == 0)
            {
                return 0;
            }
            var lgPick = LgPickUpBo.Instance.GetWhStockOutListBySysNos(pickUpSysNos);
            return lgPick != null ? lgPick.Sum(l => l.TotalAmount) : 0;
        }

        /// <summary>
        /// 物流运单查询
        /// </summary>
        /// <param name="transactionSysNos">快递单号(订单事务编号)</param>
        /// <returns>返回运单查询结果</returns>
        /// <remarks>2014-2-17 沈强 创建</remarks>
        /// <remarks>2014-4-10 苟治国 修改 增加第三方快递</remarks>
        /// <remarks>2014-6-10 何明壮 修改 物流号刷单单号规则</remarks>
        public Result<List<OrderTransactionModel>> SreachExpress(List<string> transactionSysNos)
        {
            var result = new Result<List<OrderTransactionModel>>();
            result.Data = new List<OrderTransactionModel>();

            try
            {
                foreach (var transactionSysNo in transactionSysNos)
                {
                    var model = new OrderTransactionModel();
                    if (!string.IsNullOrWhiteSpace(transactionSysNo))
                    {
                        //ViewBag.TransactionSysNo = transactionSysNo;
                        result.Message = transactionSysNo;
                        if (transactionSysNo.StartsWith("t", StringComparison.OrdinalIgnoreCase) && (transactionSysNo.Length == 16))
                        {
                            //取得订单
                            var order = SoOrderBo.Instance.GetByTransactionSysNo(transactionSysNo);

                            if (order != null)
                            {
                                //所有订单商品
                                order.OrderItemList = SoOrderBo.Instance.GetOrderItemsByOrderId(order.SysNo);
                                //加入订单号
                                model.SoOrderSysNo = order.SysNo;
                                model.SoOrderDeliveryType = order.DeliveryTypeSysNo;

                                //订单轨迹
                                var orderLog = BLL.Web.SoOrderBo.Instance.GetOrderLogList(order.SysNo);
                                if (orderLog != null && orderLog.TData != null)
                                {
                                    model.OrderTransactionLogs.AddRange(orderLog.TData.OrderBy(t => t.SysNo));
                                }
                                //三方快递
                                //var expressList = Hyt.BLL.Logistics.LgExpressBo.Instance.GetLgExpressLogByTransactionSysNo(transactionSysNo);
                                //if (expressList != null)
                                //{
                                //    model.OrderTransactionLogs.AddRange(from item in expressList select new SoTransactionLog { LogContent = item.LogContext, OperateDate = item.LogTime });
                                //}

                                #region

                                //取得出库单
                                //var stockOuts = WhWarehouseBo.Instance.GetModelByTransactionSysNo(transactionSysNo);
                                //if (stockOuts != null && stockOuts.Any())
                                //{
                                //foreach (var whStockOut in stockOuts)
                                //{

                                #region 出库单内容

                                //var group = new TransactionGroup() { SoTransactionLogs = new List<SoTransactionLog>() };
                                //if (orderLog != null && orderLog.TData != null)
                                //{
                                //    group.SoTransactionLogs.AddRange(orderLog.TData);
                                //}
                                ////加入出库单
                                //group.WhStockOutModel = whStockOut;
                                ////从订单商品列表中删除已经出库的商品
                                //if (order.OrderItemList != null && order.OrderItemList.Any() && whStockOut.Items != null && whStockOut.Items.Any())
                                //{
                                //    foreach (var item in whStockOut.Items)
                                //    {
                                //        order.OrderItemList.Remove(order.OrderItemList.SingleOrDefault(x => x.ProductSysNo == item.ProductSysNo));
                                //    }
                                //}
                                //var stockOutLogs = SysLog.Instance.Get(LogStatus.系统日志来源.后台, LogStatus.系统日志目标类型.出库单, whStockOut.SysNo);
                                ////加入出库单轨迹
                                //if (stockOutLogs != null && stockOutLogs.Any())
                                //{
                                //    group.SoTransactionLogs.AddRange(stockOutLogs.Select(x => new SoTransactionLog()
                                //        {
                                //            SysNo = x.SysNo,
                                //            LogContent = x.Message,
                                //            Operator = x.OperatorName,
                                //            OperateDate = x.LogDate
                                //        }));
                                //}

                                #endregion

                                #region 配送单内容

                                ////取得配送单
                                //var delivery = LgDeliveryBo.Instance.GetDelivery(LogisticsStatus.配送单据类型.出库单, whStockOut.SysNo);
                                //if (delivery == null) continue;
                                //group.LgDeliveryModel = delivery;
                                //var deliveryLogs = SysLog.Instance.Get(LogStatus.系统日志来源.后台, LogStatus.系统日志目标类型.配送单, delivery.SysNo);
                                ////加入配送单轨迹
                                //if (deliveryLogs != null && deliveryLogs.Any())
                                //{
                                //    group.SoTransactionLogs.AddRange(deliveryLogs.Select(x => new SoTransactionLog()
                                //        {
                                //            SysNo = x.SysNo,
                                //            LogContent = x.Message,
                                //            Operator = x.OperatorName,
                                //            OperateDate = x.LogDate
                                //        }));
                                //}

                                #endregion

                                //model.TransactionGroups.Add(group);
                                //}
                                //}

                                #endregion

                                model.PendingProducts = order.OrderItemList;
                            }

                        }
                        else if (transactionSysNo.StartsWith("d", StringComparison.OrdinalIgnoreCase) ||
                        (transactionSysNo.StartsWith("t", StringComparison.OrdinalIgnoreCase) && (transactionSysNo.Length == 15)))
                        {
                            var logs = MkExpressLogBo.Instance.GetMkExpressLogList(transactionSysNo);
                            var orderLogs = new List<SoTransactionLog>();
                            if (logs != null && logs.Any())
                            {
                                orderLogs =
                                    logs.OrderBy(l => l.SysNo)
                                        .Where(log => log.OperateDate <= DateTime.Now)
                                        .Select(l => new SoTransactionLog()
                                        {
                                            OperateDate = l.OperateDate,
                                            Operator = l.OperatorName,
                                            LogContent = l.LogContent
                                        }).ToList();
                            }
                            model.PendingProducts = null;
                            if (orderLogs.Any())
                            {
                                model.OrderTransactionLogs.AddRange(orderLogs);
                            }

                        }
                        result.Data.Add(model);

                    }

                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", @" ");
                SysLog.Instance.Error(LogStatus.系统日志来源.百城通, "App百城通快递单查询结果页" + ex.Message, ex);
                result.Message = "发生错误！";
                return result;
            }
            if (result.Data.Any())
            {
                result.Status = true;
                if (result.Data != null && result.Data.Any())
                {
                    foreach (var item in result.Data)
                    {
                        if (item.PendingProducts != null && item.PendingProducts.Any())
                        {
                            foreach (var product in item.PendingProducts)
                            {
                                var imagePath = Web.ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image120,
                                                                                       product.ProductSysNo);
                                product.ProductName = product.ProductName + "_|_" + imagePath;
                            }
                        }
                    }
                }
                return result;
            }
            else
            {
                result.Status = false;
                result.Message = "找不到物流信息,请输入正确的单号";
                return result;
            }

        }

        /// <summary>
        /// 回填天猫的物流信息
        /// </summary>
        /// <param name="deliverySysNo">配送单号</param>
        /// <param name="deliverTypeSysno">配送方式系统编号</param>
        /// <returns>null</returns>
        /// <remarks>2014-3-25 唐文均 创建</remarks>
        /// <remarks>2014-03-30 唐文均 修改分销升舱失败日志</remarks>
        public void BackFillLogisticsInfo(int deliverySysNo, int deliverTypeSysNo)
        {

            //同时将发货信息记录下来，如果出现异常，由另外一个日志同步功能去处理。参见：任务工具的自动发货任务DRP.BLL.Job.AutoShipping
            BackFillLogisticsInfoNew(deliverySysNo,deliverTypeSysNo);

            //自动发货写入日志表功能正常之后，取消判断代码
            //if (Hyt.Util.VersionUtil.Contains("自动发货写入日志表"))
            //{

            //同时将发货信息记录下来，如果出现异常，由另外一个日志同步功能去处理。参见：任务工具的自动发货任务DRP.BLL.Job.AutoShipping
            //BackFillLogisticsInfoNew(deliverySysNo, deliverTypeSysNo);
            //return;
            //}


            //var logisticsSendList = new List<Service.Contract.MallSeller.Model.LogisticsSendRequest>();
            //Service.Contract.MallSeller.Model.LogisticsSendRequest logisticsSendRequest;
            //Service.Contract.MallSeller.Model.AuthorizationParameters authorizationParameters;

            //var delivertType = DeliveryTypeBo.Instance.GetDeliveryType(deliverTypeSysNo);

            #region 注释
            //// 配送明细
            //var deliveryItems = (List<CBLgDeliveryItem>)ILgDeliveryDao.Instance.GetCBDeliveryItems(deliverySysNo);
            //if (deliveryItems != null && deliveryItems.Count > 0)
            //{
            //    deliveryItems = deliveryItems.FindAll(o => o.NoteType == (int)LogisticsStatus.配送单据类型.出库单);

            //    if (deliveryItems != null)
            //    {
            //        // 遍历配送明细订单
            //        foreach (var item in deliveryItems)
            //        {
            //            var soOrder = SoOrderBo.Instance.GetByOutStockSysNo(item.NoteSysNo);// 订单信息
            //            if (soOrder.OrderSource == (int)OrderStatus.销售单来源.分销商升舱)
            //            {
            //                var orderInfo = DsOrderBo.Instance.GetDsOrderInfoEx(soOrder.TransactionSysNo);
            //                var dsMallType = DsMallTypeBo.Instance.GetDsMallType(orderInfo.Item2.MallTypeSysNo);

            //                if (dsMallType.SysNo == (int)DistributionStatus.商城类型预定义.天猫商城 ||
            //                    dsMallType.SysNo == (int)DistributionStatus.商城类型预定义.淘宝分销 ||
            //                    dsMallType.SysNo == (int)DistributionStatus.商城类型预定义.有赞)
            //                {
            //                    authorizationParameters = new Service.Contract.MallSeller.Model.AuthorizationParameters
            //                    {
            //                        AuthorizationCode = orderInfo.Item2.AuthCode,
            //                        MallType = dsMallType.SysNo,
            //                        ShopAccount = orderInfo.Item2.ShopAccount,
            //                    };

            //                    // 第三方配送
            //                    if (delivertType.IsThirdPartyExpress == 1)
            //                    {
            //                        logisticsSendRequest = new Service.Contract.MallSeller.Model.LogisticsSendRequest
            //                        {
            //                            CompanyCode = DsMallExpressCodeBo.Instance.GetDeliveryCompanyCode(dsMallType.SysNo, delivertType.SysNo),
            //                            ExpressCode = item.ExpressNo,
            //                            OrderID = orderInfo.Item1.MallOrderId,
            //                            AuthInfo = authorizationParameters,                                      
            //                        };
            //                    }
            //                    else
            //                    {
            //                        logisticsSendRequest = new Service.Contract.MallSeller.Model.LogisticsSendRequest
            //                        {
            //                            CompanyCode = "自建物流",
            //                            ExpressCode = orderInfo.Item1.OrderTransactionSysNo,
            //                            OrderID = orderInfo.Item1.MallOrderId,
            //                            AuthInfo = authorizationParameters,
            //                        };
            //                    }

            //                    logisticsSendList.Add(logisticsSendRequest);
            //                }
            //            }
            //        }
            //    }
            //}

            

            //foreach (var item in logisticsSendList)
            //{

            //    Extra.UpGrade.Provider.UpGradeProvider.GetInstance(item.AuthInfo.MallType).SendDelivery(
            //           new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = item.CompanyCode, HytExpressNo = item.ExpressCode, MallOrderId = item.OrderID },
            //           null);
            //}
           #endregion

            //using (var service = new ServiceProxy<Hyt.Service.Contract.MallSeller.IExtendMallOrder>())
            //{
            //    foreach (var item in logisticsSendList)
            //    {
            //        var result = service.Channel.LogisticsSend(item);
            //    }
            //}

        }

        /// <summary>
        /// 回填天猫的物流信息(新) 2014-7-30
        /// </summary>
        /// <param name="deliverySysNo">配送单号</param>
        /// <param name="deliverTypeSysno">配送方式系统编号</param>
        /// <returns>null</returns>
        /// <remarks>2014-7-30 杨文兵 创建</remarks>        
        public void BackFillLogisticsInfoNew(int deliverySysNo, int deliverTypeSysNo)
        {
            var dsMallSyncLogList = new List<DsMallSyncLog>();

            // 配送明细
            var deliveryItems = (List<CBLgDeliveryItem>)ILgDeliveryDao.Instance.GetCBDeliveryItems(deliverySysNo);
            if (deliveryItems != null && deliveryItems.Count > 0)
            {
                deliveryItems = deliveryItems.FindAll(o => o.NoteType == (int)LogisticsStatus.配送单据类型.出库单);

                if (deliveryItems != null)
                {
                    // 遍历配送明细订单
                    foreach (var item in deliveryItems)
                    {
                        var soOrder = SoOrderBo.Instance.GetByOutStockSysNo(item.NoteSysNo); ;//订单信息
                        if (soOrder.OrderSource == (int)OrderStatus.销售单来源.分销商升舱)
                        {
                            var orderInfo = DsOrderBo.Instance.GetDsOrderInfoEx(soOrder.TransactionSysNo);
                            var dsMallType = DsMallTypeBo.Instance.GetDsMallType(orderInfo.Item2.MallTypeSysNo);

                            if (dsMallType.SysNo == (int)DistributionStatus.商城类型预定义.海带网 ||
                                dsMallType.SysNo == (int)DistributionStatus.商城类型预定义.海拍客 ||
                                dsMallType.SysNo == (int)DistributionStatus.商城类型预定义.国内货栈||
                                dsMallType.SysNo == (int)DistributionStatus.商城类型预定义.格格家 ||
                                dsMallType.SysNo == (int)DistributionStatus.商城类型预定义.京东商城)
                            {
                                dsMallSyncLogList.Add(new DsMallSyncLog()
                                {
                                    Data = Newtonsoft.Json.JsonConvert.SerializeObject(new
                                    {
                                        TransactionSysNo = soOrder.TransactionSysNo,
                                        ExpressCode = item.ExpressNo,
                                        DeliveryType = deliverTypeSysNo
                                    })
                                });
                            }
                        }
                    }
                }
            }

            //将WCF调用改为 写入DsMallSyncLog 
            foreach (var dsmallSyncLog in dsMallSyncLogList)
            {
                dsmallSyncLog.CreatedDate = DateTime.Now;
                dsmallSyncLog.ElapsedTime = 0;
                dsmallSyncLog.LastSyncTime = DateTime.Now;
                dsmallSyncLog.LastUpdateDate = DateTime.Now;
                dsmallSyncLog.Status = 20;
                dsmallSyncLog.SyncNumber = 0;
                dsmallSyncLog.SyncType = 10;
                DsMallSyncLogBo.Instance.Create(dsmallSyncLog);
            }
        }


        /// <summary>
        /// 判断快递单号是否已经被使用
        /// </summary>
        /// <param name="deliverySysNo">快递方式</param>
        /// <param name="express_no">快递单号</param>
        /// <returns>是否存在</returns>
        /// <remarks>2014-04-14 朱成果 创建</remarks>
        public bool IsExistsExpressNo(int deliverySysNo, string express_no)
        {
            return Hyt.DataAccess.Logistics.ILgDeliveryDao.Instance.IsExistsExpressNo(deliverySysNo, express_no);
        }

        /// <summary>
        /// 检查是否存在未入库的入库单（入库单来源为当前入库单)
        /// </summary>
        /// <param name="whStockOutNO">出库单编号</param>
        /// <returns></returns>
        /// <remarks>2014-04-14 朱成果 创建</remarks>
        public Result CheckInStock(int whStockOutNO)
        {
            Result res = new Result() { Status = false };
            var model = Hyt.DataAccess.Warehouse.IInStockDao.Instance.GetStockInBySourceAndStatus((int)Hyt.Model.WorkflowStatus.WarehouseStatus.入库单据类型.出库单, whStockOutNO, (int)Hyt.Model.WorkflowStatus.WarehouseStatus.入库单状态.待入库);
            if (model != null)
            {
                res.Status = true;
                res.StatusCode = model.SysNo;
            }
            return res;
        }

        #region 第三方快递配送单

        /// <summary>
        /// 创建第三方快递配送单
        /// </summary>
        /// <param name="model">rp_第三方快递发货量</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>
        /// 2014-09-23 余勇 创建
        /// </remarks>
        public void CreateExpressLgDelivery(rp_第三方快递发货量 model)
        {
            Hyt.DataAccess.Logistics.ILgDeliveryDao.Instance.CreateExpressLgDelivery(model);
        }

        #endregion


        /// <summary>
        /// 回填天猫的物流信息
        /// </summary>
        /// <param name="deliverySysNo">配送单号</param>
        /// <param name="deliverTypeSysno">配送方式系统编号</param>
        /// <param name="userid">用户编号</param>
        /// <returns>null</returns>
        /// <remarks>2014-3-25 唐文均 创建</remarks>
        /// <remarks>2014-03-30 唐文均 修改分销升舱失败日志</remarks>
        public void BackFillLogisticsInfo(int deliverySysNo, int deliverTypeSysNo, int userid)
        {
            // 配送明细
            var deliveryItems = ILgDeliveryDao.Instance.GetCBDeliveryItems(deliverySysNo);
            if (deliveryItems != null)
            {
                // 遍历配送明细订单
                foreach (var item in deliveryItems)
                {
                    if (item.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                    {
                        var soOrder = SoOrderBo.Instance.GetByOutStockSysNo(item.NoteSysNo);// 订单信息
                        FillLogistics(soOrder, deliverTypeSysNo, item.ExpressNo, userid);
                    }
                }
            }
        }
        /// <summary>
        /// 升舱订单物流发货
        /// </summary>
        /// <param name="order">订单信息</param>
        /// <param name="deliverTypeSysNo">配送方式编号</param>
        /// <param name="expressNo">快递编号</param>
        /// <param name="userid">操作人</param>
        /// <remarks>2015-07-03 朱成果 创建</remarks>
        public void FillLogistics(SoOrder soOrder, int deliverTypeSysNo, string expressNo, int userid)
        {
            if (soOrder != null && soOrder.OrderSource == (int)OrderStatus.销售单来源.分销商升舱)
            {
                var dsOrder = DsOrderBo.Instance.GetEntityByHytOrderID(soOrder.SysNo).FirstOrDefault();//升舱记录
                if (dsOrder != null)
                {
                    int dealerMallSysNo = dsOrder.DealerMallSysNo;
                    var dsDealerMall = DsDealerMallBo.Instance.GetEntity(dealerMallSysNo);
                    int dealerMallTypeSysNo = dsDealerMall != null ? dsDealerMall.MallTypeSysNo : 0;
                    var dsmallSyncLog = new DsMallSyncLog()
                    {
                        //MallSysNo = dealerMallSysNo,
                        //MallTypeSysNo = dealerMallTypeSysNo,
                        //MallOrderId = dsOrder.MallOrderId,
                        Data = Newtonsoft.Json.JsonConvert.SerializeObject(new
                        {
                            TransactionSysNo = soOrder.TransactionSysNo,
                            ExpressCode = expressNo,
                            DeliveryType = deliverTypeSysNo
                        })
                    };
                    dsmallSyncLog.CreatedDate = DateTime.Now;
                    dsmallSyncLog.CreatedBy = dsmallSyncLog.LastUpdateBy = userid;
                    dsmallSyncLog.ElapsedTime = 0;
                    dsmallSyncLog.LastSyncTime = DateTime.Now;
                    dsmallSyncLog.LastUpdateDate = DateTime.Now;
                    dsmallSyncLog.Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱商城同步日志状态.等待;
                    dsmallSyncLog.SyncNumber = 0;
                    dsmallSyncLog.SyncType = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱商城同步同步类型.发货;
                    //DsMallSyncLogBo.Instance.Save(dsmallSyncLog);
                }
            }
        }




        #region 同步订单发货信息至第三方商城平台

        /// <summary>
        /// 同步订单发货信息至第三方商城平台
        /// </summary>
        /// <returns>同步成功条数</returns>
        /// <remarks>
        /// 吴琨 2017-08-30
        /// 2017-10-31 杨浩 重构
        /// </remarks>
        /// 
        public string SynchroOrder(int sysno=0)
        {
            var synchroOrder = IDsMallSyncLogDao.Instance.GetSynchroOrder(sysno);
            var success = 0; //成功同步数量
            var fail = 0; //失败同步数量
            //循环同步
            foreach (var item in synchroOrder)
            {
                int elapsedMilliseconds = 0;
                var watch = new Stopwatch();
                watch.Start();
                try
                {                  
                    var json = JObject.Parse(item.Data);
                    var expressCode = json["ExpressCode"].ToString(); //快递单号
                    var transactionSysNo = json["TransactionSysNo"].ToString(); //平台号                  
                    //分销商订单
                    var dsOrderInfo=BLL.MallSeller.DsOrderBo.Instance.GetEntityByTransactionSysNo(transactionSysNo).FirstOrDefault();
                    //物流公司编号
                    string deliveryType = json["DeliveryType"].ToString();
                    var deliveryName = IDsMallSyncLogDao.Instance.GetDeliveryName(Convert.ToInt32(deliveryType));  //系统物流公司名称
                    var mallInfo = BLL.Distribution.DsDealerMallBo.Instance.GetDsDealerMall(dsOrderInfo.DealerMallSysNo);
                    var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(mallInfo.DealerAppSysNo);
                    int type = mallInfo.MallTypeSysNo;
                    string deliveryCompanyCode = DsMallExpressCodeBo.Instance.GetDeliveryCompanyCode(type, Convert.ToInt32(deliveryType));

                    #region  注释
                    //var isrtn = true;
                    //switch (mallInfo.MallTypeSysNo)
                    //{
                    //    case (int)DistributionStatus.商城类型预定义.京东商城:
                    //        deliveryType = deliveryName.StringToInt<Hyt.Model.WorkflowStatus.DistributionStatus.京东物流公司>().ToString();
                    //        deliveryType = deliveryType == "-1" ? Convert.ToInt32(Hyt.Model.WorkflowStatus.DistributionStatus.京东物流公司.厂家自送).ToString() : deliveryType;
                    //        type = (int)DistributionStatus.商城类型预定义.京东商城;
                    //        break;
                    //    case (int)DistributionStatus.商城类型预定义.格格家:
                    //        deliveryType = deliveryName;
                    //        type = (int)DistributionStatus.商城类型预定义.格格家;
                    //        break;
                    //    #region 暂未实现
                    //    //case (int)DistributionStatus.商城类型预定义.淘宝分销:
                    //    //      DeliveryType = GetDeliveryName;
                    //    //      Type = (int)DistributionStatus.商城类型预定义.淘宝分销;
                    //    //      break;
                    //    //case (int)DistributionStatus.商城类型预定义.天猫商城:
                    //    //      DeliveryType = GetDeliveryName;
                    //    //      Type = (int)DistributionStatus.商城类型预定义.天猫商城;
                    //    //      break;
                    //    #endregion
                    //    case (int)DistributionStatus.商城类型预定义.一号店:
                    //        deliveryType = deliveryName.StringToInt<Hyt.Model.WorkflowStatus.DistributionStatus.一号店物流公司>().ToString();
                    //        type = (int)DistributionStatus.商城类型预定义.一号店;
                    //        break;
                    //    case (int)DistributionStatus.商城类型预定义.有赞:
                    //        deliveryType = deliveryName.StringToInt<Hyt.Model.WorkflowStatus.DistributionStatus.有赞物流公司>().ToString();
                    //        type = (int)DistributionStatus.商城类型预定义.有赞;
                    //        break;
                    //    case (int)DistributionStatus.商城类型预定义.海拍客:
                    //        type = (int)DistributionStatus.商城类型预定义.海拍客;
                    //        deliveryType = DsMallExpressCodeBo.Instance.GetDeliveryCompanyCode(type,Convert.ToInt32(deliveryType));                          
                    //        break;
                    //    default:
                    //        isrtn = false;
                    //        IDsMallSyncLogDao.Instance.UpdateDsMallSyncLogStatus("本次同步失败：此店铺类型未知或接口暂未实现!", 20, item.SysNo);
                    //        break;
                    //}
                    #endregion
                   // var deliverytype = ILgDeliveryTypeDao.Instance.GetLgDeliveryType(Convert.ToInt32(DeliveryType));//配送方式

                    var auth = new AuthorizationParameters()
                    {            
                         MallType = type,
                         ShopAccount = mallInfo.ShopAccount,
                         DealerApp=appInfo,
                         AuthorizationCode=mallInfo.AuthCode,
                    };

                   
                    //调用接口,执行同步
                    var rtn = UpGradeProvider.GetInstance(type).SendDelivery(
                         new Extra.UpGrade.Model.DeliveryParameters
                         {
                             CompanyCode = deliveryCompanyCode,
                             HytExpressNo = expressCode,
                             MallOrderId = dsOrderInfo.MallOrderId
                         }, auth);

                      watch.Stop();
                      elapsedMilliseconds = (int)watch.ElapsedMilliseconds;

                       //更改日志状态
                       if (rtn.Status)
                       {
                           IDsMallSyncLogDao.Instance.UpdateDsMallSyncLogStatus("同步成功", 10, item.SysNo, elapsedMilliseconds);
                           success++;
                       }
                       else
                       {
                           IDsMallSyncLogDao.Instance.UpdateDsMallSyncLogStatus(rtn.Message, 20, item.SysNo, elapsedMilliseconds);                          
                           fail++;
                       }
                  
                }
                catch (Exception e)
                {
                    //更改日志状态
                    IDsMallSyncLogDao.Instance.UpdateDsMallSyncLogStatus("本次同步失败：" + e.Message, 20, item.SysNo, elapsedMilliseconds);
                    fail++;
                }
            }

            return string.Format("成功同步{0}条,失败同步{1}条", success, fail);
        }

        #endregion

    }
    /// <summary>
    /// 第三方物流配置操作 杨云奕
    /// </summary>
    public class ThirdDeliveryConfigBo : BOBase<ThirdDeliveryConfigBo>
    {
        public ThirdDeliveryConfig GetThirdDeliveryConfigBySysNo(int sysNo)
        {
            return IThirdDeliveryConfigDao.Instance.GetThirdDeliveryConfigBySysNo(sysNo);
        }
    }






}