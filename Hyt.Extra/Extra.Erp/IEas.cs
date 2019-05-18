﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.Erp.Model;
using Extra.Erp.Model.Borrowing;
using Extra.Erp.Model.Sale;
using Hyt.Model.Transfer;

namespace Extra.Erp
{
    /// <summary>
    /// 金蝶eas接口
    /// </summary>
    /// <remarks>2013-9-1 杨浩 创建</remarks>
    public interface IEasProvider
    {
        #region 库存查询

        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="storageOrgNumber">组织结构编码</param>
        /// <param name="erpCode">商品编码</param>
        /// <param name="erpWarehouseSysNo">Eas仓库编号</param>
        /// <param name="warehouseSysNo">Hyt仓库编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        Result<IList<Inventory>> GetInventory(string storageOrgNumber, string[] erpCode, string erpWarehouseSysNo, int warehouseSysNo);

        #endregion

        #region 借货还货

        /// <summary>
        /// 借货
        /// </summary>
        /// <param name="model">借货明细</param>
        /// <param name="description">单据摘要</param>
        /// <param name="flowIdentify">Hyt借货系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        Result Borrow(List<BorrowInfo> model, string description,string flowIdentify);

        /// <summary>
        /// 还货
        /// </summary>
        /// <param name="model">还货明细</param>
        /// <param name="description">单据摘要</param>
        /// <param name="flowIdentify">Hyt借货系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        Result Return(List<BorrowInfo> model, string description, string flowIdentify);

       // /// <summary>
       // /// 借货(异步)
       // /// </summary>
       // /// <param name="model">借货明细</param>
       // /// <param name="description">单据摘要</param>
       // /// <remarks>2013-9-27 杨浩 添加</remarks>
       //void BorrowAsync(List<BorrowInfo> model, string description);

       // /// <summary>
       // /// 还货(异步)
       // /// </summary>
       // /// <param name="model">还货明细</param>
       // /// <param name="description">单据摘要</param>
       // /// <returns></returns>
       // /// <remarks>2013-9-27 杨浩 添加</remarks>
       //void ReturnAsync(List<BorrowInfo> model, string description);

        #endregion

        #region 销售出库、退货

        /// <summary>
        /// 销售出库
        /// </summary>
        /// <param name="model">出库明细</param>
        /// <param name="customer">送货客户</param>
        /// <param name="description">Eas单据摘要(Hyt订单号)</param>
        /// <param name="flowIdentify">Hyt订单事物编号</param>
        /// <returns>bool</returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        Result SaleOutStock(List<SaleInfo> model, string customer, string description, string flowIdentify);

        ///// <summary>
        ///// 销售出库(异步)
        ///// </summary>
        ///// <param name="model">出库明细</param>
        ///// <param name="customer">送货客户</param>
        ///// <param name="description">单据摘要(升舱订单号|商城订单号)</param>
        ///// <returns>bool</returns>
        ///// <remarks>2013-9-27 杨浩 添加</remarks>
        //void SaleOutStockAsync(List<SaleInfo> model, string customer, string description);

        /// <summary>
        /// 销售退货
        /// </summary>
        /// <param name="model">退货明细</param>
        /// <param name="customer">送货客户</param>
        /// <param name="description">单据摘要(请备注订单号、会员名称)</param>
        /// <param name="flowIdentify">Hyt订单事物编号</param>
        /// <returns>bool</returns>
        /// <remarks>2013-9-27 杨浩 添加</remarks>
        Result SaleInStock(List<SaleInfo> model, string customer, string description, string flowIdentify);

        ///// <summary>
        ///// 销售退货(异步)
        ///// </summary>
        ///// <param name="model">退货明细</param>
        ///// <param name="customer">送货客户</param>
        ///// <param name="description">单据摘要</param>
        ///// <returns>bool</returns>
        ///// <remarks>2013-9-27 杨浩 添加</remarks>
        //void SaleInStockAsync(List<SaleInfo> model, string customer, string description);

        #endregion

        #region 收款导入

        /// <summary>
        /// 导入收款单据
        /// </summary>
        /// <param name="model">收款明细</param>
        /// <param name="receivingType">收款单类型(5:商品收款单;10:服务收款单)</param>
        /// <param name="customer">往来账客户编号</param>
        /// <param name="isUpgrades">是否为升舱</param>
        /// <param name="description">单据摘要(请备注订单号(升舱：备注淘宝订单号))</param>
        /// <param name="flowIdentify">Hyt订单事物编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-25 杨浩 创建</remarks>
        Result Receiving(List<Model.Receiving.ReceivingInfo> model, 收款单类型 receivingType, string customer, bool isUpgrades, string description, string flowIdentify);

        ///// <summary>
        ///// 导入收款单据(异步)
        ///// </summary>
        ///// <param name="model">收款明细</param>
        ///// <param name="receivingType">收款单类型(5:商品收款单;10:服务收款单)</param>
        ///// <param name="customer">往来账客户编号</param>
        ///// <param name="isUpgrades">是否为升舱</param>
        ///// <param name="description">单据摘要(请备注订单号(升舱：备注淘宝订单号))</param>
        ///// <returns></returns>
        ///// <remarks>2013-9-25 杨浩 创建</remarks>
        //void ReceivingAsync(List<Model.Receiving.ReceivingInfo> model, 收款单类型 receivingType, string customer, bool isUpgrades, string description);

        ///// <summary>
        ///// 导入付款单据(异步)
        ///// </summary>
        ///// <param name="model">付款明细</param>
        ///// <param name="customer">往来账客户编号</param>
        ///// <param name="isUpgrades">是否为升舱</param>
        ///// <param name="description">单据摘要(请备注订单号(升舱：备注淘宝订单号))</param>
        ///// <returns></returns>
        ///// <remarks>2013-9-25 杨浩 创建</remarks>
        //void PaymentAsync(List<Model.Receiving.ReceivingInfo> model, string customer, bool isUpgrades, string description);

        /// <summary>
        /// 导入付款单据(同步)
        /// </summary>
        /// <param name="model">付款明细</param>
        /// <param name="customer">往来账客户编号</param>
        /// <param name="isUpgrades">是否为升舱</param>
        /// <param name="description">单据摘要(请备注订单号(升舱：备注淘宝订单号))</param>
        /// <param name="flowIdentify">Hyt订单事物编号</param>
        /// <returns></returns>
        /// <remarks>2013-9-25 杨浩 创建</remarks> 
        Result Payment(List<Model.Receiving.ReceivingInfo> model, string customer, bool isUpgrades, string description, string flowIdentify);

        #endregion

        #region 基础
        /// <summary>
        /// 重新同步数据
        /// </summary>
        /// <param name="sysNo">Eas同步系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-22 杨浩 创建</remarks>
        Result Resynchronization(int sysNo, bool isSave=false);

        /// <summary>
        /// 获取Eas同步数据
        /// </summary>
        /// <param name="model">Eas</param>
        /// <returns></returns>
        /// <remarks>2013-10-22 杨浩 添加</remarks>
        Result<string> GetData(CBEasSyncLog model);
        #endregion
    }
}