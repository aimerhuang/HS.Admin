using Hyt.DataAccess.Base;
using Hyt.Model.B2CApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Cargo
{
    /// <summary>
    /// 调拨但操作类
    /// </summary>
    /// <remarks>2015-9-15 杨云奕 添加</remarks>
    public abstract class ICargoOrderBao : DaoBase<ICargoOrderBao>
    {
        #region 调拨申请单

        /// <summary>
        /// 新增调拨申请单
        /// </summary>
        /// <param name="applyOrder">申请单实体</param>
        public abstract void InnerDBApplyOrder(DBApplyOrder applyOrder);
        /// <summary>
        /// 新增/修改调拨申请单子表信息
        /// </summary>
        /// <param name="applyOrderItemList">调拨申请单子表</param>
        public abstract void InnerOrUpdataDBApplyOrderItemList(List<DBApplyOrderItem> applyOrderItemList);
        /// <summary>
        /// 更新调拨申请单
        /// </summary>
        /// <param name="applyOrder">申请单实体</param>
        public abstract void UpdateDBApplyOrder(DBApplyOrder applyOrder);


        /// <summary> 
        /// 更新审核状态
        /// </summary>
        /// <param name="sysNo">调拨单号</param>
        /// <param name="OnLineStatus">状态名称</param>
        /// <param name="Status">状态值</param>
        /// <param name="Content">附加内容</param>
        /// <param name="AuditUserNo">审核人员编号</param>
        /// <param name="AuditTime">审核时间</param>
        public abstract int UpdateDBApplyOrderByStatus(int sysNo, string OnLineStatus, int Status, string Content, int AuditUserNo, DateTime AuditTime);

        /// <summary>
        /// 获取调拨单
        /// </summary>
        /// <param name="SysNo">调拨单号</param>
        /// <returns>调拨单实体</returns>
        public abstract DBApplyOrder GetDBApplyOrderData(int SysNo);
        /// <summary>
        /// 获取调拨单子表
        /// </summary>
        /// <param name="PSysNo">调拨单号</param>
        /// <returns>返回子表数据集合</returns>
        public abstract List<DBApplyOrderItem> GetDBApplyOrderItemListData(int PSysNo);

        #endregion

        #region 出货申请单
        /// <summary>
        /// 获取出货单
        /// </summary>
        /// <param name="sysNo">出货单号</param>
        /// <returns>出货单实体</returns>
        public abstract DBShipmentOrder GetDBShipmentOrder(int sysNo);
        /// <summary>
        /// 返回出货单列表集合
        /// </summary>
        /// <param name="sysNo">调拨单号</param>
        /// <returns>返回出货单集合列表</returns>
        public abstract List<DBShipmentOrder> GetDBShipmentOrderList(int psysNo);

        /// <summary>
        /// 出货申请单明细表
        /// </summary>
        /// <param name="ShipmentSysNo">出货单明细列表</param>
        /// <returns>出货明细列表集合</returns>
        public abstract List<DBShipmentOrderItem> GetDBShipmentOrderItemList(int ShipmentSysNo);

        /// <summary>
        /// 返表单出货单同子表回数据集合
        /// </summary>
        /// <param name="psysNo">申请单编号</param>
        /// <returns>申请单数据集合</returns>
        public abstract List<DBShipmentOrder> GetDBShipmentOrderListByHavaItem(int psysNo);

        /// <summary>
        /// 更新或修改出货
        /// </summary>
        /// <param name="shioOrder">新增和修改出货单</param>
        public abstract void InnerOrUpdataDBShipmentOrder(DBShipmentOrder shipOrder);

        /// <summary>
        /// 更新或修改出货单明细
        /// </summary>
        /// <param name="ItemList">出货单明细集合</param>
        public abstract void InnerOrUpdateDBShipmentOrderItem(List<DBShipmentOrderItem> ItemList);

        /// <summary>
        /// 修改出货单状态
        /// </summary>
        /// <param name="sysNo">出货单号</param>
        /// <param name="statusValue">状态值</param>
        /// <param name="statusTxt">状态内容</param>
        /// <param name="updataBy">更新人</param>
        /// <param name="updataDate">更新时间</param>
        /// <param name="Content">状态更新说明</param>
        public abstract int UpdataDBShipmentOrderStatus(int sysNo, int statusValue, int statusTxt, string updataBy, DateTime updataDate, string Content);

        #endregion

        #region 执行调货后，门店的盈亏情况
        /// <summary>
        /// 获取调拨情况后发生的门点成本变动表单
        /// </summary>
        /// <param name="sysNo">变动表单编号</param>
        /// <returns>变动记录表单</returns>
        public abstract DBCastOrder GetCastOrder(int sysNo);
        /// <summary>
        /// 获取调货后门店成本变化记录表单
        /// </summary>
        /// <param name="castOrderNo">成本编号</param>
        /// <returns>门店成本记录列表</returns>
        public abstract List<DBCastOrderItem> GetCastOrderItemList(int castOrderNo);

        /// <summary>
        /// 更新财务状态
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <param name="statusValue">状态值</param>
        /// <param name="statusTxt">状态内容</param>
        /// <param name="updataBy">更新人员</param>
        /// <param name="updataDate">更新时间</param>
        /// <param name="Content">附加内容</param>
        public abstract int UpdataDBCastOrderStatus(int sysNo, int statusTxt, string updataBy,
            DateTime updataDate, string Content);

        /// <summary>
        /// 更新或添加梦到成本变动表单
        /// </summary>
        /// <param name="castOrder">变动表单实体</param>
        public abstract void InnerOrUpDataCastOrder(DBCastOrder castOrder);

        /// <summary>
        /// 添加或者更新成本变动表单明细
        /// </summary>
        /// <param name="castItemList">变动表单明细</param>
        public abstract void InnerOrUpdataCastOrderItem(List<DBCastOrderItem> castItemList);

        #endregion

        #region 操作日志

        /// <summary>
        /// 获取操作日志记录、调拨申请，发货申请
        /// </summary>
        /// <param name="PSysNo">申请单编号</param>
        /// <param name="sysType">类型</param>
        /// <returns>返回日志列表集合</returns>
        public abstract List<DBOrderLog> GetDBOrderLogList(int PSysNo, string sysType);

        /// <summary>
        /// 插入日志信息
        /// </summary>
        /// <param name="orderLog">日志实体</param>
        public abstract void InnerOrderLogData(DBOrderLog orderLog);


        #endregion


        #region 成本计算公式
        /// <summary>
        /// 计算公式集合
        /// </summary>
        /// <returns></returns>
        public abstract List<DBCostCalculate> GetCalculateList();
        /// <summary>
        /// 添加和修改公式集合
        /// </summary>
        /// <param name="calculate"></param>
        public abstract void InnerOrUpdataDBCostCalculate(DBCostCalculate calculate);
        /// <summary>
        /// 计算公式字典集合
        /// </summary>
        /// <returns></returns>
        public abstract List<DBCalculateDictionary> GetCalculateDictionaryList();
        /// <summary>
        /// 添加和修改字典集合
        /// </summary>
        /// <param name="dictinary"></param>
        public abstract void InnerOrUpdataDBCalculateDictionary(DBCalculateDictionary dictinary);
        #endregion

    }
}
