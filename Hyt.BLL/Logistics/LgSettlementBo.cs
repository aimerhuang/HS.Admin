using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Hyt.BLL.Basic;
using Hyt.BLL.CRM;
using Hyt.BLL.Finance;
using Hyt.BLL.LevelPoint;
using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.BLL.RMA;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Finance;
using Hyt.DataAccess.RMA;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.DataAccess.Logistics;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using ILgPickUpDao = Hyt.DataAccess.Logistics.ILgPickUpDao;
using Hyt.BLL.Report;
using Hyt.DataAccess.Order;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// BO结算单业务逻辑
    /// </summary>
    /// <remarks>2013-06-15 黄伟 创建</remarks>
    public class LgSettlementBo : BOBase<LgSettlementBo>, ILgSettlementBo
    {
        /// <summary>
        /// 生成入库单标识
        /// </summary>
        const int InStockStatusCode = -1000;//生成入库单标识
        /// <summary>
        /// 查找/高级查找
        /// </summary>
        /// <param name="currPageIndex">当前pageIndex</param>
        /// <param name="searchParas">高级查询参数model</param>
        /// <param name="warehouses">当前登录人员有权限的仓库</param>
        /// <returns>结算单主表实体</returns>
        /// <remarks>2013-06-15 黄伟 创建</remarks>
        public Pager<CBLgSettlement> Search(int? currPageIndex, ParaLogisticsLgsettlement searchParas, List<int> warehouses)
        {
            return ILgSettlementDao.Instance.Search(currPageIndex, searchParas, warehouses);
        }

        /// <summary>
        /// 下拉选项-取得仓库列表
        /// </summary>
        /// <returns>仓库列表</returns>
        /// <remarks>2013-06-18 黄伟 创建</remarks>
        public Dictionary<int, string> GetWareHouse()
        {
            return ILgSettlementDao.Instance.GetWareHouse();
        }

        /// <summary>
        /// 下拉选项-取得配送人员列表
        /// </summary>
        /// <param name="whSysNo">仓库编号</param>
        /// <returns>配送人员列表</returns>
        /// <remarks>2013-06-18 黄伟 创建</remarks>
        public Dictionary<int, string> GetDeliveryMan(int? whSysNo)
        {
            return ILgSettlementDao.Instance.GetDeliveryMan(whSysNo);
        }

        /// <summary>
        /// 更新结算单 待结算 = 10,已结算 = 20,作废 = -10,
        /// </summary>
        /// <param name="lstSettlementSysNo">结算单系统编号列表逗号分隔</param>
        /// <param name="settlementStatus">操作类型:审核 approve或作废cancel</param>
        /// <param name="opreatorSysNo">操作人员</param>
        /// <returns>封装的实体(Status,StatusCode,Message)</returns>
        /// <remarks>
        /// 2013-06-28 黄伟 创建
        /// </remarks>
        [Obsolete("结算单主表作废逻辑")]
        public Result UpdateStatus(IList<int> lstSettlementSysNo, LogisticsStatus.结算单状态 settlementStatus,
                                   int opreatorSysNo)
        {
            //LogisticsStatus.结算单状态 settlementStatus = updateType == "approve"
            //                                             ? LogisticsStatus.结算单状态.已结算
            //                                             : LogisticsStatus.结算单状态.作废;

            return ILgSettlementDao.Instance.UpdateStatus(lstSettlementSysNo, settlementStatus, opreatorSysNo);
        }

        /// <summary>
        /// 获取结算单明细
        /// </summary>
        /// <param name="settlementSysNo">结算单主表系统编号</param>
        /// <returns>结算单明细列表</returns>
        /// <remarks>2013-06-28 黄伟 创建</remarks>
        public LgSettlement GetLgSettlementWithItems(int settlementSysNo)
        {
            return ILgSettlementItemDao.Instance.GetLgSettlementWithItems(settlementSysNo);
        }

        /// <summary>
        /// 通过订单号查询结算单列表
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>结算单列表</returns>
        /// <remarks>2013-12-06 余勇 创建</remarks>
        public List<CBLgSettlement> GetLgSettlementListByOrderSysNo(int orderSysNo)
        {
            return ILgSettlementDao.Instance.GetLgSettlementListByOrderSysNo(orderSysNo);
        }

        /// <summary>
        /// 根据出库单号获取结算单明细
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <returns>结算单明细</returns>
        /// <remarks>2013-08-22 黄志勇 创建</remarks>
        public LgSettlementItem GetLgSettlementItemByStockOut(int stockOutSysNo)
        {
            return ILgSettlementItemDao.Instance.GetLgSettlementItemByStockOut(stockOutSysNo);
        }

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="sysNo">结算单明细系统编号</param>
        /// <returns>出库单明细</returns>
        /// <remarks>2013-07-05 黄伟 创建</remarks>
        public List<WhStockOutItem> GetWhStockOutDetails(int sysNo)
        {
            return ILgSettlementItemDao.Instance.GetWhStockOutDetails(sysNo);
        }

        /// <summary>
        /// 获取生成结算单所需的model
        /// </summary>
        /// <param name="deliverySysNos">配送单系统编号集合</param>
        /// <param name="userSysNo">当前用户系统编号</param>
        /// <returns>生成结算单model</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        public CBLgSettlement GetCbLgSettlement(int[] deliverySysNos, int userSysNo)
        {
            //配送单明细列表(只含出库单)
            var lgDeliveryItemList =
                LgDeliveryBo.Instance.GetDeliveryItemList(deliverySysNos)
                //.Where(p => p.NoteType == LogisticsStatus.配送单据类型.出库单.GetHashCode()) //inclued 11.22 huangwei 
                            .ToList();
            //出库单系统编号集合
            var lstStockOutSysNos =
                lgDeliveryItemList.Where(p => p.NoteType == LogisticsStatus.配送单据类型.出库单.GetHashCode())
                                  .Select(p => p.NoteSysNo)
                                  .ToArray();
            var lstPickUps =
                lgDeliveryItemList.Where(p => p.NoteType == LogisticsStatus.配送单据类型.取件单.GetHashCode())
                                  .Select(p => p.NoteSysNo)
                                  .ToArray();

            return new CBLgSettlement
                {
                    CreatedBy = userSysNo, //创建人
                    DeliveryUserSysNo = LgDeliveryBo.Instance.GetDelivery(deliverySysNos[0]).DeliveryUserSysNo, //配送人
                    LgDeliveryItems = LgDeliveryBo.Instance.GetDeliveryItemList(deliverySysNos), //配送单明细集合
                    WhStockOuts = WhWarehouseBo.Instance.GetWhStockOutList(lstStockOutSysNos), //出库单集合
                    LgPickUps = lstPickUps.Any() ? ILgPickUpDao.Instance.GetLgPickUpList(lstPickUps) : null, //取件单集合
                    BsPaymentTypes = GetBsPaymentTypes(null),
                    LgDeliverys = LgDeliveryBo.Instance.GetDeliveryList(deliverySysNos)
                };
        }

        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <param name="sysNo">支付方式系统编号,null表示返回所有</param>
        /// <returns>支付方式列表</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        public IList<BsPaymentType> GetBsPaymentTypes(int? sysNo)
        {
            if (sysNo != null)
            {
                var list = new List<BsPaymentType> {PaymentTypeBo.Instance.GetPaymentTypeFromMemory(sysNo.Value)};
                return list;
            }
            return ILgSettlementDao.Instance.GetBsPaymentTypes(null);
        }


        /// <summary>
        /// 获取结算单明细支付方式集合
        /// </summary>
        /// <param name="item">结算单明细</param>
        /// <returns>支付方式集合</returns>
        /// <remarks>2013-07-31 黄伟 创建</remarks>
        public string GetSettlementDetailsPayType(LgSettlementItem item)
        {
            //收款明细支付方式
            var stockOutSysNo = item.StockOutSysNo;
            var soOrderSysNo = WhWarehouseBo.Instance.GetSoOrder(stockOutSysNo).SysNo;
            var fnReceiptVoucher = FnReceiptVoucherBo.Instance.GetReceiptVoucherByOrder(soOrderSysNo);
            //没有相关订单的收款单
            if (fnReceiptVoucher == null)
                return "";
            var fnReceiptVoucherSysNo = FnReceiptVoucherBo.Instance.GetReceiptVoucherByOrder(soOrderSysNo).SysNo;
            var lstFnReceiptVoucherItem = FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(fnReceiptVoucherSysNo);

            var lstRtn = new List<string>();
            //支付宝(凭证),现金(凭证),刷卡(凭证,卡号)
            lstFnReceiptVoucherItem.ForEach(p =>
                {
                    var lstPayType = GetBsPaymentTypes(p.PaymentTypeSysNo);
                    if (lstPayType != null && lstPayType.Any())
                    {
                        lstRtn.Add(lstPayType.First().PaymentName + "(" + p.VoucherNo +
                                   (string.IsNullOrWhiteSpace(p.CreditCardNumber) ? ")" : "," + p.CreditCardNumber + ")"));
                    }
                });
            return string.Join(",", lstRtn);
        }

        #region 创建结算单 hefang created,modified by huangwei

        /// <summary>
        /// 创建结算单
        /// </summary>
        /// <param name="cbCreateSettlement">生成结算单model</param>
        /// <param name="userIp">访问者ip</param>
        /// <param name="withTran">是否使用事物</param>
        /// <returns>Result int</returns>
        /// <remarks>
        /// 2013/7/2 何方 创建
        /// 2013-07-15 黄伟 修改
        /// 2013-11-25 黄伟 整理结构化代码
        /// </remarks>
        public Result<int> CreateSettlement(CBCreateSettlement cbCreateSettlement, string userIp, bool withTran = true)
        {
            var result = new Result<int> { Status = true };

            #region 配送单配送在途状态查核
            if (!cbCreateSettlement.DeliverySysNos.Any())
            {
                return new Result<int> { Status = false, Message = "找不到配送单,无法进行结算" };

            }
            //配送人员系统编号-配送单同一配送人员才能进行结算,取一笔找出
            var deliUser = LgDeliveryBo.Instance.GetDelivery(cbCreateSettlement.DeliverySysNos.FirstOrDefault()).DeliveryUserSysNo;
            cbCreateSettlement.DeliverySysNos.ToList().ForEach(sysNo =>
                {
                    var deli = LgDeliveryBo.Instance.GetDelivery(sysNo);

                    if (deli == null || deli.Status != LogisticsStatus.配送单状态.配送在途.GetHashCode())
                    {
                        result = new Result<int>
                            {
                                Status = false,
                                Message = string.Format("配送单{0}不是配送在途状态,不能进行结算!", sysNo)
                            };
                    }
                });
            if (!result.Status)
                return result;
            #endregion

            //获取model
            var model = GetCbLgSettlement(cbCreateSettlement.DeliverySysNos, cbCreateSettlement.OperatorSysNo);

            var lstSettlementItem = new List<CBLgSettlementItem> { };
            //settlementItemList 赋值
            /*
             一笔出库单
             * 对应一笔结算单明细
             * 对应一笔配送单明细
            */
            var codAmountTotal = 0.0m; //结算单到付总金额
            var paidAmountTotal = 0.0m; //结算单预付总金额

            if (model.WhStockOuts != null && model.WhStockOuts.Any())
            {
                #region 配送单中出库单处理

                /*
             * 添加到结算单明细
            */
                model.WhStockOuts.ToList().ForEach(p =>
                    {
                        var delliveryItem =
                            model.LgDeliveryItems.ToList()
                                 .FirstOrDefault(
                                     delItem =>
                                     delItem.NoteType == LogisticsStatus.配送单据类型.出库单.GetHashCode() &&
                                     delItem.NoteSysNo == p.SysNo);

                        var dicProduct = new Dictionary<int, int>();//orderItemSysNo,signQuantity

                        var stockOutInfo =
                            cbCreateSettlement.StockOutInfos.FirstOrDefault(cb => cb.StockOutSysNo == p.SysNo);
                        //if (stockOutInfo == null)
                        //{
                        //    throw new Exception("创建结算单,出现多个出库单号!");
                        //}
                        var soOrder = IOutStockDao.Instance.GetSoOrder(p.SysNo);
                        var signedProductInfos = stockOutInfo.SignedProductInfos;//the signed product infos when confirmed
                        //该出库单签收商品数量及系统编号
                        if (signedProductInfos != null && signedProductInfos.Any())
                            signedProductInfos.ToList().ForEach(q => dicProduct.Add(q.SysNo, q.Qty));

                        var theStockOutOfcbCreateSettlement =
                            cbCreateSettlement.StockOutInfos.Single(cb => cb.StockOutSysNo == p.SysNo);

                        decimal itemPayAmount;
                        //预付,则结算单item付款金额为0
                        if (p.IsCOD == WarehouseStatus.是否到付.否.GetHashCode())
                            itemPayAmount = 0;
                        else
                        {
                            //到付,判断用户有无输入:未填写则默认现金,出库单应收金额
                            var pList = theStockOutOfcbCreateSettlement.PayItemList;
                            var payAmountInput = pList == null
                                                     ? 0m
                                                     : theStockOutOfcbCreateSettlement.PayItemList.Sum(
                                                         payItem => payItem.PayAmount);
                            itemPayAmount = payAmountInput == 0 ? p.Receivable : payAmountInput;
                        }

                        lstSettlementItem.Add(new CBLgSettlementItem
                            {
                                CreatedBy = cbCreateSettlement.OperatorSysNo,
                                CreatedDate = DateTime.Now,
                                DeliverySysNo = delliveryItem == null ? 0 : delliveryItem.DeliverySysNo,
                                DeliveryUserSysNo = model.DeliveryUserSysNo,
                                DeliveryItemStatus = stockOutInfo.Status, //配送单明细状态
                                StockOutSysNo = p.SysNo,
                                OrderSysNo = p.OrderSysNO,
                                SignStatus = theStockOutOfcbCreateSettlement.Status,
                                PayAmount = itemPayAmount,
                                PayItemList = theStockOutOfcbCreateSettlement.PayItemList ?? new List<PayItem>(),
                                TransactionSysNo = soOrder.TransactionSysNo, //事务编号
                                SignNumber = dicProduct
                                //RMAOrderInfo = cbCreateSettlement.RMAOrderInfo //退货的订单相关信息
                            });
                        //该出库单为预付
                        //if (p.IsCOD == 0)
                        if (p.IsCOD == WarehouseStatus.是否到付.否.GetHashCode())
                        {
                            //结算单预付总金额
                            paidAmountTotal += p.StockOutAmount;
                        }
                        //到付
                        else
                        {
                            //结算单到付总金额
                            switch ((LogisticsStatus.配送单明细状态)theStockOutOfcbCreateSettlement.Status)
                            {
                                case LogisticsStatus.配送单明细状态.拒收:
                                    codAmountTotal += 0;
                                    break;
                                case LogisticsStatus.配送单明细状态.未送达:
                                    codAmountTotal += 0;
                                    break;
                                case LogisticsStatus.配送单明细状态.已签收:
                                    //部分签收及已签收
                                    codAmountTotal += itemPayAmount;
                                    break;
                            }
                        }
                    });
                #endregion
            }

            //没有出库单或者任何取件单
            if (lstSettlementItem.Count == 0 && (model.LgPickUps == null || !model.LgPickUps.Any()))
            {
                result = new Result<int> { Status = false, Message = "配送单中没有出库单或取件单" };
                return result;
            }

            #region save changes to DB
            Action doFun = () =>
            {
                var settlementSysNo = 0;
                string InStockNos = string.Empty;
                //有结算明细
                if (lstSettlementItem.Any())
                {
                    #region 结算单主表

                    var settlement = new LgSettlement
                    {
                        AuditDate = DateTime.Now,
                        AuditorSysNo = cbCreateSettlement.OperatorSysNo,
                        CODAmount = codAmountTotal,
                        CreatedBy = cbCreateSettlement.OperatorSysNo,
                        CreatedDate = DateTime.Now,
                        DeliveryUserSysNo = lstSettlementItem.First().DeliveryUserSysNo,
                        PaidAmount = paidAmountTotal,
                        TotalAmount = codAmountTotal, // + paidAmountTotal, 11.18只显示到付金额
                        Status = (int)LogisticsStatus.结算单状态.已结算,
                        Remarks = cbCreateSettlement.Remarks ?? "",
                        WarehouseSysNo = WhWarehouseBo.Instance.Get(lstSettlementItem.First().StockOutSysNo).WarehouseSysNo
                    };

                    //创建结算单主表
                    settlementSysNo = ILgSettlementDao.Instance.Create(settlement);

                    #endregion

                    #region 结算单明细(针对出库单)

                    foreach (var item in lstSettlementItem)
                    {
                        item.SettlementSysNo = settlementSysNo;
                        item.Status = (int)LogisticsStatus.结算单明细状态.已结算;
                        ILgSettlementItemDao.Instance.Create(item);
                        //修改配送单明细状态
                        LgDeliveryBo.Instance.UpdateDeliveryItemStatus(item.DeliverySysNo, LogisticsStatus.配送单据类型.出库单,
                                                                       item.StockOutSysNo,
                                                                       (LogisticsStatus.配送单明细状态)item.DeliveryItemStatus);
                        //获取出库单
                        var stockOut = WhWarehouseBo.Instance.Get(item.StockOutSysNo);
                        if (stockOut != null)
                        {
                            var soorderValid = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO).OrderSource !=
                   OrderStatus.销售单来源.业务员补单.GetHashCode();
                            if (!soorderValid && (LogisticsStatus.配送单明细状态)item.SignStatus != LogisticsStatus.配送单明细状态.已签收)
                            {
                                throw new Exception("订单 " + stockOut.OrderSysNO + " 来源为补单,只能选择签收!");
                            }

                        }

                        //根据结算状态走流程
                        switch ((LogisticsStatus.配送单明细状态)item.SignStatus)
                        {
                            case LogisticsStatus.配送单明细状态.拒收:
                                var r1 = Reject(settlementSysNo, item.DeliverySysNo, item.StockOutSysNo,
                                        cbCreateSettlement.OperatorSysNo);
                                InStockNos = BuildInStockNos(InStockNos, r1);

                                break;
                            case LogisticsStatus.配送单明细状态.未送达:
                                var r2 = Undeliverable(settlementSysNo, item.DeliverySysNo, item.StockOutSysNo,
                                                cbCreateSettlement.OperatorSysNo);
                                InStockNos = BuildInStockNos(InStockNos, r2);

                                break;
                            case LogisticsStatus.配送单明细状态.已签收:
                                Sign(item, cbCreateSettlement.OperatorSysNo);
                                break;
                            default:
                                throw new Exception("配送单状态错误");
                        }
                    }

                    #endregion
                }

                #region 配送单中取件单处理
                //获取配送人员信用额度,第三方快递信用为null

                /*logic for lgPickUps取件单
                                    1.将配送单下非已入库的取件单置为待取件
                                    2.增加配送员信用额度
                                */
                if (model.LgPickUps != null)
                {
                    model.LgPickUps.ToList().ForEach(p =>
                    {
                        #region update credit--third party no user credit

                        var creditAmount = LgDeliveryBo.Instance.GetLgPickUpTotalCount(new[] { p.SysNo });
                        DeliveryUserCreditBo.Instance.UpdateRemaining(p.WarehouseSysNo, deliUser, creditAmount, 0, "配送取件结算,结算单号:" + settlementSysNo);


                        #endregion

                        #region 更改取件单状态
                        if (p.Status != LogisticsStatus.取件单状态.已入库.GetHashCode())
                        {
                            //更改取件单非已入库的为待取件
                            LgPickUpBo.Instance.UpdatePickUpStatus(p.SysNo,
                                                                   LogisticsStatus.取件单状态.待取件,
                                                                   cbCreateSettlement.OperatorSysNo);

                            //更改配送单明细为未送达
                            //更新配送单明细状态为未送达
                            var theLgDelItem =
                                model.LgDeliveryItems.Single(
                                    delItem =>
                                    delItem.NoteType == LogisticsStatus.配送单据类型.取件单.GetHashCode() &&
                                    delItem.NoteSysNo == p.SysNo);
                            LgDeliveryBo.Instance.UpdateDeliveryItemStatus(theLgDelItem.DeliverySysNo, LogisticsStatus.配送单据类型.取件单, p.SysNo,
                                                                           LogisticsStatus.配送单明细状态.未送达);

                        }
                        else
                        {
                            #region 更新配送单明细状态为已签收)
                            var theLgDelItem = model.LgDeliveryItems.Single(
                                                    delItem =>
                                                    delItem.NoteType == LogisticsStatus.配送单据类型.取件单.GetHashCode() &&
                                                    delItem.NoteSysNo == p.SysNo);

                            LgDeliveryBo.Instance.UpdateDeliveryItemStatus(theLgDelItem.DeliverySysNo, LogisticsStatus.配送单据类型.取件单, p.SysNo,
                                                                           LogisticsStatus.配送单明细状态.已签收);
                            #endregion
                        }
                        #endregion

                    });
                }

                #endregion

                #region 更新配送单状态为已结算

                //var deliverySysNos = (from item in lstSettlementItem
                //                      select item.DeliverySysNo).Distinct().ToList();
                foreach (var deliverySysNo in cbCreateSettlement.DeliverySysNos)
                {
                    LgDeliveryBo.Instance.UpdateStatus(deliverySysNo, LogisticsStatus.配送单状态.已结算);
                }

                #endregion

                result = new Result<int> { Status = true, Message = "结算创建成功.", Data = settlementSysNo };

                if (result.Status && !string.IsNullOrEmpty(InStockNos)) //提示当前入库单号去入库
                {
                    result.Message += "请注意完成入库单(" + InStockNos + ")的入库操作";
                }

                //log
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "创建结算单",
                                         LogStatus.系统日志目标类型.结算单, settlementSysNo, null, userIp,
                                         cbCreateSettlement.OperatorSysNo);
            };
            try
            {
                 
                    doFun.Invoke();
                
            }
            catch (Exception ex)
            {
                result = new Result<int> { Status = false, Message = ex.Message };
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.结算单, 0, ex,
                                      cbCreateSettlement.OperatorSysNo);
                //return result;
                throw;  //抛出异常 余勇 修改 2014-06-19
            }

            #endregion

            if (!lstSettlementItem.Any())
            {
                //只有取件单,未生成结算单,返回结算单系统编号为0
                result.Data = 0;
            }

            return result;
        }
        /// <summary>
        /// 组合入库单号显示
        /// </summary>
        /// <param name="InStockNos"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private string BuildInStockNos(string inStockNos, Result r)
        {
            string str = inStockNos;
            if (r.StatusCode == InStockStatusCode)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += ",";
                }
                str += r.Message;
            }
            return str;
        }

        #region app签收

        /// <summary>
        /// get the appsign info by delivery SysNos  
        /// </summary>
        /// <param name="deliverySysNos">list of delivery sysnos</param>
        /// <returns>list of LgAppSignStatus</returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        public Result<List<LgAppSignInfo>> GetAppSignInfo(List<int> deliverySysNos)
        {
            try
            {
                var lstLgAppSignStatus = ILgSettlementDao.Instance.GetAppSignStatus(deliverySysNos);
                if (!lstLgAppSignStatus.Any())
                {
                    return new Result<List<LgAppSignInfo>> { Data = new List<LgAppSignInfo>(), Status = true };
                }
                var lstLgAppSignItem =
                    ILgSettlementDao.Instance.GetAppSignItem(lstLgAppSignStatus.Select(p => p.SysNo).ToList());
                var lstLgAppSignInfo = new List<LgAppSignInfo>();
                lstLgAppSignStatus.ForEach(p =>
                    {
                        var signItems = lstLgAppSignItem.Where(item => item.AppSignStatusSysNo == p.SysNo)
                            .Select(item => new SignedProductInfo { SysNo = item.NoteItemSysNo, Qty = item.SignQuantity }).ToList();
                        //将出库单明细编号转换为订单明细编号
                        var signItemsOrderItem =
                            signItems.Select(
                                si =>
                                new SignedProductInfo
                                    {
                                        SysNo = WhWarehouseBo.Instance.GetWhStockOutItem(si.SysNo).OrderItemSysNo,
                                        Qty = si.Qty
                                    }).ToList();


                        //var signOption = (LogisticsStatus.配送单明细状态) p.Status + "";
                        lstLgAppSignInfo.Add(new LgAppSignInfo
                            {
                                DelSysNo = p.SysNo,
                                NoteSysNo = p.NoteSysNo,
                                NoteType = p.NoteType,
                                //SignOption = signItems.Any() ? "部分签收" : signOption == "已签收" ? "签收" : signOption,
                                SignOption = p.Status,
                                SignedProductInfos = signItemsOrderItem
                            });
                    });
                return new Result<List<LgAppSignInfo>> { Data = lstLgAppSignInfo, Status = true };
            }
            catch (Exception ex)
            {
                return new Result<List<LgAppSignInfo>> { Status = false, Message = @"服务器发生错误,请联系管理员." };
            }
        }

        #endregion

        #region 未送达

        /// <summary>
        /// 未能送达
        /// </summary>
        /// <param name="settlementSysNo">结算单系统编号.</param>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="stockOutSysNo">出库单系统编号.</param>
        /// <param name="operatorSysNo">操作人系统编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-11 何方 创建
        /// </remarks>
        private Result Undeliverable(int settlementSysNo, int deliverySysNo, int stockOutSysNo, int operatorSysNo)
        {
            Result r = new Result() { Status = true };
            //更新配送单明细状态为未送达
            LgDeliveryBo.Instance.UpdateDeliveryItemStatus(deliverySysNo, LogisticsStatus.配送单据类型.出库单, stockOutSysNo,
                                                           LogisticsStatus.配送单明细状态.未送达);
            //修改出库单状态为待配送
            WhWarehouseBo.Instance.UpdateStockOutStatus(stockOutSysNo, WarehouseStatus.出库单状态.待配送, operatorSysNo);

            var stockOut = WhWarehouseBo.Instance.Get(stockOutSysNo);

            //增加配送员信用额度 黄伟 2013-12-23
            var deliUserSysNo = LgDeliveryBo.Instance.GetDelivery(deliverySysNo).DeliveryUserSysNo;
            var creditAmount = LgDeliveryBo.Instance.GetDeliveryCredit(new[] { stockOutSysNo });
            DeliveryUserCreditBo.Instance.UpdateRemaining(stockOut.WarehouseSysNo, deliUserSysNo, creditAmount, 0, string.Format("配送未送达,出库单号:{0},结算单号:{1}", stockOutSysNo, settlementSysNo));

            var soOrderEn = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);
            if (soOrderEn != null)
            {
                SoOrderBo.Instance.WriteSoTransactionLog(soOrderEn.TransactionSysNo,
                                                         string.Format("结算未送达:{0}", settlementSysNo),
                                                         SyUserBo.Instance.GetUserName(operatorSysNo));
            }
            r.StatusCode = InStockStatusCode;//未送达入库
            r.Message = Hyt.BLL.Logistics.LgDeliveryBo.Instance.CreateInStockByStockOut(stockOut, operatorSysNo, string.Format("配送未送达,出库单号:{0},结算单号:{1}", stockOutSysNo, settlementSysNo)).ToString();
            return r;
        }

        #endregion

        #region 拒收

        /// <summary>
        /// 拒收
        /// </summary>
        /// <param name="settlementSysNo">结算单系统编号.</param>
        /// <param name="deliverySysNo">配送单系统编号.</param>
        /// <param name="stockOutSysNo">出库单系统编号.</param>
        /// <param name="operatorSysNo">操作人系统编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-11 何方 创建
        /// </remarks>
        private Result Reject(int settlementSysNo, int deliverySysNo, int stockOutSysNo, int operatorSysNo)
        {
            Result r = new Result() { Status = true };
            //获取出库单
            var stockOut = WhWarehouseBo.Instance.Get(stockOutSysNo);
            if (stockOut == null)
            {
                throw new Exception("不存在系统编号为" + stockOutSysNo + "的出库单");
            }

            //获取收款单
            var receiptVoucher = IFnReceiptVoucherDao.Instance.GetEntity((int)FinanceStatus.收款来源类型.销售单,
                                                                         stockOut.OrderSysNO);
            if (receiptVoucher == null)
            {
                throw new Exception("订单 " + stockOut.OrderSysNO + " 不存在收款单");
            }

            //增加配送员信用额度 黄伟 2013-12-18
            var deliUserSysNo = LgDeliveryBo.Instance.GetDelivery(deliverySysNo).DeliveryUserSysNo;
            var creditAmount = LgDeliveryBo.Instance.GetDeliveryCredit(new[] { stockOutSysNo });
            DeliveryUserCreditBo.Instance.UpdateRemaining(stockOut.WarehouseSysNo, deliUserSysNo, creditAmount, 0, string.Format("配送拒收,出库单号:{0},结算单号:{1}", stockOutSysNo, settlementSysNo));

            //修改出库单状态为拒收
            WhWarehouseBo.Instance.UpdateStockOutStatus(stockOutSysNo, WarehouseStatus.出库单状态.拒收, operatorSysNo);

            var returnNumber = new Dictionary<int, int> { };
            foreach (var item in stockOut.Items)
            {
                returnNumber.Add(item.OrderItemSysNo, item.ProductQuantity);
            }

            //判断订单是否已经付款
            //if (receiptVoucher.Status == (int) FinanceStatus.收款单状态.已确认)
            if (stockOut.Receivable == 0)
            {
                #region 退货

                //set the orderItemSysno and qty --huangwei 2013-11-21
                var stockoutItems = stockOut.Items.ToList();
                var orderItems =
                    stockoutItems.Select(
                        p => new CBRMAOrderItemInfo { OrderItemSysNo = p.OrderItemSysNo, Qty = p.ProductQuantity })
                                 .Distinct().ToList();
                var rmaOrderInfo = new CBRMAOrderInfo
                    {
                        OrderSysNo = stockOut.OrderSysNO,
                        lstRMAOrderItemInfo = orderItems
                    };

                /* kept for check logic.--huangwei 2013-11-21
                var orderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(stockOut.OrderSysNO);
                var rmaOrderInfo = new CBRMAOrderInfo
                    {
                        OrderSysNo = stockOut.OrderSysNO,
                        lstRMAOrderItemInfo =
                            orderItems.Select(p => new CBRMAOrderItemInfo {OrderItemSysNo = p.SysNo, Qty = p.Quantity})
                                      .ToList()
                    };
                */
                Return(stockOut.SysNo, returnNumber, operatorSysNo, "会员拒收", false, RmaStatus.退换货申请单来源.拒收);

                #endregion
            }
            else //未付款的当做取消订单处理
            {
                //作废收款单
                FnReceiptVoucherBo.Instance.UpdateOrderReceiptStatus(stockOut.OrderSysNO, FinanceStatus.收款单状态.作废);

                /*
                货到付款-第一单签拒收,拒收的出库单:生成入库单,所有已出库的出库单需要生成入库单,所有出库单状态改为拒收. 
                */

                //当前已出库出库单需要生成入库单
                r.StatusCode = InStockStatusCode;//未付款拒收入库
                r.Message = Hyt.BLL.Logistics.LgDeliveryBo.Instance.CreateInStockByStockOut(stockOut, operatorSysNo, string.Format("配送拒收,出库单号:{0},结算单号:{1}", stockOutSysNo, settlementSysNo)).ToString();

                //更新订单所有出库单状态为拒收
                WhWarehouseBo.Instance.UpdateOrderStockOutStatus(stockOut.OrderSysNO, WarehouseStatus.出库单状态.拒收,
                                                                 operatorSysNo);
                SoOrderBo.Instance.DeclinedOrder(stockOut.OrderSysNO, SyUserBo.Instance.GetSyUser(operatorSysNo));
            }

            var soOrderEn = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);
            if (soOrderEn != null)
            {
                SoOrderBo.Instance.WriteSoTransactionLog(soOrderEn.TransactionSysNo,
                                                         string.Format("结算拒收:{0}", settlementSysNo),
                                                         SyUserBo.Instance.GetUserName(operatorSysNo));
            }
            return r;
        }

        #endregion

        #region 签收,部分签收

        /// <summary>
        /// 签收,部分签收
        /// </summary>
        /// <param name="item">结算明细.</param>
        /// <param name="operatorSysNo">操作人.</param>
        /// <returns>void</returns>
        /// <remarks>
        /// 2013-07-11 何方 创建
        /// </remarks>
        private void Sign(CBLgSettlementItem item, int operatorSysNo)
        {
            #region 出库单增加配送员信用额度 黄伟

            var theStockOut = WhWarehouseBo.Instance.Get(item.StockOutSysNo);
            var userCredit =
                ILgDeliveryUserCreditDao.Instance.GetLgDeliveryUserCredit(item.DeliveryUserSysNo,
                                                                          theStockOut.WarehouseSysNo);
            if (userCredit != null)
            {
                //userCredit.RemainingDeliveryCredit += theStockOut.StockOutAmount;
                //userCredit.RemainingDeliveryCredit +=
                //    LgDeliveryBo.Instance.GetDeliveryCredit(new[] { item.StockOutSysNo });
                //ILgDeliveryUserCreditDao.Instance.Update(userCredit);

                var creditAmount = LgDeliveryBo.Instance.GetDeliveryCredit(new[] { item.StockOutSysNo });
                DeliveryUserCreditBo.Instance.UpdateRemaining(theStockOut.WarehouseSysNo, userCredit.DeliveryUserSysNo, creditAmount, 0, "出库签收结算,结算单号:" + item.SettlementSysNo);
            }

            #endregion 增加配送员信用额度

            #region 更新配送单明细状态为已签收)
            LgDeliveryBo.Instance.UpdateDeliveryItemStatus(item.DeliverySysNo, LogisticsStatus.配送单据类型.出库单,
                                                           item.StockOutSysNo, LogisticsStatus.配送单明细状态.已签收);
            #endregion

            #region 修改出库单状态为已签收
            WhWarehouseBo.Instance.UpdateStockOutStatus(item.StockOutSysNo, WarehouseStatus.出库单状态.已签收, operatorSysNo);

            var receipt = IFnReceiptVoucherDao.Instance.GetEntity((int)FinanceStatus.付款来源类型.销售单, item.OrderSysNo);
            if (receipt == null)
            {
                throw new Exception(string.Format("订单 {0} 不存在收款单", item.OrderSysNo));
            }

            var isPartialSign = item.SignNumber != null && item.SignNumber.Any();
            #endregion

            #region 添加收款单明细信息

            /*eascode收款科目:未输入则取现金和出库单仓库*/
            var modelEasCode =
                GetEasCodeByWhAndPayType(theStockOut.WarehouseSysNo, Hyt.Model.SystemPredefined.PaymentType.现金)
                    .FirstOrDefault(p => p.Selected);

            /*
            * 如果是预付,则不写入收款单
            */
            if (item.PayAmount != 0)
            {
                var receiptVoucherItemList = new List<FnReceiptVoucherItem> { };

                //没有选择更多支付方式
                if (item.PayItemList == null || item.PayItemList.Count == 0)
                {
                    receiptVoucherItemList.Add
                        (new FnReceiptVoucherItem
                            {
                                Amount = theStockOut.Receivable, //结算改为全款 
                                CreatedBy = operatorSysNo,
                                CreatedDate = DateTime.Now,
                                // PaymentTypeSysNo = GetBsPaymentTypes(null).First(pay => pay.PaymentName == "现金").SysNo,
                                PaymentTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.现金,
                                VoucherNo = "",
                                CreditCardNumber = "",
                                ReceiptVoucherSysNo = receipt.SysNo,
                                Status = FinanceStatus.收款单明细状态.有效.GetHashCode(),
                                ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.仓库,
                                //2013.11.6 朱成果
                                ReceivablesSideSysNo = theStockOut.WarehouseSysNo, //2013.11.6 朱成果,
                                EasReceiptCode = modelEasCode == null ? null : modelEasCode.Value
                                //huangwei 2013-11-19
                            });
                }
                //选择了更多支付方式
                else
                {
                    receiptVoucherItemList = (
                                                 from p in item.PayItemList
                                                 select new FnReceiptVoucherItem
                                                     {
                                                         //结算改为全款
                                                         //Amount = p.PayAmount,
                                                         Amount = p.PayAmount, //the amount of the specific paytype 
                                                         CreatedBy = operatorSysNo,
                                                         CreatedDate = DateTime.Now,
                                                         PaymentTypeSysNo = p.PayType,
                                                         VoucherNo = p.PayNo,
                                                         CreditCardNumber = p.PosCardNo,
                                                         ReceiptVoucherSysNo = receipt.SysNo,
                                                         Status = FinanceStatus.收款单明细状态.有效.GetHashCode(),
                                                         ReceivablesSideType =
                                                             (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.仓库,
                                                         //2013.11.6 朱成果
                                                         ReceivablesSideSysNo = theStockOut.WarehouseSysNo,
                                                         //2013.11.6 朱成果
                                                         EasReceiptCode = p.EasCode == "无对应科目" ? null : p.EasCode
                                                         //huangwei 2013-11-19
                                                     }).ToList();

                }

                #region 金额特殊处理,补全金额


                /*签收部分签收都结算全款
                    */
                /*将退货金额即差额写入现金*/
                //部分签收会写入signnumer

                //get the right amount 现金=出库单应收金额-其他支付方式金额

                decimal payAmount = receiptVoucherItemList.Sum(payItem => payItem.Amount);//实付总额

                var cashPayTypeAmount = payAmount < theStockOut.Receivable ? theStockOut.Receivable - payAmount : 0;//补充现金支付


                if (cashPayTypeAmount > 0)
                {
                    if (isPartialSign)
                    {
                        //add the cash payment
                        receiptVoucherItemList.Add(new FnReceiptVoucherItem
                            {
                                Amount = cashPayTypeAmount,
                                CreatedBy = operatorSysNo,
                                CreatedDate = DateTime.Now,
                                PaymentTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.现金,
                                //VoucherNo = p.PayNo,
                                //CreditCardNumber = p.PosCardNo,
                                ReceiptVoucherSysNo = receipt.SysNo,
                                Status = FinanceStatus.收款单明细状态.有效.GetHashCode(),
                                ReceivablesSideType =
                                    (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.仓库,
                                //2013.11.6 朱成果
                                ReceivablesSideSysNo = theStockOut.WarehouseSysNo,
                                //2013.11.6 朱成果
                                EasReceiptCode = modelEasCode == null ? null : modelEasCode.Value
                                //huangwei 2013-11-19

                            });
                    }
                    else
                    {
                        throw new Exception("实收金额小于应收金额");
                    }
                }

                #endregion

                FnReceiptVoucherBo.Instance.CreateReceiptVoucherItem(receiptVoucherItemList);

                /*
                 Step1修改: 收款单实收金额=收款单实收金额+结算收款金额;收款单ReceivedAmount+=theStockOut.Receivable
                 Step2如果 收款单实收金额==收款单应收金额,修改订单支付状态为已付款IncomeAmount=ReceivedAmount
                 */
                receipt.ReceivedAmount = payAmount + cashPayTypeAmount;

                //黄伟 可能会出现负数,金额不相等
                //if (receipt.ReceivedAmount == receipt.IncomeAmount)
                //{

                //修改订单支付状态为已付款
                SoOrderBo.Instance.UpdatePayStatus(theStockOut.OrderSysNO, OrderStatus.销售单支付状态.已支付);
                //}
                //同步支付时间的到订单主表
                ISoOrderDao.Instance.UpdateOrderPayDteById(theStockOut.OrderSysNO);
                FnReceiptVoucherBo.Instance.Update(receipt);

                //自动确认收款单 11.29 huangwei
                FnReceiptVoucherBo.Instance.AutoConfirmReceiptVoucher(theStockOut.OrderSysNO, SyUserBo.Instance.GetSyUser(operatorSysNo));
            }

            //var test = IFnReceiptVoucherDao.Instance.GetEntity((int)FinanceStatus.付款来源类型.销售单, item.OrderSysNo).Status;
            #endregion

            #region 写入订单日志
            var soOrder = SoOrderBo.Instance.GetEntity(theStockOut.OrderSysNO);
            if (soOrder != null)
            {
                var deliveryType = DeliveryTypeBo.Instance.GetDeliveryType(theStockOut.DeliveryTypeSysNo);
                var signtype = isPartialSign ? "部分签收" : "签收";
                if (deliveryType.IsThirdPartyExpress == 1)
                {
                    signtype = deliveryType.DeliveryTypeName + "发货";
                }
                string partialSignMsg = null;
                var lstPartialSignInfo = new List<string>();
                if (isPartialSign)
                {
                    item.SignNumber.ToList().ForEach(p => lstPartialSignInfo.Add(p.Key + ":" + p.Value + ";"));
                }
                partialSignMsg = lstPartialSignInfo.Any() ? "--部分签收明细(订单明细编号:数量):" + string.Join("", lstPartialSignInfo) : "";

                var orderLog = string.Format("结算单（{0}）已生成({1}),出库单{2}，本次交易完成，欢迎您再次光临！{3}", item.SettlementSysNo,
                                            signtype, theStockOut.SysNo, partialSignMsg);
                //系统日志
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "订单签收日志:" + orderLog,
                         LogStatus.系统日志目标类型.订单, theStockOut.OrderSysNO, null, null,
                         operatorSysNo);
                ;
                //订单日志
                orderLog = string.Format(" 生成结算单（{0}），{1}出库单{2}。", item.SettlementSysNo,
                                         signtype, theStockOut.SysNo);
                SoOrderBo.Instance.WriteSoTransactionLog(soOrder.TransactionSysNo,
                                         orderLog,
                                         SyUserBo.Instance.GetUserName(operatorSysNo));
                //SoOrderBo.Instance.WriteSoTransactionLog(soOrder.TransactionSysNo,
                //                                         orderLog,
                //                                         SyUserBo.Instance.GetUserName(operatorSysNo));
            }
            #endregion

            #region 签收商品明细数量是否少于订单出库明细商品数量则进行退货

            if (!isPartialSign)
            {
                //没有签收数量默认为全部签收,不进行退货
                return;
            }
            //获取出库单
            var stockOut = WhWarehouseBo.Instance.Get(item.StockOutSysNo);
            //出库商品总数
            var stockOutProductCount = stockOut.Items.Sum(x => x.ProductQuantity);
            //签收商品总数
            var signProductCount = item.SignNumber.Sum(x => x.Value);

            if (signProductCount < stockOutProductCount)
            {
                var returnNumber = new Dictionary<int, int>();
                foreach (var orderItemSysNo in item.SignNumber.Keys)
                {
                    if (item.SignNumber[orderItemSysNo] < 0)
                    {
                        throw new Exception(string.Format("商品 {0} 签收数量为{1},签收商品数量不能小于0", orderItemSysNo,
                                                          item.SignNumber[orderItemSysNo]));
                    }

                    var productQuantity = stockOut.Items.First(x => x.OrderItemSysNo == orderItemSysNo).ProductQuantity;
                    var returnQuantity = productQuantity -
                                         item.SignNumber[orderItemSysNo];

                    if (returnQuantity < 0)
                    {
                        throw new Exception(string.Format("商品 {0} 签收数量为{1},签收商品数量不能大于出库数量{2}", orderItemSysNo,
                                                          item.SignNumber[orderItemSysNo], productQuantity));
                    }
                    if (returnQuantity != 0)
                    {
                        returnNumber.Add(orderItemSysNo, returnQuantity);
                    }
                }
                Return(stockOut.SysNo, returnNumber, operatorSysNo, "部分签收", true, RmaStatus.退换货申请单来源.部分签收);
            }

            #endregion

        }

        #endregion

        #region 退货

        /// <summary>
        /// 退货
        /// </summary>
        /// <param name="stockOutSysNo">出库单单号.</param>
        /// <param name="returnNumber">退货商品号和数量.</param>
        /// <param name="operatorSysNo">操作人系统编号.</param>
        /// <param name="reason">退货原因.</param>
        /// <param name="autoComplete">是否自动完成</param>
        /// <param name="sourceType">退换货申请来源</param>
        /// <returns>void</returns>
        /// <remarks>
        /// 2013/7/12 何方 创建
        /// 2013-08-07 黄伟 修改 添加deliveryUserSysNo以扣减配送员信用额度
        /// </remarks>
        private void Return(int stockOutSysNo, Dictionary<int, int> returnNumber, int operatorSysNo, string reason,
                            bool autoComplete, RmaStatus.退换货申请单来源 sourceType)
        {
            //获取出库单
            var stockOut = WhWarehouseBo.Instance.GetStockOutInfo(stockOutSysNo);

            //退货
            Return(stockOut, returnNumber, operatorSysNo, reason, autoComplete, sourceType);
        }

        /// <summary>
        /// 已付款拒收或部分签收.创建退货
        /// </summary>
        /// <param name="stockOut">出库单</param>
        /// <param name="returnNumber">退货订单明细编号和数量.</param>
        /// <param name="operatorSysNo">操作人</param>
        /// <param name="reason">退货原因.</param>
        /// <param name="autoComplete">是否自动完成</param>
        /// <param name="sourceType">退换货申请单来源</param>
        /// <returns>void</returns>
        /// <exception cref="System.Exception">商品退货数量小于等于
        /// or
        /// 商品退货数量大于等于出库数量,不能进行退货</exception>
        /// <remarks>
        /// 2013-07-11 何方 创建
        /// </remarks>
        private void Return(CBWhStockOut stockOut, Dictionary<int, int> returnNumber, int operatorSysNo, string reason,
                            bool autoComplete, RmaStatus.退换货申请单来源 sourceType)
        {

            if (returnNumber == null || !returnNumber.Any())
            {
                throw new Exception("商品退货数量小于等于0");
            }
            //var order = SoOrderBo.Instance.GetEntity(cbRMAOrderInfo.OrderSysNo);
            var order = SoOrderBo.Instance.GetEntity(stockOut.OrderSysNO);
            //出库商品总数
            var stockOutProductCount = stockOut.Items.Sum(x => x.ProductQuantity);
            //退货商品总数
            var returnProductCount = returnNumber.Values.Sum();

            if (returnProductCount <= 0)
            {
                throw new Exception("商品退货数量小于等于0");
            }

            if (returnProductCount > stockOutProductCount)
            {
                throw new Exception("商品退货数量大于出库数量,不能进行退货");
            }

            //调用接口重新计算部分签收订单的金额
            //应退金额
            //var returnAmount = RmaBo.Instance.CalculateRmaAmount(cbRMAOrderInfo.OrderSysNo,
            //                                                     cbRMAOrderInfo.lstRMAOrderItemInfo.ToDictionary(
            //                                                         p => p.OrderItemSysNo,
            //                                                         p => p.Qty)).OrginAmount;
            var returnCalculate = RmaBo.Instance.CalculateRmaAmount(order.SysNo, returnNumber);

            #region 创建退货单(RMA)

            var retrun = new CBRcReturn
                {
                    TransactionSysNo= Guid.NewGuid().ToString().Replace("-", ""),
                    AuditorBy = operatorSysNo,
                    AuditorDate = DateTime.Now,
                    CreateBy = operatorSysNo,
                    CreateDate = DateTime.Now,
                    LastUpdateBy = operatorSysNo,
                    LastUpdateDate = DateTime.Now,
                    CustomerSysNo = order.CustomerSysNo,
                    HandleDepartment = (int)Model.WorkflowStatus.RmaStatus.退换货处理部门.客服中心,
                    OrderSysNo = stockOut.OrderSysNO,
                    OrginAmount = returnCalculate.OrginAmount,
                    OrginPoint = returnCalculate.OrginPoint,
                    OrginCoin = returnCalculate.OrginCoin,
                    InvoiceSysNo = stockOut.InvoiceSysNo,
                    IsPickUpInvoice = (int)RmaStatus.是否取回发票.是,
                    PickUpAddressSysNo = stockOut.ReceiveAddressSysNo,
                    PickUpTime = "",
                    RefundBy = operatorSysNo,
                    RefundDate = DateTime.Now,
                    RefundTotalAmount = returnCalculate.RefundTotalAmount,
                    RefundPoint = returnCalculate.RefundPoint,
                    RefundCoin = returnCalculate.RefundCoin,
                    RefundType = (int)RmaStatus.退换货退款方式.原路返回,
                    RMARemark = "",
                    RmaType = (int)RmaStatus.RMA类型.售后退货,
                    Source = (int)sourceType,
                    WarehouseName = stockOut.WarehouseName,
                    WarehouseSysNo = stockOut.WarehouseSysNo,
                    PickupTypeSysNo = PickupType.普通取件,
                    Status = (int)(autoComplete ? RmaStatus.退换货状态.已完成 : RmaStatus.退换货状态.待入库) //自动完则为已完成,负责为待入库

                };
            var rmaSysNo = DataAccess.RMA.IRcReturnDao.Instance.Insert(retrun);
            var rma = RmaBo.Instance.GetRMA(rmaSysNo);

            #region 创建退货换单明细

            foreach (var r in returnNumber)
            {
                var stockOutItem = stockOut.Items.First(x => x.OrderItemSysNo == r.Key);
                var orderItem = SoOrderBo.Instance.GetOrderItem(r.Key);
                var item = new RcReturnItem
                    {

                        ProductSysNo = orderItem.ProductSysNo,
                        ReturnSysNo = rmaSysNo,
                        RmaQuantity = r.Value,
                        RmaReason = reason,
                        ReturnType = (int)RmaStatus.商品退换货类型.新品,
                        TransactionSysNo = rma.TransactionSysNo,
                        OriginPrice = stockOutItem.OriginalPrice,
                        ReturnPriceType = (int)RmaStatus.商品退款价格类型.原价,
                        RefundProductAmount = returnCalculate.OrderItemAmount.FirstOrDefault(t => t.Key == r.Key).Value,
                        StockOutItemSysNo = stockOutItem.SysNo,
                        ProductName = stockOutItem.ProductName

                    };
                item.SysNo=Hyt.DataAccess.RMA.IRcReturnItemDao.Instance.Insert(item); //插入明细表
                rma.RMAItems.Add(item);
            }

            #endregion

            #endregion

            #region 创建入库单
            var transactionSysNo = rma.TransactionSysNo;
           //var stockInSysNo = InStockBo.Instance.CreateStockIn(stockOut, returnNumber, operatorSysNo,
            //                                                    WarehouseStatus.入库单据类型.RMA单,
            //                                                    rmaSysNo, transactionSysNo);
            WhStockIn inEntity = new WhStockIn
            {
                CreatedBy = operatorSysNo,
                CreatedDate = DateTime.Now,
                DeliveryType = (int)WarehouseStatus.入库物流方式.拒收,
                IsPrinted = (int)WarehouseStatus.是否已经打印拣货单.否,
                SourceSysNO = rma.SysNo,
                SourceType = (int)WarehouseStatus.入库单据类型.RMA单,
                Status = autoComplete ? (int)WarehouseStatus.入库单状态.已入库 : (int)WarehouseStatus.入库单状态.待入库,
                TransactionSysNo = rma.TransactionSysNo,
                WarehouseSysNo = rma.WarehouseSysNo,
                LastUpdateBy = operatorSysNo,
                LastUpdateDate = DateTime.Now,
                Remarks=reason
            };
            inEntity.ItemList = new List<WhStockInItem>();
            //入库明细
            foreach (RcReturnItem item in rma.RMAItems)
            {
           
                inEntity.ItemList.Add(new WhStockInItem
                {
                    CreatedBy = operatorSysNo,
                    CreatedDate = DateTime.Now,
                    ProductName = item.ProductName,
                    ProductSysNo = item.ProductSysNo,
                    StockInQuantity = item.RmaQuantity,
                    RealStockInQuantity = autoComplete ? item.RmaQuantity : 0,
                    LastUpdateBy = operatorSysNo,
                    LastUpdateDate = DateTime.Now,
                    SourceItemSysNo = item.SysNo //记录入库单明细来源单号（退换货明细编号)
                });
            }
            var stockInSysNo = InStockBo.Instance.CreateStockIn(inEntity); //保存入库单数据
            #endregion

            #region 修改出库单状态为部分退货或全部退货

            WhWarehouseBo.Instance.UpdateStockOutStatus(stockOut.SysNo,
                                                        stockOutProductCount == returnProductCount
                                                            ? WarehouseStatus.出库单状态.全部退货
                                                            : WarehouseStatus.出库单状态.部分退货, operatorSysNo);
            //SoOrderBo.Instance.WriteSoTransactionLog(order.TransactionSysNo,string.Format("结算单退货创建RMA,出库单编号:{0}",stockOut.SysNo),SyUserBo.Instance.GetUserName(operatorSysNo));

            #endregion

            //修改出库单明细退货数量
            WhWarehouseBo.Instance.UpdateStockOutItemReturnQuantity(stockOut.SysNo, returnNumber);

            #region  创建付款单

            // 吴文强确认  退换金额小于等于0也创建退款单
            //if (returnAmount >= 0)
            //{
            //获取收款单
            var receiptVoucher = IFnReceiptVoucherDao.Instance.GetEntity((int)FinanceStatus.付款来源类型.销售单,
                                                                         stockOut.OrderSysNO);
            var paymentVoucher = new FnPaymentVoucher
                {
                    TransactionSysNo = transactionSysNo,
                    PayableAmount = returnCalculate.RefundTotalAmount,
                    CreatedBy = operatorSysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = operatorSysNo,
                    LastUpdateDate = DateTime.Now,
                    CustomerSysNo = stockOut.CustomerSysNo,
                    PayerSysNo = operatorSysNo,
                    Remarks = "已付款拒收或部分签收.创建退货,退款",
                    Source = (int)FinanceStatus.付款来源类型.退换货单,
                    SourceSysNo = rmaSysNo,
                    Status = (int)FinanceStatus.付款单状态.待付款
                };
            //自动完成则创建付款单明细
            if (autoComplete)
            {
                paymentVoucher.Remarks += " 自动完成";
                paymentVoucher.Status = (int)FinanceStatus.付款单状态.已付款;
                paymentVoucher.PayDate = DateTime.Now;
                paymentVoucher.PaidAmount = returnCalculate.RefundTotalAmount;

                //付款单明细
                var paymentVoucherItem = new FnPaymentVoucherItem
                    {
                        Amount = returnCalculate.RefundTotalAmount,

                        CreatedBy = operatorSysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = operatorSysNo,
                        LastUpdateDate = DateTime.Now,
                        PayDate = DateTime.Now,
                        PaymentType = (int)FinanceStatus.付款单付款方式.现金,
                        Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单明细状态.已付款,
                        VoucherNo = "",
                        //added by huangwei,for added columns logic 
                        PaymentToType =
                            sourceType.Equals(RmaStatus.退换货申请单来源.拒收)
                                ? FinanceStatus.付款方类型.财务中心.GetHashCode()
                                : FinanceStatus.付款方类型.仓库.GetHashCode(),
                        PaymentToSysNo = sourceType.Equals(RmaStatus.退换货申请单来源.拒收) ? 0 : stockOut.WarehouseSysNo    //此处应为仓库编号，姑将stockInSysNo改为stockOut.WarehouseSysNo 余勇 2017-07-22
                     };
                int paymentVoucherSysNo= FinanceBo.Instance.CreatePaymentVoucher(paymentVoucher, paymentVoucherItem);
                //退货扣减积分 refer to Hyt.BLL.LevelPoint.PointBo.Instance.RMADecreasePoint(pay.CustomerSysNo, rma.OrderSysNo, (int)rma.RefundCoin, rma.RefundPoint, pay.TransactionSysNo);
                PointBo.Instance.RMADecreasePoint(order.CustomerSysNo, order.SysNo, (int)retrun.RefundCoin,
                                                  retrun.RefundPoint, order.TransactionSysNo);
                #region 操作EAS
                //排除升舱订单
                if (order.OrderSource != Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱.GetHashCode())
                {
                    CBFnPaymentVoucher newpay = new CBFnPaymentVoucher()
                    {
                        PayableAmount = paymentVoucher.PayableAmount,
                        PaidAmount = paymentVoucher.PaidAmount,
                        CreatedBy = paymentVoucher.CreatedBy,
                        CreatedDate = paymentVoucher.CreatedDate,
                        CustomerSysNo = paymentVoucher.CustomerSysNo,
                        Source = paymentVoucher.Source,
                        SourceSysNo = paymentVoucher.SourceSysNo,
                        RefundBank = paymentVoucher.RefundBank,
                        RefundAccountName = paymentVoucher.RefundAccountName,
                        RefundAccount = paymentVoucher.RefundAccount,
                        TransactionSysNo = paymentVoucher.TransactionSysNo,
                        Status = paymentVoucher.Status,
                        SysNo = paymentVoucherSysNo,
                        VoucherItems = new List<FnPaymentVoucherItem>()
                    };
                    newpay.VoucherItems.Add(paymentVoucherItem);
                    Hyt.BLL.Finance.FinanceBo.Instance.PartialSignWriteEas(newpay, operatorSysNo);
                }

                #endregion
            }
            else
            {
                FinanceBo.Instance.CreatePaymentVoucher(paymentVoucher, new List<FnPaymentVoucherItem> { });
            }

            //}

            #endregion
        }

        #endregion

        #endregion

        /// <summary>
        /// 获取结算单关联收款单是否存在至少一笔已确认,若确认,不允许作废结算单
        /// 需在订单相关页面操作,取消确认
        /// </summary>
        /// <param name="lstSettlementSysNo">结算单编号集合</param>
        /// <returns>result true:已确认;false:待确认</returns>
        /// <remarks>2013-8-8 黄伟 创建</remarks>
        [Obsolete]
        public Result IsFnReciptVoucherConfirmed(List<int> lstSettlementSysNo)
        {
            return ILgSettlementDao.Instance.IsFnReciptVoucherConfirmed(lstSettlementSysNo);
        }

        /// <summary>
        /// 作废结算单明细
        /// </summary>
        /// <param name="settlementItemSysNo">结算单明细系统编号.</param>
        /// <param name="operatorSysNo">操作人.</param>
        /// <param name="userIp">访问者ip</param>
        /// <returns>void</returns>
        /// <exception cref="System.Exception">
        /// </exception>
        /// <remarks>
        /// 2013/8/20 何方 创建
        /// </remarks>
        public void CancelSettlementItem(int settlementItemSysNo, int operatorSysNo, string userIp)
        {
            //获取结算单明细
            var settlementItem = GetSettlementItem(settlementItemSysNo);
            if (settlementItem == null)
            {
                throw new Exception(string.Format("结算单明细:{0}不存在", settlementItemSysNo));
            }
            //获取配送单明细
            var deliveryItem = LgDeliveryBo.Instance.GetDeliveryItem(settlementItem.DeliverySysNo,
                                                                     settlementItem.StockOutSysNo,
                                                                     LogisticsStatus.配送单据类型.出库单);
            if (deliveryItem == null)
            {
                throw new Exception(string.Format("配送单:{0}不存在出库单:{1}", settlementItem.DeliverySysNo,
                                                  settlementItem.StockOutSysNo));
            }

            var stockOut = WhWarehouseBo.Instance.Get(settlementItem.StockOutSysNo);

            if (stockOut == null)
            {
                throw new Exception(string.Format("出库单:{0}不存在", settlementItem.StockOutSysNo));
            }
            var status = (LogisticsStatus.配送单明细状态)deliveryItem.Status;

            //  if (deliveryItem.IsCOD == 1)
            if (stockOut.Receivable > 0) //货到付款,未付款
            {
                switch (status)
                {
                    case LogisticsStatus.配送单明细状态.拒收:
                        //未付款拒收
                        NonPaymentTurnDownReceive(stockOut, operatorSysNo);
                        break;
                    case LogisticsStatus.配送单明细状态.已签收:

                        if (IsPartialSign(settlementItem.StockOutSysNo))
                        {
                            //货到付款(未付款)第一单部分签收(部分退货)
                            VoidCodFirstPartialSign(stockOut, operatorSysNo);
                        }
                        else
                        {
                            //到付(未付款)首单全部签收
                            VoidCodFirstAllSign(stockOut, operatorSysNo);
                        }

                        break;
                    case LogisticsStatus.配送单明细状态.未送达:
                        //未送达 ->走公共流程
                        break;
                    default:
                        throw new Exception(string.Format("配送单明细:{0}的状态为{1},不能作废结算单明细{2}", deliveryItem.SysNo, status,
                                                          settlementItemSysNo));
                }
            }
            else if (stockOut.Receivable == 0) //已付款
            {
                switch (status)
                {
                    case LogisticsStatus.配送单明细状态.拒收:
                        //已付款拒收
                        PaymentTurnDownReceive(stockOut, operatorSysNo);
                        break;
                    case LogisticsStatus.配送单明细状态.已签收:
                        //已付款全部签收 ->走公共流程
                        break;
                    case LogisticsStatus.配送单明细状态.未送达:
                        //未送达 ->走公共流程
                        break;
                    default:
                        throw new Exception(string.Format("配送单明细:{0}的状态为{1},不能作废结算单明细{2}", deliveryItem.SysNo, status,
                                                          settlementItemSysNo));
                }
            }
            else
            {
                throw new Exception(string.Format("出库单:{0}应收款金额:{1}小于0", settlementItem.StockOutSysNo,
                                                  stockOut.Receivable));
            }
            //公共流程
            VoidPublicWorkflow(settlementItem, operatorSysNo);

            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "作废结算单",
                                     LogStatus.系统日志目标类型.结算单, settlementItemSysNo, null, userIp,
                                     operatorSysNo);
        }

        /// <summary>
        /// 获取结算单明细
        /// </summary>
        /// <param name="sysNo">结算单明细系统编号</param>
        /// <returns>
        /// 结算单明细
        /// </returns>
        /// <remarks>
        /// 2013-08-20 何方 创建
        /// </remarks>
        public LgSettlementItem GetSettlementItem(int sysNo)
        {
            return ILgSettlementItemDao.Instance.GetLgSettlementItem(sysNo);
        }

        #region 结算单明细作废相关 黄伟

        /// <summary>
        /// 作废到付首单全部签收
        /// </summary>
        /// <param name="stockOut">出库单实体</param>
        /// <param name="operatorSysNo">操作人员系统编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-8-21 黄伟 创建</remarks>
        private void VoidCodFirstAllSign(WhStockOut stockOut, int operatorSysNo)
        {
            #region 收款单已确认:抛出异常信息

            var checkFnResult = CheckFnVoucherConfirmed(stockOut.OrderSysNO);
            if (checkFnResult.Status)
                throw new Exception(checkFnResult.Message);

            #endregion

            #region 存在rma单:抛出异常信息

            var checkRmaResult = CheckRmaExist(stockOut.OrderSysNO);
            if (checkRmaResult.Status)
                throw new Exception(checkRmaResult.Message);

            #endregion

            #region 如果剩余出库单已经配送,则不能作废结算单明细,抛出异常信息

            var soOrder = IOutStockDao.Instance.GetSoOrder(stockOut.SysNo);
            if (soOrder == null)
                throw new Exception("出库单 " + stockOut.SysNo + " 找不到所在的订单!");

            var lstStockOut = WhWarehouseBo.Instance.GetWhStockOutListByOrderID(soOrder.SysNo).ToList();
            if (lstStockOut.Any(p => p.SysNo != stockOut.SysNo && p.Status == (int)WarehouseStatus.出库单状态.配送中))
                throw new Exception("订单中其它出库单已经配送,不能作废!");

            #endregion

            #region 收款单已收金额为0

            var fnVoucher = checkFnResult.Data;
            //no stamp available,just set and update
            fnVoucher.ReceivedAmount = 0;
            fnVoucher.LastUpdateBy = operatorSysNo;
            fnVoucher.LastUpdateDate = DateTime.Now;
            FnReceiptVoucherBo.Instance.Update(fnVoucher);

            #endregion

            #region 收款单明细-作废
            //如果订单是到付的,则作废所有收款单明细,否则,不做处理
            var paymentType = PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo).PaymentType;
            if (paymentType == BasicStatus.支付方式类型.到付.GetHashCode())
            {
                //作废所有收到款明细
                IFnReceiptVoucherItemDao.Instance.GetListByReceiptNo(fnVoucher.SysNo).ForEach(p => FnReceiptVoucherBo.Instance.InvalidReceiptVoucherItem(p.SysNo));
            }
            #endregion

            #region 修改订单状态为未付款

            SoOrderBo.Instance.UpdatePayStatus(stockOut.OrderSysNO, OrderStatus.销售单支付状态.未支付);

            #endregion

            #region 其他出库单到付金额修改为订单金额

            lstStockOut.Where(p => p.SysNo != stockOut.SysNo).ToList().ForEach(i =>
                {
                    i.Receivable = soOrder.OrderAmount;
                    i.LastUpdateBy = operatorSysNo;
                    i.LastUpdateDate = DateTime.Now;
                    IOutStockDao.Instance.Update(i);
                });

            #endregion
        }

        /// <summary>
        /// 作废:货到付款第一单部分签收(部分退货)
        /// </summary>
        /// <param name="stockOut">出库单</param>
        /// <param name="operatorSysNo">操作人员系统编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-8-21 黄伟 创建</remarks>
        private void VoidCodFirstPartialSign(WhStockOut stockOut, int operatorSysNo)
        {
            #region 收款单已确认:抛出异常信息,返回

            var checkFnResult = CheckFnVoucherConfirmed(stockOut.OrderSysNO);
            if (checkFnResult.Status)
                throw new Exception(checkFnResult.Message);

            #endregion

            #region 存在rma单:抛出异常信息,返回

            var checkRmaResult = CheckRmaExist(stockOut.OrderSysNO);
            if (checkRmaResult.Status)
                throw new Exception(checkRmaResult.Message);

            #endregion

            #region 如果剩余出库单已经配送,则不能作废结算单明细,抛出异常信息

            var soOrder = IOutStockDao.Instance.GetSoOrder(stockOut.SysNo);
            if (soOrder == null)
                throw new Exception("出库单 " + stockOut.SysNo + " 找不到所在的订单!");

            var lstStockOut = WhWarehouseBo.Instance.GetWhStockOutListByOrderID(soOrder.SysNo).ToList();
            if (lstStockOut.Any(p => p.SysNo != stockOut.SysNo && p.Status == (int)WarehouseStatus.出库单状态.配送中))
                throw new Exception("订单中其它出库单已经配送,不能作废!");

            #endregion

            #region 收款单-待收款 收款单明细-作废

            var fnVoucher = checkFnResult.Data;
            //no stamp available,just set and update
            fnVoucher.Status = (int)FinanceStatus.收款单状态.待确认;
            fnVoucher.LastUpdateBy = operatorSysNo;
            fnVoucher.LastUpdateDate = DateTime.Now;
            FnReceiptVoucherBo.Instance.Update(fnVoucher);

            #region 收款单明细-作废
            //如果订单是到付的,则作废所有收款单明细,否则,不做处理
            var paymentType = PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo).PaymentType;
            if (paymentType == BasicStatus.支付方式类型.到付.GetHashCode())
            {
                //作废所有收到款明细
                IFnReceiptVoucherItemDao.Instance.GetListByReceiptNo(fnVoucher.SysNo).ForEach(p => FnReceiptVoucherBo.Instance.InvalidReceiptVoucherItem(p.SysNo));
            }
            #endregion

            #endregion

            #region RMA单-作废

            var lstRmaItem = checkRmaResult.Data;
            var rma = RmaBo.Instance.GetRcReturnEntity(lstRmaItem.First().RMAID);
            rma.Status = (int)RmaStatus.退换货状态.作废;
            IRcReturnDao.Instance.Update(rma);

            #endregion

            #region 入库单-作废

            var stockIn = IInStockDao.Instance.GetWhStockInByVoucherSource(
                (int)WarehouseStatus.入库单据类型.RMA单, rma.SysNo);
            stockIn.Status = (int)WarehouseStatus.入库单状态.作废;
            IInStockDao.Instance.UpdateWhStockIn(stockIn);

            //IInStockDao.Instance.GetWhStockInItemList(stockIn.SysNo).ToList().ForEach(p =>
            //    {

            //        p.LastUpdateBy = operatorSysNo;
            //        p.LastUpdateDate = DateTime.Now;
            //        IInStockDao.Instance.UpdateWhStockInItem(p);
            //    });

            #endregion

            #region 出库单:退货数量设置为0

            IOutStockDao.Instance.GetWhStockOutItemList(stockOut.SysNo).ToList().ForEach(p =>
                {
                    p.ReturnQuantity = 0;
                    p.LastUpdateBy = operatorSysNo;
                    p.LastUpdateDate = DateTime.Now;

                    IOutStockDao.Instance.UpdateOutItem(p);
                });

            #endregion

            #region RMA对应付款单:作废 付款单明细-作废

            #region 付款单-作废

            //作废付款单及付款单子表
            var fnPaymentVoucher =
                IFnPaymentVoucherDao.Instance.GetEntityByVoucherSource(
                    (int)FinanceStatus.付款来源类型.退换货单, rma.SysNo);
            fnPaymentVoucher.Status = (int)FinanceStatus.付款单状态.作废;
            IFnPaymentVoucherDao.Instance.UpdateVoucher(fnPaymentVoucher);

            #endregion

            #region 付款单明细-作废

            var lstFnPaymentVoucherItem =
                IFnPaymentVoucherDao.Instance.GetVoucherItems(fnPaymentVoucher.SysNo);

            lstFnPaymentVoucherItem.ForEach(p =>
                {
                    p.Status = FinanceStatus.付款单明细状态.作废.GetHashCode();
                    IFnPaymentVoucherDao.Instance.UpdateVoucherItem(p);
                });

            #endregion

            #endregion
        }

        /// <summary>
        /// 根据订单编号检查关联收款单状态
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>封装实体FnReceiptVoucher</returns>
        /// <remarks>2013-8-20 黄伟 创建</remarks>
        public Result<FnReceiptVoucher> CheckFnVoucherConfirmed(int orderSysNo)
        {
            var fnReceiptVoucher = IFnReceiptVoucherDao.Instance.GetEntity((int)FinanceStatus.收款来源类型.销售单, orderSysNo);
            return fnReceiptVoucher.Status == (int)FinanceStatus.收款单状态.已确认
                       ? new Result<FnReceiptVoucher> { Status = true, Message = "收款单已确认,无法作废!", Data = fnReceiptVoucher }
                       : new Result<FnReceiptVoucher> { Status = false, Data = fnReceiptVoucher };
        }

        /// <summary>
        /// 根据订单编号检查是否存在RMA单
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns> Result:data=List CBRmaReturnItem</returns>
        /// <remarks>2013-8-21 黄伟 创建</remarks>
        public Result<List<CBRmaReturnItem>> CheckRmaExist(int orderSysNo)
        {
            var lstRma = IRcReturnItemDao.Instance.GetListByOrder(orderSysNo);
            if (lstRma == null || !lstRma.Any())
                return new Result<List<CBRmaReturnItem>> { Status = false };
            var rma = IRcReturnDao.Instance.GetEntity(lstRma.First().RMAID);
            //非作废状态
            return (rma.Source != RmaStatus.退换货申请单来源.拒收.GetHashCode() &&
                    rma.Source != RmaStatus.退换货申请单来源.部分签收.GetHashCode() && rma.Status != (int)RmaStatus.退换货状态.作废)
                       ? new Result<List<CBRmaReturnItem>>
                           {
                               Status = true,
                               Message = "用户已经退换货,不能作废,请联系客户作废退换货申请单以后再作废",
                               Data = lstRma
                           }
                       : new Result<List<CBRmaReturnItem>> { Status = false, Data = lstRma };
        }

        #endregion

        #region 结算单明细作废-公共流程 黄伟

        /// <summary>
        /// 结算单明细作废-公共流程
        /// </summary>
        /// <param name="item">结算单明细</param>
        /// <param name="operatorSysNo">操作人员系统编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-8-21 黄伟 创建</remarks>
        private void VoidPublicWorkflow(LgSettlementItem item, int? operatorSysNo)
        {
            if (item == null)
                throw new ArgumentNullException("item", @"结算单明细不可为空");
            if (operatorSysNo == null)
                throw new ArgumentNullException("operatorSysNo", @"操作人员不可为空");

            #region 出库单:待配送(重新计算订单状态)

            WhWarehouseBo.Instance.UpdateStockOutStatus(item.StockOutSysNo, WarehouseStatus.出库单状态.待配送,
                                                        (int)operatorSysNo);

            #endregion

            #region 结算单明细:作废

            item.LastUpdateBy = (int)operatorSysNo;
            item.LastUpdateDate = DateTime.Now;
            item.Status = (int)LogisticsStatus.结算单明细状态.作废;
            Update(item);

            #endregion

            #region 配送单明细:作废

            ILgDeliveryDao.Instance.UpdateDeliveryItemStatus(item.DeliverySysNo,
                                                             LogisticsStatus.配送单据类型.出库单,
                                                             item.StockOutSysNo,
                                                             LogisticsStatus.配送单明细状态.作废);

            #endregion

            #region 重新计算:结算单应收款额

            //更改结算单相应的到付或者预付金额
            var settlement = ILgSettlementDao.Instance.GetLgSettlement(item.SettlementSysNo).First();
            var stockOut = IOutStockDao.Instance.GetEntity(item.StockOutSysNo);
            if (stockOut.IsCOD == 1)
                settlement.CODAmount -= stockOut.Receivable;
            else
                settlement.PaidAmount -= stockOut.Receivable;

            #endregion

            //写入订单日志
            var theStockOut = WhWarehouseBo.Instance.Get(item.StockOutSysNo);
            var soOrder = SoOrderBo.Instance.GetEntity(theStockOut.OrderSysNO);
            if (soOrder != null)
            {
                SoOrderBo.Instance.WriteSoTransactionLog(soOrder.TransactionSysNo,
                                                         string.Format("结算单明细作废:{0}", item.SysNo),
                                                         SyUserBo.Instance.GetUserName((int)operatorSysNo));
            }
        }

        #endregion

        /// <summary>
        /// 出库单未付款拒收
        /// </summary>
        /// <param name="whStockOut">出库单实体</param>
        /// <param name="operationSysNo">操作人员系统编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-08-20 沈强 创建</remarks>
        private void NonPaymentTurnDownReceive(WhStockOut whStockOut, int operationSysNo)
        {
            #region 查询出库单所在的订单

            SoOrder soOrder = DataAccess.Warehouse.IOutStockDao.Instance.GetSoOrder(whStockOut.SysNo);
            if (soOrder == null)
            {
                throw new Exception("出库单 " + whStockOut.SysNo + " 找不到所在的订单！");
            }

            #endregion

            #region 获取订单中的出库单集合

            var whStockOutCollection =
                Warehouse.WhWarehouseBo.Instance.GetWhStockOutListByOrderID(soOrder.SysNo).ToList();

            if (whStockOutCollection == null || whStockOutCollection.Count == 0)
            {
                throw new Exception("没有找到出库单 " + whStockOut.SysNo + " 所在订单中的其他出库单！");
            }

            #endregion

            var whStockOuts = whStockOutCollection.Where(r => r.Status == (int)WarehouseStatus.出库单状态.拒收).ToList();

            if (!whStockOuts.Any())
            {
                throw new Exception("不是所有出库单状态是拒收");
            }

            foreach (var item in whStockOuts)
            {
                //获取出库单所在的入库单
                /*
                Pager<WhStockIn> whStockIns =
                    DataAccess.Warehouse.IInStockDao.Instance.GetWhStockInList(new ParaInStockFilter()
                    {
                        SourceType = (int)WarehouseStatus.入库单据类型.出库单,
                        SourceSysNo = item.SysNo
                    }, int.MaxValue);

                //入库单数量大于一时，抛出异常
                if (whStockIns.Rows.Count > 1)
                {
                    throw new Exception("出库单 " + item.SysNo + " 所在的入库单有多个！");
                }
                */
                var stockin = IInStockDao.Instance.GetStockInBySource((int)WarehouseStatus.入库单据类型.出库单,
                                                                      item.SysNo);

                //入库单数量等于一时，更新出库单状态，以及入库单和入库单明细状态
                if (stockin != null)
                {
                    if (stockin.Status == (int)WarehouseStatus.入库单状态.已入库)
                    {
                        throw new Exception("入库单 " + stockin.SysNo + " 处于已入库状态,不能作废!");
                    }
                    item.Status = (int)WarehouseStatus.出库单状态.待配送;
                    item.LastUpdateBy = operationSysNo;
                    item.LastUpdateDate = DateTime.Now;

                    #region 作废入库单

                    stockin.ItemList = new List<WhStockInItem>();
                    stockin.Status = (int)WarehouseStatus.入库单状态.作废;
                    stockin.LastUpdateBy = operationSysNo;
                    stockin.LastUpdateDate = DateTime.Now;

                    DataAccess.Warehouse.IInStockDao.Instance.UpdateWhStockIn(stockin);

                    #endregion
                }

                    //入库单数量等于零时，仅更新出库单状态
                else
                {
                    item.Status = (int)WarehouseStatus.出库单状态.待出库;
                    item.LastUpdateBy = operationSysNo;
                    item.LastUpdateDate = DateTime.Now;
                }
                //更新出库单状态
                DataAccess.Warehouse.IOutStockDao.Instance.Update(item);
            }

            #region 更新收款单状态

            //更新收款单状态
            FnReceiptVoucher fnReceiptVoucher =
                DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity((int)FinanceStatus.收款来源类型.销售单, soOrder.SysNo);
            fnReceiptVoucher.Status = (int)FinanceStatus.收款单状态.待确认;
            fnReceiptVoucher.LastUpdateBy = operationSysNo;
            fnReceiptVoucher.LastUpdateDate = DateTime.Now;
            DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(fnReceiptVoucher);

            #endregion
        }

        /// <summary>
        /// 出库单已付款拒收
        /// </summary>
        /// <param name="whStockOut">出库单实体</param>
        /// <param name="operationSysNo">操作人员系统编号</param>
        /// <returns>void</returns>
        /// <remarks>2013-08-20 沈强 创建</remarks>
        private void PaymentTurnDownReceive(WhStockOut whStockOut, int operationSysNo)
        {
            #region 获取出库单中的有效出库单明细

            var whStockOutItems =
                DataAccess.Warehouse.IOutStockDao.Instance.GetWhStockOutItemList(whStockOut.SysNo).ToList();
            var items = whStockOutItems.Where(w => w.Status == (int)WarehouseStatus.出库单明细状态.有效).ToList();
            if (items.Count == 0)
            {
                throw new Exception("出库单 " + whStockOut.SysNo + " 中没有有效的出库单明细！");
            }

            #endregion

            #region 获取出库单所在的有效退货单

            //根据出库单明细系统编号，获取退货单
            var rcReturns = RmaBo.Instance.Get(items[0].SysNo).ToList();
            var returnitem = rcReturns.Where(r => r.Status == (int)RmaStatus.退换货状态.待入库).ToList();
            //var rma = RmaBo.Instance.GetRcReturnEntity(returnitem.First().RMAID);
            //有效的退货单只能有一个
            if (returnitem.Count == 0 || returnitem.Count > 1)
            {
                throw new Exception("出库单 " + whStockOut.SysNo + " 关联入库单已入库,不允许作废！");
            }

            #endregion

            #region 获取出库单所在的付款单

            //根据退货单系统编号，获取付款单
            var fnReceiptVoucher = IFnReceiptVoucherDao.Instance.GetEntity((int)FinanceStatus.收款来源类型.退换货单,
                                                                           returnitem[0].SysNo);

            if (fnReceiptVoucher != null && (fnReceiptVoucher.Status == (int)FinanceStatus.付款单状态.已付款 ||
                                             fnReceiptVoucher.Status == (int)FinanceStatus.付款单状态.部分付款))
            {
                throw new Exception("出库单 " + whStockOut.SysNo + " 所在的付款单已付款不能作废！");
            }

            #endregion

            #region 作废退货单

            returnitem[0].Status = (int)RmaStatus.退换货状态.作废;
            returnitem[0].LastUpdateBy = operationSysNo;
            returnitem[0].LastUpdateDate = DateTime.Now;
            DataAccess.RMA.IRcReturnDao.Instance.Update(returnitem[0]);

            #endregion

            #region 作废入库单

            //获取出库单所在的入库单
            //Pager<WhStockIn> whStockIns =
            //    DataAccess.Warehouse.IInStockDao.Instance.GetWhStockInList(new ParaInStockFilter()
            //    {
            //        SourceType = (int)WarehouseStatus.入库单据类型.RMA单,
            //        SourceSysNo = returnitem.First().SysNo
            //    }, int.MaxValue);

            //入库单数量大于一时，抛出异常
            //if (whStockIns.Rows.Count > 1)
            //{
            //    throw new Exception("出库单 " + whStockOut.SysNo + " 所在的入库单有多个！");
            //}
            //if (whStockIns.Rows.Count == 1)
            //{
            //    whStockIns.Rows[0].ItemList = new List<WhStockInItem>();
            //    whStockIns.Rows[0].Status = (int)WarehouseStatus.入库单状态.作废;
            //    whStockIns.Rows[0].LastUpdateBy = operationSysNo;
            //    whStockIns.Rows[0].LastUpdateDate = DateTime.Now;

            //    DataAccess.Warehouse.IInStockDao.Instance.UpdateWhStockIn(whStockIns.Rows[0]);
            //}

            var stockin = IInStockDao.Instance.GetStockInBySource((int)WarehouseStatus.入库单据类型.RMA单,
                                                                  returnitem.First().SysNo);
            if (stockin != null)
            {
                stockin.Status = (int)WarehouseStatus.入库单状态.作废;
                stockin.LastUpdateBy = operationSysNo;
                stockin.LastUpdateDate = DateTime.Now;
                IInStockDao.Instance.UpdateWhStockIn(stockin);
            }

            #endregion

            #region 有效出库单明细商品数量设置为零

            items.ForEach(p =>
                {
                    p.ReturnQuantity = 0;
                    p.LastUpdateBy = operationSysNo;
                    p.LastUpdateDate = DateTime.Now;
                    DataAccess.Warehouse.IOutStockDao.Instance.UpdateOutItem(p);
                });

            #endregion

            #region 作废付款单

            if (fnReceiptVoucher == null) return;

            fnReceiptVoucher.Status = (int)FinanceStatus.付款单状态.作废;
            fnReceiptVoucher.LastUpdateBy = operationSysNo;
            fnReceiptVoucher.LastUpdateDate = DateTime.Now;
            IFnReceiptVoucherDao.Instance.Update(fnReceiptVoucher);

            // 付款单明细-作废
            var lstFnPaymentVoucherItem =
                IFnPaymentVoucherDao.Instance.GetVoucherItems(fnReceiptVoucher.SysNo);
            lstFnPaymentVoucherItem.ForEach(p =>
                {
                    p.Status = (int)FinanceStatus.付款单明细状态.作废;
                    p.LastUpdateBy = operationSysNo;
                    p.LastUpdateDate = DateTime.Now;
                    IFnPaymentVoucherDao.Instance.UpdateVoucherItem(p);
                });

            #endregion
        }

        /// <summary>
        /// 是否部分签收
        /// </summary>
        /// <param name="stockOutSysNo">出库单号.</param>
        /// <returns>true或false</returns>
        /// <remarks>
        /// 2013/8/21 何方 创建
        /// </remarks>
        public bool IsPartialSign(int stockOutSysNo)
        {
            var stockOut = WhWarehouseBo.Instance.Get(stockOutSysNo);
            if (stockOut.Status != (int)WarehouseStatus.出库单状态.部分退货)
            {
                return false;
            }
            var item = stockOut.Items.FirstOrDefault(t => t.ReturnQuantity > 0);
            if (item == null)
            {
                throw new Exception(string.Format("出库单:{0}状态是{1},所有明细中却没有退货数量", stockOutSysNo,
                                                  WarehouseStatus.出库单状态.部分退货));
            }
            var rma =
                RmaBo.Instance.Get(item.SysNo, RmaStatus.退换货申请单来源.部分签收)
                     .Where(x => x.Status == (int)RmaStatus.退换货状态.已完成);

            if (!rma.Any())
            {
                throw new Exception(string.Format("出库单:{0}状态是{1},却没有退货单", stockOutSysNo,
                                                  WarehouseStatus.出库单状态.部分退货));
            }

            if (rma.Count() > 2)
            {
                throw new Exception(string.Format("出库单:{0}状态是{1},存在{2}多个退换货单", stockOutSysNo,
                                                  WarehouseStatus.出库单状态.部分退货, rma.Count()));
            }
            return true;
        }

        /// <summary>
        /// 更新结算单明细
        /// </summary>
        /// <param name="item">结算单明细</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2013-8-21 黄伟 创建</remarks>
        public int Update(LgSettlementItem item)
        {
            return ILgSettlementItemDao.Instance.Update(item);
        }

        /// <summary>
        /// (部分签收)出库单修改数量后返回应退金额
        /// </summary>
        /// <param name="model">退货订单相关信息(实体对应CBRMAOrderInfo)</param>
        /// <returns>应退金额</returns>
        /// <remarks>2013-8-21 黄伟 创建</remarks>
        public decimal GetProductPrice(CBRMAOrderInfo model)
        {
            var rma = RmaBo.Instance.CalculateRmaAmount(model.OrderSysNo,
                                                        model.lstRMAOrderItemInfo.ToDictionary(p => p.OrderItemSysNo,
                                                                                               p => p.Qty));
            //return rma.OrginAmount;
            return rma.RefundTotalAmount;
        }


        /// <summary>
        /// 获取结算单关联收款单编号
        /// </summary>
        /// <param name="stockOutSysNo"></param>
        /// <returns>收款单编号</returns>
        /// <remarks>2013-11-14 黄伟 创建</remarks>
        public int GetReceiptSysNoByStockSysNo(int stockOutSysNo)
        {
            var soOrder = IOutStockDao.Instance.GetSoOrder(stockOutSysNo);
            var fnReceiptVoucher = IFnReceiptVoucherDao.Instance.GetEntity((int)FinanceStatus.收款来源类型.销售单, soOrder.SysNo);
            return fnReceiptVoucher.SysNo;
        }

        /// <summary>
        /// 根据结算单编号获取配送类型名(第三方快递)
        /// </summary>
        /// <param name="settlementSysNo">结算单编号</param>
        /// <returns>配送类型名</returns>
        /// <remarks>2013-11-14 黄伟 创建</remarks>
        public string GetDelTypeName(int settlementSysNo)
        {
            var delSysNo = GetLgSettlementWithItems(settlementSysNo).Items.First().DeliverySysNo;
            var del = LgDeliveryBo.Instance.GetDelivery(delSysNo);
            var delType = DeliveryTypeBo.Instance.GetDeliveryType(del.DeliveryTypeSysNo);
            return delType == null ? null : delType.DeliveryTypeName;
        }

        /// <summary>
        /// query FnReceiptTitleAssociation by warehouse and paytype
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="payTypeSysNo">付款方式系统编号</param>
        /// <returns>list of FnReceiptTitleAssociation</returns>
        /// <remarks>2013-10-10 黄伟 创建</remarks>
        public List<SelectListItem> GetEasCodeByWhAndPayType(int whSysNo, int payTypeSysNo)
        {
            var lstEas = ReceiptManagementBo.Instance.QueryEasByWhAndPayType(whSysNo, payTypeSysNo);
            if (lstEas == null || !lstEas.Any())
            {
                return new List<SelectListItem>
                    {
                        new SelectListItem
                            {
                                Text = @"无对应科目",
                                Value = "",
                                Selected = true
                            }
                    };
            }
            return lstEas.Select(p => new SelectListItem
                {
                    Text = p.EasReceiptName,
                    Value = p.EasReceiptCode,
                    Selected = p.IsDefault == 1
                }).ToList();
        }


        /// <summary>
        /// 写门店注册会员明细统计数据
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="amount">金额</param>
        /// <returns></returns>
        /// <remarks>2014-1-17 杨文彬　创建</remarks>
        /// <remarks>2014-1-17 黄志勇　修改</remarks>
        public void WriteShopNewCustomerDetail(int customerSysNo, decimal amount)
        {
            try
            {
                // 注册月份与结算月份相同
                var model = ReportBO.Instance.SelectShopNewCustomerDetail(customerSysNo);
                if (model == null) return;
                if (model.RegisterDate.ToString("yyyy-MM") == DateTime.Now.ToString("yyyy-MM"))
                {
                    ReportBO.Instance.UpdateShopNewCustomerDetail(model, amount);
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.订单, customerSysNo);
            }

        }

        ///// <summary>
        ///// 结算单结算后，更新新增会员明细表
        ///// </summary>
        ///// <param name="lgSettlementSysNo">结算单明细主表系统编号</param>
        ///// <returns></returns>
        ///// <remarks>
        ///// 2014-01-16 黄志勇 创建
        ///// 筛选条件：
        ///// 门店下单
        ///// 仓库类型为门店
        ///// 出库单不为作废
        ///// 结算单明细已结算
        ///// 结算单已结算
        ///// 客户注册来源为门店
        ///// 客户注册来源编号与门店编号相同
        ///// 注册月份与结算审核月份相同
        ///// </remarks>
        //public void UpdateShopNewCustomerDetail(int lgSettlementSysNo)
        //{
        //    try
        //    {
        //        //客户编号
        //        var customerSysNo = 0;
        //        //出库单明细
        //        var stockOuts = new Dictionary<int, WhStockOut>();
        //        //结算单及明细
        //        var settlementEx = GetLgSettlementWithItems(lgSettlementSysNo);
        //        if (settlementEx != null && settlementEx.Status == (int)LogisticsStatus.结算单状态.已结算 && settlementEx.Items != null && settlementEx.Items.Count >0)
        //        {
        //            //结算单明细
        //            var settlementItems = settlementEx.Items.Where(i => i.Status == (int) LogisticsStatus.结算单明细状态.已结算);
        //            if (settlementItems.Any())
        //            {
        //                foreach (var settlementItem in settlementItems)
        //                {
        //                    //出库单及明细
        //                    var outStock = WhWarehouseBo.Instance.Get(settlementItem.StockOutSysNo);
        //                    if (outStock != null && outStock.Items != null && outStock.Items.Count > 0)
        //                    {
        //                        var warehouse = WhWarehouseBo.Instance.GetWarehouseEntity(outStock.WarehouseSysNo);  //仓库
        //                        if (outStock.Status != (int)WarehouseStatus.出库单状态.作废 && warehouse != null &&
        //                            warehouse.WarehouseType == (int)WarehouseStatus.仓库类型.门店)
        //                        {
        //                            var order = SoOrderBo.Instance.GetEntity(outStock.OrderSysNO);
        //                            if (order != null && order.OrderSource == (int) OrderStatus.销售单来源.门店下单)
        //                            {
        //                                if (customerSysNo == 0) customerSysNo = order.CustomerSysNo;
        //                                var customer = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);
        //                                if (customer != null && customer.RegisterSource == (int)CustomerStatus.注册来源.门店 && customer.RegisterSourceSysNo == warehouse.SysNo.ToString() && customer.RegisterDate.ToString("yyyy-MM") == settlementEx.AuditDate.ToString("yyyy-MM"))
        //                                {
        //                                    //需累加出库单金额的出库单
        //                                    if (!stockOuts.ContainsKey(outStock.SysNo)) stockOuts.Add(outStock.SysNo, outStock);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (stockOuts.Count > 0)
        //        {
        //            var amount = stockOuts.Sum(i => i.Value.StockOutAmount);
        //            ReportBO.Instance.UpdateShopNewCustomerDetail(customerSysNo, amount);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.结算单, lgSettlementSysNo);
        //    }
        //}
    }
}