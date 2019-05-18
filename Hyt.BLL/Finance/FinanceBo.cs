using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Order;
using Hyt.BLL.RMA;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Finance;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util.Validator;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Transactions;
using Hyt.DataAccess.Order;

namespace Hyt.BLL.Finance
{
    /// <summary>
    /// 财务
    /// </summary>
    /// <remarks>
    /// 2013-07-11 何方 创建
    /// 2013-07-19 朱成果  修改文件名称
    /// </remarks>
    public class FinanceBo : BOBase<FinanceBo>
    {
        private static object obj = new object();//加锁对象

        

        /// <summary>
        /// 根据事务编号获取收款单
        /// </summary>
        /// <param name="transactionSysNo">The transaction sys no.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/11 何方 创建
        /// </remarks>
        public FnReceiptVoucher GetReceiptVoucher(string transactionSysNo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建付款单
        /// </summary>
        /// <param name="paymentVoucher">府库单主表.</param>
        /// <param name="item">付款单明细.</param>
        /// <returns>付款单号</returns>
        /// <remarks>
        /// 2013-07-12 何方 创建
        /// </remarks>
        public int CreatePaymentVoucher(FnPaymentVoucher paymentVoucher, FnPaymentVoucherItem item)
        {
         return    CreatePaymentVoucher(paymentVoucher, new List<FnPaymentVoucherItem> { item });
        }

        /// <summary>
        /// 更新付款单
        /// </summary>
        /// <param name="paymentVoucher"></param>
        /// <returns></returns>
        /// <remarks>2016-06-02 杨云奕 添加</remarks>
        public void UpdatePaymentVoucher(FnPaymentVoucher paymentVoucher)
        {
            IFnPaymentVoucherDao.Instance.UpdateVoucher(paymentVoucher);

        }
        /// <summary>
        /// 创建付款单
        /// </summary>
        /// <param name="paymentVoucher"></param>
        /// <returns></returns>
        /// <remarks>2016-06-02 杨云奕 添加</remarks>
        public int CreatePaymentVoucher(FnPaymentVoucher paymentVoucher)
        {
            var paymentVoucherSysNo = IFnPaymentVoucherDao.Instance.Insert(paymentVoucher);
            return paymentVoucherSysNo;
        } 
        /// <summary>
        /// 创建付款单
        /// </summary>
        /// <param name="paymentVoucher">付款单主表.</param>
        /// <param name="items">明细列表列表.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-12 何方 创建
        /// </remarks>
        public int CreatePaymentVoucher(FnPaymentVoucher paymentVoucher, IList<FnPaymentVoucherItem> items)
        {
            var paymentVoucherSysNo = IFnPaymentVoucherDao.Instance.Insert(paymentVoucher);
            if (items != null)
            {
                foreach (var item in items)
                {
                    item.PaymentVoucherSysNo = paymentVoucherSysNo;
                    var itemSysNo = IFnPaymentVoucherDao.Instance.InsertItem(item);
                    item.SysNo = itemSysNo; //给明细付款单编号赋值 余勇 2014-07-22
                }
            }
            return paymentVoucherSysNo;
        }
        /// <summary>
        /// 创建付款单
        /// </summary>
        /// <param name="paymentVoucher"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-1-6 王耀发 创建
        /// </remarks>
        public int InsertPaymentVoucher(FnPaymentVoucher paymentVoucher)
        {
            var paymentVoucherSysNo = IFnPaymentVoucherDao.Instance.Insert(paymentVoucher);
            return paymentVoucherSysNo;
        }

        #region 操作 (财务管理)

        /// <summary>
        /// 添加网上支付(单据来源：订单)
        /// </summary>
        /// <param name="onlinePayment">数据实体</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-07-18 朱家宏 创建</remarks>
        /// <remarks>2013-11-1 黄志勇 修改</remarks>
        public int CreateOnlinePaymentFromSoOrder(FnOnlinePayment onlinePayment)
        {
            /*
             * 1.支付方式类型为预付&&未付款的订单
             * 2.修改订单相关：支付状态改为已支付 订单状态如果为待支付改为待创建出库单
             * 3.根据收款单单据来源和单据来源编号，创建收款单明细
             */

            if (onlinePayment == null)
                throw new ArgumentNullException("onlinePayment");

            onlinePayment.Source = (int)FinanceStatus.网上支付单据来源.销售单;
            onlinePayment.Status = (int)FinanceStatus.网上支付状态.有效;

            var soOrder = SoOrderBo.Instance.GetEntity(onlinePayment.SourceSysNo);
            var payStatus = soOrder.PayStatus;
            var paymentType = Basic.PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo).PaymentType;

            //创建网上支付记录
            var r = 0;
            if (paymentType == (int)BasicStatus.支付方式类型.预付 &&
                payStatus == (int)OrderStatus.销售单支付状态.未支付)
            {
                r = IFnOnlinePaymentDao.Instance.Insert(onlinePayment);
            }

            if (r == 0) return r;

        
            var userName = Sys.SyUserBo.Instance.GetUserName(onlinePayment.CreatedBy);//创建人姓名
            SoOrderBo.Instance.WriteSoTransactionLog(soOrder.TransactionSysNo,
                                                                   string.Format(Constant.ORDER_TRANSACTIONLOG_PAY,
                                                                                 Util.FormatUtil.FormatCurrency(
                                                                                     onlinePayment.Amount, 2)),
                                                                   userName);
            //创建收款单明细
            var receiptVoucherItem = new FnReceiptVoucherItem
                {
                    Amount = onlinePayment.Amount,
                    CreatedBy = onlinePayment.CreatedBy,
                    LastUpdateBy = onlinePayment.CreatedBy,
                    VoucherNo = onlinePayment.VoucherNo,
                    PaymentTypeSysNo = soOrder.PayTypeSysNo,
                    TransactionSysNo = soOrder.TransactionSysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateDate = DateTime.Now,
                    Status=(int)FinanceStatus.收款单明细状态.有效,
                    ReceivablesSideType=(int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.财务中心,

                };
            //插入收款单,收款明细，
            FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(onlinePayment.SourceSysNo, receiptVoucherItem,false);
            //同步支付时间的到订单主表
            ISoOrderDao.Instance.UpdateOrderPayDteById(soOrder.SysNo);
            return r;
        }

        #endregion

        #region 查询 (财务管理)

        /// <summary>
        /// 网上支付查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>分页查询</returns>
        /// <remarks>2013-07-18 朱家宏 创建</remarks>
        public Pager<CBFnOnlinePayment> GetOnlinePayments(ParaOnlinePaymentFilter filter)
        {
            var pager = IFnOnlinePaymentDao.Instance.GetAll(filter);
            return pager;
        }

        /// <summary>
        /// 收款单查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>分页查询</returns>
        /// <remarks>2013-07-19 朱家宏 创建</remarks>
        public Pager<CBFnReceiptVoucher> GetReceiptVouchers(ParaVoucherFilter filter)
        {
            var pager = IFnReceiptVoucherDao.Instance.GetAll(filter);
            return pager;
        }

        /// <summary>
        /// 预收现金收款单查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="currentUserSysNo">当前登录用户系统编号</param>
        /// <returns>分页查询</returns>
        /// <remarks>2013-07-19 朱家宏 创建</remarks>
        public Pager<CBFnReceiptVoucher> GetFnReceipt(ParaWarehouseFilter filter, int currentUserSysNo)
        {
            var pager = IFnReceiptVoucherDao.Instance.GetFnReceipt(filter, currentUserSysNo);
            if (pager.Rows != null)
            {
                foreach (var voucher in pager.Rows)
                {
                    voucher.Confirmer = Sys.SyUserBo.Instance.GetUserName(voucher.ConfirmedBy);
                }
            }
            return pager;
        }

        /// <summary>
        /// 创建收款单
        /// </summary>
        /// <param name="paymentVoucher"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-6-24 王耀发 创建
        /// </remarks>
        public int InsertReceiptVoucher(FnReceiptVoucher receiptVoucher)
        {
            var receiptVoucherSysNo = IFnReceiptVoucherDao.Instance.Insert(receiptVoucher);
            return receiptVoucherSysNo;
        }

        /// <summary>
        /// 付款单查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>分页查询</returns>
        /// <remarks>2013-07-19 朱家宏 创建</remarks>
        public Pager<CBPaymentVoucher> GetPaymentVouchers(ParaVoucherFilter filter)
        {
            var pager = IFnPaymentVoucherDao.Instance.GetAll(filter);
            return pager;
        }
        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="voucherNo">交易凭证</param>
        /// <returns></returns>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public FnOnlinePayment GetOnlinePaymentByVoucherNo(string voucherNo)
        {
            return IFnOnlinePaymentDao.Instance.GetOnlinePaymentByVoucherNo(voucherNo);
        }
        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="paymentTypeSysNo">支付类型编号</param>
        /// <param name="voucherNo">交易凭证</param>
        /// <returns></returns>
        /// <remarks>2016-09-10 杨浩 创建</remarks>
        public FnOnlinePayment GetOnlinePaymentByVoucherNo(int paymentTypeSysNo, string voucherNo)
        {
            return IFnOnlinePaymentDao.Instance.GetOnlinePaymentByVoucherNo(paymentTypeSysNo,voucherNo);
        }
        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="SourceSysNo">源单号</param>
        /// <returns></returns>
        /// <remarks>2016-4-19 王耀发 创建</remarks>
        public FnOnlinePayment GetOnlinePaymentBySourceSysNo(int SourceSysNo)
        {
            return IFnOnlinePaymentDao.Instance.GetOnlinePaymentBySourceSysNo(SourceSysNo);
        }

        /// <summary>
        /// 通过订单来源和订单编号获取在线付款单数据
        /// </summary>
        /// <param name="Source">网上订单来源</param>
        /// <param name="SourceSysNo">订单编号</param>
        /// <returns>
        /// 2016-04-19 杨云奕 添加
        /// </returns>
        public FnOnlinePayment GetOnlinePaymentBySourceSysNo(Hyt.Model.WorkflowStatus.FinanceStatus.网上支付单据来源 Source,int SourceSysNo)
        {
            return IFnOnlinePaymentDao.Instance.GetOnlinePaymentBySourceSysNo(Source, SourceSysNo);
        }

        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="SourceSysNo">源单号</param>
        /// <returns></returns>
        /// <remarks>2016-4-19 王耀发 创建</remarks>
        public CBFnOnlinePayment GetOnPaymentBySourceSysNo(int SourceSysNo)
        {
            return IFnOnlinePaymentDao.Instance.GetOnPaymentBySourceSysNo(SourceSysNo);
        }
        #endregion

        #region 付款单信息处理
        /// <summary>
        /// 获取付款单及付款明细信息
        /// </summary>
        /// <param name="sysNo">付款单编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-19 朱成果 创建</remarks>
        public CBFnPaymentVoucher GetPayment(int sysNo)
        {
            var model = Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.GetEntity(sysNo);
            if (model != null)
            {
                model.VoucherItems = Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.GetVoucherItems(sysNo);
            }
            return model;
        }
        /// <summary>
        /// 获取付款单及付款明细信息
        /// </summary>
        /// <param name="source">来源</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-26 朱成果 创建</remarks>
        public CBFnPaymentVoucher GetPayment(int source, int sourceSysNo)
        {
            var model = Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.GetEntityByVoucherSource(source, sourceSysNo);
            if (model != null)
            {
                model.VoucherItems = Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.GetVoucherItems(model.SysNo);
            }
            return model;
        }
        /// <summary>
        /// 获取付款单信息（不包括付款明细)
        /// </summary>
        /// <param name="sysNo">付款单编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-19 朱成果 创建</remarks>
        public FnPaymentVoucher GetPaymentVoucher(int sysNo)
        {
            return Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.GetEntity(sysNo);
        }
        /// <summary>
        /// 添加付款单明细
        /// </summary>
        /// <param name="item">明细实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-19 朱成果 创建</remarks>
        public int InsertPaymentVoucherItem(FnPaymentVoucherItem item)
        {
            return Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.InsertItem(item);
        }
        /// <summary>
        /// 获取付款单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-01-7 王耀发 创建</remarks>
        public FnPaymentVoucherItem GetVoucherItem(int sysNo)
        {
            return IFnPaymentVoucherDao.Instance.GetVoucherItem(sysNo);
        }

        /// <summary>
        /// 作废付款单明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <param name="user">操作人</param>
        /// <remarks>2013-07-22 朱成果 创建</remarks>
        public void CancelPaymentVoucherItem(int sysNo, SyUser user)
        {
            var payitem = IFnPaymentVoucherDao.Instance.GetVoucherItem(sysNo);
            if (payitem == null)
            {
                throw new Exception("付款单明细不存在");
            }
            if (payitem.Status == (int)Model.WorkflowStatus.FinanceStatus.付款单明细状态.待付款)
            {
                payitem.Status = (int)Model.WorkflowStatus.FinanceStatus.付款单明细状态.作废;
                payitem.LastUpdateBy = user.SysNo;
                payitem.LastUpdateDate = DateTime.Now;
                IFnPaymentVoucherDao.Instance.UpdateVoucherItem(payitem);
            }
            else
            {
                throw new Exception("当前状态不满足作废条件");
            }
        }

        /// <summary>
        /// 取消确认付款单明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-07-22 黄志勇 创建</remarks>
        public void CancelConfirmPaymentVoucherItem(int sysNo, SyUser user)
        {
            var payitem = IFnPaymentVoucherDao.Instance.GetVoucherItem(sysNo);
            if (payitem == null)
            {
                throw new Exception("付款单明细不存在");
            }
            if (payitem.Status == (int)Model.WorkflowStatus.FinanceStatus.付款单明细状态.已付款)
            {
                var paymentVoucher = IFnPaymentVoucherDao.Instance.GetEntity(payitem.PaymentVoucherSysNo);
                if (paymentVoucher == null) throw new Exception("付款单不存在");
                var paidAmount = paymentVoucher.PaidAmount - payitem.Amount;
                if (paidAmount < 0)
                    throw new Exception("付款单取消确认后已付金额小于0");
                paymentVoucher.PaidAmount = paidAmount;
                paymentVoucher.LastUpdateBy = user.SysNo;
                paymentVoucher.LastUpdateDate = DateTime.Now;
                IFnPaymentVoucherDao.Instance.UpdateVoucher(paymentVoucher);
                payitem.Status = (int)Model.WorkflowStatus.FinanceStatus.付款单明细状态.待付款;
                payitem.LastUpdateBy = user.SysNo;
                payitem.LastUpdateDate = DateTime.Now;
                IFnPaymentVoucherDao.Instance.UpdateVoucherItem(payitem);
            }
            else
            {
                throw new Exception("当前状态不满足取消确认条件");
            }
        }

        /// <summary>
        /// 确认付款单明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-07-22 黄志勇 创建</remarks>
        public void ConfirmPaymentVoucherItem(int sysNo, SyUser user)
        {
            var payitem = IFnPaymentVoucherDao.Instance.GetVoucherItem(sysNo);
            if (payitem == null)
            {
                throw new Exception("付款单明细不存在");
            }
            if (payitem.Status == (int)Model.WorkflowStatus.FinanceStatus.付款单明细状态.待付款)
            {
                var paymentVoucher = IFnPaymentVoucherDao.Instance.GetEntity(payitem.PaymentVoucherSysNo);
                if (paymentVoucher == null) throw new Exception("付款单不存在");
                var paidAmount = paymentVoucher.PaidAmount + payitem.Amount;
                if (paidAmount > paymentVoucher.PayableAmount)
                    throw new Exception("付款单确认后已付金额大于应付金额");
                paymentVoucher.PaidAmount = paidAmount;
                paymentVoucher.LastUpdateBy = user.SysNo;
                paymentVoucher.LastUpdateDate = DateTime.Now;
                IFnPaymentVoucherDao.Instance.UpdateVoucher(paymentVoucher);
                payitem.Status = (int)Model.WorkflowStatus.FinanceStatus.付款单明细状态.已付款;
                payitem.PayerSysNo = user.SysNo;
                payitem.PayDate = DateTime.Now;
                payitem.LastUpdateBy = user.SysNo;
                payitem.LastUpdateDate = DateTime.Now;
                IFnPaymentVoucherDao.Instance.UpdateVoucherItem(payitem);
            }
            else
            {
                throw new Exception("当前状态不满足取消确认条件");
            }
        }

        /// <summary>
        /// 完成付款
        /// </summary>
        /// <param name="sysNo">付款单号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-22 朱成果 创建
        /// 2013-09-24 黄志勇 修改
        /// 2016-6-21 杨浩 新增采购单处理
        /// </remarks> 
        /// 
        public void CompletePayment(int sysNo, SyUser user)
        {
            var pay = GetPaymentVoucher(sysNo);
            if (pay == null)
            {
                throw new Exception("付款单信息不存在");
            }
            if (pay.Status == (int)Model.WorkflowStatus.FinanceStatus.付款单状态.已付款 || pay.Status == (int)FinanceStatus.付款单状态.作废)
            {
                throw new Exception("当前付款单已经付款或者作废");
            }
            if (pay.PaidAmount < pay.PayableAmount)
            {
                throw new Exception("存在待付金额");
            }
            pay.Status = (int)Model.WorkflowStatus.FinanceStatus.付款单状态.已付款;
            pay.LastUpdateBy = user.SysNo;
            pay.LastUpdateDate = DateTime.Now;
            pay.PayDate = DateTime.Now;
            pay.PayerSysNo = user.SysNo;
            IFnPaymentVoucherDao.Instance.UpdateVoucher(pay);
            if (pay.Source == (int)FinanceStatus.付款来源类型.退换货单)
            {
                RmaBo.Instance.PaymentCompleteCallBack(sysNo, user);
            }
            else if (pay.Source == (int)FinanceStatus.付款来源类型.销售单)
            {
                BLL.RMA.RefundReturnDao.Instance.PaymentCompleteCallBack(sysNo,user);//完成退款单
                Hyt.BLL.Order.SoOrderBo.Instance.WriteSoTransactionLog(pay.TransactionSysNo, string.Format(Constant.ORDER_RETURNCASH, sysNo), user.UserName);
            }
            else if (pay.Source == (int)FinanceStatus.付款来源类型.采购单)
            {
                BLL.Purchase.PrPurchaseBo.Instance.UpdateStatus(pay.SourceSysNo,(int)PurchaseStatus.采购单付款状态.已付款,-1,-1);
            }
        }
        #endregion

        #region 门店退现与EAS
        /// <summary>
        /// 门店退现与EAS
        /// </summary>
        /// <param name="pay">付款单及付款单明细</param>
        /// <param name="user">操作人</param>
         /// <remarks>
        /// 2013-12-11 朱成果 创建
        /// </remarks>
        public void WriteEasPaymentVoucher(CBFnPaymentVoucher pay, SyUser user)
        {
            if (pay == null) return;
            if (pay == null || pay.Status != FinanceStatus.付款单状态.已付款.GetHashCode())
            {
                return;
            }
            if (pay.VoucherItems != null && pay.VoucherItems.Count > 0)
            {
                string  orderSysNo=string.Empty;
                string  customer = string.Empty;
                if (pay.Source == (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.销售单)
                {
                    orderSysNo = pay.SourceSysNo.ToString();
                    customer = WhWarehouseBo.Instance.GetErpCustomerCode(pay.SourceSysNo);
                }
                else if(pay.Source == (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.退换货单)
                {
                    var rma = Hyt.BLL.RMA.RmaBo.Instance.GetRcReturnEntity(pay.SourceSysNo);
                    if (rma != null)
                    {
                        orderSysNo = rma.OrderSysNo.ToString();
                        customer = WhWarehouseBo.Instance.GetErpCustomerCode(rma.OrderSysNo);
                    }
                }
                var lst = new List<Extra.Erp.Model.Receiving.ReceivingInfo>();
                string easNum = Extra.Erp.Model.EasConstant.HytWharehouse;//商城仓库
                string organizationCode = string.Empty;//组织机构代码
                string payeeAccountBank = string.Empty;//收款账户
                string settlementType = Extra.Erp.Model.EasConstant.SettlementType_Cash;//仓库只有现金，01:Eas中的现金
                foreach (FnPaymentVoucherItem pItem in pay.VoucherItems)
                {
                    if (pItem.Status == (int)FinanceStatus.付款单明细状态.已付款 && pItem.Amount > 0)
                    {
                        #region 付款明细
                        if ( pItem.PaymentToType == (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款方类型.仓库&&pItem.PaymentType==(int)FinanceStatus.付款单付款方式.现金)
                        {
                            var warehouse = WhWarehouseBo.Instance.GetWarehouse(pItem.PaymentToSysNo);//地区仓库
                            var oraganization = Hyt.BLL.Basic.OrganizationBo.Instance.GetOrganization(pItem.PaymentToSysNo);
                            if (warehouse != null)
                            {
                                easNum = warehouse.ErpCode;
                            }
                            else
                            {
                                easNum = string.Empty;
                            }
                            if (oraganization != null)
                            {
                                organizationCode = oraganization.Code;
                            }
                            //付款科目
                            var km=Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetFnReceiptTitleAssociation(pItem.PaymentToSysNo, Hyt.Model.SystemPredefined.PaymentType.现金).OrderByDescending(m=>m.IsDefault).FirstOrDefault();
                            lst.Add(new Extra.Erp.Model.Receiving.ReceivingInfo()
                            {
                                Amount = pItem.Amount,
                                OrderSysNo = orderSysNo,
                                WarehouseNumber = easNum,
                                WarehouseSysNo = warehouse.SysNo,
                                PayeeAccount = km==null?string.Empty:km.EasReceiptCode,
                                PayeeAccountBank = payeeAccountBank,
                                SettlementType = settlementType,
                                OrganizationCode = organizationCode,
                            });
                        }
                        #endregion
                    }
                }
                if (lst.Count > 0)
                {
                    try
                    {
                        SoOrder orderinfo=null;
                        if(!string.IsNullOrEmpty(orderSysNo))
                        {
                            orderinfo=Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(int.Parse(orderSysNo));

                        }
                        Extra.Erp.EasProviderFactory.CreateProvider().Payment(lst, customer, false, orderSysNo, orderinfo==null?string.Empty:orderinfo.TransactionSysNo);
                    }
                    catch (Exception ex)
                    {
                        Hyt.BLL.Log.SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "门店退货退现写EAS",
                                         LogStatus.系统日志目标类型.EAS, pay.SysNo, ex, string.Empty, user.SysNo);
                    }

                }
            }

        }
        #endregion

        #region 部分签收退款到EAS
        /// <summary>
        /// 部分签收退款到EAS
        /// </summary>
        /// <param name="pay">付款单及付款单明细</param>
        /// <param name="operatorSysNo">操作人编号</param>
        /// <remarks>
        /// 2014-07-22 余勇 创建
        /// </remarks>
        public void PartialSignWriteEas(CBFnPaymentVoucher pay, int operatorSysNo)
        {
            if (pay == null) return;
            if (pay == null || pay.Status != FinanceStatus.付款单状态.已付款.GetHashCode())
            {
                return;
            }
            if (pay.VoucherItems != null && pay.VoucherItems.Count > 0)
            {
                string orderSysNo = string.Empty;
                string customer = string.Empty;
                if (pay.Source == (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.销售单)
                {
                    orderSysNo = pay.SourceSysNo.ToString();
                    customer = WhWarehouseBo.Instance.GetErpCustomerCode(pay.SourceSysNo);
                }
                else if (pay.Source == (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.退换货单)
                {
                    var rma = Hyt.BLL.RMA.RmaBo.Instance.GetRcReturnEntity(pay.SourceSysNo);
                    if (rma != null)
                    {
                        orderSysNo = rma.OrderSysNo.ToString();
                        customer = WhWarehouseBo.Instance.GetErpCustomerCode(rma.OrderSysNo);
                    }
                }
                var lst = new List<Extra.Erp.Model.Receiving.ReceivingInfo>();
                string easNum = Extra.Erp.Model.EasConstant.HytWharehouse;//商城仓库
                string organizationCode = string.Empty;//组织机构代码
                string payeeAccountBank = string.Empty;//收款账户
                string settlementType = Extra.Erp.Model.EasConstant.SettlementType_Cash;//仓库只有现金，01:Eas中的现金
                foreach (FnPaymentVoucherItem pItem in pay.VoucherItems)
                {
                    if (pItem.Status == (int)FinanceStatus.付款单明细状态.已付款 && pItem.Amount > 0)
                    {
                        #region 付款明细
                        if (pItem.PaymentToType == (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款方类型.仓库 && pItem.PaymentType == (int)FinanceStatus.付款单付款方式.现金)
                        {
                            var warehouse = WhWarehouseBo.Instance.GetWarehouse(pItem.PaymentToSysNo);//地区仓库
                            var oraganization = Hyt.BLL.Basic.OrganizationBo.Instance.GetOrganization(pItem.PaymentToSysNo);
                            if (warehouse != null)
                            {
                                easNum = warehouse.ErpCode;
                            }
                            else
                            {
                                easNum = string.Empty;
                            }
                            if (oraganization != null)
                            {
                                organizationCode = oraganization.Code;
                            }
                            //付款科目
                            var km = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetFnReceiptTitleAssociation(pItem.PaymentToSysNo, Hyt.Model.SystemPredefined.PaymentType.现金).OrderByDescending(m => m.IsDefault).FirstOrDefault();
                            lst.Add(new Extra.Erp.Model.Receiving.ReceivingInfo()
                            {
                                Amount = pItem.Amount,
                                OrderSysNo = orderSysNo,
                                WarehouseNumber = easNum,
                                WarehouseSysNo = pItem.PaymentToSysNo,
                                PayeeAccount = km == null ? string.Empty : km.EasReceiptCode,
                                PayeeAccountBank = payeeAccountBank,
                                SettlementType = settlementType,
                                OrganizationCode = organizationCode,
                            });
                        }
                        #endregion
                    }
                }
                if (lst.Count > 0)
                {
                    try
                    {
                        SoOrder orderinfo = null;
                        if (!string.IsNullOrEmpty(orderSysNo))
                        {
                            orderinfo = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(int.Parse(orderSysNo));

                        }
                        Extra.Erp.EasProviderFactory.CreateProvider().Payment(lst, customer, false, orderSysNo, orderinfo == null ? string.Empty : orderinfo.TransactionSysNo);
                    }
                    catch (Exception ex)
                    {
                        Hyt.BLL.Log.SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "部分签收写EAS",
                                         LogStatus.系统日志目标类型.EAS, pay.SysNo, ex, string.Empty, operatorSysNo);
                    }

                }
            }

        }
        #endregion

        #region 加盟商对账
        /// <summary>
        /// 获取加盟商对账数据
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="source">来源</param>
        /// <param name="user">用户</param>
        ///  <param name="remark">备注</param>
        /// <returns></returns>
        /// <remarks>2014-8-21 朱成果 创建</remarks>
        public List<FnThirdpartyReconciliation> GetReconciliationExcelData(Stream stream,int source,SyUser user,string remark)
        {
            List<FnThirdpartyReconciliation> lstdata = new List<FnThirdpartyReconciliation>() ;
            if (source == (int)Model.WorkflowStatus.FinanceStatus.第三方财务对账来源.支付宝)
            {
                #region 支付宝数据账单
                FnThirdpartyReconciliation item = null;
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheetAt(1);
                if (sheet != null)
                {
                    for (int i = 0; i <= sheet.LastRowNum; i++)
                    {
                        var row = sheet.GetRow(i);
                        if (row != null && row.Cells.Count > 10 && row.GetCell(10).StringCellValue == "交易付款")
                        {
                            item = new FnThirdpartyReconciliation();
                            item.FnNo = row.GetCell(0).StringCellValue.Replace("\t", "");//财务流水号
                            item.OperationNo = row.GetCell(1).StringCellValue.Replace("\t", "");//业务流水号
                            item.TraderNo = row.GetCell(2).StringCellValue.Replace("\t", "");//订单号
                            item.ProductName = row.GetCell(3).StringCellValue.Replace("\t", "");//商品名称
                            item.TradeDate = row.GetCell(4).DateCellValue;//发生时间
                            item.BuyerAccount = row.GetCell(5).StringCellValue.Replace("\t", "");//对方账号
                            item.Amount = (decimal)row.GetCell(6).NumericCellValue;//金额
                            item.Source = source;
                            item.CreatedBy = user.SysNo;
                            item.CreatedDate = DateTime.Now;
                            item.Remarks = remark;
                            item.Status = (int)Model.WorkflowStatus.FinanceStatus.第三方财务对账状态.待对账.GetHashCode();
                            if (item.Amount > 0)
                            {
                                lstdata.Add(item);
                            }
                        }
                    }
                }
                #endregion
            }
            return lstdata;
        }

       
        /// <summary>
        /// 添加加盟商对账
        /// </summary>
        /// <param name="item">对账数据</param>
        /// <returns></returns>
        /// <remarks>2014-8-21 朱成果 创建</remarks>
        public int InsertReconciliation(FnThirdpartyReconciliation item)
        {
            lock (obj)//防止并发执行插入
            {
                return IFnThirdpartyReconciliationDao.Instance.Insert(item);
            }
        }

        /// <summary>
        /// 查询对账数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-8-21 朱成果 创建</remarks>
        public Pager<FnThirdpartyReconciliation> QueryReconciliation(Model.Parameter.ParaReconciliationFilter filter)
        {
            return IFnThirdpartyReconciliationDao.Instance.Query(filter);
        }

        /// <summary>
        ///  加盟商对账
        /// </summary>
        /// <param name="lst">列表</param>
        /// <remarks>2014-8-21 朱成果 创建</remarks>
        public void CheckReconciliation(FnThirdpartyReconciliation item)
        {
            if(item.SysNo>0)
            {
                if (item.Source == (int)Model.WorkflowStatus.FinanceStatus.第三方财务对账来源.支付宝)
                {
                    item.TraderNo = item.TraderNo.Replace("T200P", "");
                    item.Remarks = string.Format("【加盟商对账确认，财务流水号({0})】", item.FnNo);
                    IFnThirdpartyReconciliationDao.Instance.CheckAlipayReconciliation(item);
                }
            }
        }

       /// <summary>
       /// 加盟商批量对账
       /// </summary>
       /// <param name="pageindex">当前页</param>
       /// <param name="pagsize">每页条数</param>
        /// <remarks>2014-8-21 朱成果 创建</remarks>
       public void CheckMultiReconciliation(int pageindex,int pagsize)
        {
            Model.Parameter.ParaReconciliationFilter filter = new ParaReconciliationFilter();
            filter.Id = pageindex;
            filter.PageSize = pagsize;
            filter.Status = Model.WorkflowStatus.FinanceStatus.第三方财务对账状态.待对账.GetHashCode();
            var lst = QueryReconciliation(filter);
            if(lst.Rows!=null)
            {
                foreach(var item in lst.Rows)
                {
                    CheckReconciliation(item);
                }
            }
        }
        #endregion 
       /// <summary>
       /// 获取订单的在线支付记录
       /// </summary>
       /// <param name="orderSysNo"></param>
       /// <returns>sysNo</returns>
       /// <remarks>2014-05-13 何方 创建</remarks>
       public IList<FnOnlinePayment> GetOnlinePaymentList(int orderSysNo)
       {
           return IFnOnlinePaymentDao.Instance.Get(orderSysNo);
       }

       //避免并发同步造成重复处理
       private static readonly object Lockobj = new object();

       /// <summary>
       /// 在线支付更新订单支付状态
       /// </summary>
       /// <param name="onlinePayment">支付内容</param>
       /// <param name="payType">支付方式 Grand.Model.SystemPredefined.PaymentType</param>
       /// <returns>网上支付单据编号</returns>
       /// <remarks>2013-11-29 黄波 创建</remarks>
       /// <remarks>2016-3-15 刘伟豪 修改 支付后减库存</remarks>
       public bool UpdateOrderPayStatus(FnOnlinePayment onlinePayment, int payType)
       {
           //此段代码为了兼容二期订单号   兼容完成后删除
           //if (onlinePayment.SourceSysNo.ToString().Length == 10)
           //    onlinePayment.SourceSysNo = int.Parse(onlinePayment.SourceSysNo.ToString().Remove(0, 4));

           //避免并发同步造成重复处理
           lock (Lockobj)
           {

               var order = SoOrderBo.Instance.GetEntity(onlinePayment.SourceSysNo);

               if (order == null)
               {
                   Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "找不到订单" + onlinePayment.SourceSysNo, LogStatus.系统日志目标类型.网上支付, onlinePayment.SourceSysNo, "", 0);
                   return false;
               }
               if (order.PayStatus == (int)OrderStatus.销售单支付状态.已支付)
               {
                   #region 如果订单已经支付而且存在但同步在线支付单据号则返回true
                   var onlinePaymentList = FinanceBo.Instance.GetOnlinePaymentList(order.SysNo);
                   if (onlinePaymentList.Any(x => x.VoucherNo == onlinePayment.VoucherNo))
                   {
                       return true;
                   }
                   else
                   {
                       Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                           "订单" + onlinePayment.SourceSysNo + "已支付,却找不到单据号为" + onlinePayment.VoucherNo + "的在线支付记录",
                           LogStatus.系统日志目标类型.网上支付, onlinePayment.SourceSysNo, "", 0);
                       return false;
                   }
                   #endregion
               }
               if (order.PayStatus == (int)OrderStatus.销售单支付状态.未支付)
               {
                   try
                   {
                       using (var tran = new TransactionScope())
                       {
                           CreateOnlinePaymentFromSoOrder(order, onlinePayment);
                           SoOrderBo.Instance.UpdateOrderPayType(onlinePayment.SourceSysNo, payType);

                           //减库存 2016-3-15 刘伟豪
                           //减库存标识：1-支付后减库存，0-出库后减库存
                           if (BLL.Config.Config.Instance.GetGeneralConfig().ReducedInventory == 1)
                           {
                               var orderItems = BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(order.SysNo);
                               foreach (var item in orderItems)
                               {
                                   BLL.Warehouse.PdProductStockBo.Instance.UpdateStockQuantity(order.DefaultWarehouseSysNo, item.ProductSysNo, item.Quantity);
                               }
                           }

                           tran.Complete();
                       }

                       if (BLL.Config.Config.Instance.GetGeneralConfig().IsSellBusinessClose == 0)
                       {
                           //进入订单分销提成
                           //BLL.CRM.CrCustomerBo.Instance.ExecuteSellBusinessRebates(order);
                       }

                       //付款送积分
                       //Grand.BLL.LevelPoint.PointBo.Instance.OrderIncreasePoint(order.CustomerSysNo, order.SysNo, (int)order.CashPay, order.TransactionSysNo);

                       //if (BLL.Config.Config.Instance.GetGeneralConfig().IsSendMessageToCustomerAfterPay == 1)
                       //{
                       //    //支付成功发送短信给买家
                       //    //2016-2-20 10:15 刘伟豪 创建
                       //    var customer = BLL.CRM.CrCustomerBo.Instance.GetModel(order.OrderCreatorSysNo);
                       //    var dealer = BLL.Stores.StoresBo.Instance.GetStoreById(order.DealerSysNo);
                       //    var customerName = string.IsNullOrWhiteSpace(customer.NickName) ? customer.Name : customer.NickName;
                       //    if (customerName == "未关注")
                       //        customerName = customer.Account;

                       //    var code = customerName;
                       //    var key = "买家付款后收到的短信";
                       //    var sign = dealer.ErpName;
                       //    //BLL.Web.CrCustomerBo.Instance.SendTemplateSms(customer.MobilePhoneNumber, code, key, sign);
                       //}

                       return true;
                   }
                   catch (Exception ex)
                   {
                       Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message,
                           LogStatus.系统日志目标类型.网上支付, onlinePayment.SourceSysNo, ex, "", 0);
                       return false;
                   }
               }

               Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台,
                   "在线支付失败,原因未知,订单状态为" + (OrderStatus.销售单支付状态)order.PayStatus,
                   LogStatus.系统日志目标类型.网上支付, onlinePayment.SourceSysNo, "", 0);
               return false;
           }
       }

       #region 操作 (财务管理)

       /// <summary>
       /// 添加网上支付(单据来源：订单)
       /// </summary>
       /// <param name="onlinePayment">数据实体</param>
       /// <returns>sysNo</returns>
       /// <remarks>2013-07-18 朱家宏 创建</remarks>
       /// <remarks>2013-11-1 黄志勇 修改</remarks>
       public int CreateOnlinePaymentFromSoOrder(SoOrder soOrder, FnOnlinePayment onlinePayment)
       {
           /*
            * 1.支付方式类型为预付&&未付款的订单
            * 2.修改订单相关：支付状态改为已支付 订单状态如果为待支付改为待创建出库单
            * 3.根据收款单单据来源和单据来源编号，创建收款单明细
            */

           if (onlinePayment == null)
               throw new ArgumentNullException("onlinePayment");

           onlinePayment.Source = (int)FinanceStatus.网上支付单据来源.销售单;
           onlinePayment.Status = (int)FinanceStatus.网上支付状态.有效;

           //var soOrder = SoOrderBo.Instance.GetEntity(onlinePayment.SourceSysNo);
           var payStatus = soOrder.PayStatus;
           var paymentType = Basic.PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo).PaymentType;

           //创建网上支付记录
           var r = 0;
           if (paymentType == (int)BasicStatus.支付方式类型.预付 &&
               payStatus == (int)OrderStatus.销售单支付状态.未支付)
           {
               r = IFnOnlinePaymentDao.Instance.Insert(onlinePayment);
           }

           if (r == 0) return r;


           var userName = Sys.SyUserBo.Instance.GetUserName(onlinePayment.CreatedBy); //创建人姓名
           SoOrderBo.Instance.WriteSoTransactionLog(soOrder.TransactionSysNo,
               string.Format(Constant.ORDER_TRANSACTIONLOG_PAY,
                   Util.FormatUtil.FormatCurrency(
                       onlinePayment.Amount, 2)),
               userName);
           //创建收款单明细
           var receiptVoucherItem = new FnReceiptVoucherItem
           {
               Amount = onlinePayment.Amount,
               CreatedBy = onlinePayment.CreatedBy,
               LastUpdateBy = onlinePayment.CreatedBy,
               VoucherNo = onlinePayment.VoucherNo,
               PaymentTypeSysNo = soOrder.PayTypeSysNo,
               TransactionSysNo = soOrder.TransactionSysNo,
               CreatedDate = DateTime.Now,
               LastUpdateDate = DateTime.Now,
               Status = (int)FinanceStatus.收款单明细状态.有效,
               ReceivablesSideType = (int)FinanceStatus.收款方类型.财务中心,

           };
           //插入收款单,收款明细，
           FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(soOrder, receiptVoucherItem);
           //同步支付时间的到订单主表
           ISoOrderDao.Instance.UpdateOrderPayDteById(soOrder.SysNo);
           return r;
       }

       #endregion

       /// <summary>
       /// 获取在线支付订单合集
       /// </summary>
       /// <param name="SysNos"></param>
       /// <returns></returns>
       public List<CBFnOnlinePayment> GetOnlinePaymentList(string SysNos)
       {
           return IFnOnlinePaymentDao.Instance.GetOnlinePaymentList(SysNos);
       }
    }
}
