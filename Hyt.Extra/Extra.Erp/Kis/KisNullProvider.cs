using System;
using System.Collections.Generic;
using Extra.Erp.Model;
using Extra.Erp.Model.Borrowing;
using Extra.Erp.Model.Receiving;
using Extra.Erp.Model.Sale;
using Hyt.Model.Transfer;
using Hyt.Util.Serialization;
using Extra.Erp.Model.BaseData;
using Extra.Erp.DataContract;

namespace Extra.Erp.Kis
{
    /// <summary>
    /// Kis接口空策略
    /// </summary>
    /// <remarks>2013-9-27 杨浩 添加</remarks>
    public class KisNullProvider : IKisProvider
    {
        #region 库存查询

        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="storageOrgNumber"></param>
        /// <param name="erpCode">商品编码</param>
        /// <param name="erpWarehouseSysNo">仓库编号</param>
        /// <param name="warehouseSysNo"></param>
        /// <returns>库存</returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result<IList<Inventory>> GetInventory(string storageOrgNumber, string[] erpCode, string erpWarehouseSysNo, int warehouseSysNo)
        {
            //return new Result<IList<Inventory>> { Data = null, Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
            var data = KisCore.WebInventory(storageOrgNumber, erpCode, erpWarehouseSysNo, warehouseSysNo);
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
        /// <returns>状态</returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Model.Result Borrow(List<Model.Borrowing.BorrowInfo> model, string description, string flowIdentify)
        {
            //var result = KisCore.Instance.OtherIssueBillFacade(model, 借货状态.借货, description,flowIdentify,null, false, false);
            //return result;
            return new Result { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
        }
        

        /// <summary>
        /// 还货
        /// </summary>
        /// <param name="model">还货明细</param>
        /// <param name="description">单据摘要</param>
        /// <param name="flowIdentify"></param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Model.Result Return(List<Model.Borrowing.BorrowInfo> model, string description, string flowIdentify)
        {
            //var result = KisCore.Instance.OtherIssueBillFacade(model, 借货状态.还货, description,flowIdentify,null, false, false);
            //return result;
            return new Result { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
        }

        #endregion

        #region 销售出库、退货

        /// <summary>
        /// 销售出库
        /// </summary>
        /// <param name="model">出库明细</param>
        /// <param name="customer">分销商erp编号</param>
        /// <param name="description">单据摘要(商城订单号/升舱订单号)</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Model.Result SaleOutStock(List<Model.Sale.SaleInfo> model, string customer, string description, string flowIdentify)
        {
            //var status = KisCore.Instance.SaleIssueBillFacade(model, 出库状态.出库, customer, description,flowIdentify,null, false, false);
            //return status;
            return new Result { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
        }
        

        /// <summary>
        /// 销售退货
        /// </summary>
        /// <param name="model">退货明细</param>
        /// <param name="customer">分销商erp编号</param>
        /// <param name="description">单据摘要(商城订单号/升舱订单号)</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Model.Result SaleInStock(List<Model.Sale.SaleInfo> model, string customer, string description, string flowIdentify)
        {
            //var status = KisCore.Instance.SaleIssueBillFacade(model, 出库状态.退货, customer, description,flowIdentify,null, false, false);
            //return status;
            return new Result { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
        }
        /// <summary>
        /// 销售单查询
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns></returns>
        /// <remarks>2016-12-12 杨浩 创建</remarks>
        public Result<List<SaleSearchResponse>> SaleSearch(SaleSearchRequest request)
        {
            return new Result<List<SaleSearchResponse>> { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };       
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
        public Result TransferStock(List<TransferStockInfo> model, string description, string flowIdentify)
        {
            return new Result { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
        }
        #endregion 

        #region 采购入库、退货
        /// <summary>
        /// 采购退货
        /// </summary>
        /// <param name="model">出库明细</param>
        /// <param name="description">Kis单据摘要(出库单号)</param>
        /// <param name="flowIdentify">采购订单系统编号</param>
        /// <param name="purchaseOrderBillNo">采购订单单据编号</param>
        /// <returns>bool</returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result PurchaseOutStock(List<PurchaseInfo> model, string description, string flowIdentify, string purchaseOrderBillNo)
        {
            return new Result { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
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
            return new Result { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
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
        public Model.Result Receiving(List<Model.Receiving.ReceivingInfo> model, 收款单类型 receivingType, string customer, bool isUpgrades, string description, string flowIdentify)
        {
            //var status = KisCore.Instance.ReceivingBillFacade(model, receivingType, customer, description, flowIdentify,null, false, false);
            //return status;
            return new Result { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
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
                                  string description, string flowIdentify)
        {
          //return  Receiving(model, 收款单类型.退销售回款, customer, isUpgrades, description, flowIdentify);
            return new Result { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
        }

        #endregion

        #region 基础

        /// <summary>
        /// 重新同步数据
        /// </summary>
        /// <param name="sysNo">Eas同步系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result Resynchronization(int sysNo,bool isSave=false)
        {
            return new Result() { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
        }

        /// <summary>
        /// 获取Eas同步数据
        /// </summary>
        /// <param name="model">Eas</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        public Result<string> GetData(CBEasSyncLog model)
        {
            return new EasProvider().GetData(model);
        }
        /// <summary>
        /// 获取Kis全部计量单位
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-11-22 杨浩 添加</remarks>
        public Result<IList<KisUnit>> GetAllUnit()
        {
            return new Result<IList<KisUnit>>() { Message = Model.EasConstant.EAS_MESSAGE_CLOSE, Status = false };
        }
        #endregion
    }
}
