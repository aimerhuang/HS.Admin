using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Transactions;
using Hyt.DataAccess.Finance;
using Hyt.DataAccess.Logistics;
using Hyt.DataAccess.Oracle.Finance;
using Hyt.DataAccess.Oracle.Logistics;
using Hyt.DataAccess.Oracle.Order;
using Hyt.DataAccess.Oracle.Warehouse;
using Hyt.DataAccess.RMA;
using Hyt.DataAccess.Sys;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle
{
    /// <summary>
    /// DaoImpl结算单维护
    /// </summary>
    /// <remarks>2013-06-15 黄伟 创建</remarks>
    public class LgSettlementDaoImpl : ILgSettlementDao
    {
        private const int PageSize = 10;

        /// <summary>
        /// 查找/高级查找
        /// </summary>
        /// <param name="currPageIndex">当前pageIndex</param>
        /// <param name="searchParas">高级查询参数集合</param>
        /// <param name="warehouses">当前登录人员有权限的仓库</param>
        /// <returns>结算单主表实体</returns>
        /// <remarks>2013-06-15 黄伟 创建</remarks>
        public override Pager<CBLgSettlement> Search(int? currPageIndex, ParaLogisticsLgsettlement searchParas, List<int> warehouses)
        {
            var userSysNos = searchParas.CreatedByAdv == null ? null : ISyUserDao.Instance.GetSyUsersByName(searchParas.CreatedByAdv).Select(p => p.SysNo).ToList();
            var strUsers = "";
            if (userSysNos != null && userSysNos.Any())
            {
                strUsers = "and M.CreatedBy in(" + string.Join(",", userSysNos) + ")";
            }
            else
            {
                if (searchParas.CreatedByAdv != null)
                {
                    strUsers = "and M.CreatedBy in(-1)";
                }

            }

            #region 没有仓库权限

            if (warehouses == null || !warehouses.Any())
            {
                return new Pager<CBLgSettlement>
                    {
                        Rows = null,
                        TotalRows = 0
                    };
            }

            #endregion

            var sqlFrom = "lgsettlement m";
            var strWhere =
                @" 1=1
and M.WAREHOUSESYSNO in({0}) --in(@param) in list not supported in context.select.where--huangwei
                        and (@STOCKOUTSYSNO is null or exists(select * from LgSettlementItem d where d.settlementsysno=m.sysno and stockoutsysno=@STOCKOUTSYSNO) )
                        --and (@WAREHOUSESYSNO is null or exists(select * from LgSettlementItem d where d.settlementsysno=m.sysno and exists(select * from whstockout where Sysno=d.stockoutsysno and --WAREHOUSESYSNO=@WAREHOUSESYSNO))) 
                        and (@WAREHOUSESYSNO is null or WAREHOUSESYSNO=@WAREHOUSESYSNO)--the col has been added into table lgsettlement,so use warehousesysno in lgsettlement not from others as before
                        and (@settleSysNo is null or M.sysNo=@settleSysNo) 
                        and (@Deliveryusersysno is null or M.Deliveryusersysno=@Deliveryusersysno) 
                        {1} 
                        and (@Status is null or M.Status=@Status)
                        and ((@beginDate is null or M.CreatedDate >= @beginDate)
                        and (@endDate is null or M.CreatedDate < @endDate))";
            strWhere = string.Format(strWhere, string.Join(",", warehouses), strUsers);


            var pagerResult = new Pager<CBLgSettlement> { PageSize = PageSize, CurrentPage = currPageIndex ?? 1 };

            using (var context = Context.UseSharedConnection(true))
            {
                pagerResult.TotalRows = context.Select<int>(
                    @"count(0)")
                                               .From(sqlFrom)
                                               .AndWhere(strWhere)
                                               .Parameter("STOCKOUTSYSNO", searchParas.DoSysNoAdv) //出库单系统编号
                                               .Parameter("WAREHOUSESYSNO", searchParas.TxtWareHouse) //仓库系统编号
                                               .Parameter("settleSysNo", searchParas.SettleSysNoAdv) //结算单系统编号
                                               .Parameter("Deliveryusersysno", searchParas.SelDelManAdv) //用户系统编号
                    //.Parameter("Createdby", searchParas.CreatedByAdv) //创建人
                    //.Parameter("Createdby", searchParas.CreatedByAdv) //创建人
                                               .Parameter("Status", searchParas.SelStatusAdv) //状态
                                               .Parameter("beginDate", searchParas.BeginDateAdv) //开始日期
                                               .Parameter("endDate", searchParas.EndDateAdv) //结束日期
                                               .QuerySingle();
                pagerResult.Rows = context.Select<CBLgSettlement>(
                                            @"m.SysNo,m.Deliveryusersysno,m.WAREHOUSESYSNO,
                                                m.Createdby,m.Totalamount,
                                                --(select sum(payamount) from LgSettlementItem where settlementsysno=m.sysno) as PaidAmount,
                                    m.PaidAmount,
                                    m.status,m.createddate")
                                          .From(sqlFrom)
                                          .AndWhere(strWhere)
                                          .Parameter("STOCKOUTSYSNO", searchParas.DoSysNoAdv) //出库单系统编号
                                          .Parameter("WAREHOUSESYSNO", searchParas.TxtWareHouse) //仓库系统编号
                                          .Parameter("settleSysNo", searchParas.SettleSysNoAdv) //结算单系统编号
                                          .Parameter("Deliveryusersysno", searchParas.SelDelManAdv) //用户系统编号
                    //.Parameter("Createdby", searchParas.CreatedByAdv) //创建人
                    //.Parameter("Createdby", searchParas.CreatedByAdv) //创建人
                                          .Parameter("Status", searchParas.SelStatusAdv) //状态
                                          .Parameter("beginDate", searchParas.BeginDateAdv) //开始日期
                                          .Parameter("endDate", searchParas.EndDateAdv) //结束日期
                                          .OrderBy("m.SysNo desc")
                                          .Paging(currPageIndex ?? 1, PageSize)
                                          .QueryMany();
            }

            return pagerResult;

        }



        /// <summary>
        /// 通过订单号查询结算单列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>结算单列表</returns>
        /// <remarks>2013-12-06 余勇 创建</remarks>
        public override List<CBLgSettlement> GetLgSettlementListByOrderSysNo(int orderSysNo)
        {
            return Context.Sql(@"select m.SysNo,m.Deliveryusersysno,m.WAREHOUSESYSNO,
                                    m.Createdby,m.Totalamount,
                                    m.PaidAmount, m.status,m.createddate 
                                from lgsettlement m where
                                      exists(select 1 from LgSettlementItem d inner join WhStockOut c 
                                            on d.stockoutsysno=c.sysno
                                            where d.settlementsysno=m.sysno 
                                      and c.ordersysno=@ORDERSYSNO) 
                                      order by m.sysno")
                                .Parameter("ORDERSYSNO", orderSysNo)
                                .QueryMany<CBLgSettlement>();
        }

        /// <summary>
        /// 获取结算单列表
        /// </summary>
        /// <param name="sysNo">结算单系统编号</param>
        /// <returns>结算单列表,参数为空返回所有</returns>
        /// <remarks>2013-07-04 黄伟 创建</remarks>
        public override List<LgSettlement> GetLgSettlement(int? sysNo)
        {
            return sysNo == null
                       ? Context.Sql(@"select *
                                from LgSettlement")
                                .QueryMany<LgSettlement>()
                       : Context.Sql(@"select *
                                from LgSettlement
                                where sysno=@sysno"
                             )
                                .Parameter("sysno", sysNo)
                                .QueryMany<LgSettlement>();
        }

        #region 仓库及配送人员下拉选项
        /*
         1仓库是从所有仓库里面选 显示所有仓库的名称WhWarehouse
         2配送人员是所有的系统用户syuser
         3仓库和配送人员关联-syuserwarehouse 关联表
        */

        /// <summary>
        /// 下拉选项-取得仓库列表
        /// </summary>
        /// <returns>仓库列表</returns>
        /// <remarks>2013-06-18 黄伟 创建</remarks>
        public override Dictionary<int, string> GetWareHouse()
        {
            var dic = new Dictionary<int, string>();
            Context.Sql("select sysno,warehousename from whwarehouse").QueryMany<WhWarehouse>()
                .ForEach(p => dic.Add(p.SysNo, p.WarehouseName));
            return dic;
        }

        /// <summary>
        /// 下拉选项-取得配送人员列表
        /// </summary>
        /// <param name="whSysNo">仓库编号</param>
        /// <returns>配送人员列表</returns>
        /// <remarks>2013-06-18 黄伟 创建</remarks>
        public override Dictionary<int, string> GetDeliveryMan(int? whSysNo)
        {
            var dic = new Dictionary<int, string>();
            if (whSysNo != null)
                Context.Sql(@"select * from syuser where sysno in 
                            (select usersysno from syuserwarehouse
                             where wharehousesysno =@0
                             )", whSysNo).QueryMany<SyUser>()
                    .ForEach(p => dic.Add(p.SysNo, p.UserName));
            else
                Context.Sql("select * from syuser").QueryMany<SyUser>().ForEach(p => dic.Add(p.SysNo, p.UserName));
            return dic;
        }

        #endregion

        /// <summary>
        /// 更新结算单
        /// </summary>
        /// <param name="lstSettlementSysNo">结算单系统编号集合</param>
        /// <param name="status">审核或作废,枚举结算单状态</param>
        /// <param name="userSysNo">操作人系统编号</param>
        /// <returns>封装的实体(Status,StatusCode,Message)</returns>
        /// <remarks>
        /// 2013-06-28 黄伟 创建
        /// </remarks>
        public override Result UpdateStatus(IList<int> lstSettlementSysNo, LogisticsStatus.结算单状态 status, int userSysNo)
        {
            var success = false;
            //预留审核逻辑
            var strMsgSuccess = status == LogisticsStatus.结算单状态.已结算 ? "审核成功!" : "作废成功!";
            var strMsgFailed = status == LogisticsStatus.结算单状态.已结算 ? "审核失败!" : "作废失败!";

            //取消事务,目前已经禁止作废结算单 何方 2014/04/21
            //更新结算单状态
            Context.Sql(
                "update lgsettlement set status=@status,lastupdateby=@userSysNo where sysno in(@settlementSysNo)")
                   .Parameter("status", status.GetHashCode())
                   .Parameter("userSysNo", userSysNo)
                   .Parameter("settlementSysNo", lstSettlementSysNo)
                   .Execute();

            if (status == LogisticsStatus.结算单状态.作废)
            {
                //通过结算单明细来更新相应单据
                lstSettlementSysNo.ToList().ForEach(s =>
                    {
                        var lstLgSettlementItem = LgSettlementItemDaoImpl.Instance.GetLgSettlementItems(s);

                        lstLgSettlementItem.ForEach(p =>
                            {
                                #region 百城当日达 非第三方快递

                                var soOrder = IOutStockDao.Instance.GetSoOrder(p.StockOutSysNo);
                                var stockOut = IOutStockDao.Instance.GetModel(p.StockOutSysNo);
                                var lgDelivery = ILgDeliveryDao.Instance.GetDelivery(p.DeliverySysNo);

                                #region 出库单/配送单明细状态是签收/拒收

                                switch (stockOut.Status)
                                {
                                    #region 拒收
                                    case (int)WarehouseStatus.出库单状态.拒收:
                                        /* 首单拒收并且货到付款
                                            该订单所有出库单都是拒收则为第一单拒收,如果存在其他签收出库单则不是第一单拒收
                                        */
                                        if (IsRejectAtFirst(soOrder.SysNo) && stockOut.IsCOD == 1)
                                            break;

                                        #region 退换货子流程
                                        DoRMAForVoidSettlement(soOrder.SysNo);
                                        break;

                                        #endregion

                                    #endregion

                                    #region 部分签收
                                    case (int)WarehouseStatus.出库单状态.部分退货:
                                        /* 
                                           部分签收走退换货子流程
                                        */
                                        #region 退换货子流程
                                        DoRMAForVoidSettlement(soOrder.SysNo);
                                        #endregion

                                        break;
                                    #endregion

                                }

                                #endregion

                                #region 收款单处理
                                //出库单应收金额是否>0
                                if (stockOut.Receivable > 0)
                                {
                                    var fnReceiptVoucher = FnReceiptVoucherDaoImpl.Instance.GetEntity((int)FinanceStatus.收款来源类型.销售单, soOrder.SysNo);
                                    //fnReceiptVoucher.Status = (int)FinanceStatus.收款单状态.待确认;
                                    //IFnReceiptVoucherDao.Instance.Update(fnReceiptVoucher);

                                    //收款单明细-若存在则修改状态为无效
                                    //FnReceiptVoucherItemDaoImpl.Instance.Update(fnReceiptVoucher.SysNo,
                                    //                                            FinanceStatus.收款单明细状态.无效);
                                }
                                #endregion

                                #region 配送单-配送在途
                                ILgDeliveryDao.Instance.UpdateStatus(p.DeliverySysNo, LogisticsStatus.配送单状态.配送在途);
                                #endregion

                                #region 配送单明细-待签收
                                ILgDeliveryDao.Instance.UpdateDeliveryItemStatus(p.DeliverySysNo,
                                                                                    LogisticsStatus.配送单据类型.出库单,
                                                                                    p.StockOutSysNo,
                                                                                    LogisticsStatus.配送单明细状态.待签收);
                                #endregion

                                //#region 配送员信用额度-扣减信用额度 bugid 3031 moved to bo
                                //var delUserCredit = ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(lgDelivery.DeliveryUserSysNo,
                                //                                                                           stockOut.WarehouseSysNo);
                                //delUserCredit.RemainingDeliveryCredit -= stockOut.StockOutAmount;
                                //ILgDeliveryUserCreditDao.Instance.Update(delUserCredit);
                                //#endregion

                                #region 出库单子流程
                                /*若是全部签收,则只更新出库单状态->配送中*/
                                /* else:
                                 * 是否有入库单?
                                 Y:出库单-配送中;入库单-作废
                                 N:出库单-待出库
                                */
                                if (stockOut.Status == (int)WarehouseStatus.出库单状态.已签收)
                                {
                                    IOutStockDao.Instance.UpdateStatus(p.StockOutSysNo, WarehouseStatus.出库单状态.配送中);
                                }
                                else
                                {
                                    var stockIn =
                                        IInStockDao.Instance.GetWhStockInByVoucherSource(
                                            (int)WarehouseStatus.入库单据类型.出库单, stockOut.SysNo);
                                    if (stockIn != null)
                                    {
                                        IOutStockDao.Instance.UpdateStatus(p.StockOutSysNo, WarehouseStatus.出库单状态.配送中);
                                        stockIn.Status = (int)WarehouseStatus.入库单状态.作废;
                                        IInStockDao.Instance.UpdateWhStockIn(stockIn);
                                    }
                                    else
                                    {
                                        IOutStockDao.Instance.UpdateStatus(p.StockOutSysNo, WarehouseStatus.出库单状态.待出库);
                                    }
                                    /* removed for 出库单会更新订单状态 
                                    var orderSysNo = OutStockDaoImpl.Instance.GetSoOrder(p.StockOutSysNo).SysNo;
                                    //修改订单:已出库 OrderStatus.销售单状态.已创建出库单
                                    SoOrderDaoImpl.Instance.UpdateOrderStatus(orderSysNo, (int)OrderStatus.销售单状态.已创建出库单);
                                    */
                                }
                                #endregion

                                #endregion
                            });
                    });


                success = true;


            }
            return success
                       ? new Result { Status = true, StatusCode = 0, Message = strMsgSuccess }
                       : new Result { Status = false, StatusCode = -1, Message = strMsgFailed };

        }

        /// <summary>
        /// 结算单作废-退换货子流程
        /// </summary>
        /// <param name="soOrderSysNo">订单编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-8-9 黄伟 创建</remarks>
        private void DoRMAForVoidSettlement(int soOrderSysNo)
        {
            var theRMA = IRcReturnItemDao.Instance.GetListByOrder(soOrderSysNo);
            if (theRMA == null || !theRMA.Any())
                return;

            #region 退货单-作废

            var rmaSysNo =
                IRcReturnItemDao.Instance.GetListByOrder(soOrderSysNo).First().RMAID;
            var rma = IRcReturnDao.Instance.GetEntity(rmaSysNo);
            rma.Status = (int)RmaStatus.退换货状态.作废;
            IRcReturnDao.Instance.Update(IRcReturnDao.Instance.GetEntity(rmaSysNo));

            #endregion

            #region 入库单-作废

            var stockInRMA = InStockDaoImpl.Instance.GetWhStockInByVoucherSource(
                (int)WarehouseStatus.入库单据类型.RMA单, rmaSysNo);
            stockInRMA.Status = (int)WarehouseStatus.入库单状态.作废;
            InStockDaoImpl.Instance.UpdateWhStockIn(stockInRMA);

            #endregion

            #region 付款单-作废
            //作废付款单及付款单子表
            var fnPaymentVoucher =
                IFnPaymentVoucherDao.Instance.GetEntityByVoucherSource(
                    (int)FinanceStatus.付款来源类型.退换货单, rmaSysNo);
            fnPaymentVoucher.Status = (int)FinanceStatus.付款单状态.作废;
            IFnPaymentVoucherDao.Instance.UpdateVoucher(fnPaymentVoucher);
            #endregion

            #region 付款单明细-作废
            var lstFnPaymentVoucherItem =
                IFnPaymentVoucherDao.Instance.GetVoucherItems(fnPaymentVoucher.Status);
            lstFnPaymentVoucherItem.ForEach(IFnPaymentVoucherDao.Instance.UpdateVoucherItem);
            #endregion

        }

        /// <summary>
        /// 创建结算单
        /// </summary>
        /// <param name="lgSettlement">结算单实体.</param>
        /// <returns>新建的结算单系统编号</returns>
        /// <remarks>
        /// 2013/7/11 何方 创建
        /// </remarks>
        public override int Create(LgSettlement lgSettlement)
        {
            if (lgSettlement.LastUpdateDate == DateTime.MinValue)
            {
                lgSettlement.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (lgSettlement.Stamp == DateTime.MinValue)
            {
                lgSettlement.Stamp = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var sysno = Context.Insert<LgSettlement>("lgSettlement", lgSettlement)
                     .AutoMap(p => p.SysNo, p => p.Items)
                      .ExecuteReturnLastId<int>("SysNo");
            return sysno;
        }

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="sysNo">支付方式系统编号,null表示返回所有</param>
        /// <returns>支付方式列表</returns>
        /// <remarks>2013-8-8 黄伟 创建</remarks>
        public override IList<BsPaymentType> GetBsPaymentTypes(int? sysNo)
        {
            //有效
            return Context.Sql("select * from bspaymenttype where status=1").QueryMany<BsPaymentType>();
        }

        /// <summary>
        /// 获取结算单关联收款单及付款单是否存在至少一笔已确认,若确认,不允许作废结算单
        /// 需在订单相关页面操作,取消确认
        /// </summary>
        /// <param name="lstSettlementSysNo">结算单编号集合</param>
        /// <returns>true:已确认;false:待确认</returns>
        /// <remarks>2013-8-8 黄伟 创建</remarks>
        public override Result<List<string>> IsFnReciptVoucherConfirmed(List<int> lstSettlementSysNo)
        {
            var lstData = new List<string>();
            foreach (var lgSettlementSysNo in lstSettlementSysNo)
            {
                var lstItem = ILgSettlementItemDao.Instance.GetLgSettlementItems(lgSettlementSysNo);
                var lstOrder =
                    lstItem.Select(lgSettlementItem => IOutStockDao.Instance.GetSoOrder(lgSettlementItem.StockOutSysNo));
                foreach (var soOrder in lstOrder)
                {
                    //是否存在rma关联付款单确认或部分确认状态
                    var rmaConfirmed = 0;
                    var rmaList = IRcReturnItemDao.Instance.GetListByOrder(soOrder.SysNo);
                    var fnPaymentVoucherSysNo = 0;
                    //存在rmaitem,通过rmaitem找到rma
                    if (rmaList.Any())
                    {
                        var rmaSysNo = rmaList.First().RMAID;
                        var rma = IRcReturnDao.Instance.GetEntity(rmaSysNo);
                        /* 判断付款单是待付款才能作废,否则提示错误信息*/
                        var fnPaymentVoucher =
                            IFnPaymentVoucherDao.Instance.GetEntityByVoucherSource(
                                (int)FinanceStatus.付款来源类型.退换货单, rmaSysNo);
                        if (fnPaymentVoucher.Status != (int)FinanceStatus.付款单状态.作废)
                        {
                            rmaConfirmed = rmaSysNo;
                            fnPaymentVoucherSysNo = fnPaymentVoucher.SysNo;
                        }
                    }

                    var fnReceiptVoucher = FnReceiptVoucherDaoImpl.Instance.GetEntity((int)FinanceStatus.收款来源类型.销售单,
                                                                                      soOrder.SysNo);
                    var fnLink = fnReceiptVoucher.SysNo;
                    var strMsg = new StringBuilder();
                    if (fnReceiptVoucher.Status == (int)FinanceStatus.收款单状态.已确认)
                    {
                        strMsg.Append(string.Format("<div>结算单{0}不能作废:关联收款单{1}(订单{2})已确认", lgSettlementSysNo, fnLink,
                                                    soOrder.SysNo));

                    }
                    if (strMsg.Length > 0)
                        strMsg.Append(rmaConfirmed != 0 ? string.Format(",关联付款单{0}已确认</div>", rmaConfirmed) : "</div>");
                    else
                    {
                        if (rmaConfirmed != 0)
                            strMsg.Append(string.Format("<div>结算单{0}不能作废:关联付款单{1}未作废</div>", lgSettlementSysNo, fnPaymentVoucherSysNo));
                    }
                    //存在关联收款单或付款单已确认
                    if (strMsg.Length > 0)
                        lstData.Add(strMsg.ToString());
                }
                #region marked linq
                //lstData.AddRange(from soOrder in lstOrder
                //                 let fnReceiptVoucher = FnReceiptVoucherDaoImpl.Instance.GetEntity((int) FinanceStatus.收款来源类型.销售单, soOrder.SysNo)
                //                 where fnReceiptVoucher.Status == (int) FinanceStatus.收款单状态.已确认
                //                 //let url = "/Finance/ReceiptVoucherDetail/" + fnReceiptVoucher.SysNo
                //                 //let fnLink = "<span class='blue'><a name='fnLink' linkRef='" + url + "'>" + fnReceiptVoucher.SysNo + "</a></span>"
                //                 let fnLink = fnReceiptVoucher.SysNo
                //                 select string.Format("<div>结算单{0}关联收款单{1}(订单{2})已确认</div>", lgSettlementSysNo, fnLink, soOrder.SysNo));
                #endregion

            }
            if (lstData.Any())
                return new Result<List<string>>
                {
                    Status = false,
                    Data = lstData
                };
            return new Result<List<string>> { Status = true };

        }

        /// <summary>
        /// 判断是否首单拒收
        /// 该订单那所有出库单都是拒收则为第一单拒收,
        /// 如果存在其他签收出库单则不是第一单拒收
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>true:首单拒收;false:非首单拒收</returns>
        /// <remarks>2013-8-8 黄伟 创建</remarks>
        private bool IsRejectAtFirst(int orderSysNo)
        {
            var lstStockOut = OutStockDaoImpl.Instance.GetWhStockOutListByOrderID(orderSysNo).ToList();
            return lstStockOut.All(whStockOut => whStockOut.Status == (int)WarehouseStatus.出库单状态.拒收);
        }

        /// <summary>
        /// get the appsign info by delivery SysNos  
        /// </summary>
        /// <param name="deliverySysNo">配送员系统编号</param>
        /// <param name="noteSysNo">出库单</param>
        /// <param name="noteType">单据类型</param>
        /// <returns>list of LgAppSignStatus</returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        /// <remarks>2014-03-24 周唐炬 修改</remarks>
        public override LgAppSignStatus GetAppSignStatusforDeliveryItem(int deliverySysNo, int noteSysNo, int noteType)
        {
            const string sql = "select * from LgAppSignStatus where noteSysNo =(@sysnos) and NoteType=@noteType and DeliverySysNo=@deliverySysNo";
            return Context.Sql(sql).Parameter("sysnos", noteSysNo).Parameter("noteType", noteType).Parameter("deliverySysNo", deliverySysNo).QuerySingle<LgAppSignStatus>();
        }

        /// <summary>
        /// get the appsign info by delivery SysNos  
        /// </summary>
        /// <param name="deliverySysNos">list of delivery sysnos</param>
        /// <returns>list of LgAppSignStatus</returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        public override List<LgAppSignStatus> GetAppSignStatus(List<int> deliverySysNos)
        {
            var sql = "select * from LgAppSignStatus where deliverysysno in(@sysnos)";
            var lstResult = Context.Sql(sql).Parameter("sysnos", deliverySysNos).QueryMany<LgAppSignStatus>();
            return lstResult;
        }

        /// <summary>
        /// get the partial sign info by AppSignStatus sysnos  
        /// </summary>
        /// <param name="appSignSysNos">list of AppSignStatus sysnos</param>
        /// <returns>list of LgAppSignItem</returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        public override List<LgAppSignItem> GetAppSignItem(List<int> appSignSysNos)
        {
            var sql = "select * from LgAppSignItem where AppSignStatusSysNo in(@sysnos)";
            var lstResult = Context.Sql(sql).Parameter("sysnos", appSignSysNos).QueryMany<LgAppSignItem>();
            return lstResult;
        }


        /// <summary>
        /// get the partial sign info by AppSignStatus sysnos  
        /// </summary>
        /// <param name="appSignSysNo">AppSignStatus sysno</param>
        /// <returns>list of LgAppSignItem</returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        public override List<LgAppSignItem> GetAppSignItem(int appSignSysNo)
        {
            var sql = "select * from LgAppSignItem where AppSignStatusSysNo =@sysnos";
            var lstResult = Context.Sql(sql).Parameter("sysnos", appSignSysNo).QueryMany<LgAppSignItem>();
            return lstResult;
        }

    }
}
