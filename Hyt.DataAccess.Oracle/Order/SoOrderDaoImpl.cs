using System;
using System.Collections;
using Hyt.DataAccess.Order;
using System.Collections.Generic;
using System.Linq;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
using Hyt.Model.Manual;
using System.Reflection;
using Hyt.DataAccess.Distribution;
using Hyt.Model.Order;
using Hyt.Model.DouShabaoModel;
using Hyt.Util.Serialization;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 订单主表信息数据访问实现
    /// </summary>
    /// <remarks>2013-06-13 朱家宏 创建</remarks>
    public class SoOrderDaoImpl : ISoOrderDao
    {

        /// <summary>
        /// 获取精简订单详情（在线支付改变订单状态使用）
        /// </summary>
        /// <param name="sysno">订单编号</param>
        /// <returns></returns>
        /// <remarks>2017-09-27 杨浩 创建</remarks>
        public override SoOrder GetSimplifyOrderInfo(int sysno)
        {
            return Context.Sql("select SysNo,PayStatus,PayTypeSysNo,TransactionSysNo,OnlineStatus,CashPay,OrderCreatorSysNo from soorder where sysno=@0", sysno)
                .QuerySingle<SoOrder>();
        }
        /// <summary>
        /// 更新支付和在线支付状态
        /// </summary>
        /// <param name="sysno">订单号</param>
        /// <param name="payStatus"></param>
        /// <param name="onlineStatus"></param>
        /// <param name="status">订单状态</param>
        /// <param name="customsPayStatus">支付推海关状态</param>
        /// <param name="tradeCheckStatus">支付推国检状态</param>
        /// <param name="payTypeSysNo">支付类型系统编号</param>
        /// <remarks>2017-09-27 杨浩 创建</remarks>
        public override int UpdatePayStatusAndOnlineStatusAndStatus(int sysno, int payStatus, string onlineStatus, int status, int customsPayStatus, int tradeCheckStatus, int payTypeSysNo)
        {
      
            return Context.Sql("update soorder set PayStatus=@PayStatus,onlineStatus=@onlineStatus,status=@status,CustomsPayStatus=@CustomsPayStatus,TradeCheckStatus=@TradeCheckStatus,payTypeSysNo=@payTypeSysNo where SysNo=@SysNo")
                  .Parameter("PayStatus", payStatus)
                  .Parameter("onlineStatus", onlineStatus)
                  .Parameter("status", status)
                  .Parameter("SysNo", sysno)
                  .Parameter("CustomsPayStatus", customsPayStatus)
                  .Parameter("TradeCheckStatus", tradeCheckStatus)
                  .Parameter("payTypeSysNo", payTypeSysNo)
                  .Execute();
        }
        /// <summary>
        /// 根据订单编号获取配送单
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-09-09 杨浩 创建</remarks>
        public override List<LgDeliveryItem> GetDeliveryItem(int sysNo)
        {
            return Context.Sql(@"SELECT di.* FROM LgSettlementItem AS si
								 INNER JOIN [WhStockOut] AS wout ON si.StockOutSysNo=wout.SysNo
								 INNER JOIN [LgDeliveryItem] AS di ON di.DeliverySysNo=si.DeliverySysNo WHERE wout.OrderSysNO=@0 and wout.Status>0  ", sysNo).QueryMany<LgDeliveryItem>();
        }
        /// <summary>
        /// 根据订单系统编号列表和状态获取订单列表
        /// </summary>
        /// <param name="sysNos">订单系统编号集（多个逗号分隔）</param>
        /// <param name="status">订单状态</param>
        /// <returns></returns>
        /// <remarks>2016-6-26 杨浩 创建</remarks>
        public override List<SoOrder> GetOrderListBySysNosAndStatus(string sysNos, int status)
        {
            string sql = @"SELECT * from SoOrder where [Status]=55 and SysNo in("+sysNos+")";
            return Context.Sql(sql).QueryMany<SoOrder>();
        }
        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="orderStatusText">订单状态文本</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public override int UpdateOrderStatus(int orderSysNo, string orderStatusText, int orderStatus)
        {
            return Context.Sql("update soorder set status=@status, OnlineStatus=@OnlineStatus,ReceivingConfirmDate=GETDATE() where SysNo=@SysNo")
                  .Parameter("status", orderStatus)
                  .Parameter("OnlineStatus", orderStatusText)
                  .Parameter("SysNo", orderSysNo).Execute();
        }
        /// <summary>
        /// 更新订单配送方式
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <param name="DeliveryTypeSysNo"></param>
        /// <returns></returns>
        public override int UpdateOrderDeliveryType(int orderSysNo, int DeliveryTypeSysNo)
        {
            return Context.Sql("update soorder set DeliveryTypeSysNo=@DeliveryTypeSysNo where SysNo=@SysNo")
                 .Parameter("DeliveryTypeSysNo", DeliveryTypeSysNo)
                 .Parameter("SysNo", orderSysNo).Execute();
        }
        /// <summary>
        /// 更新订单配送备注
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <param name="DeliveryRemarks"></param>
        /// <returns></returns>
        /// <remarks>2017-5-5 罗勤尧 创建</remarks>
        public override int UpdateOrderDeliveryRemarks(int orderSysNo, string DeliveryRemarks)
        {
            return Context.Sql("update soorder set DeliveryRemarks=@DeliveryRemarks where SysNo=@SysNo")
                 .Parameter("DeliveryRemarks", DeliveryRemarks)
                 .Parameter("SysNo", orderSysNo).Execute();
        }

        /// <summary>
        /// 获取超时为确认收货的订单编号列表
        /// </summary>
        /// <param name="timeOutDay">超时天数</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public override IList<SoOrder> GetConfirmReceiptOrderSysNoList(int timeOutDay)
        {
            string sql = @"SELECT so.* FROM [FnOnlinePayment] as fp
                            inner join SoOrder as so on so.SysNo=fp.SourceSysNo 
                            where fp.[Source]=10 and fp.[Status]=1 and so.[Status]=55 and fp.CreatedDate< '" + DateTime.Now.AddDays(-1 * timeOutDay).ToString() + "' ";
            return Context.Sql(sql).QueryMany<SoOrder>();
        }
        /// <summary>
        /// 更新订单API执行状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="type">API类型(0:海关支付报关状态 1:商检状态 2:海关订单状态)</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-28 杨浩 创建</remarks>
        public override int UpdateOrderApiStatus(int status, int apiType,int orderSysNo)
        {

            string setSql=" set ";
            if (apiType == 0)
                setSql += " CustomsPayStatus=";
            else if(apiType==1)
                setSql += " TradeCheckStatus=";
            else if(apiType==2)
                setSql += " CustomsStatus=";
            else if(apiType==3)
                setSql += " CBLogisticsSendStatus=";
            else if(apiType==4)
                setSql += " EhkingCipStatus=";
            setSql += "@status";
            return Context.Sql("update SoOrder " + setSql + " where sysNo=@orderSysNo")
                   .Parameter("status", status)
                   .Parameter("orderSysNo", orderSysNo)
                   .Execute();
        }

        #region 订单分页查询

        /// <summary>
        /// 订单分页查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        /// <remarks>2013-06-13 朱家宏 创建</remarks>
        /// <remarks>2013-06-19 余勇 修改表bsjobpool 为 SyJobPool 添加仓库表WhwareHouse左连接</remarks>
        /// <remarks>2013-06-24 朱家宏 重构</remarks>
        /// <remarks>2013-08-30 黄志勇 修改，增加现金支付</remarks>
        /// <remarks>2013-11-28 黄志勇 修改，查询订单事务编号</remarks>
        /// <remarks>2013-12-20 黄志勇 修改，增加商城订单号</remarks>
        /// <remarks>2016-04-05 杨云奕 修改，增加订单区域和地址</remarks>
        ///  <remarks>2017-05-04 罗勤尧 修改，增加第三方订单支付时间，和第三方订单号，获取所有订单号</remarks>
        public override void GetSoOrders(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            #region 以前的
            //            var sql =
//                @"soOrder a 
//                                   left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO                                                                                                  --会员表
//                                   left join SyJobPool c on c.tasksysno=a.sysno and (c.tasktype={0} or c.tasktype={1})                                                                                                        --订单池
//                                   left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo                                                                                        --收货地址表 
//                                   left join BsArea q on d.AreaSysNo=q.SysNo
//								   left join BsArea q1 on q.ParentSysNo=q1.SysNo
//								   left join BsArea q2 on q1.ParentSysNo=q2.SysNo
//                                   left join SyUser e on e.sysno=a.OrderCreatorSysno                                                                                                    --系统用户表
//                                   left join SyUser f on f.sysno=a.AuditorSysNo                                                                                                         --系统用户表
//                                   left join WhwareHouse g on a.DefaultWarehouseSysNo = g.SYSNO                                                                                         --仓库表
//                                   left join bsPaymentType i on a.PayTypeSysNo=i.sysNo                                                                                                  --支付方式
//                                   left join LgDeliveryType j on a.DeliveryTypeSysNo=j.sysNo                                                                                            --配送方式       
//                                   left join syUser k on k.sysno = c.assignersysno                                                                                                      --系统用户表(分配人姓名)
//                                   left join DsDealer dea on a.DealerSysNo = dea.SysNo  
//                                   left join  DsOrder so on so.OrderTransactionSysNo =a.TransactionSysNo                                 
//                                   left join 
//                                   (
//                                    --select max(fn.CreatedDate) as CreatedDate,fn.SourceSysNo
//                                    --from FnOnlinePayment fn 
//                                    --group by fn.SourceSysNo
//
//                                    select max(fi.CreatedDate) as CreatedDate,fn.SourceSysNo
//                                    from FnReceiptVoucher fn  left join FnReceiptVoucherItem as fi on fn.SysNo=fi.ReceiptVoucherSysNo 
//                                    where fi.[Status]=1
//                                    group by fn.SourceSysNo
//
//                                   ) as fn on fn.SourceSysNo = a.SysNo                                                                                                                  --获取订单的支付时间
//                where {2}";
#endregion

            var startdate = DateTime.Now.AddMonths(-2);
//            var sql =
//               @"soOrder a 
//                                   left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO                                                                                                  --会员表
//                                   left join SyJobPool c on c.tasksysno=a.sysno and (c.tasktype={0} or c.tasktype={1})                                                                                                        --订单池
//                                   left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo                                                                                        --收货地址表 
//                                   left join BsArea q on d.AreaSysNo=q.SysNo
//								   left join BsArea q1 on q.ParentSysNo=q1.SysNo
//								   left join BsArea q2 on q1.ParentSysNo=q2.SysNo
//                                   left join SyUser e on e.sysno=a.OrderCreatorSysno                                                                                                    --系统用户表
//                                   left join SyUser f on f.sysno=a.AuditorSysNo                                                                                                         --系统用户表
//                                   left join WhwareHouse g on a.DefaultWarehouseSysNo = g.SYSNO                                                                                         --仓库表
//                                   left join bsPaymentType i on a.PayTypeSysNo=i.sysNo                                                                                                  --支付方式
//                                   left join LgDeliveryType j on a.DeliveryTypeSysNo=j.sysNo                                                                                            --配送方式       
//                                   left join syUser k on k.sysno = c.assignersysno                                                                                                      --系统用户表(分配人姓名)
//                                   left join DsDealer dea on a.DealerSysNo = dea.SysNo  
//                                   left join  DsOrder so on so.OrderTransactionSysNo =a.TransactionSysNo                                 
                //where  {2}";
            var sql = @"soOrder a  left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO "
                                 
                                  + "left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo  "
                                 
                                   + "left join SyUser e on e.sysno=a.OrderCreatorSysno "
                                  + "left join SyUser f on f.sysno=a.AuditorSysNo  "
                                  + "left join WhwareHouse g on a.DefaultWarehouseSysNo = g.SYSNO "
                                  + "left join bsPaymentType i on a.PayTypeSysNo=i.sysNo "
                                  + "left join LgDeliveryType j on a.DeliveryTypeSysNo=j.sysNo  "
                                  //+ "left join syUser k on k.sysno = c.assignersysno  "
                                  + "left join DsDealer dea on a.DealerSysNo = dea.SysNo  "
                + "left join  DsOrder so on so.OrderTransactionSysNo =a.TransactionSysNo  "

               + "where {2}";
            #region 构造sql
            var paras = new ArrayList();
            var where = "1=1";

        
            //if (filter.IsBindDealer)
            //{
            //    where += " and dea.SysNo = " + filter.DealerSysNo;
            //}
           



      

            int i = 0;
            if (filter.Keyword != null && filter.Keyword != "")
            {
                int sysno = 0;
                if (int.TryParse(filter.Keyword, out sysno))
                {
                    where += " and (a.sysNo=@p0p" + i;
                    paras.Add(filter.Keyword);
                    i++;

                    where += " or a.OrderNo=@p0p" + i;
                    paras.Add(filter.Keyword);
                    i++;

                    //where += " or so.MallOrderId=@p0p" + i;
                    //paras.Add(filter.Keyword);
                    //i++;
                }
                else
                {
                    where += " and (a.OrderNo=@p0p" + i;
                    paras.Add(filter.Keyword);
                    i++;

                  
                }
                where += " or so.MallOrderId=@p0p" + i;
                paras.Add(filter.Keyword);
                i++;

                where += " or d.Name =@p0p" + i;
                paras.Add(filter.Keyword);
                i++;

                where += " or d.MobilePhoneNumber=@p0p" + i;
                paras.Add(filter.Keyword);
                i++;

                where += " or b.account=@p0p" + i + ")";
                paras.Add(filter.Keyword);
                i++;
            }
            if (filter.OrderStatus != null && string.IsNullOrEmpty(filter.OrderStatusList))
            {
                where += " and a.status = (@p0p" + i+")";
                paras.Add(filter.OrderStatus);
                i++;
            }
            else if (!string.IsNullOrEmpty(filter.OrderStatusList))
            {
                where += " and a.status in (" + filter.OrderStatusList + ")";
                //paras.Add();
                //i++;
            }

            if (!string.IsNullOrWhiteSpace(filter.CustomerName))
            {
                where += " and charindex(b.name,@p0p" + i + ")>0";
                paras.Add(filter.CustomerName);
                i++;
            }
            if (filter.OrderSource != null)
            {
                where += " and a.OrderSource=@p0p" + i;
                paras.Add(filter.OrderSource);
                i++;
            }
            if (filter.PayTypeSysNo != null)
            {
                where += " and a.PayTypeSysNo=@p0p" + i;
                paras.Add(filter.PayTypeSysNo);
                i++;
            }
            if (filter.DeliveryTypeSysNo != null)
            {
                where += " and a.DeliveryTypeSysNo=@p0p" + i;
                paras.Add(filter.DeliveryTypeSysNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ProductName))
            {
                where +=
                    " and exists (select 1 from soOrderItem b where b.ordersysno=a.sysno and charindex(b.productname,@p0p" + i + ")>0)";
                paras.Add(filter.ProductName);
                i++;
            }
            if (filter.WhStockOutSysNo != null)
            {
                where +=
                    " and exists (select 1 from WhStockOut b where b.TransactionSysNo=a.TransactionSysNo and b.SysNo=@p0p" + i + ")";
                paras.Add(filter.WhStockOutSysNo);
                i++;
            }
            if (filter.MinOrderAmount != null)
            {
                where += " and a.OrderAmount>=@p0p" + i;
                paras.Add(filter.MinOrderAmount);
                i++;
            }
            if (filter.MaxOrderAmount != null)
            {
                where += " and a.OrderAmount<=@p0p" + i;
                paras.Add(filter.MaxOrderAmount);
                i++;
            }
            if (filter.BeginDate != null)
            {
                where += " and a.createDate>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate != null)
            {
                where += " and a.createDate<@p0p" + i;
                paras.Add(filter.EndDate);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.CustomerMobile))
            {
                string p0p = "@p0p" + i;
                where += string.Format(" and (b.MobilePhoneNumber={0} or d.MobilePhoneNumber={0} or b.Account={0}) ", p0p);
                paras.Add(filter.CustomerMobile);
                i++;
            }
            //if (filter.ExecutorSysNo != null)
            //{
            //    where += " and c.ExecutorSysNo=@p0p" + i;
            //    paras.Add(filter.ExecutorSysNo);
            //    i++;
            //}
            //if (filter.TaskTypes != null)
            //{
            //    var taskTypes = string.Join(",", filter.TaskTypes);
            //    where += " and  exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = c.TaskType)";
            //    paras.Add(taskTypes);
            //    i++;
            //}
            if (!string.IsNullOrWhiteSpace(filter.ReceiveName))
            {
                where += " and charindex(d.name,@p0p" + i + ")>0";
                paras.Add(filter.ReceiveName);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ReceiveTel))
            {
                where += " and (d.PhoneNumber=@p0p" + i;
                i++;
                where += " or d.MobilePhoneNumber=@p0p" + i + ")";
                i++;
                paras.Add(filter.ReceiveTel);
                paras.Add(filter.ReceiveTel);
            }
            //if (!string.IsNullOrWhiteSpace(filter.OrderCreator))
            //{
            //    where += " and charindex(e.UserName,@p0p" + i + ")>0";
            //    paras.Add(filter.OrderCreator);
            //    i++;
            //}
            if (!string.IsNullOrWhiteSpace(filter.Auditor))
            {
                where += " and charindex(f.UserName,@p0p" + i + ")>0";
                paras.Add(filter.Auditor);
                i++;
            }
            if (filter.StoreSysNoList != null)
            {
                var storeSysNos = string.Join(",", filter.StoreSysNoList);
                where += " and exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = a.DefaultWarehouseSysno)";
                paras.Add(storeSysNos);
                i++;
            }
            if (filter.ExceptedDeliveryTypeSysNo != null)
            {
                where += " and a.DeliveryTypeSysNo!=@p0p" + i;
                paras.Add(filter.ExceptedDeliveryTypeSysNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.CustomerAccount))
            {
                where += " and b.Account=@p0p" + i;
                paras.Add(filter.CustomerAccount);
                i++;
            }
            if (filter.CustomerSysNo != null)
            {
                where += " and b.SysNo=@p0p" + i;
                paras.Add(filter.CustomerSysNo);
                i++;
            }
            //if (filter.SettlementStatus != null)
            //{
            //    where +=
            //        " and exists (select distinct tmp.TransactionSysNo from LgSettlementItem tmp where a.transactionsysno=tmp.transactionsysno and tmp.Status=@p0p" + i + ")";
            //    paras.Add(filter.SettlementStatus);
            //    i++;
            //}
            if (filter.OrderSourceSysNoList != null)
            {
                var orderSourceSysNos = string.Join(",", filter.OrderSourceSysNoList);
                where += " and exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = a.orderSourceSysNo)";
                paras.Add(orderSourceSysNos);
                i++;
            }
            if (filter.PaymentType != null)
            {
                where += " and i.PaymentType=@p0p" + i;
                paras.Add(filter.PaymentType);
                i++;
            }
            //if (!string.IsNullOrWhiteSpace(filter.ExpressNo))
            //{
            //    where +=
            //        " and exists (select distinct tmp.TransactionSysNo from LgDeliveryItem tmp where a.transactionsysno=tmp.transactionsysno and tmp.ExpressNo=@p0p" + i + ")";
            //    paras.Add(filter.ExpressNo);
            //    i++;
            //}
            //if (!string.IsNullOrWhiteSpace(filter.MallOrderId))
            //{
            //    where +=
            //        " and exists (select distinct tmp.OrderTransactionSysNo from DsOrder tmp where a.transactionsysno=tmp.OrderTransactionSysNo and charindex(tmp.MallOrderId,@p0p" + i + ")>0)";
            //    paras.Add(filter.MallOrderId);
            //    i++;
            //}
            if (filter.NonInvalidStatus != null)
            {
                where += " and a.status <> @p0p" + i;
                paras.Add((int)OrderStatus.销售单状态.作废);
                i++;
            }
            if (filter.OrderSysNo != null)
            {
                where += " and a.sysno = @p0p" + i;
                paras.Add(filter.OrderSysNo);
                i++;
            }
            //if (!string.IsNullOrWhiteSpace(filter.MallShopName))
            //{
            //    where +=
            //        " and exists (" +
            //        " select distinct tmp.OrderTransactionSysNo from DsOrder tmp " +
            //        " left join DsDealerMall tmpMall on tmp.DealerMallSysNo=tmpMall.SysNo " +
            //        " where a.transactionsysno=tmp.OrderTransactionSysNo and charindex(tmpMall.ShopName,@p0p" + i + ")>0)";
            //    paras.Add(filter.MallShopName);
            //    i++;
            //}
            //增加仓库查询条件 余勇 2014-07-31
            if (filter.WarehouseSysNo != null)
            {
                where += " and a.DefaultWarehouseSysNo = @p0p" + i;
                paras.Add(filter.WarehouseSysNo);
                i++;
            }
            //增加付款时间搜索条件 王耀发 2016-2-17
            if (filter.PayBeginDate != null)
            {
                where += " and a.PayDate>=@p0p" + i;
                paras.Add(filter.PayBeginDate);
                i++;
            }
            if (filter.PayEndDate != null)
            {
                where += " and a.PayDate<@p0p" + i;
                paras.Add(filter.PayEndDate);
                i++;
            }
            ////增加第三方付款时间搜索条件 罗勤尧 2017-5-3
            //if (filter.PayBeginDate != null)
            //{
            //    where += " or so.PayTime>=@p0p" + i;
            //    paras.Add(filter.PayBeginDate);
            //    i++;
            //}
            //if (filter.PayEndDate != null)
            //{
            //    where += " and so.PayTime<@p0p" + i;
            //    paras.Add(filter.PayEndDate);
            //    i++;
            //}
            if (filter.PayStatus != null)
            {
                if (filter.PayStatus == 10)
                {
                    where += " and (a.PayStatus = @p0p" + i + " and a.Status>0  and a.Status<30) ";
                }
                else
                {
                    where += " and a.PayStatus = @p0p" + i;
                }
                
                //where += " and a.PayStatus = @p0p" + i;
                paras.Add(filter.PayStatus);
                i++;
            }
            //判断是否属于第三方订单 王耀发 2016-4-26 创建
            if (filter.Supply != 0)
            {
                where += " and g.Supply = @p0p" + i;
                paras.Add(filter.Supply);
                i++;
            }


            //是否绑定所有仓库
            if (filter.HasAllWarehouse)
            {
                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        where += " and dea.SysNo = " + filter.DealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                    }
                }
                if (filter.SelectedAgentSysNo != -1)
                {
                    if (filter.SelectedDealerSysNo != -1)
                    {
                        where += " and dea.SysNo = " + filter.SelectedDealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.SelectedAgentSysNo;
                    }
                }
            }
            else
            {

                if (filter.Warehouses != null && filter.Warehouses.Count > 0)
                {
                    var wList = "";
                    foreach (var w in filter.Warehouses)
                    {
                        if (wList == "")
                            wList = w.SysNo.ToString();
                        else
                            wList += ',' + w.SysNo.ToString();
                    }
                    wList = "(" + wList + ")";
                    where += " and a.DefaultWarehouseSysNo in " + wList;
                }
                else
                {
                    where += " and a.DefaultWarehouseSysNo = -1";
                }


            }
           // where += " and a.CreateDate>'" + startdate.ToString() + "' and a.CreateDate<'" + DateTime.Now.ToString() + "'";
            sql = string.Format(sql, (int)SystemStatus.任务对象类型.客服订单审核, (int)SystemStatus.任务对象类型.客服订单提交出库, where);
            #endregion

            var dataList = Context.Select<CBSoOrder>(@"a.SysNo,a.OrderNo,a.Status,a.SendStatus,a.GZJCStatus,a.NsStatus,a.TransactionSysNo,g.WarehouseName,g.BackWarehouseName,a.DefaultWarehouseSysNo as WhStockOutSysNo,a.CashPay,a.PayTypeSysNo,a.CustomsPayStatus,a.TradeCheckStatus,a.CustomsStatus,a.DefaultWarehouseSysNo,a.DeliveryRemarks,
                                   b.Name as CustomerName,d.Name as ReceiveName,b.account as customerAccount,b.sysNo as CustomerId,a.CBLogisticsSendStatus,
                                   d.MobilePhoneNumber as ReceiveTel,j.DeliveryTypeName,j.OverseaCarrier,
                                   i.PaymentName,a.OrderAmount,
                                   a.OrderSource,a.CreateDate,g.CreatedDate as StockOutDate,a.AuditorDate,
                                   a.LastUpdateDate,'' as ApplyName,f.UserName as AuditorName,
                                   e.UserName as OrderCreator,a.PayStatus,a.ImgFlag,g.WarehouseType,g.Logistics,g.Customs,g.Inspection,dea.DealerName,
                                   a.PayDate as PaymentDate" +
                                   ",d.StreetAddress ").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            var dataIdList = Context.Select<int>(@"a.SysNo ").From(sql);
            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            dataIdList.Parameters(paras);
            var idlist = dataIdList.QueryMany();
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("a.CREATEDATE desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
            pager.IdRows = idlist;
        }



        /// <summary>
        /// 订单分页查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        ///  <remarks>2017-09-18 罗勤尧 添加</remarks>
        ///   <remarks>2017-09-18 修改支付时间查询逻辑 优化</remarks>
        public override void GetSoOrdersNew(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            string join = "";
            if (!string.IsNullOrWhiteSpace(filter.ErpCode))
            {
                join = "left join SoOrderItem si on a.sysno  =si.OrderSysNo left join PdProduct p on si.ProductSysNo=p.SysNo ";
            }
            var sql =@"soOrder a  "+join+" left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO "
                                   +"left join SyJobPool c on c.tasksysno=a.sysno and (c.tasktype={0} or c.tasktype={1}) "  
                                   +"left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo  " 
                                   +"left join BsArea q on d.AreaSysNo=q.SysNo " 
								    +"left join BsArea q1 on q.ParentSysNo=q1.SysNo "
								    +"left join BsArea q2 on q1.ParentSysNo=q2.SysNo "
                                    +"left join SyUser e on e.sysno=a.OrderCreatorSysno " 
                                   +"left join SyUser f on f.sysno=a.AuditorSysNo  " 
                                   +"left join WhwareHouse g on a.DefaultWarehouseSysNo = g.SYSNO " 
                                   +"left join bsPaymentType i on a.PayTypeSysNo=i.sysNo " 
                                   +"left join LgDeliveryType j on a.DeliveryTypeSysNo=j.sysNo  "      
                                   +"left join syUser k on k.sysno = c.assignersysno  " 
                                   +"left join DsDealer dea on a.DealerSysNo = dea.SysNo  "
                                    + "left join  DsOrder so on so.OrderTransactionSysNo =a.TransactionSysNo  "                          
                                                                                                                                              
                +"where {2}";

            #region 构造sql
            var paras = new ArrayList();
            var where = "1=1";


            //if (filter.IsBindDealer)
            //{
            //    where += " and dea.SysNo = " + filter.DealerSysNo;
            //}
            int i = 0;
            if (filter.Keyword != null && filter.Keyword != "")
            {
                int sysno = 0;
                if (int.TryParse(filter.Keyword, out sysno))
                {
                    where += " and (a.sysNo=@p0p" + i;
                    paras.Add(filter.Keyword);
                    i++;

                    where += " or a.OrderNo=@p0p" + i;
                    paras.Add(filter.Keyword);
                    i++;

                    //where += " or so.MallOrderId=@p0p" + i;
                    //paras.Add(filter.Keyword);
                    //i++;
                }
                else
                {
                    where += " and (a.OrderNo=@p0p" + i;
                    paras.Add(filter.Keyword);
                    i++;


                }
                where += " or so.MallOrderId=@p0p" + i;
                paras.Add(filter.Keyword);
                i++;

                where += " or d.Name =@p0p" + i;
                paras.Add(filter.Keyword);
                i++;

                where += " or d.MobilePhoneNumber=@p0p" + i;
                paras.Add(filter.Keyword);
                i++;

                where += " or b.account=@p0p" + i + ")";
                paras.Add(filter.Keyword);
                i++;
            }
            if (filter.OrderStatus != null && string.IsNullOrEmpty(filter.OrderStatusList))
            {
                where += " and a.status = (@p0p" + i + ")";
                paras.Add(filter.OrderStatus);
                i++;
            }
            else if (!string.IsNullOrEmpty(filter.OrderStatusList))
            {
                where += " and a.status in (" + filter.OrderStatusList + ")";
                //paras.Add();
                //i++;
            }

            if (!string.IsNullOrWhiteSpace(filter.CustomerName))
            {
                where += " and charindex(b.name,@p0p" + i + ")>0";
                paras.Add(filter.CustomerName);
                i++;
            }
            if (filter.OrderSource != null)
            {
                where += " and a.OrderSource=@p0p" + i;
                paras.Add(filter.OrderSource);
                i++;
            }
            if (filter.PayTypeSysNo != null)
            {
                where += " and a.PayTypeSysNo=@p0p" + i;
                paras.Add(filter.PayTypeSysNo);
                i++;
            }
            if (filter.DeliveryTypeSysNo != null)
            {
                where += " and a.DeliveryTypeSysNo=@p0p" + i;
                paras.Add(filter.DeliveryTypeSysNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ProductName))
            {
                where +=
                    " and exists (select 1 from soOrderItem b where b.ordersysno=a.sysno and charindex(b.productname,@p0p" + i + ")>0)";
                paras.Add(filter.ProductName);
                i++;
            }
            if (filter.WhStockOutSysNo != null)
            {
                where +=
                    " and exists (select 1 from WhStockOut b where b.TransactionSysNo=a.TransactionSysNo and b.SysNo=@p0p" + i + ")";
                paras.Add(filter.WhStockOutSysNo);
                i++;
            }
            if (filter.MinOrderAmount != null)
            {
                where += " and a.OrderAmount>=@p0p" + i;
                paras.Add(filter.MinOrderAmount);
                i++;
            }
            if (filter.MaxOrderAmount != null)
            {
                where += " and a.OrderAmount<=@p0p" + i;
                paras.Add(filter.MaxOrderAmount);
                i++;
            }
            if (filter.BeginDate != null)
            {
                where += " and a.createDate>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate != null)
            {
                where += " and a.createDate<@p0p" + i;
                paras.Add(filter.EndDate);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.CustomerMobile))
            {
                string p0p = "@p0p" + i;
                where += string.Format(" and (b.MobilePhoneNumber={0} or d.MobilePhoneNumber={0} or b.Account={0}) ", p0p);
                paras.Add(filter.CustomerMobile);
                i++;
            }
            if (filter.ExecutorSysNo != null)
            {
                where += " and c.ExecutorSysNo=@p0p" + i;
                paras.Add(filter.ExecutorSysNo);
                i++;
            }
            if (filter.TaskTypes != null)
            {
                var taskTypes = string.Join(",", filter.TaskTypes);
                where += " and  exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = c.TaskType)";
                paras.Add(taskTypes);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ReceiveName))
            {
                where += " and charindex(d.name,@p0p" + i + ")>0";
                paras.Add(filter.ReceiveName);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ReceiveTel))
            {
                where += " and (d.PhoneNumber=@p0p" + i;
                i++;
                where += " or d.MobilePhoneNumber=@p0p" + i + ")";
                i++;
                paras.Add(filter.ReceiveTel);
                paras.Add(filter.ReceiveTel);
            }
            //if (!string.IsNullOrWhiteSpace(filter.OrderCreator))
            //{
            //    where += " and charindex(e.UserName,@p0p" + i + ")>0";
            //    paras.Add(filter.OrderCreator);
            //    i++;
            //}
            if (!string.IsNullOrWhiteSpace(filter.Auditor))
            {
                where += " and charindex(f.UserName,@p0p" + i + ")>0";
                paras.Add(filter.Auditor);
                i++;
            }
            if (filter.StoreSysNoList != null)
            {
                var storeSysNos = string.Join(",", filter.StoreSysNoList);
                where += " and exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = a.DefaultWarehouseSysno)";
                paras.Add(storeSysNos);
                i++;
            }
            if (filter.ExceptedDeliveryTypeSysNo != null)
            {
                where += " and a.DeliveryTypeSysNo!=@p0p" + i;
                paras.Add(filter.ExceptedDeliveryTypeSysNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.CustomerAccount))
            {
                where += " and b.Account=@p0p" + i;
                paras.Add(filter.CustomerAccount);
                i++;
            }
            if (filter.CustomerSysNo != null)
            {
                where += " and b.SysNo=@p0p" + i;
                paras.Add(filter.CustomerSysNo);
                i++;
            }
            //if (filter.SettlementStatus != null)
            //{
            //    where +=
            //        " and exists (select distinct tmp.TransactionSysNo from LgSettlementItem tmp where a.transactionsysno=tmp.transactionsysno and tmp.Status=@p0p" + i + ")";
            //    paras.Add(filter.SettlementStatus);
            //    i++;
            //}
            if (filter.OrderSourceSysNoList != null)
            {
                var orderSourceSysNos = string.Join(",", filter.OrderSourceSysNoList);
                where += " and exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = a.orderSourceSysNo)";
                paras.Add(orderSourceSysNos);
                i++;
            }
            if (filter.PaymentType != null)
            {
                where += " and i.PaymentType=@p0p" + i;
                paras.Add(filter.PaymentType);
                i++;
            }
            //if (!string.IsNullOrWhiteSpace(filter.ExpressNo))
            //{
            //    where +=
            //        " and exists (select distinct tmp.TransactionSysNo from LgDeliveryItem tmp where a.transactionsysno=tmp.transactionsysno and tmp.ExpressNo=@p0p" + i + ")";
            //    paras.Add(filter.ExpressNo);
            //    i++;
            //}
            //if (!string.IsNullOrWhiteSpace(filter.MallOrderId))
            //{
            //    where +=
            //        " and exists (select distinct tmp.OrderTransactionSysNo from DsOrder tmp where a.transactionsysno=tmp.OrderTransactionSysNo and charindex(tmp.MallOrderId,@p0p" + i + ")>0)";
            //    paras.Add(filter.MallOrderId);
            //    i++;
            //}
            if (filter.NonInvalidStatus != null)
            {
                where += " and a.status <> @p0p" + i;
                paras.Add((int)OrderStatus.销售单状态.作废);
                i++;
            }
            if (filter.OrderSysNo != null)
            {
                where += " and a.sysno = @p0p" + i;
                paras.Add(filter.OrderSysNo);
                i++;
            }
            //if (!string.IsNullOrWhiteSpace(filter.MallShopName))
            //{
            //    where +=
            //        " and exists (" +
            //        " select distinct tmp.OrderTransactionSysNo from DsOrder tmp " +
            //        " left join DsDealerMall tmpMall on tmp.DealerMallSysNo=tmpMall.SysNo " +
            //        " where a.transactionsysno=tmp.OrderTransactionSysNo and charindex(tmpMall.ShopName,@p0p" + i + ")>0)";
            //    paras.Add(filter.MallShopName);
            //    i++;
            //}
            //增加仓库查询条件 余勇 2014-07-31
            if (filter.WarehouseSysNo != null)
            {
                where += " and a.DefaultWarehouseSysNo = @p0p" + i;
                paras.Add(filter.WarehouseSysNo);
                i++;
            }
            //增加付款时间搜索条件 王耀发 2016-2-17
            if (filter.PayBeginDate != null)
            {
                where += " and a.PayDate>=@p0p" + i;
                paras.Add(filter.PayBeginDate);
                i++;
            }
            if (filter.PayEndDate != null)
            {
                where += " and a.PayDate<@p0p" + i;
                paras.Add(filter.PayEndDate);
                i++;
            }
            ////增加第三方付款时间搜索条件 罗勤尧 2017-5-3
            //if (filter.PayBeginDate != null)
            //{
            //    where += " or so.PayTime>=@p0p" + i;
            //    paras.Add(filter.PayBeginDate);
            //    i++;
            //}
            //if (filter.PayEndDate != null)
            //{
            //    where += " and so.PayTime<@p0p" + i;
            //    paras.Add(filter.PayEndDate);
            //    i++;
            //}
            if (filter.PayStatus != null)
            {
                if (filter.PayStatus == 10)
                {
                    where += " and (a.PayStatus = @p0p" + i + " and a.Status>0  and a.Status<30) ";
                }
                else
                {
                    where += " and a.PayStatus = @p0p" + i;
                }

                //where += " and a.PayStatus = @p0p" + i;
                paras.Add(filter.PayStatus);
                i++;
            }
            //判断是否属于第三方订单 王耀发 2016-4-26 创建
            if (filter.Supply != 0)
            {
                where += " and g.Supply = @p0p" + i;
                paras.Add(filter.Supply);
                i++;
            }

            if (!string.IsNullOrWhiteSpace(filter.ErpCode))
            {
                where += " and p.ErpCode= @p0p" + i;
                paras.Add(filter.ErpCode);
                i++;
            }
            //是否绑定所有仓库
            if (filter.HasAllWarehouse)
            {
                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        where += " and dea.SysNo = " + filter.DealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                    }
                }
                if (filter.SelectedAgentSysNo != -1)
                {
                    if (filter.SelectedDealerSysNo != -1)
                    {
                        where += " and dea.SysNo = " + filter.SelectedDealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.SelectedAgentSysNo;
                    }
                }
            }
            else
            {

                if (filter.Warehouses != null && filter.Warehouses.Count > 0)
                {
                    var wList = "";
                    foreach (var w in filter.Warehouses)
                    {
                        if (wList == "")
                            wList = w.SysNo.ToString();
                        else
                            wList += ',' + w.SysNo.ToString();
                    }
                    wList = "(" + wList + ")";
                    where += " and a.DefaultWarehouseSysNo in " + wList;
                }
                else
                {
                   // where += " and a.DefaultWarehouseSysNo = -1";
                }


            }
            sql = string.Format(sql, (int)SystemStatus.任务对象类型.客服订单审核, (int)SystemStatus.任务对象类型.客服订单提交出库, where);
            #endregion

            var dataList = Context.Select<CBSoOrder>(@"a.SysNo,a.OrderNo,a.Status,a.SendStatus,a.GZJCStatus,a.NsStatus,a.TransactionSysNo,g.WarehouseName,g.BackWarehouseName,a.DefaultWarehouseSysNo as WhStockOutSysNo,a.CashPay,a.PayTypeSysNo,a.CustomsPayStatus,a.TradeCheckStatus,a.CustomsStatus,a.DefaultWarehouseSysNo,a.DeliveryRemarks,
                                   b.Name as CustomerName,d.Name as ReceiveName,b.account as customerAccount,b.sysNo as CustomerId,a.CBLogisticsSendStatus,
                                   d.MobilePhoneNumber as ReceiveTel,j.DeliveryTypeName,j.OverseaCarrier,
                                   i.PaymentName,a.OrderAmount,floor(a.CashPay) as Point,
                                   a.OrderSource,a.CreateDate,g.CreatedDate as StockOutDate,a.AuditorDate,
                                   a.LastUpdateDate,k.username as AssignName,'' as ApplyName,f.UserName as AuditorName,c.SysNo as JobSysNo,
                                   e.UserName as OrderCreator,a.PayStatus,a.ImgFlag,g.WarehouseType,g.Logistics,g.Customs,g.Inspection,dea.DealerName,so.MallOrderId,so.PayTime,
                                   a.PayDate as PaymentDate" +
                                   ",d.StreetAddress, (q2.AreaName +' ' +q1.AreaName +' '+ q.AreaName) as AreaInfo ").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            var dataIdList = Context.Select<int>(@"a.SysNo ").From(sql);
            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            dataIdList.Parameters(paras);
            var idlist = dataIdList.QueryMany();
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("a.CREATEDATE desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
            pager.IdRows = idlist;
        }

        /// <summary>
        /// 订单分页查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns></returns>
        ///  <remarks>2017-09-18 罗勤尧 添加</remarks>
        ///   <remarks>2017-09-18消减字段取消不必要关联 修改支付时间查询逻辑 优化</remarks>
        public override void GetSoOrdersB2C(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            string join = "";
            if (!string.IsNullOrWhiteSpace(filter.ErpCode))
            {
                join = "left join SoOrderItem si on a.sysno  =si.OrderSysNo left join PdProduct p on si.ProductSysNo=p.SysNo ";
            }
            var sql = @"soOrder a  " + join + " left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO "
                                   //+ "left join SyJobPool c on c.tasksysno=a.sysno and (c.tasktype={0} or c.tasktype={1}) "
                                   + "left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo  "
                                   //+ "left join BsArea q on d.AreaSysNo=q.SysNo "
                                   // + "left join BsArea q1 on q.ParentSysNo=q1.SysNo "
                                   // + "left join BsArea q2 on q1.ParentSysNo=q2.SysNo "
                                    //+ "left join SyUser e on e.sysno=a.OrderCreatorSysno "
                                   + "left join SyUser f on f.sysno=a.AuditorSysNo  "
                                   + "left join WhwareHouse g on a.DefaultWarehouseSysNo = g.SYSNO "
                                   + "left join bsPaymentType i on a.PayTypeSysNo=i.sysNo "
                                   + "left join LgDeliveryType j on a.DeliveryTypeSysNo=j.sysNo  "
                                   //+ "left join syUser k on k.sysno = c.assignersysno  "
                                   + "left join DsDealer dea on a.DealerSysNo = dea.SysNo  "
                                    //+ "left join  DsOrder so on so.OrderTransactionSysNo =a.TransactionSysNo  "

                + "where {2}";

            #region 构造sql
            var paras = new ArrayList();
            var where = "1=1";


            //if (filter.IsBindDealer)
            //{
            //    where += " and dea.SysNo = " + filter.DealerSysNo;
            //}
            int i = 0;
            if (filter.Keyword != null && filter.Keyword != "")
            {
                int sysno = 0;
                if (int.TryParse(filter.Keyword, out sysno))
                {
                    where += " and (a.sysNo=@p0p" + i;
                    paras.Add(filter.Keyword);
                    i++;

                    where += " or a.OrderNo=@p0p" + i;
                    paras.Add(filter.Keyword);
                    i++;

                    //where += " or so.MallOrderId=@p0p" + i;
                    //paras.Add(filter.Keyword);
                    //i++;
                }
                else
                {
                    where += " and (a.OrderNo=@p0p" + i;
                    paras.Add(filter.Keyword);
                    i++;


                }
                //where += " or so.MallOrderId=@p0p" + i;
                //paras.Add(filter.Keyword);
                //i++;

                where += " or d.Name =@p0p" + i;
                paras.Add(filter.Keyword);
                i++;

                where += " or d.MobilePhoneNumber=@p0p" + i;
                paras.Add(filter.Keyword);
                i++;

                where += " or b.account=@p0p" + i + ")";
                paras.Add(filter.Keyword);
                i++;
            }
            if (filter.OrderStatus != null && string.IsNullOrEmpty(filter.OrderStatusList))
            {
                where += " and a.status = (@p0p" + i + ")";
                paras.Add(filter.OrderStatus);
                i++;
            }
            else if (!string.IsNullOrEmpty(filter.OrderStatusList))
            {
                where += " and a.status in (" + filter.OrderStatusList + ")";
                //paras.Add();
                //i++;
            }

            if (!string.IsNullOrWhiteSpace(filter.CustomerName))
            {
                where += " and charindex(b.name,@p0p" + i + ")>0";
                paras.Add(filter.CustomerName);
                i++;
            }
            if (filter.OrderSource != null)
            {
                where += " and a.OrderSource=@p0p" + i;
                paras.Add(filter.OrderSource);
                i++;
            }
            if (filter.PayTypeSysNo != null)
            {
                where += " and a.PayTypeSysNo=@p0p" + i;
                paras.Add(filter.PayTypeSysNo);
                i++;
            }
            if (filter.DeliveryTypeSysNo != null)
            {
                where += " and a.DeliveryTypeSysNo=@p0p" + i;
                paras.Add(filter.DeliveryTypeSysNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ProductName))
            {
                where +=
                    " and exists (select 1 from soOrderItem b where b.ordersysno=a.sysno and charindex(b.productname,@p0p" + i + ")>0)";
                paras.Add(filter.ProductName);
                i++;
            }
            if (filter.WhStockOutSysNo != null)
            {
                where +=
                    " and exists (select 1 from WhStockOut b where b.TransactionSysNo=a.TransactionSysNo and b.SysNo=@p0p" + i + ")";
                paras.Add(filter.WhStockOutSysNo);
                i++;
            }
            if (filter.MinOrderAmount != null)
            {
                where += " and a.OrderAmount>=@p0p" + i;
                paras.Add(filter.MinOrderAmount);
                i++;
            }
            if (filter.MaxOrderAmount != null)
            {
                where += " and a.OrderAmount<=@p0p" + i;
                paras.Add(filter.MaxOrderAmount);
                i++;
            }
            if (filter.BeginDate != null)
            {
                where += " and a.createDate>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate != null)
            {
                where += " and a.createDate<@p0p" + i;
                paras.Add(filter.EndDate);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.CustomerMobile))
            {
                string p0p = "@p0p" + i;
                where += string.Format(" and (b.MobilePhoneNumber={0} or d.MobilePhoneNumber={0} or b.Account={0}) ", p0p);
                paras.Add(filter.CustomerMobile);
                i++;
            }
            //if (filter.ExecutorSysNo != null)
            //{
            //    where += " and c.ExecutorSysNo=@p0p" + i;
            //    paras.Add(filter.ExecutorSysNo);
            //    i++;
            //}
            //if (filter.TaskTypes != null)
            //{
            //    var taskTypes = string.Join(",", filter.TaskTypes);
            //    where += " and  exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = c.TaskType)";
            //    paras.Add(taskTypes);
            //    i++;
            //}
            if (!string.IsNullOrWhiteSpace(filter.ReceiveName))
            {
                where += " and charindex(d.name,@p0p" + i + ")>0";
                paras.Add(filter.ReceiveName);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ReceiveTel))
            {
                where += " and (d.PhoneNumber=@p0p" + i;
                i++;
                where += " or d.MobilePhoneNumber=@p0p" + i + ")";
                i++;
                paras.Add(filter.ReceiveTel);
                paras.Add(filter.ReceiveTel);
            }
            //if (!string.IsNullOrWhiteSpace(filter.OrderCreator))
            //{
            //    where += " and charindex(e.UserName,@p0p" + i + ")>0";
            //    paras.Add(filter.OrderCreator);
            //    i++;
            //}
            if (!string.IsNullOrWhiteSpace(filter.Auditor))
            {
                where += " and charindex(f.UserName,@p0p" + i + ")>0";
                paras.Add(filter.Auditor);
                i++;
            }
            if (filter.StoreSysNoList != null)
            {
                var storeSysNos = string.Join(",", filter.StoreSysNoList);
                where += " and exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = a.DefaultWarehouseSysno)";
                paras.Add(storeSysNos);
                i++;
            }
            if (filter.ExceptedDeliveryTypeSysNo != null)
            {
                where += " and a.DeliveryTypeSysNo!=@p0p" + i;
                paras.Add(filter.ExceptedDeliveryTypeSysNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.CustomerAccount))
            {
                where += " and b.Account=@p0p" + i;
                paras.Add(filter.CustomerAccount);
                i++;
            }
            if (filter.CustomerSysNo != null)
            {
                where += " and b.SysNo=@p0p" + i;
                paras.Add(filter.CustomerSysNo);
                i++;
            }
            //if (filter.SettlementStatus != null)
            //{
            //    where +=
            //        " and exists (select distinct tmp.TransactionSysNo from LgSettlementItem tmp where a.transactionsysno=tmp.transactionsysno and tmp.Status=@p0p" + i + ")";
            //    paras.Add(filter.SettlementStatus);
            //    i++;
            //}
            if (filter.OrderSourceSysNoList != null)
            {
                var orderSourceSysNos = string.Join(",", filter.OrderSourceSysNoList);
                where += " and exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = a.orderSourceSysNo)";
                paras.Add(orderSourceSysNos);
                i++;
            }
            if (filter.PaymentType != null)
            {
                where += " and i.PaymentType=@p0p" + i;
                paras.Add(filter.PaymentType);
                i++;
            }
            //if (!string.IsNullOrWhiteSpace(filter.ExpressNo))
            //{
            //    where +=
            //        " and exists (select distinct tmp.TransactionSysNo from LgDeliveryItem tmp where a.transactionsysno=tmp.transactionsysno and tmp.ExpressNo=@p0p" + i + ")";
            //    paras.Add(filter.ExpressNo);
            //    i++;
            //}
            //if (!string.IsNullOrWhiteSpace(filter.MallOrderId))
            //{
            //    where +=
            //        " and exists (select distinct tmp.OrderTransactionSysNo from DsOrder tmp where a.transactionsysno=tmp.OrderTransactionSysNo and charindex(tmp.MallOrderId,@p0p" + i + ")>0)";
            //    paras.Add(filter.MallOrderId);
            //    i++;
            //}
            if (filter.NonInvalidStatus != null)
            {
                where += " and a.status <> @p0p" + i;
                paras.Add((int)OrderStatus.销售单状态.作废);
                i++;
            }
            if (filter.OrderSysNo != null)
            {
                where += " and a.sysno = @p0p" + i;
                paras.Add(filter.OrderSysNo);
                i++;
            }
            //if (!string.IsNullOrWhiteSpace(filter.MallShopName))
            //{
            //    where +=
            //        " and exists (" +
            //        " select distinct tmp.OrderTransactionSysNo from DsOrder tmp " +
            //        " left join DsDealerMall tmpMall on tmp.DealerMallSysNo=tmpMall.SysNo " +
            //        " where a.transactionsysno=tmp.OrderTransactionSysNo and charindex(tmpMall.ShopName,@p0p" + i + ")>0)";
            //    paras.Add(filter.MallShopName);
            //    i++;
            //}
            //增加仓库查询条件 余勇 2014-07-31
            if (filter.WarehouseSysNo != null)
            {
                where += " and a.DefaultWarehouseSysNo = @p0p" + i;
                paras.Add(filter.WarehouseSysNo);
                i++;
            }
            //增加付款时间搜索条件 王耀发 2016-2-17
            if (filter.PayBeginDate != null)
            {
                where += " and a.PayDate>=@p0p" + i;
                paras.Add(filter.PayBeginDate);
                i++;
            }
            if (filter.PayEndDate != null)
            {
                where += " and a.PayDate<@p0p" + i;
                paras.Add(filter.PayEndDate);
                i++;
            }
            ////增加第三方付款时间搜索条件 罗勤尧 2017-5-3
            //if (filter.PayBeginDate != null)
            //{
            //    where += " or so.PayTime>=@p0p" + i;
            //    paras.Add(filter.PayBeginDate);
            //    i++;
            //}
            //if (filter.PayEndDate != null)
            //{
            //    where += " and so.PayTime<@p0p" + i;
            //    paras.Add(filter.PayEndDate);
            //    i++;
            //}
            if (filter.PayStatus != null)
            {
                if (filter.PayStatus == 10)
                {
                    where += " and (a.PayStatus = @p0p" + i + " and a.Status>0  and a.Status<30) ";
                }
                else
                {
                    where += " and a.PayStatus = @p0p" + i;
                }

                //where += " and a.PayStatus = @p0p" + i;
                paras.Add(filter.PayStatus);
                i++;
            }
            //判断是否属于第三方订单 王耀发 2016-4-26 创建
            if (filter.Supply != 0)
            {
                where += " and g.Supply = @p0p" + i;
                paras.Add(filter.Supply);
                i++;
            }

            if (!string.IsNullOrWhiteSpace(filter.ErpCode))
            {
                where += " and p.ErpCode= @p0p" + i;
                paras.Add(filter.ErpCode);
                i++;
            }
            //是否绑定所有仓库
            if (filter.HasAllWarehouse)
            {
                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        where += " and dea.SysNo = " + filter.DealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                    }
                }
                if (filter.SelectedAgentSysNo != -1)
                {
                    if (filter.SelectedDealerSysNo != -1)
                    {
                        where += " and dea.SysNo = " + filter.SelectedDealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.SelectedAgentSysNo;
                    }
                }
            }
            else
            {

                if (filter.Warehouses != null && filter.Warehouses.Count > 0)
                {
                    var wList = "";
                    foreach (var w in filter.Warehouses)
                    {
                        if (wList == "")
                            wList = w.SysNo.ToString();
                        else
                            wList += ',' + w.SysNo.ToString();
                    }
                    wList = "(" + wList + ")";
                    where += " and a.DefaultWarehouseSysNo in " + wList;
                }
                else
                {
                    //where += " and a.DefaultWarehouseSysNo = -1";
                }


            }
            sql = string.Format(sql, (int)SystemStatus.任务对象类型.客服订单审核, (int)SystemStatus.任务对象类型.客服订单提交出库, where);
            #endregion

            var dataList = Context.Select<CBSoOrder>(@"a.SysNo,a.OrderNo,a.Status,g.BackWarehouseName,a.CashPay ,a.DeliveryRemarks,a.ImgFlag
                                  ,a.DefaultWarehouseSysNo as WhStockOutSysNo
                                  ,d.Name  as ReceiveName,b.account  as customerAccount 
                                  ,b.sysNo as CustomerId,
                                   d.MobilePhoneNumber as ReceiveTel,j.DeliveryTypeName ,
                                   i.PaymentName ,
                                   a.OrderSource,a.CreateDate
                                   ,f.UserName  as AuditorName,
                                   a.PayStatus,dea.DealerName ,
                                   a.PayDate  as PaymentDate ").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            var dataIdList = Context.Select<int>(@"a.SysNo ").From(sql);
            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            dataIdList.Parameters(paras);
            var idlist = dataIdList.QueryMany();
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("a.CREATEDATE desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
            pager.IdRows = idlist;
        }
        /// <summary>
        /// 订单分页
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询</param>
        /// <returns></returns>
        /// <remarks>2013-11-12 朱家宏 创建</remarks>
        [Obsolete("短路版")]
        private void _GetSoOrders(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            const string sql =
                @"soOrder a 
                                   left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO                                                                                                 --会员表
                                   left join SyJobPool c on c.tasksysno=a.sysno                                                                                                         --订单池
                                   left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo                                                                                        --收货地址表 
                                   left join SyUser e on e.sysno=a.OrderCreatorSysno                                                                                                    --系统用户表
                                   left join SyUser f on f.sysno=a.AuditorSysNo                                                                                                         --系统用户表
                                   left join WhwareHouse g on a.DefaultWarehouseSysNo = g.SYSNO                                                                                         --仓库表
                                   left join WhwareHouse h on a.ordersourcesysno = h.SYSNO                                                                                              --门店信息(仓库表 仓库类型:20)
                                   left join bsPaymentType i on a.PayTypeSysNo=i.sysNo                                                                                                  --支付方式
                                   left join LgDeliveryType j on a.DeliveryTypeSysNo=j.sysNo                                                                                            --配送方式       
                                   left join syUser k on k.sysno = c.assignersysno                                                                                                      --系统用户表(分配人姓名)
                  where 
                                   (@0 is null or a.SysNo=@0) and                                                                                                     --订单号
                                   (@1 is null or a.status=@1) and                                                                                                  --订单状态
                                   (@2 is null or charindex(b.name,@2)>0) and                                                                                         --会员姓名
                                   (@3 is null or a.OrderSource=@3) and                                                                                             --订单来源
                                   (@4 is null or a.PayTypeSysNo=@4) and                                                                                          --支付类型
                                   (@5 is null or a.DeliveryTypeSysNo=@5) and                                                                           --配送类型
                                   (@6 is null or exists (select 1 from soOrderItem b where b.ordersysno=a.sysno and charindex(b.productname,@6)>0)) and                --商品名称
                                   (@7 is null or exists (select 1 from WhStockOut b where b.TransactionSysNo=a.TransactionSysNo and b.SysNo=@7)) and       --出库单号
                                   (@8 is null or a.OrderAmount>=@8) and                                                                                      --订单总额下限
                                   (@9 is null or a.OrderAmount<=@9) and                                                                                      --订单总额上限 
                                   (@10 is null or a.createDate>=@10) and                                                                                                 --订单创建日期(起)
                                   (@11 is null or a.createDate<@11) and                                                                                                      --订单创建日期(止) 
                                   (@12 is null or b.MobilePhoneNumber=@12) and                                                                                 --会员手机号
                                   (@13 is null or c.ExecutorSysNo=@13) and                                                                                       --我的订单条件
                                   (@14 is null or exists (select 1 from splitstr(@14,',') tmp where tmp.col = c.TaskType)) and                           --“我的订单”包括@客服订单出库 客服订单审核
                                   (@15 is null or charindex(d.name,@15)>0) and                                                                                           --收货人
                                   (@16 is null or (d.PhoneNumber=@16 or d.MobilePhoneNumber=@16)) and                                                          --收货电话
                                   (@17 is null or charindex(e.UserName,@17)>0) and                                                                                     --订单创建人
                                   (@18 is null or charindex(f.UserName,@18)>0) and                                                                                               --订单审核人
                                   (@19 is null or a.DefaultWarehouseSysno=@19) and                                                               --缺货订单 0
                                   (@20 is null or a.DeliveryTypeSysNo!=@20) and                                                          --排除的配送类型
                                   (@21 is null or exists (select 1 from splitstr(@21,',') tmp where tmp.col = h.sysNo)) and                          --门店筛选
                                   (@22 is null or b.Account=@22) and                                                                                                 --会员会员号
                                   (@23 is null or b.SysNo=@23) and                                                                                                       --会员编号
                                   (@24 is null or exists (select distinct tmp.TransactionSysNo from LgSettlementItem tmp where a.transactionsysno=tmp.transactionsysno and tmp.Status=@24))   --结算单状态
                order by a.CREATEDATE desc";

            var dataList = Context.Select<CBSoOrder>(@"a.SysNo,a.Status,a.TransactionSysNo,g.WarehouseName,a.DefaultWarehouseSysNo as WhStockOutSysNo,a.CashPay,
                                   b.Name as CustomerName,d.Name as ReceiveName,b.account as customerAccount,
                                   d.MobilePhoneNumber as ReceiveTel,j.DeliveryTypeName,
                                   i.PaymentName,a.OrderAmount,floor(a.CashPay) as Point,
                                   a.OrderSource,a.CreateDate,g.CreatedDate as StockOutDate,a.AuditorDate,
                                   a.LastUpdateDate,k.username as AssignName,'' as ApplyName,f.UserName as AuditorName,c.SysNo as JobSysNo,
                                   e.UserName as OrderCreator").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            var storeSysNos = filter.StoreSysNoList == null ? null : string.Join(",", filter.StoreSysNoList);
            var taskTypes = filter.TaskTypes == null ? null : string.Join(",", filter.TaskTypes);

            var paras = new object[]
                {
                    filter.OrderSysNo, //filter.OrderSysNo,
                    filter.OrderStatus,// filter.OrderStatus,
                    filter.CustomerName, //filter.CustomerName,
                    filter.OrderSource,// filter.OrderSource,
                    filter.PayTypeSysNo,// filter.PayTypeSysNo,
                    filter.DeliveryTypeSysNo, //filter.DeliveryTypeSysNo,
                    filter.ProductName, //filter.ProductName,
                    filter.WhStockOutSysNo, //filter.WhStockOutSysNo,
                    filter.MinOrderAmount, //filter.MinOrderAmount,
                    filter.MaxOrderAmount, //filter.MaxOrderAmount,
                    filter.BeginDate, //filter.BeginDate,
                    filter.EndDate, //filter.EndDate,
                    filter.CustomerMobile, //filter.CustomerMobile,
                    filter.ExecutorSysNo,// filter.ExecutorSysNo,
                    taskTypes, //taskTypes,
                    filter.ReceiveName,// filter.ReceiveName,
                    filter.ReceiveTel, //filter.ReceiveTel, filter.ReceiveTel,
                    filter.OrderCreator,// filter.OrderCreator,
                    filter.Auditor, filter.Auditor,
                    //filter.DefaultWarehouseSysNoList, filter.DefaultWarehouseSysNoList,
                    filter.ExceptedDeliveryTypeSysNo, //filter.ExceptedDeliveryTypeSysNo,
                    storeSysNos, storeSysNos,
                    filter.CustomerAccount, //filter.CustomerAccount,
                    filter.CustomerSysNo, //filter.CustomerSysNo,
                    filter.SettlementStatus//, filter.SettlementStatus
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("a.CREATEDATE desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

        }

        /// <summary>
        /// 通过事物编号来获取订单
        /// </summary>
        /// <param name="value">事务编号</param>
        /// <returns>订单</returns>
        /// <remarks> 2013-09-06 朱家宏 创建</remarks>
        public override SoOrder GetByTransactionSysNo(string value)
        {
            return
                Context.Sql("select * from soorder where TransactionSysNo=@0", value)
                       .QuerySingle<SoOrder>();
        }

        /// <summary>
        /// 通过出库单号来获取订单
        /// </summary>
        /// <param name="outStockSysNo">出库单号</param>
        /// <returns>订单</returns>
        /// <remarks> 2013-09-13 郑荣华 创建</remarks>
        public override SoOrder GetByOutStockSysNo(int outStockSysNo)
        {
            return
                Context.Sql(
                    "select t.* from soorder t inner join WhStockOut a on a.ordersysno=t.sysno where a.sysno=@0",
                    outStockSysNo)
                       .QuerySingle<SoOrder>();
        }

        #endregion

        #region [订单详情]
        /// <summary>
        /// 获取订单时间搓
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>时间戳</returns>
        ///<remarks>2013-11-7 朱成果 创建</remarks>
        public override DateTime GetOrderStamp(int sysNo)
        {
            return Context.Sql("select Stamp from soorder where sysno=@0", sysNo).QuerySingle<DateTime>();
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单详情</returns>
        ///<remarks>2013-06-13 朱成果 创建
        ///2014-5-14 杨文兵 在DAL实现层有几个地方对此方式进行了引用，需要重构删除移除。
        /// </remarks>
        public override SoOrder GetEntity(int sysNo)
        {
            return Context.Sql("select * from soorder where sysno=@0", sysNo).QuerySingle<SoOrder>();
        }
        /// <summary>
        /// 获取豆沙包签名需要的参数
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>2017-07-06 罗熙 创建</returns>
        public override Signparameter GetSignparameter(int sysNo)
        {
            return Context.Sql("select s.OrderAmount as TotalAmount ,c.Name as BuyerName,c.MobilePhoneNumber as BuyerMobile from SoOrder s ,CrCustomer c where  s.SysNo=@0 and c.SysNo =s.CustomerSysNo", sysNo).QuerySingle<Signparameter>();
            
        }
        /// <summary>
        /// 豆沙包获取配送方式，身份证，总价，(运费)，下单时间，物流单号，重量，物流单号，运费已删除
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>2017-07-07 罗熙 创建</returns>
        public override DouShabaoOrderParameter Getotherparameter(int sysNo)
        {
            return Context.Sql("select l.DeliveryTypeName as ExpressChannel,cr.IDCardNo as IdCard,s.OrderAmount as TotalAmount,s.CreateDate as OrderTime,s.OrderNo as PurchasOrderNo,pp.GrosWeight as TotalWeight,pp.SalesMeasurementUnit as SalesMeasurementUnit,ld.ExpressNo as ExpressNo from LgDeliveryType l,SoOrder s, CrCustomer c,CrReceiveAddress cr,PdProduct pp,SoOrderItem si,LgDeliveryItem ld where  l.SysNo=s.DeliveryTypeSysNo and c.SysNo =s.CustomerSysNo and c.SysNo=cr.CustomerSysNo and si.OrderSysNo=s.SysNo and pp.SysNo=si.ProductSysNo and ld.TransactionSysNo = s.TransactionSysNo and s.SysNo=" + sysNo).QuerySingle<DouShabaoOrderParameter>();
        }
        //select l.DeliveryTypeName as ExpressChannel,cr.IDCardNo  as IdCard,s.OrderAmount  as TotalAmount,s.CreateDate as OrderTime,,s.OrderNo as PurchasOrderNo from LgDeliveryType l,SoOrder s, CrCustomer c,CrReceiveAddress cr where s.SysNo=35245 and l.SysNo=s.DeliveryTypeSysNo and c.SysNo =S.CustomerSysNo and c.SysNo=cr.CustomerSysNo
        /// <summary>
        /// 获取商品列表所需要的参数
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-07-10 罗熙 创建</returns>
        public override ProductList GetProductlist(int sysNo)
        {
            return Context.Sql("select pb.Name as ProductBrand,si.ProductName as ProductName,si.Quantity as ProductNum,si.SalesUnitPrice as ProductPrice,pc.CategoryName as ProductCategory,ba.AreaName as DestinationCity from PdBrand pb,PdProduct pp,SoOrderItem si,SoOrder so,PdCategoryAssociation pa,PdCategory pc,CrReceiveAddress cr,CrCustomer cc,BsArea ba where pp.BrandSysNo=pb.SysNo and so.SysNo=@0 and si.OrderSysNo =so.SysNo and si.ProductSysNo = pp.Sysno and pp.SysNo=pa.ProductSysNo and pa.CategorySysNo=pc.SysNo and cc.SysNo= so.CustomerSysNo and cr.CustomerSysNo=cc.SysNo and ba.SysNo =(select ba.ParentSysNo from BsArea ba where ba.SysNo=cr.AreaSysNo)", sysNo).QuerySingle<ProductList>();
        }
        /// <summary>
        /// 获取订单详情列表商品图片
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单商品图片</returns>
        ///<remarks>2016-08-04 罗远康 创建</remarks>
        public override IList<PdProductImage> GetOrderProductImgUrl(int sysNo)
        {
            const string sql = @"SELECT PPI.ImageUrl
                                FROM  PdProductImage AS PPI LEFT JOIN SoOrderItem AS SOI ON PPI.ProductSysNo=SOI.ProductSysNo
                                WHERE PPI.Status=1 AND SOI.OrderSysNo=@0";
            var list = Context.Sql(sql, sysNo).QueryMany<PdProductImage>();

            //返回结果集
            return list;
        }
        /// <summary>
        /// 根据订单编号获取订单详情
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-9 杨浩 创建</remarks>
        public override SoOrder GetOrderByOrderNo(string orderNo)
        {
            return Context.Sql("select * from soorder where orderNo=@0", orderNo).QuerySingle<SoOrder>();
        }

        /// <summary>
        /// 根据订单编号获取订单详情利嘉模板
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2017-5-18 罗勤尧 创建</remarks>
        public override LiJiaOrderModel GetLiJiaOrderByOrderNo(int orderNo)
        {
            LiJiaOrderModel lijiamodel = new LiJiaOrderModel();

            lijiamodel = Context.Sql(@"select distinct a.SysNo as SourceOrderNo,dea.UserSysNo as MemberId,dea.DealerName as MemberName,d.Name as ReceiptName,d.MobilePhoneNumber as ReceiptPhoneNumber,d.IDCardNo as IDNumber,(q2.AreaName +' ' +q1.AreaName +' '+ q.AreaName+''+d.StreetAddress) as ReceiptAddress
 ,a.DeliveryRemarks as Memo,a.CreateDate as OrderCreateTime,a.FreightAmount as ShippingTotalAmount,a.OrderAmount as OrderTotalAmount,
 (a.ProductAmount+a.ProductChangeAmount-a.ProductDiscountAmount) as GoodsTotalAmount,a.OrderAmount as OrderPaymentAmount,
                                   fn.CreatedDate as OrderPaymentTime,so.PayTime,
                                   i.PaymentName as OrderPaymentType,[dbo].[func_GetVoucherNo](OITEM.OrderSysNo) as OrderPaymentNo,a.FreightAmount as ForeignShippingPrice,'CNY' as ForeignCurency,1 as ForeignRate
                                   ,a.OrderAmount as ForeignTotalAmount
                                   from soOrder a 
                                   INNER JOIN SoOrderItem OITEM ON OITEM.OrderSysNo = a.SysNo
                                   left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO                                                              
                                   left join SyJobPool c on c.tasksysno=a.sysno and (c.tasktype=10 or c.tasktype=15)                                                                                                        --订单池
                                   left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo                                                                                        --收货地址表 
                                   left join BsArea q on d.AreaSysNo=q.SysNo
								   left join BsArea q1 on q.ParentSysNo=q1.SysNo
								   left join BsArea q2 on q1.ParentSysNo=q2.SysNo                                         
                                   left join bsPaymentType i on a.PayTypeSysNo=i.sysNo                                                                                                  --支付方式
                                   left join  DsOrder so on so.OrderTransactionSysNo =a.TransactionSysNo 
                                    left join DsDealer dea on a.DealerSysNo = dea.SysNo                                  
                                   left join 
                                   (
                                   
                                    select max(fi.CreatedDate) as CreatedDate,fn.SourceSysNo
                                    from FnReceiptVoucher fn  left join FnReceiptVoucherItem as fi on fn.SysNo=fi.ReceiptVoucherSysNo 
                                    where fi.[Status]=1
                                    group by fn.SourceSysNo

                                   ) as fn on fn.SourceSysNo = a.SysNo         where a.SysNo=@0", orderNo).QuerySingle<LiJiaOrderModel>();
            var sql = @"SoOrderItem ";
            lijiamodel.OrderItem = Context.Sql(@"select p.ErpCode as GPluNo,SoOrderItem.ProductName as ProductName,Quantity as ProductCount,SalesUnitPrice as ProductPrice,(SalesAmount+ChangeAmount) as ProductTotalAmount,
 'CNY' as ForeignCurency , 1 as ForeignRate ,(SalesAmount+ChangeAmount) as ForeignTotalAmount,SalesUnitPrice as ForeignPrice from SoOrderItem
 left join PdProduct p on p.SysNo= ProductSysNo where OrderSysNo=@0", orderNo).QueryMany<LiJiaOrderItem>();
            //lijiamodel.OrderItem=
            return lijiamodel;
        }
        /// <summary>
        /// 仅仅返回订单的时间戳和金额相关的信息
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单金额相关字段</returns>
        ///<remarks>2013-06-13 朱成果 创建</remarks>
        public override SoOrderAmount GetSoOrderAmount(int sysNo)
        {
            //string str = string.Empty;
            //Type t = typeof(SoOrderAmount);
            //foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Public))
            //{
            //    if (!string.IsNullOrEmpty(str))
            //    {
            //        str += ",";
            //    }
            //    str += pi.Name;
            //}
            return Context.Sql(@"select 
                                ProductAmount,
                                ProductDiscountAmount,
                                ProductChangeAmount,
                                CouponAmount,
                                FreightDiscountAmount,
                                FreightChangeAmount,
                                OrderDiscountAmount,
                                OrderAmount,
                                FreightAmount,
                                CashPay,
                                CoinPay,
                                SysNo,
                                Stamp 
                                from soorder 
                                where sysno=@0", sysNo).QuerySingle<SoOrderAmount>();
        }

        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns>订单状态</returns>
        /// <remarks>2013-08-29 朱成果  创建</remarks>
        public override int GetOrderStatus(int orderId)
        {
            return Context.Sql("select Status from soorder where sysno=@0", orderId).QuerySingle<int>();
        }

        #endregion

        #region 更新订单

        /// <summary>
        /// 更新订单主表
        /// </summary>
        /// <param name="soOrder">实体</param>
        /// <returns>true成功 false失败</returns>
        /// <remarks>2013-06-14 朱家宏 创建</remarks>
        public override bool Update(SoOrder soOrder)
        {
            soOrder.Stamp = GetOrderStamp(soOrder.SysNo);
            if (soOrder.Stamp == DateTime.MinValue)
            {
                soOrder.Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (soOrder.PayDate == DateTime.MinValue)
            {
                soOrder.PayDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var r = Context.Update("SoOrder", soOrder)
                           .AutoMap(o => o.SysNo, o => o.Customer, o => o.ReceiveAddress, o => o.OrderItemList,
                                    o => o.OrderInvoice,o=>o.OnlinePayment)
                           .Where("SysNo", soOrder.SysNo)
                           .Execute();
            return (r > 0);
        }

        /// <summary>
        /// 更新销售单状态值
        /// </summary>
        /// <param name="orderId">销售单编号</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// <remarks>2013-06-25 朱成果  创建</remarks>
        public override void UpdateOrderStatus(int orderId, int status)
        {
            Context.Sql("update soorder set status=@status where SysNo=@SysNo")
                   .Parameter("status", status)
                   .Parameter("SysNo", orderId).Execute();
        }

        /// <summary>
        /// 更新销售单支付时间
        /// </summary>
        /// <param name="orderId">销售单编号</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        /// <remarks>2017-09-15 罗勤尧  创建</remarks>
        public override void UpdateOrderPayDte(int orderId, DateTime PayDate)
        {
            Context.Sql("update soorder set PayDate=@PayDate where SysNo=@SysNo")
                   .Parameter("PayDate", PayDate)
                   .Parameter("SysNo", orderId).Execute();
        }

        /// <summary>
        /// 同步销售单支付时间
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-09-15 罗勤尧  创建</remarks>
        public override void UpdateAllOrderPayDte()
        {
            Context.Sql("update soOrder  set PayDate=(select max(fi.CreatedDate) as CreatedDate from FnReceiptVoucher fn  left join FnReceiptVoucherItem as fi on fn.SysNo=fi.ReceiptVoucherSysNo where fi.[Status]=1 and fn.SourceSysNo =soOrder.SysNo group by fn.SourceSysNo)").Execute();
        }

        /// <summary>
        ///同步指定销售单支付时间
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-09-15 罗勤尧  创建</remarks>
        public override void UpdateOrderPayDteById(int orderId)
        {
            Context.Sql("update soOrder  set PayDate=(select max(fi.CreatedDate) as CreatedDate from FnReceiptVoucher fn  left join FnReceiptVoucherItem as fi on fn.SysNo=fi.ReceiptVoucherSysNo where fi.[Status]=1 and fn.SourceSysNo =soOrder.SysNo group by fn.SourceSysNo) where SysNo=@SysNo").Parameter("SysNo", orderId).Execute();
        }
        /// <summary>
        /// 更新销售单发送状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="SendStatus"></param>
        /// <remarks>2015-09-17 王耀发  创建</remarks>
        public override void UpdateOrderSendStatus(int orderId, int SendStatus)
        {
            Context.Sql("update soorder set SendStatus=@SendStatus where SysNo=@SysNo")
                   .Parameter("SendStatus", SendStatus)
                   .Parameter("SysNo", orderId).Execute();
        }

        #endregion

        #region 添加订单主表

        /// <summary>
        /// 插入销售单主表
        /// </summary>
        /// <param name="entity">销售单主表实体</param>
        /// <returns>销售单主表实体（带编号）</returns>
        /// <remarks>2013-06-27 黄志勇  创建</remarks>
        public override SoOrder InsertEntity(SoOrder entity)
        {
            #region 2016-9-8 杨浩 注释
            ////当前用户对应分销商SysNo
            //CBDsDealer Dealer = IDsDealerDao.Instance.GetDsDealerByUserNo(entity.OrderCreatorSysNo);
            //CBDsDealer varDealer = new CBDsDealer();
            ////当前用户没有经销商时，默认为信营全球购，2015-12-28 王耀发 创建
            //if (Dealer == null)
            //{
            //    varDealer = IDsDealerDao.Instance.GetDsDealer(0);
            //}
            //else
            //{
            //    varDealer = Dealer;
            //}
            ////添加订单对应当前用户的分销商SysNo 2015-12-11 王耀发 创建
            //entity.DealerSysNo = varDealer.SysNo;
            #endregion

            if (entity.AuditorDate == DateTime.MinValue)
            {
                entity.AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (entity.CancelDate == DateTime.MinValue)
            {
                entity.CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (entity.Stamp == DateTime.MinValue)
            {
                entity.Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (entity.PayDate == DateTime.MinValue)
            {
                entity.PayDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            entity.SysNo = Context.Insert("SoOrder", entity)
                                  .AutoMap(o => o.SysNo, o => o.TransactionSysNo, o => o.Customer, o => o.ReceiveAddress,
                                           o => o.OrderItemList, o => o.OrderInvoice, o => o.Stamp, o => o.SysNo,o=>o.OnlinePayment)
                                  .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }

        /// <summary>
        /// 插入订单数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2016-06-09 王耀发  创建</remarks>
        public override SoOrder Inser(SoOrder entity)
        {
            if (entity.PayDate == DateTime.MinValue)
            {
                entity.PayDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (entity.PayDate == DateTime.MinValue)
            {
                entity.PayDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            entity.SysNo = Context.Insert("SoOrder", entity)
                      .AutoMap(o => o.SysNo, o => o.TransactionSysNo, o => o.Customer, o => o.ReceiveAddress,
                               o => o.OrderItemList, o => o.OrderInvoice, o => o.Stamp, o => o.SysNo, o => o.OnlinePayment)
                      .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }

        #endregion

        #region 更新订单前台显示状态

        /// <summary>
        /// 根据订单号更新订单前台显示状态
        /// </summary>
        /// <param name="orderID">订单号</param>
        /// <param name="onlineStatus">前台显示状态</param>
        /// <returns></returns>
        /// <remarks>2013-07-04 朱成果  创建</remarks> 
        public override void UpdateOnlineStatusByOrderID(int orderID, string onlineStatus)
        {
            Context.Sql("update soorder set onlinestatus=@onlineStatus where SysNo=@SysNo")
                   .Parameter("onlineStatus", onlineStatus)
                   .Parameter("SysNo", orderID).Execute();
        }

        /// <summary>
        /// 根据事物编号更新订单前台显示状态
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        ///  <param name="onlineStatus">前台显示状态</param>
        /// <returns></returns>
        /// <remarks>2013-07-04 朱成果  创建</remarks> 
        public override void UpdateOnlineStatusByTransactionSysNo(string transactionSysNo, string onlineStatus)
        {
            Context.Sql("update soorder set onlinestatus=@onlineStatus where transactionsysno=@transactionsysno")
                   .Parameter("onlineStatus", onlineStatus)
                   .Parameter("transactionsysno", transactionSysNo).Execute();
        }

        #endregion

        #region 出库单查询

        /// <summary>
        /// 出库单分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>可分页的出库单表</returns>
        /// <remarks>2013-07-06 朱家宏 创建</remarks>
        public override Pager<CBOutStockOrder> GetOutStockOrders(ParaOutStockOrderFilter filter)
        {
            var pager = new Pager<CBOutStockOrder>
                {
                    CurrentPage = filter.Id
                };

            var sql =
                @"(select  a.sysno as StockOutSysNo,a.ordersysno,a.receivable,a.status as StockOutStatus,
                           b.CREATEDATE as orderCreateDate,
                           c.name as receiverName,c.mobilephonenumber as receiverMobile,
                           d.account as customerAccount,d.sysNo as customerSysNo,
                           e.warehousename,a.LastUpdateDate
                   from whstockout a 
                           inner join soOrder b on b.sysno=a.ordersysno
                           left join SoReceiveAddress c on c.sysno=a.ReceiveAddressSysNo
                           left join crCustomer d on d.sysno=b.customersysno
                           inner join WhwareHouse e on a.warehousesysno = e.SYSNO
                   where
                   {0}           
                  ) tb";

            var exceptedStatuses = filter.StockOutStatusListExcepted == null
                                       ? null
                                       : string.Join(",", filter.StockOutStatusListExcepted);
            var deliveryTypeSysNos = filter.DeliveryTypeSysNoList == null
                                         ? null
                                         : string.Join(",", filter.DeliveryTypeSysNoList);

            #region 构造sql
            var paras = new ArrayList();
            var where = "1=1 ";
            int i = 0;
            if (!string.IsNullOrWhiteSpace(deliveryTypeSysNos))
            {
                where +=
                    " and exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col=a.deliverytypesysno)";
                paras.Add(deliveryTypeSysNos);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ReceiverTel))
            {
                where += " and (c.PhoneNumber=@p0p" + i + " or c.MobilePhoneNumber=@p0p1)";
                paras.Add(filter.ReceiverTel);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.ReceiverName))
            {
                where += " and charindex(c.name,@p0p" + i + ")>0";
                paras.Add(filter.ReceiverName);
                i++;
            }
            if (filter.OrderSysNo != null)
            {
                where += " and b.sysNo=@p0p" + i;
                paras.Add(filter.OrderSysNo);
                i++;
            }
            if (filter.WhStockOutSysNo != null)
            {
                where += " and a.sysNo=@p0p" + i;
                paras.Add(filter.WhStockOutSysNo);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.CustomerAccount))
            {
                where += " and d.account=@p0p" + i;
                paras.Add(filter.CustomerAccount);
                i++;
            }
            if (filter.OrderBeginDate != null)
            {
                where += " and b.createDate>=@p0p" + i;
                paras.Add(filter.OrderBeginDate);
                i++;
            }
            if (filter.OrderEndDate != null)
            {
                where += " and b.createDate<@p0p" + i;
                paras.Add(filter.OrderEndDate);
                i++;
            }
            if (filter.PickUpBeginDate != null)
            {
                where += " and a.PickUpDate>=@p0p" + i;
                paras.Add(filter.PickUpBeginDate);
                i++;
            }
            if (filter.PickUpEndDate != null)
            {
                where += " and a.PickUpDate<@p0p" + i;
                paras.Add(filter.PickUpEndDate);
                i++;
            }
            if (filter.StoreSysNoList != null)
            {
                var storeSysNos = filter.StoreSysNoList == null ? null : string.Join(",", filter.StoreSysNoList);
                where +=
                    " and exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = e.sysNo)";
                paras.Add(storeSysNos);
                i++;
            }
            if (filter.StockOutStatus != null)
            {
                where += " and a.Status=@p0p" + i;
                paras.Add(filter.StockOutStatus);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(exceptedStatuses))
            {
                where +=
                    " and not exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = a.Status)";
                paras.Add(exceptedStatuses);
                i++;
            }
            if (filter.SignTime != null)
            {
                where += " and a.SignTime>=@p0p" + i;
                paras.Add(filter.SignTime);
                i++;
            }

            sql = string.Format(sql, where);
            #endregion

            var dataList = Context.Select<CBOutStockOrder>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            pager.TotalRows = dataCount.QuerySingle();

            pager.Rows =
                dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            return pager;
        }

        /// <summary>
        /// 出库单分页
        /// </summary>
        /// <param name="filter">查询</param>
        /// <returns>分页</returns>
        /// <remarks>2013-07-06 朱家宏 创建</remarks>
        [Obsolete]
        private Pager<CBOutStockOrder> _GetOutStockOrders(ParaOutStockOrderFilter filter)
        {
            var pager = new Pager<CBOutStockOrder>
            {
                CurrentPage = filter.Id
            };

            const string sql =
                @"(select  a.sysno as StockOutSysNo,a.ordersysno,a.receivable,a.status as StockOutStatus,
                           b.CREATEDATE as orderCreateDate,
                           c.name as receiverName,c.mobilephonenumber as receiverMobile,
                           d.account as customerAccount,d.sysNo as customerSysNo,
                           e.warehousename,a.LastUpdateDate
                   from whstockout a 
                           inner join soOrder b on b.sysno=a.ordersysno
                           left join SoReceiveAddress c on c.sysno=a.ReceiveAddressSysNo
                           left join crCustomer d on d.sysno=b.customersysno
                           inner join WhwareHouse e on a.warehousesysno = e.SYSNO
                   where
                           (@0 is null or exists (select 1 from splitstr(@0,',') tmp where tmp.col=a.deliverytypesysno)) and      --配送方式
                           (@1 is null or (c.PhoneNumber=@1 or c.MobilePhoneNumber=@1)) and     --收货电话  
                           (@2 is null or charindex(c.name,@2)>0) and                                    --收货人姓名
                           (@3 is null or b.sysNo=@3) and                                                --订单号
                           (@4 is null or a.sysNo=@4) and                                      --出库单号
                           (@5 is null or d.account=@5) and                                    --会员号
                           (@6 is null or b.createDate>=@6) and                                  --订单创建日期(起)
                           (@7 is null or b.createDate<@7) and                                       --订单创建日期(止) 
                           (@8 is null or a.PickUpDate>=@8) and                                --自提时间(起)
                           (@9 is null or a.PickUpDate<@9) and                                     --自提时间(止) 
                           (@10 is null or exists (select 1 from splitstr(@10,',') tmp where tmp.col = e.sysNo)) and                              --门店筛选
                           (@11 is null or a.Status=@11) and                                       --出库单状态             
                           (@12 is null or not exists (select 1 from splitstr(@12,',') tmp where tmp.col = a.Status)) and                  --出库单状态不包含筛选
                           (@13 is null or a.SignTime>=@13)                                                    --签收日期 
                 order by  a.LastUpdateDate desc           
                  ) tb";

            var dataList = Context.Select<CBOutStockOrder>("tb.*").From(sql);

            var dataCount = Context.Select<int>("count(0)").From(sql);

            var storeSysNos = filter.StoreSysNoList == null ? null : string.Join(",", filter.StoreSysNoList);
            var exceptedStatuses = filter.StockOutStatusListExcepted == null
                                       ? null
                                       : string.Join(",", filter.StockOutStatusListExcepted);
            var deliveryTypeSysNos = filter.DeliveryTypeSysNoList == null
                                         ? null
                                         : string.Join(",", filter.DeliveryTypeSysNoList);

            //查询日期上限+1
            filter.OrderEndDate = filter.OrderEndDate == null ? (DateTime?)null : filter.OrderEndDate.Value.AddDays(1);
            filter.PickUpEndDate = filter.PickUpEndDate == null
                                       ? (DateTime?)null
                                       : filter.PickUpEndDate.Value.AddDays(1);

            var paras = new object[]
                {
                    deliveryTypeSysNos,
                    filter.ReceiverTel,
                    filter.ReceiverName,
                    filter.OrderSysNo,
                    filter.WhStockOutSysNo,
                    filter.CustomerAccount,
                    filter.OrderBeginDate,
                    filter.OrderEndDate,
                    filter.PickUpBeginDate,
                    filter.PickUpEndDate,
                    storeSysNos,
                    filter.StockOutStatus,
                    exceptedStatuses,
                    filter.SignTime
                };

            dataList.Parameters(paras);

            dataCount.Parameters(paras);

            pager.TotalRows = dataCount.QuerySingle();

            pager.Rows =
                dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            return pager;
        }

        #endregion

        #region 检查订单是否满足完结条件

        /// <summary>
        /// 检查订单是否满足完结条件
        /// </summary>
        /// <param name="sysNo">订单号</param>
        /// <returns>true /flase</returns>
        /// <remarks>2013-07-04 朱成果  创建</remarks> 
        public override bool CheckedOrderFinish(int sysNo)
        {
            bool flg = false;
            int notoutstouckCount =
                Context.Sql(
                    "select count(1) from SoOrderItem where OrderSysNo=@OrderSysNo and Quantity>RealStockOutQuantity")
                       .Parameter("OrderSysNo", sysNo).QuerySingle<int>();
            if (notoutstouckCount == 0) //全部创建出库单
            {
                int count =
                    Context.Sql(
                        "select count(1) from WHSTOCKOUT where  ordersysno=@ordersysno and Status not in(@a,@b,@c,@d,@e)")
                           .Parameter("ordersysno", sysNo)
                           .Parameter("a", (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.已签收)
                           .Parameter("b", (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.作废)
                           .Parameter("c", (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.全部退货)
                           .Parameter("d", (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.部分退货)
                           .Parameter("e", (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.拒收)
                           .QuerySingle<int>();
                if (count == 0) //全部签收
                {
                    flg = true;
                }
            }
            return flg;
        }

        #endregion

        #region 检查订单是否满足作废条件

        /// <summary>
        /// 检查是否满足订单作废的条件
        /// </summary>
        /// <param name="sysNo">订单号</param>
        /// <returns>true/false</returns>
        /// <remarks>2013-08-29  朱成果  创建</remarks> 
        public override bool CheckedOrderCancel(int sysNo)
        {
            int count =
                Context.Sql("select count(1) from WHSTOCKOUT where  ordersysno=@ordersysno and Status not in(@a,@b)")
                       .Parameter("ordersysno", sysNo)
                       .Parameter("a", (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.作废)
                       .Parameter("b", (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.拒收)
                       .QuerySingle<int>();
            return count == 0;
        }

        #endregion

        #region 更新付款状态

        /// <summary>
        /// 更新付款状态
        /// </summary>
        /// <param name="sysNo">订单号</param>
        /// <param name="payStatus">付款状态</param>
        /// <returns></returns>
        /// <remarks>2013-08-29  朱成果  创建</remarks> 
        public override void UpdatePayStatus(int sysNo, int payStatus)
        {
            Context.Sql("update soorder set PayStatus=@PayStatus where SysNo=@SysNo")
                   .Parameter("PayStatus", payStatus)
                   .Parameter("SysNo", sysNo).Execute();

            if (payStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付)
            {
                //支付成功，将待支付状态变成待创建出库单
                Context.Sql("update soorder set Status=@NewStatus where SysNo=@SysNo and Status=@OldNewStatus")
                  .Parameter("NewStatus", (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单)
                  .Parameter("SysNo", sysNo)
                  .Parameter("OldNewStatus", (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待支付)
                  .Execute();
            }
        }

        #endregion

        #region 更新订单发票

        /// <summary>
        /// 更新发票编号
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <param name="invoiceNo">发票编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-30 朱成果  创建</remarks> 
        public override void UpdateInvoiceNo(int sysNo, int invoiceNo)
        {
            Context.Sql("update soorder set InvoiceSysNo=@InvoiceSysNo where SysNo=@SysNo")
                   .Parameter("InvoiceSysNo", invoiceNo)
                   .Parameter("SysNo", sysNo).Execute();
        }

        #endregion

        #region 获取待支付订单数

        /// <summary>
        /// 获取待支付订单数
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>待支付订单数</returns>
        /// <remarks>2013-08-27 周瑜 创建</remarks>
        /// <remarks>2013-10-29 杨浩 重写</remarks>
        public override int GetUnPaidOrderCount(int customerSysNo)
        {
            const string sql = @"select count(1) from soorder a 
                        inner join BsPaymentType b on a.PayTypeSysNo=b.sysno
                        where PayStatus = @PayStatus  and  IsOnlinePay=@IsOnlinePay and customersysno = @customersysno and (a.Status=@Status or a.Status=@Status1)";

            return Context.Sql(sql)
                       .Parameter("PayStatus", OrderStatus.销售单支付状态.未支付)
                       .Parameter("IsOnlinePay", BasicStatus.支付方式是否网上支付.是)
                       .Parameter("customersysno", customerSysNo)
                       .Parameter("Status", OrderStatus.销售单状态.待审核)
                       .Parameter("Status1", OrderStatus.销售单状态.待支付)
                       .QuerySingle<int>();
        }

        #endregion

        #region 获取未评论商品数量

        /// <summary>
        /// 获取未评论商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>未评论商品数量</returns>
        /// <remarks>2013-08-27 周瑜 创建</remarks>
        public override int GetUnCommentsCount(int customerSysNo)
        {
            return Context.Sql(@"select count(1) from SoOrder a inner join SoOrderItem b on a.sysno = b.ordersysno
where a.status = @status and a.customersysno = @customersysno and b.ProductSysNo not in (select ProductSysNo from FeProductComment where isComment=1 and customerSysNo=@customerSysNo)")
                          .Parameter("status", OrderStatus.销售单状态.已完成)
                          .Parameter("customersysno", customerSysNo)
                // .Parameter("customersysno", customerSysNo)
                          .QuerySingle<int>();
        }

        #endregion

        #region 订单编号与订单事物编号

        /// <summary>
        /// 根据订单事物编号返回订单号
        /// </summary>
        /// <param name="transactionSysNo">订单事物编号</param>
        /// <returns>订单编号></returns>
        /// <remarks>2013-09-11 朱成果 创建</remarks>
        public override int GetOrderIDByTransactionSysNo(string transactionSysNo)
        {
            return Context.Sql("select SysNo from soorder where TransactionSysNo=@TransactionSysNo")
                          .Parameter("TransactionSysNo", transactionSysNo)
                          .QuerySingle<int>();
        }

        /// <summary>
        /// 根据订单编号返回事物编号
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>订单编号返回事物编号</returns>
        /// <remarks>2013-09-11 朱成果 创建</remarks>
        public override string GetOrderTransactionSysNoByID(int orderSysNo)
        {

            return Context.Sql("select TransactionSysNo from soorder where SysNo=@SysNo")
                          .Parameter("SysNo", orderSysNo)
                          .QuerySingle<string>();
        }

        #endregion

        #region 订单优惠券

        /// <summary>
        /// 插入订单优惠券关系记录
        /// </summary>
        /// <param name="soCoupon">订单优惠券</param>
        /// <returns>优惠券</returns>
        /// <remarks>2013-09-11 朱成果 创建</remarks>
        public override SoCoupon InsertSoCoupon(SoCoupon soCoupon)
        {
            soCoupon.SysNo = Context.Insert("SoCoupon", soCoupon)
                                    .AutoMap(o => o.SysNo)
                                    .ExecuteReturnLastId<int>("SysNo");
            return soCoupon;
        }

        /// <summary>
        /// 删除订单优惠券关系记录
        /// </summary>
        /// <param name="couponSysNo">优惠券sysno.</param>
        /// <param name="orderSysNo">订单sysno.</param>
        /// <returns></returns>
        ///<remarks>2013-09-11 朱成果 创建</remarks>
        public override void DeleteSoCoupon(int couponSysNo, int orderSysNo)
        {

            string sql = "delete SoCoupon where couponSysNo = @0 and OrderSysNo = @1";
            Context.Sql(sql, couponSysNo, orderSysNo).Execute();

        }

        /// <summary>
        /// 删除订单优惠券关系记录
        /// </summary>
        /// <param name="orderSysNo">订单sysno.</param>
        /// <returns></returns>
        ///<remarks>2013-09-11 朱成果 创建</remarks>
        public override void DeleteSoCoupon(int orderSysNo)
        {
            string sql = "delete SoCoupon where OrderSysNo = @0";
            Context.Sql(sql, orderSysNo).Execute();
        }

        /// <summary>
        /// 根据订单号获取订单优惠券
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>订单优惠券集合</returns>
        /// <remarks>2013-09-13 吴文强 创建</remarks>
        public override List<SpCoupon> GetCouponByOrderSysNo(int orderSysNo)
        {
            const string sql = @"
                                select spc.* 
                                from SpCoupon spc
                                inner join SoCoupon soc on soc.couponsysno = spc.sysno
                                where soc.orderSysNo = @orderSysNo";

            return Context.Sql(sql)
                          .Parameter("orderSysNo", orderSysNo)
                          .QueryMany<SpCoupon>();
        }

        #endregion

        #region 后台首页销售单统计

        /// <summary>
        /// 销售单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="filter">分销商权限信息</param>
        /// <param name="infomation">信息对象</param>
        /// <returns>有效的销售单总数</returns>
        /// <remarks>2013-09-26 邵斌 创建</remarks>
        public override CBDefaultPageCountInfo GetOrderTotalInformation(System.DateTime startTime, System.DateTime endTime,ParaIsDealerFilter filter, ref CBDefaultPageCountInfo infomation)
        {
            //var where = "where a.status<>@status and a.createdate between @startTime and @endTime";
            var where = "where a.status<>@status";
            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    where += " and dea.SysNo = " + filter.DealerSysNo;
                }
                else
                {
                    where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                }
            }

            //统计销售单总数和销售单总金额
            var sql = @"select count(a.sysno) as OrderCount,sum(a.orderamount) as OrderAmount from SoOrder a left join DsDealer dea on a.DealerSysNo = dea.SysNo " + where;
            var info = Context.Sql(sql)
                .Parameter("status", (int)OrderStatus.销售单状态.作废)
                //.Parameter("startTime", startTime)
                //.Parameter("endTime", endTime)
                .QuerySingle<CBDefaultPageCountInfo>();

            infomation.OrderCount = info.OrderCount;
            infomation.OrderAmount = info.OrderAmount;

            return infomation;
        }

        /// <summary>
        /// 销售单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="filter">分销商权限信息</param>
        /// <param name="infomation">信息对象</param>
        /// <returns>有效的销售单总数</returns>
        /// <remarks>2013-09-26 邵斌 创建</remarks>
        public override CBDefaultPageCountInfo GetTodayOrderTotalInformation(System.DateTime startTime, System.DateTime endTime, ParaIsDealerFilter filter,
                                                                     ref CBDefaultPageCountInfo infomation)
        {
            using (var _context = Context.UseSharedConnection(true))
            {
                var where = " where 1=1 ";
                var whereStr = "";
                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        where += " and dea.SysNo = " + filter.DealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                    }
                }

                //统计待审核销售单总数
                var sql = "select count(a.sysno) as RequiredAuditOrderCount from SoOrder a left join DsDealer dea on a.DealerSysNo = dea.SysNo ";
                //whereStr = " and a.status=@status and createdate between @startTime and @endTime";
                whereStr = " and a.status=@status ";
                sql += where + whereStr;

                infomation.RequiredAuditOrderCount =
                    _context.Sql(sql)
                    .Parameter("status",(int)OrderStatus.销售单状态.待审核)
                    //.Parameter("startTime",startTime)
                    //.Parameter("endTime",endTime)
                    .QuerySingle<int>();


                //统计待出库销售单总数
                sql = "select count(a.sysno) as WaitingForDeliveryOrderCount from SoOrder a left join DsDealer dea on a.DealerSysNo = dea.SysNo ";
                //whereStr = " and status in (@status1,@status2) and createdate between @startTime and @endTime ";
                whereStr = " and a.status in (@status1,@status2) ";
                sql += where + whereStr;

                infomation.WaitingForDeliveryOrderCount =
                    _context.Sql(sql)
                    .Parameter("status1", (int)OrderStatus.销售单状态.待创建出库单)
                    .Parameter("status2", (int)OrderStatus.销售单状态.部分创建出库单)
                    //.Parameter("startTime", startTime)
                    //.Parameter("endTime", endTime)
                    .QuerySingle<int>();


                //统计缺货销售单总数
                sql = "select count(a.sysno) as OutOfStockOrderCount from SoOrder a left join DsDealer dea on a.DealerSysNo = dea.SysNo ";
                //whereStr = " and status <> @status1 and status <> @status2 and DefaultWarehouseSysNo > 0  and createdate between @startTime and @endTime ";
                whereStr = " and a.status <> @status1 and a.status <> @status2 and a.DefaultWarehouseSysNo = 0 ";
                sql += where + whereStr;

                infomation.OutOfStockOrderCount = _context.Sql(sql)
                    .Parameter("status1", (int)OrderStatus.销售单状态.待审核)
                    .Parameter("status2", (int)OrderStatus.销售单状态.作废)
                    //.Parameter("startTime", startTime)
                    //.Parameter("endTime", endTime)
                    .QuerySingle<int>();
                return infomation;
            }
        }

        #endregion

        #region 获取未付款、未审核订单
        /// <summary>
        /// 获取预付款、未支付订单
        /// </summary>
        /// <returns>返回未付款、未审核订单列表</returns>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        public override List<SoOrder> GetClearList()
        {
            //const string sql = @"select so.* from SoOrder so left join BsPaymentType bpt on so.PayTypeSysNo=bpt.sysNo where bpt.PaymentType=:PaymentType and so.Status = :Status";
            //return Context.Sql(sql)
            //              .Parameter("PaymentType", (int)BasicStatus.支付方式类型.预付) //支付方式
            //              .Parameter("Status", (int)OrderStatus.销售单状态.待支付)       //支付状态
            //              .QueryMany<SoOrder>();

            const string sql = @"select so.* from SoOrder so left join BsPaymentType bpt on so.PayTypeSysNo=bpt.sysNo where bpt.PaymentType=@PaymentType and so.paystatus = @payStatus and so.Status in(@Status,@Status1)";
            return Context.Sql(sql)
                          .Parameter("PaymentType", (int)BasicStatus.支付方式类型.预付)    //支付方式
                          .Parameter("payStatus", (int)OrderStatus.销售单支付状态.未支付)   //支付状态
                          .Parameter("Status", (int)OrderStatus.销售单状态.待审核)          //销售单状态
                          .Parameter("Status1", (int)OrderStatus.销售单状态.待支付)          //销售单状态
                          .QueryMany<SoOrder>();
        }
        #endregion

        /// <summary>
        /// 更新订单支付方式
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="payType">支付方式</param>
        /// <returns></returns>
        /// <remarks>2013-12-11 黄波 创建</remarks>
        public override void UpdateOrderPayType(int soID, int payType)
        {
            Context.Update("SoOrder").Column("PayTypeSysNo", payType).Where("SysNo", soID).Execute();
        }

        /// <summary>
        /// 查询导出订单列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-1-19 王耀发 创建</remarks>

        public override List<CBOutputSoOrders> GetExportOrderList(List<int> sysNos)
        {

            string sqlStr = "   ";
            //CASE SOR.[PayStatus] WHEN 10 THEN '等待买家付款' WHEN 20 THEN '买家已付款' ELSE '支付异常' END AS 订单状态
            string sqlText = @"SELECT row_number() OVER(order by OITEM.OrderSysNo desc) AS 序号,DEA.DealerName AS 店铺,CUS.Name AS 会员名,SOR.SysNo AS 订单号,SOR.OrderNo AS 销售订单号,LTRIM(RTRIM(ADDR.Name)) AS 订单人姓名,LTRIM(RTRIM(ADDR.IDCardNo)) AS 订单人证件号,
			LTRIM(RTRIM(ADDR.MobilePhoneNumber)) AS 订单人电话,SOR.CreateDate AS 下单时间,FN.CreatedDate AS 付款时间 ,
            Convert(varchar(100),SOR.CreateDate,23) as 销售日期,
            (SOR.CashPay) AS 订单总额 , BsPaymentType.PaymentName as 支付类型,
            
            CASE SOR.[Status] WHEN 10 THEN '待审核' WHEN 20 THEN '待支付' WHEN 30 THEN '待创建出库单' WHEN 40 THEN '部分创建出库单' WHEN 50 THEN '已创建出库单' WHEN 55 THEN '出库待接收' WHEN 100 THEN '已完成' ELSE '作废' END AS 订单状态,
            case SOR.PayStatus when 10 then '未付款' else '已付款' end as 支付状态,
			SOR.CustomerMessage AS 买家留言,dbo.func_GetAereaPath(ADDR.AreaSysNo) + ' ' + LTRIM(RTRIM(ADDR.StreetAddress)) AS 收件人地址,PDP.EasName AS 商品品名,PDP.ErpCode AS 商品货号,PDP.Barcode AS 商品条形码,OITEM.Quantity AS 申报数量,
			OITEM.SalesUnitPrice AS 申报单价,(OITEM.SalesAmount + OITEM.ChangeAmount) AS 申报总价,OITEM.ChangeAmount as 调价金额,(SOR.CouponAmount * -1) AS 优惠劵金额,SOR.FreightAmount AS 运费,[dbo].[func_GetTaxMoney](PDP.Tax,OITEM.SalesAmount,OITEM.Quantity,SOR.TaxFee,SOR.FreightAmount,SOR.SysNo) AS 税款,PDP.GrosWeight AS 毛重,PDP.NetWeight AS 净重,
			[dbo].[func_GetVoucherNo](OITEM.OrderSysNo) AS 交易号,'' AS 买家支付账号,(select top 1 ExpressNo from [LgDeliveryItem]  where right(TransactionSysNo,len(SOR.SysNo))=SOR.SysNo) AS 快递单号,convert(decimal(18, 2),((OITEM.SalesAmount-OITEM.DiscountAmount)/OITEM.Quantity)) AS 商品单价
            FROM SoOrderItem OITEM INNER JOIN SoOrder SOR ON OITEM.OrderSysNo = SOR.SysNo
			LEFT JOIN DsDealer DEA ON SOR.DealerSysNo =  DEA.SysNo
			LEFT JOIN CrCustomer CUS ON SOR.CustomerSysNo =  CUS.SysNo
            LEFT JOIN PdProduct PDP ON OITEM.ProductSysNo = PDP.SysNo
            LEFT JOIN SoReceiveAddress ADDR ON SOR.ReceiveAddressSysNo=ADDR.SysNo 
            LEFT JOIN WhWarehouse WARE ON SOR.DefaultWarehouseSysNo = WARE.SysNo 
            LEFT JOIN FnOnlinePayment FN ON FN.SourceSysNo = SOR.SysNo 
            left join BsPaymentType on BsPaymentType.SysNo=SOR.PayTypeSysNo  ";

            if (sysNos.Count != 0)
            {
                sqlText += " WHERE OITEM.OrderSysNo in (" + string.Join(",", sysNos) + ")";
            }

            List<CBOutputSoOrders> outOrders = Context.Sql(sqlText).QueryMany<CBOutputSoOrders>();

            var q = (from o in outOrders
                     group o by o.订单号).ToList();

            foreach (var gp in q)
            {
                int i = 0;
                foreach (var item in gp)
                {
                    i++;
                    if (i != 1)
                    {
                        item.店铺 = null;
                        item.会员名 = null;
                        item.订单号 = null;
                        item.销售订单号 = null;
                        item.订单人姓名 = null;
                        item.订单人证件号 = null;
                        item.订单人电话 = null;
                        item.销售日期 = null;
                        item.下单时间 = null;
                        item.付款时间 = null;
                        item.订单状态 = null;
                        item.买家留言 = null;
                        item.收件人地址 = null;
                        item.优惠劵金额 = null;
                        item.运费 = null;
                    }
                }
            }

            return outOrders;
        }

        /// <summary>
        /// 查询导出订单列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-3-16 王耀发 创建</remarks>

        public override List<CBOutputSoOrders> GetExportOrderListByDoSearch(ParaOrderFilter filter)
        {
            string sqlText = @"SELECT row_number() OVER(order by OITEM.OrderSysNo desc) AS 序号,DEA.DealerName AS 店铺,CUS.Name AS 会员名,SOR.SysNo AS 订单号,SOR.OrderNo AS 销售订单号,LTRIM(RTRIM(ADDR.Name)) AS 订单人姓名,LTRIM(RTRIM(ADDR.IDCardNo)) AS 订单人证件号,
			LTRIM(RTRIM(ADDR.MobilePhoneNumber)) AS 订单人电话,SOR.CreateDate AS 下单时间,FN.CreatedDate AS 付款时间 ,
            Convert(varchar(100),SOR.CreateDate,23) as 销售日期,
            (SOR.CashPay) AS 订单总额 , BsPaymentType.PaymentName as 支付类型,
            
            CASE SOR.[Status] WHEN 10 THEN '待审核' WHEN 20 THEN '待支付' WHEN 30 THEN '待创建出库单' WHEN 40 THEN '部分创建出库单' WHEN 50 THEN '已创建出库单' WHEN 55 THEN '出库待接收' WHEN 100 THEN '已完成' ELSE '作废' END AS 订单状态,
            case SOR.PayStatus when 10 then '未付款' else '已付款' end as 支付状态,
			SOR.CustomerMessage AS 买家留言,dbo.func_GetAereaPath(ADDR.AreaSysNo) + ' ' + LTRIM(RTRIM(ADDR.StreetAddress)) AS 收件人地址,PDP.EasName AS 商品品名,PDP.ErpCode AS 商品货号,PDP.Barcode AS 商品条形码,OITEM.Quantity AS 申报数量,
			OITEM.SalesUnitPrice AS 申报单价,(OITEM.SalesAmount + OITEM.ChangeAmount) AS 申报总价,OITEM.ChangeAmount as 调价金额,(SOR.CouponAmount * -1) AS 优惠劵金额,SOR.FreightAmount AS 运费,[dbo].[func_GetTaxMoney](PDP.Tax,OITEM.SalesAmount,OITEM.Quantity,SOR.TaxFee,SOR.FreightAmount,SOR.SysNo) AS 税款,PDP.GrosWeight AS 毛重,PDP.NetWeight AS 净重,
			[dbo].[func_GetVoucherNo](OITEM.OrderSysNo) AS 交易号,'' AS 买家支付账号,(select top 1 ExpressNo from [LgDeliveryItem]  where right(TransactionSysNo,len(SOR.SysNo))=SOR.SysNo) AS 快递单号,convert(decimal(18, 2),((OITEM.SalesAmount-OITEM.DiscountAmount)/OITEM.Quantity)) AS 商品单价
            FROM SoOrderItem OITEM INNER JOIN SoOrder SOR ON OITEM.OrderSysNo = SOR.SysNo
			LEFT JOIN DsDealer DEA ON SOR.DealerSysNo =  DEA.SysNo
			LEFT JOIN CrCustomer CUS ON SOR.CustomerSysNo =  CUS.SysNo
            LEFT JOIN PdProduct PDP ON OITEM.ProductSysNo = PDP.SysNo
            LEFT JOIN SoReceiveAddress ADDR ON SOR.ReceiveAddressSysNo=ADDR.SysNo 
            LEFT JOIN WhWarehouse WARE ON SOR.DefaultWarehouseSysNo = WARE.SysNo 
            LEFT JOIN FnOnlinePayment FN ON FN.SourceSysNo = SOR.SysNo 
            left join BsPaymentType on BsPaymentType.SysNo=SOR.PayTypeSysNo ";

            var where = " WHERE 1=1";
            //是否绑定所有仓库
            if (filter.HasAllWarehouse)
            {
                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        where += " and DEA.SysNo = " + filter.DealerSysNo;
                    }
                    else
                    {
                        where += " and DEA.CreatedBy = " + filter.DealerCreatedBy;
                    }
                }
                if (filter.SelectedAgentSysNo != -1)
                {
                    if (filter.SelectedDealerSysNo != -1)
                    {
                        where += " and DEA.SysNo = " + filter.SelectedDealerSysNo;
                    }
                    else
                    {
                        where += " and DEA.CreatedBy = " + filter.SelectedAgentSysNo;
                    }
                }
            }
            else
            {
                if (filter.Warehouses.Count > 0)
                {
                    var wList = "";
                    foreach (var w in filter.Warehouses)
                    {
                        if (wList == "")
                            wList = w.SysNo.ToString();
                        else
                            wList += ',' + w.SysNo.ToString();
                    }
                    wList = "(" + wList + ")";
                    where += " and SOR.DefaultWarehouseSysNo in " + wList;
                }
                else
                {
                    where += " and SOR.DefaultWarehouseSysNo = -1";
                }
            }
            if (filter.OrderStatus != null)
            {
                where += " and SOR.Status=" + filter.OrderStatus;
            }

            if (filter.PayStatus != null)
            {
                where += " and SOR.PayStatus = " + filter.PayStatus;
            }
            if (filter.BeginDate != null)
            {
                where += " and SOR.CreateDate>='" + filter.BeginDate + "'";

            }
            if (filter.StoreSysNoList != null)
            {
                var storeSysNos = string.Join(",", filter.StoreSysNoList);
                where += " and exists (select 1 from splitstr(" + storeSysNos + ",',') tmp where tmp.col = SOR.DefaultWarehouseSysno)";
            }
            if (filter.Keyword != null && filter.Keyword != "")
            {
                int sysno = 0;
                if (int.TryParse(filter.Keyword, out sysno))
                {
                    where += " and (SOR.SysNo=" + filter.Keyword + " or SOR.OrderNo='" + filter.Keyword + "' or CUS.Account='" + filter.Keyword + "' or ADDR.MobilePhoneNumber='" + filter.Keyword + "')";
                }
                else
                {
                    where += " and (SOR.OrderNo='" + filter.Keyword + "' or CUS.Account='" + filter.Keyword + "' or ADDR.MobilePhoneNumber='" + filter.Keyword + "')";
                }
            }

            sqlText += where;
            List<CBOutputSoOrders> outOrders = Context.Sql(sqlText).QueryMany<CBOutputSoOrders>();

            var q = (from o in outOrders
                     group o by o.订单号).ToList();

            foreach (var gp in q)
            {
                int i = 0;
                foreach (var item in gp)
                {
                    i++;
                    if (i != 1)
                    {
                        item.店铺 = null;
                        item.会员名 = null;
                        item.订单号 = null;
                        item.销售订单号 = null;
                        item.订单人姓名 = null;
                        item.订单人证件号 = null;
                        item.订单人电话 = null;
                        item.销售日期 = null;
                        item.下单时间 = null;
                        item.付款时间 = null;
                        item.订单状态 = null;
                        item.买家留言 = null;
                        item.收件人地址 = null;
                        item.优惠劵金额 = null;
                        item.运费 = null;
                        item.订单总额 = null;
                        item.支付类型 = null;
                    }
                }
            }
            return outOrders;
        }

        /// <summary>
        /// 查询导出订单列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-3-16 王耀发 创建</remarks>

        public override List<CBOutputSoOrders> GetExportOrderListByDoComplexSearch(ParaOrderFilter filter)
        {
            string sqlText = @"SELECT row_number() OVER(order by OITEM.OrderSysNo desc) AS 序号,DEA.DealerName AS 店铺,CUS.Name AS 会员名,SOR.SysNo AS 订单号,SOR.OrderNo AS 销售订单号,LTRIM(RTRIM(ADDR.Name)) AS 订单人姓名,LTRIM(RTRIM(ADDR.IDCardNo)) AS 订单人证件号,
			LTRIM(RTRIM(ADDR.MobilePhoneNumber)) AS 订单人电话,SOR.CreateDate AS 下单时间,FN.CreatedDate AS 付款时间
            ,Convert(varchar(100),SOR.CreateDate,23) as 销售日期
            ,(SOR.CashPay) AS 订单总额
            , BsPaymentType.PaymentName as 支付类型
            ,CASE SOR.[Status] WHEN 10 THEN '待审核' WHEN 20 THEN '待支付' WHEN 30 THEN '待创建出库单' WHEN 40 THEN '部分创建出库单' WHEN 50 THEN '已创建出库单' WHEN 55 THEN '出库待接收' WHEN 100 THEN '已完成' ELSE '作废' END AS 订单状态,
            case SOR.PayStatus when 10 then '未付款' else '已付款' end as 支付状态,
			SOR.CustomerMessage AS 买家留言,dbo.func_GetAereaPath(ADDR.AreaSysNo) + ' ' + LTRIM(RTRIM(ADDR.StreetAddress)) AS 收件人地址,PDP.EasName AS 商品品名,PDP.ErpCode AS 商品货号,PDP.Barcode AS 商品条形码,OITEM.Quantity AS 申报数量,
			OITEM.SalesUnitPrice AS 申报单价,(OITEM.SalesAmount + OITEM.ChangeAmount) AS 申报总价,OITEM.ChangeAmount as 调价金额,(SOR.CouponAmount * -1) AS 优惠劵金额,SOR.FreightAmount AS 运费,[dbo].[func_GetTaxMoney](PDP.Tax,OITEM.SalesAmount,OITEM.Quantity,SOR.TaxFee,SOR.FreightAmount,SOR.SysNo) AS 税款,PDP.GrosWeight AS 毛重,PDP.NetWeight AS 净重,
			[dbo].[func_GetVoucherNo](OITEM.OrderSysNo) AS 交易号,'' AS 买家支付账号,(select top 1 ExpressNo from [LgDeliveryItem]  where right(TransactionSysNo,len(SOR.SysNo))=SOR.SysNo) AS 快递单号,convert(decimal(18, 2),((OITEM.SalesAmount-OITEM.DiscountAmount)/OITEM.Quantity)) AS 商品单价
            FROM SoOrderItem OITEM INNER JOIN SoOrder SOR ON OITEM.OrderSysNo = SOR.SysNo
			LEFT JOIN DsDealer DEA ON SOR.DealerSysNo =  DEA.SysNo
			LEFT JOIN CrCustomer CUS ON SOR.CustomerSysNo =  CUS.SysNo
            LEFT JOIN PdProduct PDP ON OITEM.ProductSysNo = PDP.SysNo
            LEFT JOIN SoReceiveAddress ADDR ON SOR.ReceiveAddressSysNo=ADDR.SysNo 
            LEFT JOIN WhWarehouse WARE ON SOR.DefaultWarehouseSysNo = WARE.SysNo 
            left join BsPaymentType on BsPaymentType.SysNo=SOR.PayTypeSysNo
            LEFT JOIN
            (
                select max(fn.CreatedDate) as CreatedDate,fn.SourceSysNo
                from FnOnlinePayment fn 
                group by fn.SourceSysNo
            ) AS FN on FN.SourceSysNo = SOR.SysNo 
            LEFT JOIN SyUser F on F.SysNo = SOR.AuditorSysNo  ";

            var where = " WHERE 1=1";
            //是否绑定所有仓库
            if (filter.HasAllWarehouse)
            {
                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        where += " and DEA.SysNo = " + filter.DealerSysNo;
                    }
                    else
                    {
                        where += " and DEA.CreatedBy = " + filter.DealerCreatedBy;
                    }
                }
                if (filter.SelectedAgentSysNo != -1)
                {
                    if (filter.SelectedDealerSysNo != -1)
                    {
                        where += " and DEA.SysNo = " + filter.SelectedDealerSysNo;
                    }
                    else
                    {
                        where += " and DEA.CreatedBy = " + filter.SelectedAgentSysNo;
                    }
                }
            }
            else
            {
                if (filter.Warehouses.Count > 0)
                {
                    var wList = "";
                    foreach (var w in filter.Warehouses)
                    {
                        if (wList == "")
                            wList = w.SysNo.ToString();
                        else
                            wList += ',' + w.SysNo.ToString();
                    }
                    wList = "(" + wList + ")";
                    where += " and SOR.DefaultWarehouseSysNo in " + wList;
                }
                else
                {
                    where += " and SOR.DefaultWarehouseSysNo = -1";
                }
            }
            if (filter.BeginDate != null)
            {
                where += " and SOR.CreateDate>='" + filter.BeginDate + "'";
            }
            if (filter.EndDate != null)
            {
                where += " and SOR.CreateDate<'" + filter.EndDate + "'";
            }
            if (filter.PayBeginDate != null)
            {
                where += " and FN.CreatedDate>='" + filter.PayBeginDate + "'";
            }
            if (filter.PayEndDate != null)
            {
                where += " and FN.CreatedDate<'" + filter.PayEndDate + "'";
            }
            if (filter.OrderSource != null)
            {
                where += " and SOR.OrderSource=" + filter.OrderSource;
            }
            if (filter.PayTypeSysNo != null)
            {
                where += " and SOR.PayTypeSysNo=" + filter.PayTypeSysNo;
            }
            if (filter.DeliveryTypeSysNo != null)
            {
                where += " and SOR.DeliveryTypeSysNo=" + filter.DeliveryTypeSysNo;
            }
            if (filter.ReceiveName != null)
            {
                where += " and ADDR.Name like '%" + filter.ReceiveName + "%'";
            }
            if (filter.ReceiveTel != null)
            {
                where += " and ADDR.MobilePhoneNumber='" + filter.ReceiveTel + "'";
            }
            if (filter.Auditor != null)
            {
                where += " and F.UserName like '%" + filter.Auditor + "%'";
            }
            if (filter.MinOrderAmount != null)
            {
                where += " and SOR.OrderAmount>=" + filter.MinOrderAmount;
            }
            if (filter.MaxOrderAmount != null)
            {
                where += " and SOR.OrderAmount<=" + filter.MaxOrderAmount;
            }
            if (filter.WarehouseSysNo != null)
            {
                where += " and SOR.DefaultWarehouseSysNo = " + filter.WarehouseSysNo;
            }
            sqlText += where;
            List<CBOutputSoOrders> outOrders = Context.Sql(sqlText).QueryMany<CBOutputSoOrders>();

            var q = (from o in outOrders
                     group o by o.订单号).ToList();

            foreach (var gp in q)
            {
                int i = 0;
                foreach (var item in gp)
                {
                    i++;
                    if (i != 1)
                    {
                        item.店铺 = null;
                        item.会员名 = null;
                        item.订单号 = null;
                        item.销售订单号 = null;
                        item.订单人姓名 = null;
                        item.订单人证件号 = null;
                        item.订单人电话 = null;
                        item.销售日期 = null;
                        item.下单时间 = null;
                        item.付款时间 = null;
                        item.订单状态 = null;
                        item.买家留言 = null;
                        item.收件人地址 = null;
                        item.优惠劵金额 = null;
                        item.运费 = null;
                        item.订单总额 = null;
                        item.支付类型 = null;
                    }
                }
            }

            return outOrders;
        }
        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <param name="soOrderSysNo">订单id</param>
        /// <returns></returns>
        /// <remarks>2015-10-16 杨云奕 添加</remarks>
        public override SoOrderMods GetSoOrderMods(int soOrderSysNo)
        {
            return Context.Sql("select * from soorder where sysno=@0", soOrderSysNo).QuerySingle<SoOrderMods>();
        }
        /// <summary>
        /// 获取订单地址信息
        /// </summary>
        /// <param name="ReceiveAddressSysNo">订单地址编号</param>
        /// <returns></returns>
        /// <remarks>2015-10-16 杨云奕 添加</remarks>
        public override SoReceiveAddressMod GetOrderReceiveAddress2(int ReceiveAddressSysNo)
        {
            string sql = @" select t.*,b.AreaName as  ReceiverArea,c.AreaName as ReceiverCity,d.AreaName as ReceiverProvince,e.AreaName as ReceiverCountry  from SoReceiveAddress  t left join bsarea b on t.AreaSysNo = b.sysno
                            left join bsarea c on b.parentsysno = c.sysno
                            left join bsarea d on c.parentsysno = d.sysno 
                            left join bsarea e on d.parentsysno = e.sysno
                            where t.sysno='" + ReceiveAddressSysNo + "'";
            return Context.Sql(sql).QuerySingle<SoReceiveAddressMod>();

        }
        /// <summary>
        /// 更新订单广州机场商检状态
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="GZJCStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-4-6 王耀发 创建</remarks>
        public override void UpdateOrderGZJCStatus(int soID, int GZJCStatus)
        {
            Context.Update("SoOrder").Column("GZJCStatus", GZJCStatus).Where("SysNo", soID).Execute();
        }
        /// <summary>
        /// 更新订单南沙商检状态
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="NsStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-4-6 王耀发 创建</remarks>
        public override void UpdateOrderNsStatus(int soID, int NsStatus)
        {
            Context.Update("SoOrder").Column("NsStatus", NsStatus).Where("SysNo", soID).Execute();
        }

        public override List<CBSoOrder> GetAllOrderByDateTime(DateTime startTime, DateTime endTime)
        {
            string sql = @" select SoOrder.*,TotalProductNumber = (select SUM(SoOrderItem.Quantity) from SoOrderItem where SoOrderItem.OrderSysNo=SoOrder.SysNo) from SoOrder 
                            where SoOrder.Status>=30 and CreateDate>='" + startTime.ToString() + "'and CreateDate<='" + endTime.ToString() + "' ";
            return Context.Sql(sql).QueryMany<CBSoOrder>();
        }
        public override List<EntityStatisticMod> GetEntityStatisticDataList(int? defaultWareSysNo, DateTime? startTime, DateTime? endTime, string type)
        {
            string amountWhere = "";
            string quantityWhere = "";
            string sql = "";
            List<EntityStatisticMod> modList = new List<EntityStatisticMod>();
            switch (type)
            {
                case "网上购买统计表":

                    if (startTime != null)
                    {
                        amountWhere += " and Sorder.CreateDate>='" + startTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate>='" + startTime.Value + "'";
                    }
                    if (endTime != null)
                    {
                        amountWhere += " and Sorder.CreateDate<='" + endTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate<='" + endTime.Value + "'";
                    }
                    sql = @"select Sorder.SysNo, (b.AreaName) EntityName ,
		                            Sorder.cashpay as Amount,
		                            Quantity=(
		                            select 
			                            sum(SoOrderItem.Quantity) 
		                            from 
			                            soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo 
                                        inner join whwarehouse on soorder.DefaultWarehouseSysNo=whwarehouse.SysNo
			                            left join  SoReceiveAddress on SoReceiveAddress.SysNo=soorder.ReceiveAddressSysNo
			                            left join BsArea xa on SoReceiveAddress.AreaSysNo=xa.SysNo
			                            left join BsArea xb on xa.ParentSysNo=xb.SysNo 
			                            left join BsArea xc on xb.ParentSysNo=xc.SysNo
			                            left join BsArea xd on xc.ParentSysNo=xd.SysNo
		                            where
			                            (soorder.SysNo=Sorder.SysNo and ((xa.ParentSysNo= b.SysNo and  (OrderSource= " +
                                                                                                (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 + " or OrderSource=" +
                                                                                                (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + " ) ) and soorder.Status>=30 and (whwarehouse.WarehouseType ='" + (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.直邮 + @"' ))) " + quantityWhere + @"
		                            )
                            from 
                            SoReceiveAddress addr right join soorder Sorder on addr.SysNo=Sorder.ReceiveAddressSysNo
                            inner join whwarehouse ware on Sorder.DefaultWarehouseSysNo=ware.SysNo
                            left join BsArea a on addr.AreaSysNo=a.SysNo
                            left join BsArea b on a.ParentSysNo=b.SysNo 
                            left join BsArea c on b.ParentSysNo=c.SysNo
                            left join BsArea d on c.ParentSysNo=d.SysNo
                            where Sorder.Status>=30   and (ware.WarehouseType ='" + (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.直邮 + @"' ) and  (OrderSource= " +
                                                                                 (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 +
                                                              " or OrderSource=" +
                                                                                 (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + " )" + amountWhere + @"
                            
                            ";
                    modList = Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
                    break;
                case "实体店销售统计":
                    if (defaultWareSysNo != null)
                    {
                        amountWhere += " and a.DefaultWarehouseSysNo = " + defaultWareSysNo.Value;
                    }
                    if (startTime != null)
                    {
                        amountWhere += " and a.CreateDate>='" + startTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate>='" + startTime.Value + "'";
                    }
                    if (endTime != null)
                    {
                        amountWhere += " and a.CreateDate<='" + endTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate<='" + endTime.Value + "'";
                    }

                    sql = @"	select a.SysNo, b.WarehouseName as EntityName ,
		                                a.cashpay as Amount,
		                                Quantity=(
		                                select 
			                                sum(SoOrderItem.Quantity) 
		                                from 
			                                soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo 
		                                where
			                               soorder.SysNo=a.SysNo and  soorder.DefaultWarehouseSysNo= a.DefaultWarehouseSysNo " + quantityWhere + @"
		                                )
	                                from soorder a inner join WhWarehouse b on  a.DefaultWarehouseSysNo=b.SysNo
	                                where  a.Status>=30 and b.WarehouseType=20 and b.Status=1  " + amountWhere + @"
	                                ";

                    modList = Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
                    break;
                case "客户购买量统计":
                    if (startTime != null)
                    {
                        amountWhere += " and Sorder.CreateDate>='" + startTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate>='" + startTime.Value + "'";
                    }
                    if (endTime != null)
                    {
                        amountWhere += " and Sorder.CreateDate<='" + endTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate<='" + endTime.Value + "'";
                    }

                    sql = @"select Sorder.SysNo,(b.AreaName) EntityName ,
		                                Sorder.cashpay as Amount,
		                                Quantity=(
		                                select 
			                                sum(SoOrderItem.Quantity) 
		                                from 
			                                soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo 
			                                inner join  SoReceiveAddress on SoReceiveAddress.SysNo=soorder.ReceiveAddressSysNo
			                                inner join BsArea xa on SoReceiveAddress.AreaSysNo=xa.SysNo
			                                inner join BsArea xb on xa.ParentSysNo=xb.SysNo 
			                                inner join BsArea xc on xb.ParentSysNo=xc.SysNo
			                                inner join BsArea xd on xc.ParentSysNo=xd.SysNo
		                                where
			                                soorder.SysNo=Sorder.SysNo and  xa.ParentSysNo= b.SysNo  and soorder.Status>=30 " + quantityWhere + @"
		                                )
                                from 
                                SoReceiveAddress addr inner join soorder Sorder on addr.SysNo=Sorder.ReceiveAddressSysNo
                                inner join BsArea a on addr.AreaSysNo=a.SysNo
                                inner join BsArea b on a.ParentSysNo=b.SysNo 
                                inner join BsArea c on b.ParentSysNo=c.SysNo
                                inner join BsArea d on c.ParentSysNo=d.SysNo
                                where Sorder.Status>=30 " + amountWhere + @"
                               ";
                    modList = Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
                    break;
                case "分销商统计表":
                    if (startTime != null)
                    {
                        amountWhere += " and Sorder.CreateDate>='" + startTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate>='" + startTime.Value + "'";
                    }
                    if (endTime != null)
                    {
                        amountWhere += " and Sorder.CreateDate<='" + endTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate<='" + endTime.Value + "'";
                    }
                    sql = @"select Sorder.SysNo, (b.AreaName) EntityName ,
		                                    Sorder.cashpay as Amount,
		                                    Quantity=(
		                                    select 
			                                    sum(SoOrderItem.Quantity) 
		                                    from 
			                                    soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo 
			                                    left join  CrCustomer on CrCustomer.SysNo=soorder.CustomerSysNo
			                                    left join BsArea xa on CrCustomer.AreaSysNo=xa.SysNo
			                                    left join BsArea xb on xa.ParentSysNo=xb.SysNo 
			                                    left join BsArea xc on xb.ParentSysNo=xc.SysNo
			                                    left join BsArea xd on xc.ParentSysNo=xd.SysNo
		                                    where
			                                    (soorder.SysNo=Sorder.SysNo and ((xa.ParentSysNo= b.SysNo and  CrCustomer.PSysNo>0 ) and soorder.Status>=30)) " + quantityWhere + @"
		                                    )
                                    from 
                                    CrCustomer cusSysNo right join soorder Sorder on cusSysNo.SysNo=Sorder.CustomerSysNo
                                    left join BsArea a on cusSysNo.AreaSysNo=a.SysNo
                                    left join BsArea b on a.ParentSysNo=b.SysNo 
                                    left join BsArea c on b.ParentSysNo=c.SysNo
                                    left join BsArea d on c.ParentSysNo=d.SysNo
                                    where Sorder.Status>=30 and  cusSysNo.PSysNo>0 " + amountWhere + @"
                                    
                                    ";
                    modList = Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
                    break;
                case "保税商品购买统计表":
                    if (startTime != null)
                    {
                        amountWhere += " and Sorder.CreateDate>='" + startTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate>='" + startTime.Value + "'";
                    }
                    if (endTime != null)
                    {
                        amountWhere += " and Sorder.CreateDate<='" + endTime.Value + "'";
                        quantityWhere += " and soorder.CreateDate<='" + endTime.Value + "'";
                    }
                    sql = @"select Sorder.SysNo,(b.AreaName) EntityName ,
		                                    Sorder.cashpay as Amount,
		                                    Quantity=(
		                                    select 
			                                    sum(SoOrderItem.Quantity) 
		                                    from 
			                                    soorder inner join SoOrderItem on  soorder.sysno=SoOrderItem.OrderSysNo 
										        inner join whwarehouse on soorder.DefaultWarehouseSysNo=whwarehouse.SysNo
			                                    left join  SoReceiveAddress on SoReceiveAddress.SysNo=soorder.ReceiveAddressSysNo
			                                    left join BsArea xa on SoReceiveAddress.AreaSysNo=xa.SysNo
			                                    left join BsArea xb on xa.ParentSysNo=xb.SysNo 
			                                    left join BsArea xc on xb.ParentSysNo=xc.SysNo
			                                    left join BsArea xd on xc.ParentSysNo=xd.SysNo
		                                    where
			                                    (soorder.SysNo=Sorder.SysNo and  xa.ParentSysNo= b.SysNo and soorder.Status>=30 and  (OrderSource= " +
                                                                                        (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 + " or OrderSource=" +
                                                                                        (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + " )  and whwarehouse.WarehouseType='" + (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.保税 + @"') " +
                                                                                                                                 quantityWhere + @"
		                                    )
                                    from 
								        SoReceiveAddress addr right join soorder Sorder on addr.SysNo=Sorder.ReceiveAddressSysNo
								        inner join whwarehouse ware on Sorder.DefaultWarehouseSysNo=ware.SysNo
								        left join BsArea a on addr.AreaSysNo=a.SysNo
								        left join BsArea b on a.ParentSysNo=b.SysNo 
								        left join BsArea c on b.ParentSysNo=c.SysNo
								        left join BsArea d on c.ParentSysNo=d.SysNo
							        where Sorder.Status>=30 and ware.WarehouseType='" + (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.保税 + @"' and  (OrderSource= " +
                                                                         (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.PC网站 +
                                                      " or OrderSource=" +
                                                                         (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.手机商城 + " ) " + amountWhere + @"
                                    
                                    ";
                    modList = Context.Sql(sql).QueryMany<Hyt.Model.Manual.EntityStatisticMod>();
                    break;
            }
            return modList;
        }

        /// <summary>
        /// 通过订单编号集合获取订单明细信息
        /// </summary>
        /// <param name="SysNos">编号集合</param>
        /// <returns></returns>
        /// <remarks>2016-06-02 杨云奕 添加</remarks>
        public override List<CBSoOrderItem> GetOrderItemsByOrderId(int[] SysNos)
        {
            string sql = " select SoOrderItem.*,PdProduct.Spec,PdProduct.ErpCode from SoOrderItem inner join PdProduct on PdProduct.SysNo=SoOrderItem.ProductSysNo where  OrderSysNo in (" + string.Join(",", SysNos) + ") ";
            return Context.Sql(sql).QueryMany<CBSoOrderItem>();
        }

        /// <summary>
        /// 获取订单的最原始订单编号，换货订单返回原始订单号，非换货订单就返回当前订单编号
        /// </summary>
        /// <param name="currectorderid">当前订单</param>
        /// <returns></returns>
        /// <remarks>2015-04-29 杨浩 创建</remarks>
        public override int GetOriginalOrderID(int currectorderid)
        {
            string sql = @"     
                               with tb
                                as
                                (
                                  select 
                                  so.sysno as orderid,
                                  rc.ordersysno as fromorderid
                                  from SoOrder so
                                  inner join RcReturn rc
                                  on so.ordersource=100 and rc.sysno=so.ordersourcesysno
                                )
                                select fromorderid
                                from tb
                                start with orderid =@currectorderid
                                connect by prior fromorderid = orderid 
                                order by fromorderid asc
                        ";
            var orderid = Context.Sql(sql)
                 .Parameter("currectorderid", currectorderid)
                 .QuerySingle<int>();

            if (orderid == null || orderid == 0)
            {
                return currectorderid;
            }
            else
            {
                return orderid;
            }

        }

        /// <summary>
        /// 订单商品毛重总和
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-7-25 杨浩 创建</remarks>
        public override decimal TotalOrderProductWeight(int orderSysNo)
        {
            string sql =string.Format(" select sum(p.GrosWeight*oi.Quantity) from [SoOrderItem] as oi inner join PdProduct as p on oi.ProductSysNo=p.SysNo where oi.OrderSysNo={0}",orderSysNo);
            return Context.Sql(sql).QuerySingle<decimal>();
        }

        #region 查询需要导出的订单（用于信营）
        /// <summary>
        /// 查询导出订单列表选择导出
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        /// <remarks>2017-5-3 罗勤尧 修改</remarks>
        /// <remarks>2017-05-04 罗勤尧 修改，增加第三方订单导出功能</remarks>

        public override void GetXXExportOrderList(ref Pager<CBXXOutputSoOrders> pager, List<int> sysNos)
        {
            //CASE SOR.[PayStatus] WHEN 10 THEN '等待买家付款' WHEN 20 THEN '买家已付款' ELSE '支付异常' END AS 订单状态
            string sqlText = @"SELECT row_number() OVER(order by OITEM.OrderSysNo desc) AS 序号,DEA.DealerName AS 店铺,b.Name AS 会员名,a.SysNo AS 订单号,a.OrderNo AS 销售订单号,LTRIM(RTRIM(d.Name)) AS 订单人姓名,LTRIM(RTRIM(d.IDCardNo)) AS 订单人证件号,
			LTRIM(RTRIM(d.MobilePhoneNumber)) AS 订单人电话,a.LastUpdateDate AS 销售日期,a.CreateDate AS 订单日期,CASE a.[Status] WHEN 10 THEN '待审核' WHEN 20 THEN '待支付' WHEN 30 THEN '待创建出库单' WHEN 40 THEN '部分创建出库单' WHEN 50 THEN '已创建出库单' WHEN 55 THEN '出库待接收' WHEN 100 THEN '已完成' ELSE '作废' END AS 订单状态,
			a.CustomerMessage AS 买家留言,a.InternalRemarks AS 对内备注,a.DeliveryRemarks AS 配送备注,dbo.func_GetAereaPath(d.AreaSysNo) + ' ' + LTRIM(RTRIM(d.StreetAddress)) AS 收件人地址,PDP.EasName AS 商品品名,PDP.ErpCode AS 商品货号,PDP.Barcode AS 商品条形码,OITEM.Quantity AS 申报数量,
			OITEM.SalesUnitPrice AS 申报单价,a.OrderAmount AS 申报总价,OITEM.ChangeAmount as 调价金额,(a.CouponAmount * -1) AS 优惠劵金额,a.FreightAmount AS 运费,[dbo].[func_GetTaxMoney](PDP.Tax,OITEM.SalesAmount,OITEM.Quantity,a.TaxFee,a.FreightAmount,a.SysNo) AS 税款,PDP.GrosWeight AS 毛重,PDP.NetWeight AS 净重,
			[dbo].[func_GetVoucherNo](OITEM.OrderSysNo) AS 交易号,'' AS 买家支付账号,LDI.ExpressNo AS 快递单号,g.WarehouseName AS 发货仓库,i.PaymentName AS 支付方式, j.DeliveryTypeName AS 运送方式,PDP.SalesMeasurementUnit AS 单位 ,FN.CreatedDate AS 付款日期,s.UserName as 代理商,so.MallOrderId as 标记,so.PayTime as 第三方付款日期 
            FROM SoOrder a  INNER JOIN SoOrderItem OITEM ON OITEM.OrderSysNo = a.SysNo
                                   left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO  
                                                                                                                          --会员表
                                   left join SyJobPool c on c.tasksysno=a.sysno and (c.tasktype=10 or c.tasktype=15)                                                                                                        --订单池
                                   left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo                                                                                        --收货地址表 
                                                                                                                        --系统用户表
                                   left join WhwareHouse g on a.DefaultWarehouseSysNo = g.SYSNO                                                                                         --仓库表
                                   left join bsPaymentType i on a.PayTypeSysNo=i.sysNo 
                                   LEFT JOIN LgDeliveryItem LDI ON LDI.TransactionSysNo=a.TransactionSysNo ---配送单明细                                                                                                 --支付方式
                                   left join LgDeliveryType j on a.DeliveryTypeSysNo=j.sysNo                                                                                            --配送方式       
                                                                                                                               --系统用户表(分配人姓名)
                                   left join DsDealer dea on a.DealerSysNo = dea.SysNo     
                                    LEFT JOIN PdProduct PDP ON OITEM.ProductSysNo = PDP.SysNo    
                                                      
                                   left join 
                                   (
                                    select max(fi.CreatedDate) as CreatedDate,fn.SourceSysNo
                                    from FnReceiptVoucher fn  left join FnReceiptVoucherItem as fi on fn.SysNo=fi.ReceiptVoucherSysNo 
                                    where fi.[Status]=1
                                    group by fn.SourceSysNo
                                   ) as fn on fn.SourceSysNo = a.SysNo
                                                LEFT JOIN SyUser s on s.SysNo = dea.CreatedBy  
                                                  left join  DsOrder so on so.OrderTransactionSysNo =a.TransactionSysNo  ";

            if (sysNos.Count != 0)
            {
                sqlText += " WHERE OITEM.OrderSysNo in (" + string.Join(",", sysNos) + ")";
            }

            //List<CBXXOutputSoOrders> outOrders = Context.Sql(sqlText).QueryMany<CBXXOutputSoOrders>();
            pager.Rows = Context.Sql(sqlText).QueryMany<CBXXOutputSoOrders>();
            //var q = (from o in outOrders
            //         group o by o.订单号).ToList();

            //foreach (var gp in q)
            //{
            //    int i = 0;
            //    foreach (var item in gp)
            //    {
            //        i++;
            //        if (i != 1)
            //        {
            //            item.店铺 = null;
            //            item.会员名 = null;
            //            item.订单号 = null;
            //            item.销售订单号 = null;
            //            item.订单人姓名 = null;
            //            item.订单人证件号 = null;
            //            item.订单人电话 = null;
            //            item.销售日期 = null;
            //            item.订单日期 = null;
            //            item.订单状态 = null;
            //            item.买家留言 = null;
            //            item.收件人地址 = null;
            //            item.优惠劵金额 = null;
            //            item.运费 = null;
            //        }
            //    }
            //}

            //return outOrders;
        }

        /// <summary>
        /// 查询导出订单列表普通导出
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        /// <remarks>2017-5-3 罗勤尧 修改</remarks>
        ///  <remarks>2017-05-04 罗勤尧 修改，增加第三方订单导出功能</remarks>

        public override void GetXXExportOrderListByDoSearch(ref Pager<CBXXOutputSoOrders> pager, ParaOrderFilter filter)
        {
            string sqlText = @"SELECT row_number() OVER(order by OITEM.OrderSysNo desc) AS 序号,DEA.DealerName AS 店铺,b.Name AS 会员名,a.SysNo AS 订单号,a.OrderNo AS 销售订单号,LTRIM(RTRIM(d.Name)) AS 订单人姓名,LTRIM(RTRIM(d.IDCardNo)) AS 订单人证件号,
			LTRIM(RTRIM(d.MobilePhoneNumber)) AS 订单人电话,a.LastUpdateDate AS 销售日期,a.CreateDate AS 订单日期,CASE a.[Status] WHEN 10 THEN '待审核' WHEN 20 THEN '待支付' WHEN 30 THEN '待创建出库单' WHEN 40 THEN '部分创建出库单' WHEN 50 THEN '已创建出库单' WHEN 55 THEN '出库待接收' WHEN 100 THEN '已完成' ELSE '作废' END AS 订单状态,
			a.CustomerMessage AS 买家留言,a.InternalRemarks AS 对内备注,a.DeliveryRemarks AS 配送备注,dbo.func_GetAereaPath(d.AreaSysNo) + ' ' + LTRIM(RTRIM(d.StreetAddress)) AS 收件人地址,PDP.EasName AS 商品品名,PDP.ErpCode AS 商品货号,PDP.Barcode AS 商品条形码,OITEM.Quantity AS 申报数量,
			OITEM.SalesUnitPrice AS 申报单价,a.OrderAmount AS 申报总价,OITEM.ChangeAmount as 调价金额,(a.CouponAmount * -1) AS 优惠劵金额,a.FreightAmount AS 运费,[dbo].[func_GetTaxMoney](PDP.Tax,OITEM.SalesAmount,OITEM.Quantity,a.TaxFee,a.FreightAmount,a.SysNo) AS 税款,PDP.GrosWeight AS 毛重,PDP.NetWeight AS 净重,
			[dbo].[func_GetVoucherNo](OITEM.OrderSysNo) AS 交易号,'' AS 买家支付账号,LDI.ExpressNo AS 快递单号,g.WarehouseName AS 发货仓库,i.PaymentName AS 支付方式, j.DeliveryTypeName AS 运送方式,PDP.SalesMeasurementUnit AS 单位 ,FN.CreatedDate AS 付款日期,s.UserName as 代理商,so.MallOrderId as 标记,so.PayTime as 第三方付款日期 
            FROM SoOrder a  INNER JOIN SoOrderItem OITEM ON OITEM.OrderSysNo = a.SysNo
                                   left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO  
                                                                                                                          --会员表
                                   left join SyJobPool c on c.tasksysno=a.sysno and (c.tasktype=10 or c.tasktype=15)                                                                                                        --订单池
                                   left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo                                                                                        --收货地址表 
                                                                                                                        --系统用户表
                                   left join WhwareHouse g on a.DefaultWarehouseSysNo = g.SYSNO                                                                                         --仓库表
                                   left join bsPaymentType i on a.PayTypeSysNo=i.sysNo 
                                   LEFT JOIN LgDeliveryItem LDI ON LDI.TransactionSysNo=a.TransactionSysNo ---配送单明细                                                                                                 --支付方式
                                   left join LgDeliveryType j on a.DeliveryTypeSysNo=j.sysNo                                                                                            --配送方式       
                                                                                                                               --系统用户表(分配人姓名)
                                   left join DsDealer dea on a.DealerSysNo = dea.SysNo     
                                    LEFT JOIN PdProduct PDP ON OITEM.ProductSysNo = PDP.SysNo    
                                                      
                                   left join 
                                   (
                                    select max(fi.CreatedDate) as CreatedDate,fn.SourceSysNo
                                    from FnReceiptVoucher fn  left join FnReceiptVoucherItem as fi on fn.SysNo=fi.ReceiptVoucherSysNo 
                                    where fi.[Status]=1
                                    group by fn.SourceSysNo
                                   ) as fn on fn.SourceSysNo = a.SysNo
                                                LEFT JOIN SyUser s on s.SysNo = dea.CreatedBy  
                                                  left join  DsOrder so on so.OrderTransactionSysNo =a.TransactionSysNo ";

            var where = " WHERE  1=1";
            //是否绑定所有仓库
            if (filter.HasAllWarehouse)
            {
                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        where += " and dea.SysNo = " + filter.DealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                    }
                }
                if (filter.SelectedAgentSysNo != -1)
                {
                    if (filter.SelectedDealerSysNo != -1)
                    {
                        where += " and dea.SysNo = " + filter.SelectedDealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.SelectedAgentSysNo;
                    }
                }
            }
            else
            {
                if (filter.Warehouses.Count > 0)
                {
                    var wList = "";
                    foreach (var w in filter.Warehouses)
                    {
                        if (wList == "")
                            wList = w.SysNo.ToString();
                        else
                            wList += ',' + w.SysNo.ToString();
                    }
                    wList = "(" + wList + ")";
                    where += " and a.DefaultWarehouseSysNo in " + wList;
                }
                else
                {
                    where += " and a.DefaultWarehouseSysNo = -1";
                }
            }
            if (filter.OrderStatus != null)
            {
                where += " and a.Status=" + filter.OrderStatus;
            }

            if (filter.PayStatus != null)
            {
                where += " and a.PayStatus = " + filter.PayStatus;
            }
            if (filter.BeginDate != null)
            {
                where += " and a.CreateDate>='" + filter.BeginDate + "'";

            }
            if (filter.StoreSysNoList != null)
            {
                var storeSysNos = string.Join(",", filter.StoreSysNoList);
                where += " and exists (select 1 from splitstr(" + storeSysNos + ",',') tmp where tmp.col = a.DefaultWarehouseSysno)";
            }
            if (filter.Keyword != null && filter.Keyword != "")
            {
                int sysno = 0;
                if (int.TryParse(filter.Keyword, out sysno))
                {
                    where += " and (a.SysNo=" + filter.Keyword + " or a.OrderNo='" + filter.Keyword + "' or b.Account='" + filter.Keyword + "' or d.MobilePhoneNumber='" + filter.Keyword + "'or so.MallOrderId='" + filter.Keyword + "')";
                }
                else
                {
                    where += " and (a.OrderNo='" + filter.Keyword + "' or b.Account='" + filter.Keyword + "' or d.MobilePhoneNumber='" + filter.Keyword + "' or so.MallOrderId='" + filter.Keyword + "')";
                }
            }

            sqlText += where;
            //List<CBXXOutputSoOrders> outOrders = Context.Sql(sqlText).QueryMany<CBXXOutputSoOrders>();
            pager.Rows = Context.Sql(sqlText).QueryMany<CBXXOutputSoOrders>();
            //var q = (from o in outOrders
            //         group o by o.订单号).ToList();

            //foreach (var gp in q)
            //{
            //    int i = 0;
            //    foreach (var item in gp)
            //    {
            //        i++;
            //        if (i != 1)
            //        {
            //            item.店铺 = null;
            //            item.会员名 = null;
            //            item.订单号 = null;
            //            item.销售订单号 = null;
            //            item.订单人姓名 = null;
            //            item.订单人证件号 = null;
            //            item.订单人电话 = null;
            //            item.销售日期 = null;
            //            item.订单日期 = null;
            //            item.订单状态 = null;
            //            item.买家留言 = null;
            //            item.收件人地址 = null;
            //            item.优惠劵金额 = null;
            //            item.运费 = null;
            //        }
            //    }
            //}
            //return outOrders;
        }

        /// <summary>
        /// 查询导出订单列表高级查询
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        /// <remarks>2017-5-3 罗勤尧 修改</remarks>
        ///  <remarks>2017-05-04 罗勤尧 修改，增加第三方订单导出功能</remarks>

        public override void GetXXExportOrderListByDoComplexSearch(ref Pager<CBXXOutputSoOrders> pager, ParaOrderFilter filter)
        {
            string sqlText = @"SELECT row_number() OVER(order by OITEM.OrderSysNo desc) AS 序号,DEA.DealerName AS 店铺,b.Name AS 会员名,a.SysNo AS 订单号,a.OrderNo AS 销售订单号,LTRIM(RTRIM(d.Name)) AS 订单人姓名,LTRIM(RTRIM(d.IDCardNo)) AS 订单人证件号,
			LTRIM(RTRIM(d.MobilePhoneNumber)) AS 订单人电话,a.LastUpdateDate AS 销售日期,a.CreateDate AS 订单日期,CASE a.[Status] WHEN 10 THEN '待审核' WHEN 20 THEN '待支付' WHEN 30 THEN '待创建出库单' WHEN 40 THEN '部分创建出库单' WHEN 50 THEN '已创建出库单' WHEN 55 THEN '出库待接收' WHEN 100 THEN '已完成' ELSE '作废' END AS 订单状态,
			a.CustomerMessage AS 买家留言,a.InternalRemarks AS 对内备注,a.DeliveryRemarks AS 配送备注,dbo.func_GetAereaPath(d.AreaSysNo) + ' ' + LTRIM(RTRIM(d.StreetAddress))  AS 收件人地址,PDP.EasName AS 商品品名,PDP.ErpCode AS 商品货号,PDP.Barcode AS 商品条形码,OITEM.Quantity AS 申报数量,
			OITEM.SalesUnitPrice AS 申报单价,a.OrderAmount AS 申报总价,OITEM.ChangeAmount as 调价金额,(a.CouponAmount * -1) AS 优惠劵金额,a.FreightAmount AS 运费,[dbo].[func_GetTaxMoney](PDP.Tax,OITEM.SalesAmount,OITEM.Quantity,a.TaxFee,a.FreightAmount,a.SysNo) AS 税款,PDP.GrosWeight AS 毛重,PDP.NetWeight AS 净重,
			[dbo].[func_GetVoucherNo](OITEM.OrderSysNo) AS 交易号,'' AS 买家支付账号,LDI.ExpressNo AS 快递单号,g.WarehouseName AS 发货仓库,i.PaymentName AS 支付方式, j.DeliveryTypeName AS 运送方式,PDP.SalesMeasurementUnit AS 单位 ,a.PayDate AS 付款日期,s.UserName as 代理商,so.MallOrderId as 标记,so.PayTime as 第三方付款日期 
            FROM SoOrder a  INNER JOIN SoOrderItem OITEM ON OITEM.OrderSysNo = a.SysNo
                                   left join PdProduct p on OITEM.ProductSysNo=p.SysNo
                                   left join crcustomer b on a.CUSTOMERSYSNO = b.SYSNO  
                                                                                                                          --会员表
                                   left join SyJobPool c on c.tasksysno=a.sysno and (c.tasktype=10 or c.tasktype=15)                                                                                                        --订单池
                                   left join SoReceiveAddress d on d.sysno=a.ReceiveAddressSysNo                                                                                        --收货地址表 
                                                                                                                        --系统用户表
                                   left join WhwareHouse g on a.DefaultWarehouseSysNo = g.SYSNO                                                                                         --仓库表
                                   left join bsPaymentType i on a.PayTypeSysNo=i.sysNo 
                                   LEFT JOIN LgDeliveryItem LDI ON LDI.TransactionSysNo=a.TransactionSysNo ---配送单明细                                                                                                 --支付方式
                                   left join LgDeliveryType j on a.DeliveryTypeSysNo=j.sysNo                                                                                            --配送方式       
                                                                                                                               --系统用户表(分配人姓名)
                                   left join DsDealer dea on a.DealerSysNo = dea.SysNo     
                                    LEFT JOIN PdProduct PDP ON OITEM.ProductSysNo = PDP.SysNo    
                                                LEFT JOIN SyUser s on s.SysNo = dea.CreatedBy  
                                                  left join  DsOrder so on so.OrderTransactionSysNo =a.TransactionSysNo  ";

            var where = " WHERE 1=1";
            //是否绑定所有仓库
            if (filter.HasAllWarehouse)
            {
                //判断是否绑定所有分销商
                if (!filter.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (filter.IsBindDealer)
                    {
                        where += " and dea.SysNo = " + filter.DealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.DealerCreatedBy;
                    }
                }
                if (filter.SelectedAgentSysNo != -1)
                {
                    if (filter.SelectedDealerSysNo != -1)
                    {
                        where += " and dea.SysNo = " + filter.SelectedDealerSysNo;
                    }
                    else
                    {
                        where += " and dea.CreatedBy = " + filter.SelectedAgentSysNo;
                    }
                }
            }
            else
            {
                if (filter.Warehouses.Count > 0)
                {
                    var wList = "";
                    foreach (var w in filter.Warehouses)
                    {
                        if (wList == "")
                            wList = w.SysNo.ToString();
                        else
                            wList += ',' + w.SysNo.ToString();
                    }
                    wList = "(" + wList + ")";
                    where += " and a.DefaultWarehouseSysNo in " + wList;
                }
                else
                {
                    where += " and a.DefaultWarehouseSysNo = -1";
                }
            }
            if (filter.BeginDate != null)
            {
                where += " and a.CreateDate>='" + filter.BeginDate + "'";
            }
            if (filter.EndDate != null)
            {
                where += " and a.CreateDate<'" + filter.EndDate + "'";
            }
            if (filter.PayBeginDate != null)
            {
                where += " and a.PayDate>='" + filter.PayBeginDate + "'";
            }
            if (filter.PayEndDate != null)
            {
                where += " and a.PayDate<'" + filter.PayEndDate + "'";
            }
            //增加第三方付款时间搜索条件 罗勤尧 2017-5-3
            //if (filter.PayBeginDate != null)
            //{
            //    where += " or so.PayTime>='" + filter.PayBeginDate + "'";

            //}
            //if (filter.PayEndDate != null)
            //{
            //    where += " and so.PayTime<'" + filter.PayEndDate + "'";

            //}
            if (filter.OrderSource != null)
            {
                where += " and a.OrderSource=" + filter.OrderSource;
            }
            if (filter.PayTypeSysNo != null)
            {
                where += " and a.PayTypeSysNo=" + filter.PayTypeSysNo;
            }
            if (filter.DeliveryTypeSysNo != null)
            {
                where += " and a.DeliveryTypeSysNo=" + filter.DeliveryTypeSysNo;
            }
            if (filter.ReceiveName != null)
            {
                where += " and d.Name like '%" + filter.ReceiveName + "%'";
            }
            if (filter.ReceiveTel != null)
            {
                where += " and d.MobilePhoneNumber='" + filter.ReceiveTel + "'";
            }
            if (filter.Auditor != null)
            {
                where += " and s.UserName like '%" + filter.Auditor + "%'";
            }
            if (filter.MinOrderAmount != null)
            {
                where += " and a.OrderAmount>=" + filter.MinOrderAmount;
            }
            if (filter.MaxOrderAmount != null)
            {
                where += " and a.OrderAmount<=" + filter.MaxOrderAmount;
            }
            if (filter.WarehouseSysNo != null)
            {
                where += " and a.DefaultWarehouseSysNo = " + filter.WarehouseSysNo;
            }
            if (!string.IsNullOrWhiteSpace(filter.ErpCode))
            {
                where += " and p.ErpCode = '" + filter.ErpCode + "'";
            }
            sqlText += where;
            //List<CBXXOutputSoOrders> outOrders = Context.Sql(sqlText).QueryMany<CBXXOutputSoOrders>();
            pager.Rows = Context.Sql(sqlText).QueryMany<CBXXOutputSoOrders>();
            //var q = (from o in outOrders
            //         group o by o.订单号).ToList();

            //foreach (var gp in q)
            //{
            //    int i = 0;
            //    foreach (var item in gp)
            //    {
            //        i++;
            //        if (i != 1)
            //        {
            //            item.店铺 = null;
            //            item.会员名 = null;
            //            item.订单号 = null;
            //            item.销售订单号 = null;
            //            item.订单人姓名 = null;
            //            item.订单人证件号 = null;
            //            item.订单人电话 = null;
            //            item.销售日期 = null;
            //            item.订单日期 = null;
            //            item.订单状态 = null;
            //            item.买家留言 = null;
            //            item.收件人地址 = null;
            //            item.优惠劵金额 = null;
            //            item.运费 = null;
            //        }
            //    }
            //}

            //return outOrders;
        }
        #endregion


        #region 又一城订单添加推送
        /// <summary>
        /// 根据订单号查询是否提推送过又一城详情
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public override SoAddOrderToU1City GetU1CityEntity(int OrderSysNo)
        {
            return Context.Sql("select * from SoAddOrderToU1City where OrderSysNo=@0", OrderSysNo).QuerySingle<SoAddOrderToU1City>();
        }
        /// <summary>
        /// 添加一条记录状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override int InserU1City(SoAddOrderToU1City entity)
        {
            return Context.Insert<SoAddOrderToU1City>("SoAddOrderToU1City", entity).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        #endregion

        /// <summary>
        /// 添加推送又一城返回参数记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override SoOrderToU1CityInfomation InserToU1CityInfomation(SoOrderToU1CityInfomation entity)
        {
            entity.SysNo = Context.Insert("SoOrderToU1CityInfomation", entity)
                      .AutoMap(o => o.SysNo)
                      .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }
        /// <summary>
        /// 查询是否存在记录
        /// </summary>
        /// <param name="ProductSysNo"></param>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public override int GetToU1CityInfomation(string TransactionPdSku, int OrderSysNo)
        {
            string sql = "select count(1) from SoOrderToU1CityInfomation where OrderSysNo=" + OrderSysNo + " and TransactionPdSku='" + TransactionPdSku+"'";
            return Context.Sql(sql)
                .QuerySingle<int>();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public override SoReturnOrderToU1City GetReturnOrderToU1City(int OrderSysNo)
        {
            SoReturnOrderToU1City model = null;
            if (OrderSysNo != 0)
            {
                model = Context.Sql("select * from SoReturnOrderToU1City where OrderSysNo=@OrderSysNo")
                               .Parameter("OrderSysNo", OrderSysNo)
                               .QuerySingle<SoReturnOrderToU1City>();
            }
            return model;
        }

        /// <summary>
        /// 获取物流单号及物流编码
        /// </summary>
        /// <param name="ordersysno"></param>
        /// <returns></returns>
        /// <remarks>2015-10-23 陈海裕 创建</remarks>
        public override Model.Common.LgExpressModel GetDeliveryCodeData(int ordersysno)
        {
            System.Text.StringBuilder sbSql = new System.Text.StringBuilder();
            sbSql.AppendLine(" select  OverseaCarrier,ExpressNo  from  soorder inner join LgDeliveryType ");
            sbSql.AppendLine("  on DeliveryTypeSysNo=LgDeliveryType.SysNo inner join LgDeliveryItem  on ");
            sbSql.AppendLine("  LgDeliveryItem.TransactionSysNo=soorder.TransactionSysNo ");
            sbSql.AppendLine("   where soorder.SysNo=@0 ");
            return Context.Sql(sbSql.ToString(), ordersysno).QuerySingle<Model.Common.LgExpressModel>();
        }
        /// <summary>
        /// 获取指定编号的数据
        /// </summary>
        /// <param name="SysNos"></param>
        /// <returns></returns>
        public override List<SoOrder> GetAllOrderBySysNos(string SysNos)
        {
            string sql = " select * from SoOrder where SysNo in (" + SysNos + ") ";
            return Context.Sql(sql).QueryMany<SoOrder>();
        }
        /// <summary>
        /// 更新订单表订单积分
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <param name="point"></param>
        public override void UpdateOrderPoint(int OrderSysNo, int point)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                context.Sql("update SoOrder set CoinPay=@CoinPay where sysno=@sysNo")
                    .Parameter("CoinPay", point)
                    .Parameter("sysNo", OrderSysNo)
                    .Execute();
            }
        }


        public override List<CDSoOrderItem> GetSoOrderItemByWarehouseProduct(int warehouseSysNo, int productSysNo)
        {
            string sql="select SoOrderItem.*,SoOrder.OrderNo,SoOrder.CreateDate from SoOrder inner join SoOrderItem on SoOrder.sysno=SoOrderItem.OrderSysNo  ";
            sql += " where SoOrder.PayStatus=20 and DefaultWarehouseSysNo='" + warehouseSysNo + "' and SoOrderItem.ProductSysNo='" + productSysNo + "'";
            return Context.Sql(sql).QueryMany<CDSoOrderItem>();
        }
        /// <summary>
        /// 是否全部发货
        /// </summary>
        /// <param name="orderSysno">订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-08-24 杨浩 创建</remarks>
        public override bool IsAllShip(int orderSysno)
        {
           return Context.Sql("select COUNT(1) from [WhStockOut] where [Status]>0 and [Status]<60 and OrderSysNO=@orderSysno")
                .Parameter("orderSysno", orderSysno)
                .QuerySingle<int>() == 0;
        }


        /// <summary>
        /// 获取销售订单详情
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单详情</returns>
        ///<remarks>2017-08-25 吴琨 创建
        /// </remarks>
        public override WhStockOut GetEntityTo(int sysNo)
        {
            return Context.Sql("select * from WhStockOut where sysno=@0", sysNo).QuerySingle<WhStockOut>();
        }


        /// <summary>
        /// 查询销售单表
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-08-25 吴琨 创建</returns>
        public override SoOrder GetModel(int sysNo)
        {
            string sql = "select * from SoOrder where sysNo=@sysNo";
            return Context.Sql(sql).Parameter("sysNo", sysNo).QuerySingle<SoOrder>();
        }


        /// <summary>
        /// 根据订单编号修改支付方式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool UpPayTypeSysNo(int id,int PayTypeSysNo)
        {
           return Context.Update("SoOrder")
                .Column("PayTypeSysNo", PayTypeSysNo)
                .Where("SysNo", id)
                .Execute() > 0;
        }

    }
}
