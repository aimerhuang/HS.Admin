using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Extra.SMS;
using Hyt.BLL.Logistics;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Order;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 门店订单业务
    /// </summary>
    /// <remarks>2013-07-08 朱家宏 创建</remarks>
    public class ShopOrderBo : BOBase<ShopOrderBo>
    {
        #region 查询

        #region 门店出库单

        /// <summary>
        /// 出库单分页查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-06 朱家宏 创建</remarks>
        public Pager<CBOutStockOrder> GetOutStockOrders(ParaOutStockOrderFilter filter)
        {
            filter = filter ?? new ParaOutStockOrderFilter();

            var quickSearchKeyword = filter.Keyword;

            //配送方式包括门店自提和自提
            filter.DeliveryTypeSysNoList = new List<int>
                {
                    DeliveryType.门店自提,
                    DeliveryType.自提
                };

            //排除作废出库单
            filter.StockOutStatusListExcepted = new List<int>
                {
                    (int) WarehouseStatus.出库单自提状态.作废
                };

            if (!string.IsNullOrWhiteSpace(quickSearchKeyword))
            {
                //收获手机
                if (VHelper.ValidatorRule(new Rule_Mobile(quickSearchKeyword)).IsPass && quickSearchKeyword.Length >= 11)
                    filter.ReceiverTel = quickSearchKeyword;
                //订单号
                else if (VHelper.ValidatorRule(new Rule_Number(quickSearchKeyword)).IsPass)
                    filter.OrderSysNo = int.Parse(quickSearchKeyword);
                //收货人
                else
                    filter.ReceiverName = quickSearchKeyword;
            }

            //门店筛选 
            if (filter.StoreSysNoList == null || !filter.StoreSysNoList.Any())
            {
                if (filter.Warehouses != null && filter.Warehouses.Any())
                {
                    filter.StoreSysNoList =
                        filter.Warehouses.Where(o => o.WarehouseType == (int)WarehouseStatus.仓库类型.门店)
                              .Select(o => o.SysNo)
                              .ToList();
                }
            }

            return ISoOrderDao.Instance.GetOutStockOrders(filter);
        }

        /// <summary>
        /// 出库单-待确认
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-06 朱家宏 创建</remarks>
        public Pager<CBOutStockOrder> GetOutStockOrdersToBeConfirmed(ParaOutStockOrderFilter filter)
        {
            filter.StockOutStatus = (int)WarehouseStatus.出库单自提状态.待确认;
            return GetOutStockOrders(filter);
        }

        /// <summary>
        /// 出库单-待提货
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-06 朱家宏 创建</remarks>
        public Pager<CBOutStockOrder> GetOutStockOrdersToBePicked(ParaOutStockOrderFilter filter)
        {
            filter.StockOutStatus = (int)WarehouseStatus.出库单自提状态.待自提;
            return GetOutStockOrders(filter);
        }

        /// <summary>
        /// 出库单-提货已到期
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-08 朱家宏 创建</remarks>
        public Pager<CBOutStockOrder> GetExpiredOutStockOrders(ParaOutStockOrderFilter filter)
        {
            /*
             * 1 提货日期小于今日 
             * 2 出库单状态为待自提
             */
            filter.StockOutStatus = (int)WarehouseStatus.出库单自提状态.待自提;
            filter.PickUpEndDate = DateTime.Today.AddDays(-1);
            return GetOutStockOrders(filter);
        }

        /// <summary>
        /// 出库单-今日已提货 
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-08 朱家宏 创建</remarks>
        public Pager<CBOutStockOrder> GetPickedOutStockOrdersOfToday(ParaOutStockOrderFilter filter)
        {
            /*
             * 1 签收日期为今日 
             * 2 出库单状态为已自提
             */
            filter.SignTime = DateTime.Today;
            filter.StockOutStatus = (int)WarehouseStatus.出库单自提状态.已自提;

            return GetOutStockOrders(filter);
        }

        /// <summary>
        /// 从当前用户登录信息中获取门店
        /// </summary>
        /// <param name="warehouses">登录信息中的仓库集合(CurrentUser.Warehouses)</param>
        /// <returns>门店列表</returns>
        /// <remarks>
        /// 2013-07-08 朱家宏 创建
        /// 2013-11-12 黄志勇 修改
        /// </remarks>
        public IList<WhWarehouse> GetShopsFromUserSession(IList<WhWarehouse> warehouses)
        {
            if (warehouses != null)
            {
                warehouses = warehouses.Where(o => o.WarehouseType == (int)WarehouseStatus.仓库类型.门店&&o.Status==1).OrderBy(i=>i.WarehouseName).ToList();
            }
            return warehouses;
        }

        #endregion

        #endregion

        #region 门店提货

        /// <summary>
        /// 门店自提确认
        /// </summary>
        /// <param name="sysNo">出库单编号</param>
        /// <param name="user">操作人信息</param>
        /// <returns></returns>
        /// <remarks>2013-07-06 朱成果 创建</remarks> 
        public void SelfDeliverySure(int sysNo, SyUser user)
        {
            // 1.没有应收款的出库单，向数据库里面写入一条验证码记录，并将该验证码发送到收货人手机
            // 2.有应收款的出库单，向收货人手机发送一条提示信息
            var outstock = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.GetModel(sysNo);//出库单实体
            if (outstock == null)
            {
                throw new Exception("出库单不存在");
            }
            if (outstock.Status != (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单自提状态.待确认)
            {
                return;   //非待确认状态不执行操作
            }
            var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(outstock.ReceiveAddressSysNo);
            if (receiveAddress == null)
            {
                throw new Exception("收货地址不存在");
            }
            outstock.Status = (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单自提状态.待自提;
            Hyt.DataAccess.Warehouse.IOutStockDao.Instance.Update(outstock);//更新数据库状态
            var isPay = outstock.Receivable == 0;//是否有应收款
            var Shop = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseEntity(outstock.WarehouseSysNo);//仓库信息

            var order = SoOrderBo.Instance.GetByOutStockSysNo(sysNo);//订单信息
            //写订单事务日志 余勇 添加 2014-02-24
            SoOrderBo.Instance.WriteSoTransactionLog(order.TransactionSysNo
                                    ,
                                    Constant.ORDER_TRANSACTIONLOG_SHOPORDER_SURE
                                    , user.UserName);
            #region 发送短信，邮件
            if (VHelper.ValidatorRule(new Rule_Mobile(receiveAddress.MobilePhoneNumber)).IsPass)
            {
                SmsResult R=null;
                if (isPay)//提货码和通知短信
                {
                    R = SendSelfDeliveryValidation(outstock.SysNo, outstock.OrderSysNO, receiveAddress.MobilePhoneNumber, Shop);

                }
                else//通知短信
                {
                    R = Hyt.BLL.Extras.SmsBO.Instance.发送自提通知短信(receiveAddress.MobilePhoneNumber, outstock.OrderSysNO.ToString(), Shop.StreetAddress, Shop.Phone);

                }
               if (R!=null&&R.Status == SmsResultStatus.Failue)
               {
                   throw new Exception("短信发送失败，请检查当前会员是否支持接收短信");
               }
            }
            if (VHelper.Do(receiveAddress.EmailAddress, VType.Email))
            {
                BLL.Extras.EmailBo.Instance.发送门店自提确认备货邮件(receiveAddress.EmailAddress, order.CustomerSysNo.ToString(),
                                                        order.SysNo.ToString(), Shop.StreetAddress, Shop.Phone);
            }
            #endregion
        }

        /// <summary>
        /// 延迟自提
        /// </summary>
        /// <param name="sysNO">出库单号</param>
        /// <param name="delayDate">延迟自提日期</param>
        /// <param name="reason">原因</param>
        /// <returns></returns>
        /// <remarks>2013-07-06 朱成果 创建</remarks>
        public void SelfDeliveryDelay(int sysNO, DateTime delayDate, string reason)
        {
            if (delayDate < DateTime.Now)
            {
                throw new Exception("延迟日期必需大于当前日期");
            }
            var outstock = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.GetModel(sysNO);//出库单实体
            if (outstock == null)
            {
                throw new Exception("出库单不存在");
            }
            if (outstock.Status == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单自提状态.待确认 || outstock.Status == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单自提状态.待自提)
            {
                outstock.PickUpDate = delayDate;
                outstock.Remarks = reason;
                Hyt.DataAccess.Warehouse.IOutStockDao.Instance.Update(outstock);//更新数据库状态
            }
            else
            {
                throw new Exception("出库单状态不满足延迟自提条件");
            }
        }

        /// <summary>
        /// 门店自提出库单设为缺货
        /// </summary>
        /// <param name="sysNo">出库单号</param>
        /// <param name="reason">缺货原因</param>
        /// <param name="user">操作人</param>
        /// <param name="joinPool">是否加入订单池</param>
        /// <returns></returns>
        /// <remarks>2013-07-06 朱成果 创建</remarks>
        public WhStockOut SetOutOfStock(int sysNo, string reason, Hyt.Model.SyUser user, bool joinPool=false)
        {
            var entity = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.GetModel(sysNo);
            if (entity == null)
            {
                throw new Exception("出库单不存在");
            }
            bool HaveReceivable = entity.Receivable > 0;//有应收金额
            entity.Status = (int)WarehouseStatus.出库单自提状态.作废;
            entity.Remarks = reason;
            int c = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.Update(entity);//更新出库单状态
            if (c > 0)
            {
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateSoStatusForSotckOutCancel(sysNo, user, reason);//更新订单状态
            }
            if (HaveReceivable)//有收款金额,作废全部出库单,防止没付钱就提货
            {
                var lst = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhStockOutListByOrderID(entity.OrderSysNO);
                if (lst != null && lst.Count > 0)
                {
                    foreach (WhStockOut item in lst)
                    {
                        if (item.Status != (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.作废 &&
                            item.Status != (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.已签收 &&
                            item.SysNo != sysNo)
                        {
                            item.Status = (int)WarehouseStatus.出库单自提状态.作废;
                            item.Remarks = reason;
                            c = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.Update(item);//更新出库单状态
                            if (c > 0)
                            {
                                Hyt.BLL.Order.SoOrderBo.Instance.UpdateSoStatusForSotckOutCancel(item.SysNo, user, reason);//更新订单状态
                            }
                        }
                    }
                }
            }
            //写订单池记录
            if (joinPool)
            {
                SyJobPoolPublishBo.Instance.OrderWaitStockOut(entity.OrderSysNO, null, null, null);
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("创建客服订单提交出库审核任务，销售单编号:{0}", entity.OrderSysNO), entity.OrderSysNO, null, user.SysNo);
            }
            return entity;
        }
        //提货
        private delegate void PickUpDel(WhStockOut xx);

        /// <summary>
        /// 门店提货
        /// </summary>
        /// <param name="sysNo">出库单</param>
        /// <param name="payMoney">收款金额,如为空表已付款</param>
        /// <param name="PaymentType">PaymentType.现金或者 PaymentType.刷卡</param>
        /// <param name="user">操作人</param>
        /// <param name="invoice">发票 null表示不开发票</param>
        /// <param name="voucherNo">收款单明细-交易凭证号</param>
        /// <param name="withTran">是否事物方式提交</param>
        /// <param name="easReceiptCode">EAS收款科目编码</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-08 朱成果 创建
        /// 2013/10/14 朱家宏 增加 voucherNo参数
        /// </remarks>
        public void PickUp(int sysNo, decimal? payMoney, int PaymentType, Hyt.Model.SyUser user, FnInvoice invoice, string voucherNo, bool withTran = true, string easReceiptCode = null)
        {
            var entity = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.GetModel(sysNo);
            if (entity == null)
            {
                throw new Exception("出库单不存在");
            }
            if (entity.Status == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单自提状态.已自提)
            {
                throw new Exception("该出库单商品已被提货。");
            }
            var mydel = new PickUpDel(delegate(WhStockOut xx)
            {
                if (invoice != null)//新开发票
                {
                    var fninvoice = Hyt.DataAccess.Order.IFnInvoiceDao.Instance.GetFnInvoiceByOrderID(entity.OrderSysNO);//是否已经设置了发票信息
                    if (fninvoice == null)
                    {
                        invoice.CreatedBy = user.SysNo;
                        invoice.CreatedDate = DateTime.Now;
                        invoice.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.发票状态.已开票;

                        //2013/10/21 朱家宏 添加
                        invoice.LastUpdateDate = DateTime.Now;
                        invoice.LastUpdateBy = user.SysNo;
                        invoice.TransactionSysNo = SoOrderBo.Instance.GetEntity(xx.OrderSysNO).TransactionSysNo;
                        
                        int newid = Hyt.DataAccess.Order.IFnInvoiceDao.Instance.InsertEntity(invoice);
                        xx.InvoiceSysNo = newid;//出库单关联发票
                        SoOrderBo.Instance.UpdateOrderInvoice(entity.OrderSysNO, newid); //更新订单关联发票 余勇修改为调用业务方法 Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateInvoiceNo(entity.OrderSysNO, newid);//订单关联发票
                    }
                    else
                    {
                        if (fninvoice.Status == (int)Hyt.Model.WorkflowStatus.FinanceStatus.发票状态.已开票)
                        {
                            throw new Exception("已经开具发票，不能重复开票");
                        }
                        else
                        {
                            invoice.LastUpdateBy = user.SysNo;
                            invoice.LastUpdateDate = DateTime.Now;
                            invoice.InvoiceAmount = fninvoice.InvoiceAmount;
                            invoice.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.发票状态.已开票;
                            invoice.SysNo = fninvoice.SysNo;
                            Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(invoice); //更新发票 余勇 修改 改为调用业务层方法
                            //Hyt.DataAccess.Order.IFnInvoiceDao.Instance.UpdateEntity(invoice);
                        }
                    }
                }
                xx.Status = (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单自提状态.已自提;
                xx.StockOutBy = user.SysNo;
                xx.StockOutDate = DateTime.Now;
                xx.SignTime = DateTime.Now;
                if (payMoney.HasValue)
                {
                    if (payMoney.Value > xx.Receivable)
                    {
                        throw new Exception("收款金额大于应收金额");
                    }
                   // xx.Receivable = xx.Receivable - payMoney.Value;
                }
                Hyt.DataAccess.Warehouse.IOutStockDao.Instance.Update(xx);//更新出库单状态
                GenerateOtherDataForPickUp(entity, user, payMoney, PaymentType);//创建已签收配送单，已结算结算单
                //更新收款明细
                if (payMoney.HasValue)
                {
                    FnReceiptVoucherItem item = new FnReceiptVoucherItem()
                    {
                        Amount = payMoney.Value,
                        CreatedBy = user.SysNo,
                        CreatedDate = DateTime.Now,
                        PaymentTypeSysNo = PaymentType,
                        TransactionSysNo = entity.TransactionSysNo,
                        Status = (int)Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效,
                        VoucherNo = voucherNo,
                        EasReceiptCode=easReceiptCode,
                        ReceivablesSideType=(int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.仓库,//收款单仓库
                        ReceivablesSideSysNo = entity.WarehouseSysNo   //仓库编号
                    };
                    Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(xx.OrderSysNO, item);
                }
                //同步支付时间的到订单主表
                ISoOrderDao.Instance.UpdateOrderPayDteById(xx.OrderSysNO);
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateSoStatusForSotckOutSign(xx, user);  //订单完成，并加积分
               
            });

            //外层都使用了事务将此方法包含起来，所以注释掉此段代码
            //if (withTran)
            //{
            //    using (var tran = new TransactionScope())
            //    {
            //        mydel(entity);
            //        tran.Complete();
            //    }
            //}
            //else
            //{
                mydel(entity);
            //}
            try
            {
                //a)	门店下单(付现)提货动作后自动确认收款单
                //b)	门店自提货到付款(付现)提货后自动确认收款单
                // 自动确认收款单，并写Eas数据
                if (payMoney.HasValue && (PaymentType == (int)Hyt.Model.SystemPredefined.PaymentType.现金 || PaymentType == (int)Hyt.Model.SystemPredefined.PaymentType.现金预付))
                {
                    Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.AutoConfirmReceiptVoucher(entity.OrderSysNO, user);
                }


                
                //修改ERP库存
                //Hyt.BLL.Warehouse.WhWarehouseBo.Instance.UpdateErpProductNumber(sysNo);
                //修改库存
                //Hyt.BLL.Warehouse.WhWarehouseBo.Instance.UpdateWarehouseProductStock(sysNo);
            }
            catch (Exception ex)
            {
                Hyt.BLL.Log.SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店提货或者门店下单提货",
                                         LogStatus.系统日志目标类型.EAS, sysNo, ex, string.Empty, user.SysNo);
            }
        }

        /// <summary>
        /// 订单是否已经开票
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2013-11-06 朱家宏 创建</remarks>
        public bool HasInvoiced(int orderSysNo)
        {
            var result = false;
            var fninvoice = IFnInvoiceDao.Instance.GetFnInvoiceByOrderID(orderSysNo);
            //是否已经开过发票信息
            if (fninvoice != null)
            {
                result = (fninvoice.Status == (int) FinanceStatus.发票状态.已开票);
            }
            return result;
        }

        /// <summary>
        /// 门店自提（补充业务所需的配送单和结算单)
        /// </summary>
        /// <param name="entity">出库单</param>
        /// <param name="user">操作人</param>
        /// <param name="payMoney">收款金额,如为空表已付款</param>
        /// <param name="PaymentType">PaymentType.现金或者 PaymentType.刷卡</param> 
        /// <returns></returns>
        /// <remarks>2013-08-21 朱成果 创建</remarks>
        private void GenerateOtherDataForPickUp(WhStockOut entity, Hyt.Model.SyUser user, decimal? payMoney,int PaymentType)
        {
            var f = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity((int)Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.销售单, entity.OrderSysNO);
            decimal havePaid = 0;
            if (f != null&&f.Status!=(int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单状态.作废)
            {
                havePaid = f.ReceivedAmount;//预付金额
            }
            var delivery = new LgDelivery()
            {
                DeliveryUserSysNo = user.SysNo,
                DeliveryTypeSysNo = entity.DeliveryTypeSysNo,
                CreatedBy = user.SysNo,
                CreatedDate = DateTime.Now,
                Status = (int)LogisticsStatus.配送单状态.已结算,
                StockSysNo = entity.WarehouseSysNo,
                IsEnforceAllow = 0,
                PaidAmount = havePaid,
                CODAmount = payMoney.HasValue ? payMoney.Value : 0
            };
            var deliverySysNo = Hyt.DataAccess.Logistics.ILgDeliveryDao.Instance.CreateLgDelivery(delivery);//生成结算单
            var deliveryItem = new LgDeliveryItem()
            {
                DeliverySysNo = deliverySysNo,
                NoteType = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型.出库单,
                NoteSysNo = entity.SysNo,
                Status = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单明细状态.已签收,
                AddressSysNo = entity.ReceiveAddressSysNo,
                TransactionSysNo = entity.TransactionSysNo,
                StockOutAmount = entity.StockOutAmount,
                PaymentType = payMoney.HasValue ? (int)Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.到付 : (int)Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.预付,
                Receivable = payMoney.HasValue ? payMoney.Value : 0,
                IsCOD = payMoney.HasValue ? (int)Hyt.Model.WorkflowStatus.LogisticsStatus.是否到付.是 : (int)Hyt.Model.WorkflowStatus.LogisticsStatus.是否到付.否,
                Remarks = "【门店自提系统自动创建】",
                CreatedBy = user.SysNo,
                CreatedDate = DateTime.Now
            };
            Hyt.DataAccess.Logistics.ILgDeliveryDao.Instance.AddDeliveryItem(deliveryItem);
            var settlement = new LgSettlement
            {
                AuditDate = DateTime.Now,
                AuditorSysNo = user.SysNo,
                CODAmount = payMoney.HasValue ? payMoney.Value : 0,
                CreatedBy = user.SysNo,
                CreatedDate = DateTime.Now,
                DeliveryUserSysNo = user.SysNo,
                PaidAmount = havePaid,
                TotalAmount = payMoney.HasValue ? payMoney.Value : 0,
                Status = (int)LogisticsStatus.结算单状态.已结算,
                WarehouseSysNo=entity.WarehouseSysNo,
                Remarks = "【门店自提系统自动创建】"
            };
            int settlementSysNo = Hyt.DataAccess.Logistics.ILgSettlementDao.Instance.Create(settlement);
            var settlementItem = new LgSettlementItem
             {
                 CreatedBy = user.SysNo,
                 CreatedDate = DateTime.Now,
                 DeliverySysNo = deliverySysNo,
                 DeliveryItemStatus = (int)LogisticsStatus.结算单状态.已结算,
                 PayAmount = payMoney.HasValue ? payMoney.Value : 0,
                 SettlementSysNo = settlementSysNo,
                 TransactionSysNo=entity.TransactionSysNo,
                 Status=(int)Hyt.Model.WorkflowStatus.LogisticsStatus.结算单明细状态.已结算,
                 StockOutSysNo=entity.SysNo,
                 PayType = PaymentType
              };
            Hyt.DataAccess.Logistics.ILgSettlementItemDao.Instance.Create(settlementItem);
        }

        /// <summary>
        /// 门店提货转快递
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <param name="receiveAddress">快递收货地址</param>
        /// <param name="reason">转快递原因</param>
        /// <param name="message">会员留言</param>
        /// <param name="invoice">发票 null表示不开发票</param>
        /// <param name="payType">付款方式，现金或者刷卡</param>
        /// <param name="receiveMoney">收款金额 null 表示无应收款</param>
        /// <param name="voucherNo">刷卡流水号</param>
        /// <param name="easReceiptCode">eas收款科目编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-08 朱成果 创建
        /// 2013/10/14 朱家宏 增加 voucherNo参数
        /// </remarks>
        public void SetOrderToCourier(int stockOutSysNo, SoReceiveAddress receiveAddress, string reason, string message, FnInvoice invoice, int payType, decimal? receiveMoney, SyUser user, string voucherNo,string easReceiptCode = null )
        {

            var model = SetOutOfStock(stockOutSysNo, reason, user,false);  //作废出库单
            Hyt.BLL.Order.SoOrderBo.Instance.WriteSoTransactionLog(model.TransactionSysNo, "出库单门店自提转快递:" + stockOutSysNo, user.UserName);//转快递日志
            var order = SoOrderBo.Instance.GetEntity(model.OrderSysNO);  //更新订单信息
            if (order == null)
            {
                throw new Exception("订单信息不存在");
            }
            
                order.DeliveryTypeSysNo = DeliveryType.第三方快递;//修改配送方式
                if (payType > 0)
                {
                   order.PayTypeSysNo = payType;//门店现付
                }
                else if (order.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.未支付)
                {
                   order.PayTypeSysNo = (int)PaymentType.现金预付;
                }
                order.Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核;
                order.InternalRemarks = "【门店转快递】:"+reason;
                order.CustomerMessage = message;//会员留言
                order.ReceiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);//收货地址
                order.ReceiveAddress.AreaSysNo = receiveAddress.AreaSysNo;//收货地区
                order.ReceiveAddress.StreetAddress = receiveAddress.StreetAddress;//详细收货地址
                if (invoice == null)
                {
                    order.InvoiceSysNo = 0;//发票
                }
                else
                {
                    //以前没有创建发票
                    if (order.InvoiceSysNo == 0)
                    {
                        order.OrderInvoice = new FnInvoice
                        {
                            CreatedBy = user.SysNo,
                            CreatedDate = DateTime.Now,
                            InvoiceRemarks = invoice.InvoiceRemarks,
                            InvoiceTitle = invoice.InvoiceTitle,
                            
                            InvoiceTypeSysNo = invoice.InvoiceTypeSysNo,
                            InvoiceAmount = order.CashPay,
                            Status = 10,
                            TransactionSysNo = model.TransactionSysNo
                        };
                        order.InvoiceSysNo = Hyt.BLL.Order.SoOrderBo.Instance.InsertOrderInvoice(order.OrderInvoice);
                    }
                    else  //以前有发票
                    {
                        order.OrderInvoice = Hyt.BLL.Order.SoOrderBo.Instance.GetFnInvoice(order.InvoiceSysNo);
                        order.OrderInvoice.LastUpdateBy = user.SysNo;
                        order.OrderInvoice.LastUpdateDate = DateTime.Now;
                        order.OrderInvoice.InvoiceTitle = invoice.InvoiceTitle;
                        
                        order.OrderInvoice.InvoiceTypeSysNo = invoice.InvoiceTypeSysNo;
                        order.OrderInvoice.InvoiceAmount = order.CashPay;
                        Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(order.OrderInvoice);
                    }
                }
                var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(receiveAddress.AreaSysNo,null,(int)Hyt.Model.SystemPredefined.DeliveryType.第三方快递,WarehouseStatus.仓库状态.启用).FirstOrDefault();
                if (warehouse != null)
                {
                    order.DefaultWarehouseSysNo = warehouse.SysNo;//快递仓库默认为当前仓库
                }
                SoOrderBo.Instance.UpdateOrder(order); //更新订单 余勇 修改为调用业务层方法
                //Hyt.DataAccess.Order.ISoOrderDao.Instance.Update(order);//更新订单

                SoOrderBo.Instance.SaveSoReceiveAddress(order.ReceiveAddress);//保存订单收货地址
               
                //更新收款明细
                if (receiveMoney.HasValue)
                {
                    FnReceiptVoucherItem item = new FnReceiptVoucherItem()
                    {
                        Amount = receiveMoney.Value,
                        CreatedBy = user.SysNo,
                        CreatedDate = DateTime.Now,
                        PaymentTypeSysNo = payType,
                        TransactionSysNo = order.TransactionSysNo,
                        Status = (int)Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效,
                        VoucherNo = voucherNo,
                        EasReceiptCode = easReceiptCode,
                        ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.仓库,//收款单仓库
                        ReceivablesSideSysNo = model.WarehouseSysNo   //仓库编号
                    };
                    Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(order.SysNo, item);
                    if (receiveMoney.HasValue && (payType == (int)Hyt.Model.SystemPredefined.PaymentType.现金 || payType == (int)Hyt.Model.SystemPredefined.PaymentType.现金预付))
                    {
                        Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.AutoConfirmReceiptVoucher(order.SysNo, user);//收现金自动确认收款单
                    }
                }
                //同步支付时间的到订单主表
                ISoOrderDao.Instance.UpdateOrderPayDteById(order.SysNo);
                //写订单池记录
                SyJobPoolPublishBo.Instance.OrderAuditBySysNo(order.SysNo);
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("门店提货转快递创建订单审核任务，销售单编号:{0}",
                          order.SysNo), order.SysNo, null, user.SysNo);
        }
        #endregion

        #region 门店提货验证码相关
       
        /// <summary>
        /// 发送门店提货验证码
        /// </summary>
        /// <param name="sysNo">出库单编号</param>
        /// <param name="orderNO">订单号</param>
        /// <param name="mobileNum">收货人手机</param>
        /// <param name="shop">门店</param>
        /// <returns>提货验证码</returns>
        /// <remarks>2013-07-06 朱成果 创建</remarks> 
        public SmsResult SendSelfDeliveryValidation(int sysNo, int orderNO, string mobileNum, WhWarehouse shop = null)
        {
            //随机生成6位验证码
            //string code = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6).ToLower();
            string code = Hyt.Util.WebUtil.Number(6, false);
            WhPickUpCode p = new WhPickUpCode();
            p.Code = code;
            p.StockOutSysNo = sysNo;
            p.MobilePhoneNumber = mobileNum;
            p.CreatedDate = DateTime.Now;
            SetWhPickUpCode(p);
            //SendMobileMessage(mobileNum, string.Format(Hyt.Model.SystemPredefined.Constant.SELF_DELIVERY_SURE_CODE, sysNo, code));//发送验证码
            return BLL.Extras.SmsBO.Instance.发送自提通知短信带验证码(mobileNum, orderNO.ToString(), shop.StreetAddress, shop.Phone, code);
            //BLL.Extra.EmailBO.SendMail
        }

        /// <summary>
        /// 获取门店提货验证码
        /// </summary>
        /// <param name="stockOutSysNo">出库单编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-06 朱成果 创建</remarks> 
        public WhPickUpCode GetEntityByStockOutNo(int stockOutSysNo)
        {
            return Hyt.DataAccess.Order.IWhPickUpCodeDao.Instance.GetEntityByStockOutNo(stockOutSysNo);
        }

        /// <summary>
        /// 插或者更新门店提货验证码
        /// </summary>
        /// <param name="entity">提货验证码实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-06 朱成果 创建</remarks> 
        public void SetWhPickUpCode(WhPickUpCode entity)
        {
            if (entity != null && entity.StockOutSysNo != 0)
            {
                var model = GetEntityByStockOutNo(entity.StockOutSysNo);
                if (model == null)
                {
                    Hyt.DataAccess.Order.IWhPickUpCodeDao.Instance.InsertEntity(entity);
                }
                else
                {
                    model.Code = entity.Code;
                    model.MobilePhoneNumber = entity.MobilePhoneNumber;
                    model.LastUpdateBy = entity.CreatedBy;
                    model.LastUpdateDate = DateTime.Now;
                    Hyt.DataAccess.Order.IWhPickUpCodeDao.Instance.UpdateEntity(model);
                }
            }
        }
        #endregion

        #region 是否存在未支付出库单，并返回
        /// <summary>
        /// 是否存在待支付出库单，并返回 true 存在 false 不存在
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="stockOutNo">待付款的出库单号</param>
        /// <returns>true 未支付 false 已支付</returns>
        /// <remarks>2013-08-08 朱成果 创建</remarks>
        public bool GetUnPaidStockOutNo(int orderSysNo, out int stockOutNo)
        {
            bool flg = false;
            stockOutNo = 0;
            var lst = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhStockOutListByOrderID(orderSysNo);
            if (lst != null && lst.Count > 0)
            {
                foreach (WhStockOut item in lst)
                {
                    if (item.Status != (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.作废 && item.Receivable > 0)
                    {
                        flg = true;
                        stockOutNo = item.SysNo;
                        return flg;
                    }
                }
            }
            return flg;
        }
        #endregion
    }
}
