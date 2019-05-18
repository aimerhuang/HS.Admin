using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Extra.Erp.Model;
using Extra.Erp.Model.Borrowing;
using Extra.Erp.Model.Receiving;
using Extra.Erp.Model.Sale;
using Hyt.DataAccess.Basic;
using Hyt.DataAccess.Finance;
using Hyt.DataAccess.Warehouse;
using Hyt.Infrastructure.Memory;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Util.Serialization;
using Extra.Erp.Model.BaseData;
using Extra.Erp.DataContract;

namespace Extra.Erp.Kis
{
    /// <summary>
    /// 实现Kis接口
    /// </summary>
    /// <remarks>2013-9-27 杨浩 添加</remarks>
    internal sealed class KisProvider : IKisProvider
    {

        private static readonly object _lockThis = new object();
        private static readonly List<int> _synclist = new List<int>();

        #region 库存查询

        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="storageOrgNumber">组织结构编码</param>
        /// <param name="erpCode">商品编码</param>
        /// <param name="erpWarehouseSysNo">Eas仓库编号</param>
        /// <param name="warehouseSysNo"></param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result<IList<Inventory>> GetInventory(string storageOrgNumber, string[] erpCode, string erpWarehouseSysNo, int warehouseSysNo)
        {
            var data = KisCore.WebInventory(storageOrgNumber,erpCode, erpWarehouseSysNo,warehouseSysNo);
            return data;
        }

        #endregion

        #region 借货还货

        /// <summary>
        /// 借货
        /// </summary>
        /// <param name="model">借货明细</param>
        /// <param name="description">单据摘要</param>
        /// <param name="flowIdentify"></param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result Borrow(List<BorrowInfo> model, string description, string flowIdentify)
        {
            var result = KisCore.OtherIssueBillFacade(model, 借货状态.借货, description, flowIdentify);
            return result;
        }

        /// <summary>
        /// 借货(异步)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="description">单据摘要</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public void BorrowAsync(List<BorrowInfo> model, string description)
        {
            //var callback = new AsyncCallback(CallBack);
            //var dn = new Action(() => Borrow(model, description));
            //dn.BeginInvoke(callback, dn);
            var t = new Thread(() => Borrow(model, description,""));
            t.Start();
        }

        /// <summary>
        /// 还货
        /// </summary>
        /// <param name="model">还货明细</param>
        /// <param name="description">单据摘要</param>
        /// <param name="flowIdentify"></param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result Return(List<BorrowInfo> model, string description, string flowIdentify)
        {
            var result = KisCore.OtherIssueBillFacade(model, 借货状态.还货, description, flowIdentify);
            return result;
        }

        /// <summary>
        /// 还货(异步)
        /// </summary>
        /// <param name="model">还货明细</param>
        /// <param name="description">单据摘要</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public void ReturnAsync(List<BorrowInfo> model, string description)
        {
            //var callback = new AsyncCallback(CallBack);
            //var action = new Action(() => Return(model, description));
            //action.BeginInvoke(callback, action);
            var t = new Thread(() => Return(model, description,""));
            t.Start();
        }

        #endregion

        #region 导入调拨单据

        /// <summary>
        /// 调拨单导入
        /// </summary>
        /// <param name="model">调拨明细</param>
        /// <param name="description">Kis单据摘要(调拨出库单号_调拨入库单号)</param>
        /// <param name="flowIdentify">调拨单系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-1-04 杨浩 创建</remarks>
        public Result TransferStock(List<TransferStockInfo> model, string description, string flowIdentify)
        {
            var datajson = new TransferStockInfoWraper
            {
                Model = model,
                Type = 调拨状态.入库,
                Description = description
            }.ToJson();

            var easModel = new Hyt.Model.EasSyncLog
            {
                Data = datajson,
                FlowIdentify = flowIdentify
            };
            var status = KisCore.TransferStockIssueBillFacade(easModel);

            //同步兴业嘉仓的数据
            foreach (var item in model)
            {
                if (model.First().WarehouseSysNo == 59 || model.First().WarehouseSysNo == 61 || model.First().WarehouseSysNo == 4 || model.First().WarehouseSysNo == 30 || model.First().WarehouseSysNo == 41)
                {
                    XingYe.XingYeProviderFactory.CreateProvider().TransferStock(model, description, flowIdentify);
                }
            }

            //同步利嘉
            //LiJia.LiJiaProviderFactory.CreateProvider().TransferStock(model, description, flowIdentify);
            return status;
        }


        #endregion


        #region 采购入库、退货

        /// <summary>
        /// 采购退货
        /// </summary>
        /// <param name="model">出库明细</param>
        /// <param name="description">Kis单据摘要(出库单号)</param>
        /// <param name="flowIdentify">采购订单系统编号</param>
        /// <param name="purchaseOrderSysNo">采购订单系统编号</param>
        /// <returns>bool</returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result PurchaseOutStock(List<PurchaseInfo> model, string description, string flowIdentify, string purchaseOrderSysNo)
        {
            var datajson = new PurchaseInfoWraper
            {
                Model = model,
                Type = 采购状态.退货,
                Description = description,
                PurchaseOrderSysNo = purchaseOrderSysNo

            }.ToJson();

            var easModel = new Hyt.Model.EasSyncLog
            {
                Data = datajson,
                FlowIdentify = flowIdentify

            };
            var status = KisCore.PurchaseIssueBillFacade(easModel);


            //同步兴业嘉仓数据
            if (model.First().WarehouseSysNo == 59 || model.First().WarehouseSysNo == 61 || model.First().WarehouseSysNo == 4 || model.First().WarehouseSysNo == 30 || model.First().WarehouseSysNo == 41)
	        {
                XingYe.XingYeProviderFactory.CreateProvider().PurchaseOutStock(model, description, flowIdentify, purchaseOrderSysNo);
	        } 
            //foreach (var item in model)
            //{
            //    if (item.WarehouseSysNo == 59)
            //    {
                    
            //    }
            //}
            //同步利嘉
            //LiJia.LiJiaProviderFactory.CreateProvider().PurchaseOutStock(model,description,flowIdentify,purchaseOrderSysNo);
            return status;
        }



        /// <summary>
        /// 采购入库
        /// </summary>
        /// <param name="model">入库明细</param>
        /// <param name="description">Kis单据摘要(入库单号)</param>
        /// <param name="flowIdentify">采购订单系统编号</param>
        /// <returns>bool</returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result PurchaseInStock(List<PurchaseInfo> model, string description, string flowIdentify)
        {
            var datajson = new PurchaseInfoWraper
            {
                Model = model,
                Type = 采购状态.入库,
                Description = description

            }.ToJson();

            var easModel = new Hyt.Model.EasSyncLog
            {
                Data = datajson,
                FlowIdentify = flowIdentify

            };
            var status = KisCore.PurchaseIssueBillFacade(easModel);

            //同步兴业嘉仓数据
            if (model.First().WarehouseSysNo == 59 || model.First().WarehouseSysNo == 61 || model.First().WarehouseSysNo == 4 || model.First().WarehouseSysNo == 30 || model.First().WarehouseSysNo == 41)
            {
                XingYe.XingYeProviderFactory.CreateProvider().PurchaseInStock(model, description, flowIdentify);
            } 
            //同步利嘉
            //LiJia.LiJiaProviderFactory.CreateProvider().PurchaseInStock(model,description,flowIdentify);
            return status;
        }



        #endregion

        #region 销售出库、退货、查询

        /// <summary>
        /// 销售出库
        /// </summary>
        /// <param name="model">出库明细</param>
        /// <param name="customer">分销商erp编号</param>
        /// <param name="description">单据摘要(商城订单号/升舱订单号)</param>
        /// <param name="flowIdentify">出库单事务编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result SaleOutStock(List<SaleInfo> model, string customer, string description , string flowIdentify)
        {

            var datajson = new SaleInfoWraper
            {
                Model = model,
                Type = 出库状态.出库,
                Customer = customer,
                Description = description

            }.ToJson();

            var easModel = new Hyt.Model.EasSyncLog
            {
                Data = datajson,
                FlowIdentify = flowIdentify

            };
            var status = KisCore.SaleIssueBillFacade(easModel);

            //同步兴业嘉仓数据
            if (model.First().WarehouseSysNo == 59 || model.First().WarehouseSysNo == 61 || model.First().WarehouseSysNo == 4 || model.First().WarehouseSysNo == 30 || model.First().WarehouseSysNo == 41)
            {
                XingYe.XingYeProviderFactory.CreateProvider().SaleOutStock(model, customer, description, flowIdentify);
            } 
            //同步利嘉
            LiJia.LiJiaProviderFactory.CreateProvider().SaleOutStock(model, customer, description, flowIdentify);

            return status;
        }

        /// <summary>
        /// 销售出库(异步)
        /// </summary>
        /// <param name="model">出库明细</param>
        /// <param name="customer">送货客户</param>
        /// <param name="description">单据摘要(订单号)</param>
        /// <returns></returns> 
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public void SaleOutStockAsync(List<SaleInfo> model, string customer, string description)
        {
            // action = new Action(() => SaleOutStock(model, customer, description));
            //action.BeginInvoke(CallBack, action);
            var t=new Thread(() =>SaleOutStock(model, customer, description,""));
            t.Start();
        }


        /// <summary>
        /// 销售退货
        /// </summary>
        /// <param name="model">退货明细</param>
        /// <param name="customer">分销商erp编号</param>
        /// <param name="description">单据摘要(商城订单号/升舱订单号)</param>
        /// <param name="flowIdentify"></param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result SaleInStock(List<SaleInfo> model, string customer, string description, string flowIdentify)
        {
            var datajson = new SaleInfoWraper
            {
                Model = model,
                Type = 出库状态.退货,
                Customer = customer,
                Description = description

            }.ToJson();

            var easModel = new Hyt.Model.EasSyncLog
            {
                Data = datajson,
                FlowIdentify=flowIdentify
                
            };
            var status = KisCore.SaleIssueBillFacade(easModel);

            //同步兴业嘉仓数据
            if (model.First().WarehouseSysNo == 59 || model.First().WarehouseSysNo == 61 || model.First().WarehouseSysNo == 4 || model.First().WarehouseSysNo == 30 || model.First().WarehouseSysNo == 41)
            {
                XingYe.XingYeProviderFactory.CreateProvider().SaleInStock(model, customer, description, flowIdentify);
            } 
            //同步利嘉
            LiJia.LiJiaProviderFactory.CreateProvider().SaleInStock(model,customer, description, flowIdentify);
            return status;
        }

        /// <summary>
        /// 销售退货(异步)
        /// </summary>
        /// <param name="model">退货明细</param>
        /// <param name="customer">送货客户</param>
        /// <param name="description">单据摘要</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public void SaleInStockAsync(List<SaleInfo> model, string customer, string description)
        {
            //var action = new Action(() => SaleInStock(model, customer, description));
            //action.BeginInvoke(CallBack, action);
            var t = new Thread(() => SaleInStock(model, customer, description,""));
            t.Start();
        }
        /// <summary>
        /// 销售单查询
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns></returns>
        /// <remarks>2016-12-12 杨浩 创建</remarks>
        public Result<List<SaleSearchResponse>> SaleSearch(SaleSearchRequest request)
        {
           return KisCore.SaleSearch(request);          
        }

        #endregion

        #region 收款导入

        /// <summary>
        /// 导入收款单据
        /// </summary>
        /// <param name="model">收款明细</param>
        /// <param name="receivingType">收款单类型(5:商品收款单;10:服务收款单)</param>
        /// <param name="customer">往来账客户编号</param>
        /// <param name="isUpgrades">是否为升舱</param>
        /// <param name="description">单据摘要(请备注订单号、会员名称、收款仓库(在线支付订单：备注商城、升舱：备注淘宝))</param>
        /// <param name="flowIdentify"></param>
        /// <returns></returns>
        /// <remarks>2013-9-25 杨浩 创建</remarks>
        public Result Receiving(List<ReceivingInfo> model, 收款单类型 receivingType, string customer, bool isUpgrades, string description, string flowIdentify)
        {
            var status = KisCore.ReceivingBillFacade(0,model, receivingType, customer, description, flowIdentify);
            return status;
        }

        /// <summary>
        /// 导入付款单据
        /// </summary>
        /// <param name="model">付款明细</param>
        /// <param name="customer">往来账客户编号</param>
        /// <param name="isUpgrades">是否为升舱</param>
        /// <param name="description">单据摘要(请备注订单号(升舱：备注淘宝订单号))</param>
        /// <param name="flowIdentify"></param>
        /// <returns></returns>
        /// <remarks>2013-9-25 杨浩 创建</remarks>
        public Result Payment(List<Model.Receiving.ReceivingInfo> model, string customer, bool isUpgrades,
                                  string description,string flowIdentify)
        {
           var status = KisCore.ReceivingBillFacade(0,model, 收款单类型.退销售回款, customer, description, flowIdentify);
           return status;
        }

        #endregion

        #region 基础

        /// <summary>
        /// 重新同步数据
        /// </summary>
        /// <param name="sysNo">Eas同步系统编号</param>
        /// <param name="isSave">销售出库单是否传保存状态</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result Resynchronization(int sysNo, bool isSave = false)
        {
           //string key = "Hyt.EasLog." + sysNo;
            lock (_lockThis)
            {
                //判断是否有锁
                if (_synclist.Contains(sysNo))
                    return new Result { Status = false, Message = "正在同步,请稍后再试..." };
                //加锁
                _synclist.Add(sysNo);
            }
            var model = Hyt.DataAccess.Sys.IEasDao.Instance.GetEntity(sysNo);

            if (model.Status == (int)同步状态.成功)
            {
                return new Result
                {
                    Message = "此信息已同步成功！",
                    Status = false
                };
            }
            //收款单
            bool isFull = false;
            int warehouseSysNo = 0;
            //
            var iType = (接口类型)(model.InterfaceType);
            var result = new Result<string> { };
            var watch = new Stopwatch();
            watch.Start();
            switch (iType)
            {
                case 接口类型.配送员借货还货:
                    var borrow = model.Data.ToObject<BorrowInfoWraper>();
                    result = KisCore.OtherIssueBillFacade(borrow.Model, borrow.Type, borrow.Description, model.FlowIdentify,model.DataMd5, true);
                    break;
                case 接口类型.销售出库退货:
                    //var sale = model.Data.ToObject<SaleInfoWraper>();
                    result = KisCore.SaleIssueBillFacade(model,isSave, true);
                    break;
                case 接口类型.采购入库退货:
                    result = KisCore.PurchaseIssueBillFacade(model, isSave, true);
                    break;
                case 接口类型.调拨单据导入:
                    result = KisCore.TransferStockIssueBillFacade(model, isSave, true);
                    break;
                case 接口类型.收款单据导入:
                    #region 收款单据导入
                    var receiving = model.Data.ToObject<ReceivingInfoWraper>();
                    if (model.Message.StartsWith(EasConstant.Information))
                    {
                        isFull = true;
                        foreach (var item  in receiving.Model)
                        {
                            if (item.WarehouseSysNo == 0)
                            {
                                int orderSysNo =0;
                                int.TryParse(item.OrderSysNo,out orderSysNo);
                                var wh = IOutStockDao.Instance.GetWhStockOutListByOrderID(orderSysNo, true);
                                if (wh != null && wh.Count > 0)
                                {
                                    item.Remark = "";
                                    item.WarehouseSysNo = wh.FirstOrDefault().WarehouseSysNo;
                                    warehouseSysNo = item.WarehouseSysNo;
                                    var warehouse = IWhWarehouseDao.Instance.GetWarehouse(warehouseSysNo); //地区仓库
                                    if (warehouse != null) item.WarehouseNumber = warehouse.ErpCode;
                                }
                            }
                            var oraganization = IOrganizationDao.Instance.GetOrganization(item.WarehouseSysNo);
                            if (oraganization != null)
                            {
                                item.OrganizationCode = oraganization.Code;
                            }
                            if (item.PayeeAccount != EasConstant.PayeeAccount && string.IsNullOrEmpty(item.PayeeAccount))
                            {
                                var pa =
                                    IFnReceiptTitleAssociationDao.Instance.GetList(item.WarehouseSysNo, PaymentType.现金)
                                        .OrderByDescending(m => m.IsDefault)
                                        .FirstOrDefault();
                                if (pa != null)
                                  item.PayeeAccount = pa.EasReceiptCode;
                            }
                        }
                    }
                    result = KisCore.ReceivingBillFacade(model.SysNo,receiving.Model, receiving.ReceivingType, receiving.Customer, receiving.Description, model.FlowIdentify,model.DataMd5, true);
                    break;
                    #endregion
            }
            watch.Stop();
            if (result.Status || result.StatusCode == "105")// || result.StatusCode == "9999" || result.Message.Contains("当前单据已经同步成功"))
            {
                model.Status = (int) 同步状态.成功;
                model.VoucherNo = result.Data;
                model.Message = "同步成功";
                //消息特殊处理：销售出库单,外部系统id:1e65059916eb765b4b7d17496589c904,已经同步过,对应EAS单据ID为:XSCK2014052905193353,请不要重复同步...
                if (result.StatusCode == "105" && !string.IsNullOrEmpty(result.Message))
                {
                    var reg = new Regex(@"XSCK(\d*)");
                    var m = reg.Match(result.Message);
                        model.VoucherNo = m.Value;
                }
                //消息特殊处理：当前单据已经同步成功，kis单据号为:XSCK2014081907917302
                else if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("单据重复"))
                {
                    Regex reg = new Regex(@"XSCK(\d*)");
                    var m = reg.Match(result.Message);
                    model.VoucherNo = m.Value;
                }
            }
            else
            {
                model.Status = (int) 同步状态.失败;
                model.Message = result.Message;
            }
            model.SyncNumber = model.SyncNumber + 1;
            model.StatusCode = result.StatusCode;
            model.ElapsedTime = (int)watch.ElapsedMilliseconds;
            model.LastsyncTime = DateTime.Now;
            model.LastupdateDate = DateTime.Now;
            model.LastupdateBy = Hyt.Model.SystemPredefined.User.SystemUser;
            //收款单资料不全，重新读取并更新
            if (isFull && warehouseSysNo!=0)
            {
                model.WarehouseSysNo = warehouseSysNo;
            }
            Hyt.DataAccess.Sys.IEasDao.Instance.Update(model);

            _synclist.Remove(sysNo);

            return result;
        }

        /// <summary>
        /// 获取Kis同步数据
        /// </summary>
        /// <param name="model">Kis</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result<string> GetData(CBEasSyncLog model)
        {
            var easModel = new Hyt.Model.EasSyncLog
            {
                SysNo=model.SysNo,
                Data=model.Data,
                DataMd5=model.DataMd5,
                FlowIdentify = model.FlowIdentify,
            };
            var iType = (接口类型)(model.InterfaceType);
            var result = new Result<string> { };
            switch (iType)
            {
                case 接口类型.配送员借货还货:
                    var borrow = model.Data.ToObject<BorrowInfoWraper>();
                    result = KisCore.OtherIssueBillFacade(borrow.Model, borrow.Type, borrow.Description,"", model.DataMd5,true, true, true);
                    break;
                case 接口类型.销售出库退货:
                    //var sale = model.Data.ToObject<SaleInfoWraper>();
                    result = KisCore.SaleIssueBillFacade(easModel, false, true, true, true);
                    break;
                case 接口类型.采购入库退货:
                    result = KisCore.PurchaseIssueBillFacade(easModel, false, true, true, true);
                    break;
                case 接口类型.调拨单据导入:
                    result = KisCore.TransferStockIssueBillFacade(easModel, false, true, true, true);
                    break;
                case 接口类型.收款单据导入:
                    var receiving = model.Data.ToObject<ReceivingInfoWraper>();
                    result = KisCore.ReceivingBillFacade(0,receiving.Model, receiving.ReceivingType, receiving.Customer, receiving.Description, "",model.DataMd5, true, true, true);
                    break;
            }
            return result;
        }
        /// <summary>
        /// 获取Kis全部计量单位
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-11-22 杨浩 添加</remarks>
        public Result<IList<KisUnit>> GetAllUnit()
        {
            var result = KisCore.GetAllUnit();
            return result;
        }
        #endregion
    }
}
