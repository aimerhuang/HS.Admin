using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.Erp.Model;
using Hyt.BLL.Basic;
using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Finance;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;
using Hyt.Util;
using Hyt.DataAccess.Order;

namespace Hyt.BLL.Finance
{
    /// <summary>
    /// 收款单
    /// </summary>
    /// <remarks>2013-07-08 朱成果 创建</remarks>
    public class FnReceiptVoucherBo : BOBase<FnReceiptVoucherBo>
    {

        /// <summary>
        /// 收款单来源：订单
        /// </summary>
        private int sourceFromOrder = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.销售单;

        #region 订单收款单

        /// <summary>
        /// 添加收款单明细
        /// </summary>
        /// <param name="model">收款明细</param>
        /// <returns>收款单实收金额</returns>
        /// <remarks>2014-02-13 黄志勇 创建</remarks>
        public decimal AddReceiptVoucherItem(FnReceiptVoucherItem model)
        {
            var items = new List<FnReceiptVoucherItem> { model };
            CreateReceiptVoucherItem(items);
            //获取收款单实收金额
            decimal total = GetReceiveAmount(model.ReceiptVoucherSysNo);
            //修改收款单实收金额
            var receiptVoucher = GetReceiptVoucher(model.ReceiptVoucherSysNo);
            receiptVoucher.ReceivedAmount = total;
            Update(receiptVoucher);
            return total;
        }

        /// <summary>
        /// 获取收款单实收金额
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <returns>收款单实收金额</returns>
        /// <remarks>2014-02-13 黄志勇 创建</remarks>
        public decimal GetReceiveAmount(int sysNo)
        {
            var list = GetReceiptVoucherItem(sysNo);
            decimal total = 0;
            list.ForEach(x => total += x.Amount);
            return total;
        }

        /// <summary>
        /// 获取收款单(不包括明细)
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <returns>收款单实体</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        public CBFnReceiptVoucher GetReceiptVoucher(int sysNo)
        {
            var model = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sysNo);
            return model;
        }

        /// <summary>
        /// 获取收款单
        /// </summary>
        /// <param name="source">收款单来源</param>
        /// <param name="sourceSysNo">收款单编号</param>
        /// <returns>收款单实体</returns>
        /// <remarks>2016-06-27 王耀发 创建</remarks>
        public FnReceiptVoucher GetEntity(int source, int sourceSysNo)
        {
            var model = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(source, sourceSysNo);
            return model;
        }

        /// <summary>
        /// 获取收款单明细
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <returns>收款单实体列表</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        public List<CBFnReceiptVoucherItem> GetReceiptVoucherItem(int sysNo)
        {
            return Hyt.DataAccess.Finance.IFnReceiptVoucherItemDao.Instance.GetVoucherItems(sysNo);
        }

        /// <summary>
        /// 创建订单收款单
        /// </summary>
        /// <param name="soOrder">订单</param>
        /// <returns>收款单实体</returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        public FnReceiptVoucher CreateReceiptVoucherByOrder(SoOrder soOrder)
        {
            var entity = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sourceFromOrder, soOrder.SysNo);
            if (entity != null) return null;
            var payType = Hyt.BLL.Basic.PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo);
            FnReceiptVoucher rv = new FnReceiptVoucher()
            {
                TransactionSysNo = soOrder.TransactionSysNo,
                Status = (int)FinanceStatus.收款单状态.待确认,
                Source = sourceFromOrder,
                CreatedBy = soOrder.OrderCreatorSysNo,
                CreatedDate = DateTime.Now,
                LastUpdateBy = soOrder.OrderCreatorSysNo,
                LastUpdateDate = DateTime.Now,
                IncomeAmount = soOrder.CashPay,
                IncomeType = payType==null?20:payType.PaymentType,
                SourceSysNo = soOrder.SysNo
            };
            rv.SysNo = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Insert(rv);
            return rv;
        }

        /// <summary>
        /// 更新订单收款单应收金额
        /// </summary>
        /// <param name="orderID">订单号</param>
        /// <param name="incomeAmount">应收金额</param>
        /// <returns></returns>
        ///<remarks>2013-07-08 朱成果 创建
        /// 2014-07-11 余勇 修改 检查收款单支付类型与订单支付类型是否一致，如不同则修改收款单支付类型为订单支付类型
        /// </remarks>
        public void UpdateOrderIncomeAmount(int orderID, decimal incomeAmount)
        {
            var entity = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sourceFromOrder, orderID);
            //根据订单支付方式取得支付类型
            var order = SoOrderBo.Instance.GetEntity(orderID);
            if (entity == null) //不存在收款单创建一条记录
            {
                entity = CreateReceiptVoucherByOrder(order);
            }
            var payType = PaymentTypeBo.Instance.GetEntity(order.PayTypeSysNo);
            //如果收款单支付类型与订单支付类型不符，则修改收款单支付类型为订单支付类型 余勇 2014-07-11
            if (payType != null && entity.IncomeType != payType.PaymentType)
            {
                entity.IncomeType = payType.PaymentType;
            }
            entity.IncomeAmount = incomeAmount;
            entity.LastUpdateDate = DateTime.Now;

            Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(entity);
        }

        /// <summary>
        /// 更新订单收款单状态(只更新数据不进行业务逻辑处理)
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        ///<remarks>2013-07-08 朱成果 创建</remarks>
        public void UpdateOrderReceiptStatus(int orderSysNo, FinanceStatus.收款单状态 status)
        {
            var entity = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sourceFromOrder, orderSysNo);
            if (entity == null) //不存在收款单创建一条记录
            {
                entity = CreateReceiptVoucherByOrder(SoOrderBo.Instance.GetEntity(orderSysNo));
            }
            entity.Status = (int)status;
            entity.LastUpdateDate = DateTime.Now;
            Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(entity);
        }

        /// <summary>
        /// 作废收款单
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <param name="syUser">操作用户</param>
        /// <returns></returns>
        ///<remarks>2013-07-08 朱成果 创建</remarks>
        public void CancelReceiptVoucher(int sysNo, SyUser user)
        {
            var entity = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sysNo);
            if (entity == null)
            {
                throw new Exception("该收款单不存在");
            }

            if (entity.Status != (int)FinanceStatus.收款单状态.作废 && entity.ReceivedAmount > 0)
            {
                SoOrder order = SoOrderBo.Instance.GetEntity(entity.SourceSysNo);
                if (order != null)
                {
                    var orderItems =
                   Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo).ToList();
                    if (orderItems.Exists(o => o.RealStockOutQuantity > 0))
                    {
                        throw new Exception("该订单存在已经出库的商品不能作废");
                    }
                    FnPaymentVoucher pv = new FnPaymentVoucher()
                    {
                        PayableAmount = entity.ReceivedAmount,
                        CreatedBy = user == null ? 0 : user.SysNo,
                        CreatedDate = DateTime.Now,
                        CustomerSysNo = order.CustomerSysNo,
                        Source = (int)FinanceStatus.付款来源类型.销售单,
                        TransactionSysNo = entity.TransactionSysNo,
                        SourceSysNo = order.SysNo,
                        Status = (int)FinanceStatus.付款单状态.待付款,
                        Remarks = "订单作废"
                    };
                    //保存付款单
                    pv.SysNo = Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.Insert(pv);
                    var list = Hyt.DataAccess.Finance.IFnReceiptVoucherItemDao.Instance.GetListByReceiptNo(entity.SysNo);
                    if (list != null)
                    {
                        foreach (FnReceiptVoucherItem m in list)
                        {
                            FnPaymentVoucherItem p = new FnPaymentVoucherItem()
                            {
                                Amount = m.Amount,
                                CreatedBy = user == null ? 0 : user.SysNo,
                                CreatedDate = DateTime.Now,
                                OriginalVoucherNo = m.VoucherNo,
                                OriginalPaymentTypeSysNo = m.PaymentTypeSysNo,
                                PaymentType = GetPaymentType(m.PaymentTypeSysNo),//支付方式对应付款方式
                                PaymentVoucherSysNo = pv.SysNo,
                                RefundAccount = m.CreditCardNumber,
                                TransactionSysNo = entity.TransactionSysNo,
                                Status = (int)FinanceStatus.付款单明细状态.待付款
                            };
                            //保存付款单明细
                            Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.InsertItem(p);
                        }
                    }
                    var payType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(order.PayTypeSysNo).PaymentType;
                    //修改订单状态为待支付
                    if (payType == (int)BasicStatus.支付方式类型.预付)
                    {
                        order.PayStatus = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.未支付;
                        order.Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待支付;
                        order.OnlineStatus = Constant.OlineStatusType.待支付;
                        SoOrderBo.Instance.UpdateOrder(order); //更新订单 余勇修改为调用业务层方法
                        //Hyt.DataAccess.Order.ISoOrderDao.Instance.Update(order);
                    }
                }
            }
            entity.Status = (int)FinanceStatus.收款单状态.作废;
            entity.LastUpdateDate = DateTime.Now;
            entity.LastUpdateBy = user.SysNo;
            Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(entity);
        }

        /// <summary>
        /// 确认收款单
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <param name="syUser">操作用户</param>
        /// <returns></returns>
        ///<remarks>2013-07-08 朱成果 创建</remarks>
        public void ConfirmReceiptVoucher(int sysNo, SyUser syUser)
        {
            var model = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sysNo);
            model.Status = (int)FinanceStatus.收款单状态.已确认;
            model.ConfirmedBy = syUser.SysNo;
            model.ConfirmedDate = DateTime.Now;
            model.LastUpdateBy = syUser.SysNo;
            model.LastUpdateDate = DateTime.Now;
            Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(model);
            #region 更新收款单对应订单付款状态 2013/12/12 朱成果 
            if (model != null && model.Source == Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.销售单.GetHashCode())
            {
                if (model.ReceivedAmount >= model.IncomeAmount)//收款完毕
                {
                    var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(model.SourceSysNo);
                    if (order != null && order.PayStatus != Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付.GetHashCode())
                    {
                        Hyt.BLL.Order.SoOrderBo.Instance.UpdatePayStatus(order.SysNo, Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付);
                        if (order != null && order.OnlineStatus == Constant.OlineStatusType.待支付)//更新前台显示状态
                        {
                            switch (order.Status)
                            {
                                case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核:
                                    SoOrderBo.Instance.UpdateOnlineStatusByOrderID(order.SysNo, Constant.OlineStatusType.待审核); //更新订单前台显示状态 余勇修改为调用业务层方法  Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOnlineStatusByOrderID(order.SysNo, Constant.OlineStatusType.待审核);
                                    break;
                                default:
                                    SoOrderBo.Instance.UpdateOnlineStatusByOrderID(order.SysNo, Constant.OlineStatusType.待出库); //更新订单前台显示状态 余勇修改为调用业务层方法 Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOnlineStatusByOrderID(order.SysNo, Constant.OlineStatusType.待出库);
                                    break;
                            }
                        }
                        Hyt.BLL.Log.SysLog.Instance.Warn(LogStatus.系统日志来源.后台, "确认收款单更改订单支付状态", LogStatus.系统日志目标类型.订单, order.SysNo,
                                    syUser.SysNo);
                    }

                    ///判断是否客服下单，针对客服下单而且是付款的，同时配置是支付减库存的，进行库存的增减
                    if (order.OrderSource == (int)OrderStatus.销售单来源.客服下单)
                    {
                           //减库存 2017-1-10 杨云奕
                           //减库存标识：1-支付后减库存，0-出库后减库存
                           if (BLL.Config.Config.Instance.GetGeneralConfig().ReducedInventory == 1)
                           {
                               var orderItems = BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(order.SysNo);
                               foreach (var item in orderItems)
                               {
                                   BLL.Warehouse.PdProductStockBo.Instance.UpdateStockQuantity(order.DefaultWarehouseSysNo, item.ProductSysNo, item.Quantity);
                               }
                           }
                        
                    }
                }
            }
            #endregion

        }

        /// <summary>
        /// 取消确认收款单
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <param name="syUser">操作用户</param>
        /// <returns></returns>
        ///<remarks>2013-07-08 朱成果 创建</remarks>
        public void CancelConfirmReceiptVoucher(int sysNo, SyUser syUser)
        {
            var model = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sysNo);
            model.Status = (int)FinanceStatus.收款单状态.待确认;
            model.LastUpdateBy = syUser.SysNo;
            model.LastUpdateDate = DateTime.Now;
            Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(model);
        }

        /// <summary>
        /// 修改收款单
        /// </summary>
        /// <param name="model">收款单实体</param>
        /// <returns></returns>
        ///<remarks>2013-07-08 朱成果 创建</remarks>
        public void Update(FnReceiptVoucher model)
        {
            Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(model);
        }

        /// <summary>
        /// 删除收款单明细
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <returns></returns>
        ///<remarks>2013-07-08 朱成果 创建</remarks>
        public void DeleteReceiptVoucherItem(int sysNo)
        {
            Hyt.DataAccess.Finance.IFnReceiptVoucherItemDao.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 作废收款单明细
        /// </summary>
        /// <param name="sysNo">收款明细编号</param>
        /// <returns></returns>
        ///<remarks>2013-07-08 朱成果 创建</remarks>
        public int InvalidReceiptVoucherItem(int sysNo)
        {
            var changedStatus = (int)FinanceStatus.收款单明细状态.无效;
            return Hyt.DataAccess.Finance.IFnReceiptVoucherItemDao.Instance.Invalid(sysNo, changedStatus);
        }

        /// <summary>
        /// 作废订单收款单(已付款的生成退款信息)
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="user">操作用户</param>
        /// <param name="autoPay">自动付款</param>
        /// <returns></returns>
        ///<remarks>2013-07-17 朱成果 创建</remarks>
        public void CancelOrderReceipt(SoOrder order, SyUser user,bool autoPay=false)
        {
            if (order != null)
            {
                var entity = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sourceFromOrder, order.SysNo);
                if (entity != null && entity.Status != (int)FinanceStatus.收款单状态.作废)
                {
                    //已收款，生成新的付款单,否则作废收款单
                    if (entity.ReceivedAmount > 0)
                    {
                        FnPaymentVoucher pv = new FnPaymentVoucher()
                        {
                            PayableAmount = entity.ReceivedAmount,
                            CreatedBy = user == null ? 0 : user.SysNo,
                            CreatedDate = DateTime.Now,
                            CustomerSysNo = order.CustomerSysNo,
                            Source = (int)FinanceStatus.付款来源类型.销售单,
                            TransactionSysNo = entity.TransactionSysNo,
                            SourceSysNo = order.SysNo,
                            Status =autoPay?(int)FinanceStatus.付款单状态.已付款:(int)FinanceStatus.付款单状态.待付款,
                            Remarks = "订单作废",
                            PayDate=(DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            LastUpdateDate=DateTime.Now
                        };
                        pv.SysNo = Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.Insert(pv);
                        var list = Hyt.DataAccess.Finance.IFnReceiptVoucherItemDao.Instance.GetListByReceiptNo(entity.SysNo);
                        if (list != null)
                        {
                            foreach (FnReceiptVoucherItem m in list)
                            {
                                FnPaymentVoucherItem p = new FnPaymentVoucherItem()
                                {
                                    Amount = m.Amount,
                                    CreatedBy = user == null ? 0 : user.SysNo,
                                    CreatedDate = DateTime.Now,
                                    OriginalVoucherNo = m.VoucherNo,
                                    OriginalPaymentTypeSysNo = m.PaymentTypeSysNo,
                                    PaymentType = GetPaymentType(m.PaymentTypeSysNo),//支付方式对应付款方式
                                    PaymentVoucherSysNo = pv.SysNo,
                                    RefundAccount = m.CreditCardNumber,
                                    TransactionSysNo = entity.TransactionSysNo,
                                    Status = autoPay? (int)FinanceStatus.付款单明细状态.已付款 : (int)FinanceStatus.付款单明细状态.待付款,
                                     PayDate=(DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                     LastUpdateDate=DateTime.Now
                                   
                                };
                                Hyt.DataAccess.Finance.IFnPaymentVoucherDao.Instance.InsertItem(p);
                            }
                        }
                    }
                    else
                    {
                        entity.Remark = "订单作废";
                        entity.Status = (int)FinanceStatus.收款单状态.作废;
                        entity.LastUpdateDate = DateTime.Now;
                        entity.LastUpdateBy = user == null ? 0 : user.SysNo;
                        Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(entity);
                    }
                }
            }
        }

        /// <summary>
        /// 收款方式对应付款方式
        /// </summary>
        /// <param name="OriginalPaymentTypeSysNo">原收款方式</param>
        /// <returns>付款单付款方式</returns>
        ///<remarks>2013-07-08 朱成果 创建</remarks>
        private int GetPaymentType(int OriginalPaymentTypeSysNo)
        {
            if (OriginalPaymentTypeSysNo == PaymentType.现金)
            {
                return (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.现金;
            }
            else if (OriginalPaymentTypeSysNo == PaymentType.刷卡)
            {
                return (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.转账;
            }
            else if (OriginalPaymentTypeSysNo == PaymentType.网银)
            {
                return (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.网银;
            }
            else if (OriginalPaymentTypeSysNo == PaymentType.支付宝)
            {
                return (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.支付宝;
            }
            else if (OriginalPaymentTypeSysNo == PaymentType.支票)
            {
                return (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.支票;
            }
            else if (OriginalPaymentTypeSysNo == PaymentType.转账)
            {
                return (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.转账;
            }
            else if (OriginalPaymentTypeSysNo == PaymentType.分销商预存)
            {
                return (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.分销商预存;
            }
            else
            {
                return (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.转账;//默认就转账吧
            }
        }

        /// <summary>
        /// 门店提货或者转快递，插入收款明细
        /// </summary>
        /// <param name="orderID">订单号</param>
        /// <param name="item">收款明细</param>
        /// <param name="isThrowException">是否抛出异常</param>
        /// <returns></returns>
        ///<remarks>2013-07-08 朱成果 创建</remarks>
        public void InsertOrderReceiptVoucher(int orderID, FnReceiptVoucherItem item, bool isThrowException = true)
        {
            //收款单
            var order = SoOrderBo.Instance.GetEntity(orderID);
            var entity = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sourceFromOrder, orderID);
            if (entity == null) //不存在收款单创建一条记录
            {
                entity = CreateReceiptVoucherByOrder(order);
            }
            item.ReceiptVoucherSysNo = entity.SysNo;//收款单编号
            item.TransactionSysNo = entity.TransactionSysNo;
            item.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效;
            entity.ReceivedAmount += item.Amount;
            if (entity.ReceivedAmount > entity.IncomeAmount)
            {
                Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "收款金额超过了应收款金额", LogStatus.系统日志目标类型.收款单明细, item.SysNo,item.CreatedBy);
                //在线付款不抛出异常
                if (isThrowException)
                {
                    throw new Exception("收款金额超过了应收款金额");
                }
            }
            if ((int)entity.ReceivedAmount >= (int)entity.IncomeAmount)//更新订单的支付状态
            {
                SoOrderBo.Instance.UpdatePayStatus(orderID, Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付); //更新订单支付状态 余勇修改为调用业务层方法
                if (order!=null&&order.OnlineStatus == Constant.OlineStatusType.待支付)//更新前台显示状态
                {
                    switch(order.Status)
                    {
                        case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核:
                            SoOrderBo.Instance.UpdateOnlineStatusByOrderID(order.SysNo, Constant.OlineStatusType.待审核); //更新订单前台显示状态 余勇修改为调用业务层方法  Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOnlineStatusByOrderID(orderID, Constant.OlineStatusType.待审核);
                            break;
                        default:
                            SoOrderBo.Instance.UpdateOnlineStatusByOrderID(order.SysNo, Constant.OlineStatusType.待出库); //更新订单前台显示状态 余勇修改为调用业务层方法  Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOnlineStatusByOrderID(orderID, Constant.OlineStatusType.待出库);
                            break;
                    }
                }
                //entity.Status = (int)FinanceStatus.收款单状态.已确认;
            }
            
            var itemSysNo= Hyt.DataAccess.Finance.IFnReceiptVoucherItemDao.Instance.Insert(item);
            Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(entity);
            //同步支付时间的到订单主表
            ISoOrderDao.Instance.UpdateOrderPayDteById(orderID);
            if (string.IsNullOrEmpty(item.EasReceiptCode))
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Warn, LogStatus.系统日志来源.后台, "插入收款明细FnReceiptVoucherItem时EAS收款科目为空，收款单号:" + item.ReceiptVoucherSysNo,
                                       LogStatus.系统日志目标类型.收款单, itemSysNo, null, string.Empty, 0);
            }
        }

        /// <summary>
        /// 添加收款单明细
        /// </summary>
        /// <param name="items">收款单明细.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/11 何方 创建
        /// </remarks>
        public void CreateReceiptVoucherItem(IList<FnReceiptVoucherItem> items)
        {
            foreach (var item in items)
            {
                item.SysNo = Hyt.DataAccess.Finance.IFnReceiptVoucherItemDao.Instance.Insert(item);
            }

        }

        /// <summary>
        /// 获取收款单
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>收款单实体</returns>
        /// <remarks>2013-07-30 黄伟 创建</remarks>
        public FnReceiptVoucher GetReceiptVoucherByOrder(int orderSysNo)
        {
            return IFnReceiptVoucherDao.Instance.GetReceiptVoucherByOrder(orderSysNo);
        }

        #endregion

        #region 收款科目关联

        /// <summary>
        /// 获取收款科目关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="paymentTypeSysNo">支付方式编号</param>
        /// <returns>收款科目关联列表</returns>
        /// <remarks>
        /// 2013/11/11 朱成果 创建
        /// </remarks>
        public List<FnReceiptTitleAssociation> GetFnReceiptTitleAssociation(int warehouseSysNo, int paymentTypeSysNo)
        {
            return IFnReceiptTitleAssociationDao.Instance.GetList(warehouseSysNo, paymentTypeSysNo);

        }
        #endregion

        #region 收款单确认写EAS数据

        /// <summary>
        /// 已确认收款单写EAS数据（收款单必须为已确认状态才执行）
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>2013-11-28 朱成果 创建</remarks>
        /// <remarks>2013-11-28 吴文强 修改</remarks>
        public void WriteEasReceiptVoucher(int sysNo, SyUser user)
        {
            #region 写EAS数据 2013-11-16 朱成果
            CBFnReceiptVoucher receiptVoucher = FnReceiptVoucherBo.Instance.GetReceiptVoucher(sysNo);
            if (receiptVoucher == null || receiptVoucher.Status != FinanceStatus.收款单状态.已确认.GetHashCode())
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Warn, LogStatus.系统日志来源.后台, "写EAS数据时候，收款单不存在或未确认收款单状态：" + receiptVoucher.Status,
                                         LogStatus.系统日志目标类型.收款单, sysNo, null, string.Empty, user.SysNo);
                return;
            }
            receiptVoucher.VoucherItems = Instance.GetReceiptVoucherItem(receiptVoucher.SysNo);
            if (receiptVoucher.Source == (int)FinanceStatus.收款来源类型.销售单 && receiptVoucher.VoucherItems != null && receiptVoucher.VoucherItems.Count > 0)
            {
                var lst = new List<Extra.Erp.Model.Receiving.ReceivingInfo>();
                var lstFreight = new List<Extra.Erp.Model.Receiving.ReceivingInfo>();
                var customer = WhWarehouseBo.Instance.GetErpCustomerCode(receiptVoucher.SourceSysNo);
                var order = SoOrderBo.Instance.GetEntity(receiptVoucher.SourceSysNo);//收款单对应的订单
                //是否为RMA换货下单
                bool isRma = order.OrderSource == (int)Model.WorkflowStatus.OrderStatus.销售单来源.RMA下单;
                if (isRma)
                    return;
                //订单实际运费金额
                var freightAmount = order.GetFreight().RealFreightAmount;

                foreach (var pItem in receiptVoucher.VoucherItems)
                {
                    #region 收款单
                    if (pItem.Status == (int)FinanceStatus.收款单明细状态.有效 && pItem.Amount > 0)
                    {
                        int warehouseSysNo = 0;//商城仓库系统号
                        string easNum =string.Empty;//Eas仓库编码
                        string organizationCode = string.Empty;//组织机构代码
                        string payeeAccount = string.Empty;//收款科目
                        string payeeAccountBank = string.Empty;//收款账户
                        string settlementType = Extra.Erp.Model.EasConstant.SettlementType_Cash;//仓库只有现金，01:Eas中的现金
                        string remark = string.Empty;
                        //是否是PC网站在线支付订单
                        bool isOnline = false;
                        if (pItem.ReceivablesSideType == (int)FinanceStatus.收款方类型.仓库)
                        {
                            warehouseSysNo = pItem.ReceivablesSideSysNo;
                            var warehouse = WhWarehouseBo.Instance.GetWarehouse(warehouseSysNo);//地区仓库
                            var oraganization = OrganizationBo.Instance.GetOrganization(warehouseSysNo);
                            
                            if (warehouse != null)
                            {
                                easNum = warehouse.ErpCode;
                            }
                            if (oraganization != null)
                            {
                                organizationCode = oraganization.Code;
                            }
                        }

                        if (pItem.ReceivablesSideType == (int)FinanceStatus.收款方类型.财务中心)
                        {
                            //start:如果收款单来自于商城并且为在线支付，那么eas人员就默认为第一个发货仓库 杨浩 添加
                            warehouseSysNo = pItem.ReceivablesSideSysNo;
                            if (warehouseSysNo == 0
                                && order.OrderSource != (int)OrderStatus.销售单来源.分销商升舱
                                && (order.PayTypeSysNo == PaymentType.网银 || order.PayTypeSysNo == PaymentType.支付宝 || order.PayTypeSysNo == PaymentType.微信支付 || order.PayTypeSysNo == PaymentType.易宝支付))
                            {
                                var wh = WhWarehouseBo.Instance.GetWhStockOutListByOrderID(order.SysNo, true);
                                if (wh != null && wh.Count > 0)
                                {
                                    if (wh.Count > 1)
                                        remark = "此收款单为分批出库";
                                    warehouseSysNo = wh.FirstOrDefault().WarehouseSysNo;
                                }
                                else
                                    remark = "订单还未结算";
                            }
                            if (warehouseSysNo > 0)
                            {
                                var warehouse = WhWarehouseBo.Instance.GetWarehouse(warehouseSysNo); //地区仓库
                                var oraganization = OrganizationBo.Instance.GetOrganization(warehouseSysNo);
                                if (warehouse != null)
                                {
                                    easNum = warehouse.ErpCode;
                                }
                                if (oraganization != null)
                                {
                                    organizationCode = oraganization.Code;
                                }
                            }
                            //end
                            if (PaymentType.支付宝.Equals(pItem.PaymentTypeSysNo))
                            {
                                payeeAccount = Extra.Erp.Model.EasConstant.PayeeAccount;
                                payeeAccountBank = Extra.Erp.Model.EasConstant.PayeeAccountBank_Alipay;
                                settlementType = Extra.Erp.Model.EasConstant.SettlementType_Alipay;
                                isOnline = true;
                            }
                            if (order.PayTypeSysNo == PaymentType.微信支付 || order.PayTypeSysNo == PaymentType.易宝支付)
                            {
                                isOnline = true;
                            }
                            if (PaymentType.网银.Equals(pItem.PaymentTypeSysNo))
                            {
                                payeeAccount = Extra.Erp.Model.EasConstant.PayeeAccount;
                                payeeAccountBank = Extra.Erp.Model.EasConstant.PayeeAccountBank_ChinaBank;
                                settlementType = Extra.Erp.Model.EasConstant.SettlementType_ChinaBank;
                                isOnline = true;
                            }
                        }
                        if (!isOnline && string.IsNullOrEmpty(pItem.EasReceiptCode) && warehouseSysNo>0)
                        {
                            if (pItem.PaymentTypeSysNo == Hyt.Model.SystemPredefined.PaymentType.现金)
                            {
                                //付款科目
                                var km =GetFnReceiptTitleAssociation(warehouseSysNo, PaymentType.现金).OrderByDescending(m => m.IsDefault).FirstOrDefault();
                                if (km != null) pItem.EasReceiptCode = km.EasReceiptCode;
                            }
                            //刷卡方式不导入
                            if (pItem.PaymentTypeSysNo == Hyt.Model.SystemPredefined.PaymentType.刷卡)
                            {
                                return;
                            }
                        }
                        //if (!string.IsNullOrEmpty(easNum))
                        //{
                            //应收系统收款单明细
                            var receiptAmount = lst.Count == 0 ? pItem.Amount - freightAmount : pItem.Amount; //第一条明细减去运费，运费分开导入
                            lst.Add(new Extra.Erp.Model.Receiving.ReceivingInfo()
                            {
                                Amount = receiptAmount,
                                OrderSysNo = receiptVoucher.SourceSysNo.ToString(),
                                WarehouseSysNo = warehouseSysNo,
                                WarehouseNumber = easNum,
                                PayeeAccount = (isOnline) ? payeeAccount : pItem.EasReceiptCode,
                                PayeeAccountBank = payeeAccountBank,
                                SettlementType = settlementType,
                                OrganizationCode = organizationCode,
                                Remark = remark
                            });

                            //存在运费时，添加出纳系统收款单明细
                            if (freightAmount > 0 && lstFreight.Count == 0)
                            {
                                lstFreight.Add(new Extra.Erp.Model.Receiving.ReceivingInfo()
                                {
                                    Amount = freightAmount,
                                    OrderSysNo = receiptVoucher.SourceSysNo.ToString(),
                                    WarehouseSysNo = warehouseSysNo,
                                    WarehouseNumber = easNum,
                                    PayeeAccount = (isOnline) ? payeeAccount : pItem.EasReceiptCode,
                                    PayeeAccountBank = payeeAccountBank,
                                    SettlementType = settlementType,
                                    OrganizationCode = organizationCode,
                                    Remark = remark
                                });
                            //}
                        }
                    }
                    #endregion
                }
                if (lst.Count > 0)
                {
                    Extra.Erp.EasProviderFactory.CreateProvider()
                         .Receiving(lst, 收款单类型.商品收款单, customer, false, receiptVoucher.SourceSysNo.ToString(), order.TransactionSysNo);
                }
                if (lstFreight.Count > 0)
                {
                    Extra.Erp.EasProviderFactory.CreateProvider()
                         .Receiving(lstFreight, 收款单类型.服务收款单, customer, false, receiptVoucher.SourceSysNo.ToString(), order.TransactionSysNo);
                }
            }
            #endregion
        }
        #endregion

        #region 自动确认收款单,收款方为仓库，全部为现金才自动确认
        /// <summary>
        /// 自动确认收款单,收款方为仓库，全部为现金才自动确认
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <param name="user">操作人</param>
        /// <returns>结果</returns>
        /// <remarks>2013-11-28 朱成果 创建</remarks>
        public Hyt.Model.Result AutoConfirmReceiptVoucher(int orderNo, SyUser user)
        {
            Hyt.Model.Result r = new Model.Result() { Status = false };
            var receiptVoucher = IFnReceiptVoucherDao.Instance.GetEntity((int)Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.销售单, orderNo);
            if (receiptVoucher != null)
            {
                if (receiptVoucher.Status == (int)FinanceStatus.收款单状态.已确认)
                {
                    r.Message = "收款单已经确认不用再确认";
                    return r;
                }
                decimal sum = 0;//门店仓库付现
                decimal sum1 = 0;//分销商预存款
                var lsititem=  Instance.GetReceiptVoucherItem(receiptVoucher.SysNo);
                if(lsititem!=null)
                {
                     foreach(var item in lsititem)
                     {
                         if(item.Status==(int)FinanceStatus.收款单明细状态.有效)
                         {
                               //门店或仓库收现自动确认
                               if(item.ReceivablesSideType == (int)FinanceStatus.收款方类型.仓库&&(item.PaymentTypeSysNo == PaymentType.现金 || item.PaymentTypeSysNo == PaymentType.现金预付))
                               {
                                   sum+=item.Amount;//门店仓库付现累加
                               }
                               if (item.PaymentTypeSysNo == PaymentType.分销商预存)
                               {
                                   sum1 += item.Amount;//分销商预存款累加
                               }
                         }
                     }
                }
                if (sum >= receiptVoucher.IncomeAmount)//门店仓库付现累加
                {
                    receiptVoucher.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单状态.已确认;
                    receiptVoucher.ConfirmedBy = user.SysNo;
                    receiptVoucher.ConfirmedDate = DateTime.Now;
                    receiptVoucher.Remark = "【门店或仓库收现自动确认】";
                    Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(receiptVoucher);
                    Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "门店或仓库收现自动确认", LogStatus.系统日志目标类型.收款单, receiptVoucher.SysNo,
                                    user.SysNo);
                    Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.WriteEasReceiptVoucher(receiptVoucher.SysNo, user);
                    r.Status = true;
                }
                else if (sum1 >= receiptVoucher.IncomeAmount)//分销商预存款累加
                {
                    receiptVoucher.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单状态.已确认;
                    receiptVoucher.ConfirmedBy = user.SysNo;
                    receiptVoucher.ConfirmedDate = DateTime.Now;
                    receiptVoucher.Remark = "【分销商升舱自动确认】";
                    Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(receiptVoucher);
                    Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "分销商升舱自动确认", LogStatus.系统日志目标类型.收款单, receiptVoucher.SysNo,
                                    user.SysNo);
                    //升舱订单不导入 2013-12-23 杨浩
                    //Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.WriteEasReceiptVoucher(receiptVoucher.SysNo, user);
                    r.Status = true;

                }
            }
            else
            {
                r.Status = false;
                r.Message = "收款单不存在";
            }
            return r;
        }
        #endregion


        ///  <summary>
        ///  门店提货或者转快递，插入收款明细
        ///  </summary>
        /// <param name="order">订单</param>
        /// <param name="item">收款明细</param>
        ///  <returns></returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        /// <remarks>2014-05-13 何方 重构 使用SoOrder 实体,避免重复查询SoOrder</remarks>
        public void InsertOrderReceiptVoucher(SoOrder order, FnReceiptVoucherItem item)
        {
            //收款单
            //var order = Grand.DataAccess.Order.ISoOrderDao.Instance.GetEntity(orderID);
            var entity = DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sourceFromOrder, order.SysNo);

            if (entity == null) //不存在收款单创建一条记录
            {
                entity = CreateReceiptVoucherByOrder(order);
            }


            item.ReceiptVoucherSysNo = entity.SysNo;//收款单编号
            item.TransactionSysNo = entity.TransactionSysNo;
            item.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效;


            entity.ReceivedAmount += item.Amount;

           SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("已支付金额{0},应付金额{1}", entity.ReceivedAmount, entity.IncomeAmount), LogStatus.系统日志目标类型.订单,
                  order.SysNo, null, WebUtil.GetUserIp(), item.CreatedBy);

            if (entity.ReceivedAmount > entity.IncomeAmount)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("已支付金额{0}超过了应收款金额{1}", entity.ReceivedAmount, entity.IncomeAmount), LogStatus.系统日志目标类型.订单,
                   order.SysNo, item.CreatedBy);

                throw new HytException(string.Format("已支付金额{0}超过了应收款金额{1}", entity.ReceivedAmount, entity.IncomeAmount));

            }
            if (entity.ReceivedAmount == entity.IncomeAmount) //更新订单的支付状态
            {


                DataAccess.Order.ISoOrderDao.Instance.UpdatePayStatus(order.SysNo, (int)Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付);
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "已支付金额等于应付金额,将修改订单支付状态为已付款", LogStatus.系统日志目标类型.订单,
                    order.SysNo, item.CreatedBy);



                if (order != null && order.OnlineStatus == Constant.OlineStatusType.待支付) //更新前台显示状态
                {
                    switch (order.Status)
                    {
                        case (int)Model.WorkflowStatus.OrderStatus.销售单状态.待审核:
                            DataAccess.Order.ISoOrderDao.Instance.UpdateOnlineStatusByOrderID(order.SysNo,
                                Constant.OlineStatusType.待审核);
                            break;
                        default:
                            DataAccess.Order.ISoOrderDao.Instance.UpdateOnlineStatusByOrderID(order.SysNo,
                                Constant.OlineStatusType.待出库);
                            break;
                    }
                }
                //entity.Status = (int)FinanceStatus.收款单状态.已确认;
            }
            else
            {
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "已支付金额不足应付金额,没有修订单支付状态", LogStatus.系统日志目标类型.订单,
                    order.SysNo, null, WebUtil.GetUserIp(), item.CreatedBy);
            }

            var itemSysNo = DataAccess.Finance.IFnReceiptVoucherItemDao.Instance.Insert(item);
            DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(entity);
            //同步支付时间的到订单主表
            ISoOrderDao.Instance.UpdateOrderPayDteById(order.SysNo);
            if (string.IsNullOrEmpty(item.EasReceiptCode))
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Warn, LogStatus.系统日志来源.后台, "插入收款明细FnReceiptVoucherItem时EAS收款科目为空，收款单号:" + item.ReceiptVoucherSysNo,
                                       LogStatus.系统日志目标类型.收款单, itemSysNo, null, string.Empty, 0);
            }
        }
    }
}
