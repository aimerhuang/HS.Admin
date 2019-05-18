using System;
using System.Collections.Generic;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Warehouse
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// 2013/6/27 何方 创建
    /// </remarks>
    public interface IInStockBo
    {
        /// <summary>
        /// 插入入库单
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>入库单系统编号</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        int CreateStockIn(WhStockIn model);

        /// <summary>
        /// 更新入库单
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        int UpdateStockIn(WhStockIn model);

        /// <summary>
        /// 获取入库单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单列表 Ilist</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        PagedList<WhStockIn> GetStockInList(ParaInStockFilter filter, int pageSize);

        /// <summary>
        /// 作废入库单
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <param name="user">操作用户</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        /// <remarks>2013-06-17 朱成果 修改</remarks>
        bool InStockCancel(int sysNo, SyUser user);

        /// <summary>
        /// 计算入库
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <param name="invoiceSysNo">发票系统编号</param>
        /// <param name="itemList">入库商品</param>
        /// <param name="user">操作人</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        void InStockComplete(int sysNo, int? invoiceSysNo, List<WhStockInItem> itemList, SyUser user);

        /// <summary>
        /// 自动入库
        /// </summary>
        /// <param name="sysNo">入库系统编号</param>
        /// <param name="syUserSysNo">用户系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-08-22 周唐炬 创建</remarks>
        void AutoInStock(int sysNo, int syUserSysNo);

        /// <summary>
        /// 通过系统编号获取入库单
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        WhStockIn GetStockIn(int sysNo);

        /// <summary>
        /// 通过事务编号获取入库单明细
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        WhStockIn GetStockInDetailsByTransactionSysNo(string transactionSysNo);

        /// <summary>
        /// 添加入库单明细
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        int InsertStockInItem(WhStockInItem model);

        /// <summary>
        /// 更新入库单明细
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        int UpdateStockInItem(WhStockInItem model);

        /// <summary>
        /// 删除商品入库信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        bool DeleteStockInItem(int sysNo);

        /// <summary>
        /// 通过入库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">入库单系统SysNO</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小.</param>
        /// <returns>
        /// 返回入库单商品列表
        /// </returns>
        /// <remarks>
        /// 2013-06-09 周唐炬 创建
        /// </remarks>
        PagedList<WhStockInItem> GetStockInItemList(int stockInSysNo, int pageIndex, int pageSize);

        /// <summary>
        /// 通过入库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        /// <returns>返回入库单商品列表</returns>
        /// <remarks>2013-06-24 郑荣华 创建</remarks>
        List<WhStockInItem> GetStockInItemList(int stockInSysNo);

        /// <summary>
        /// 跟新入库单状态
        /// </summary>
        /// <param name="stockInSysNo">入库单系统编号.</param>
        /// <param name="status"> 入库单状态.</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/6/27 何方 创建
        /// </remarks>
        void UpdateStockInStatus(int stockInSysNo, Model.WorkflowStatus.WarehouseStatus.入库单状态 status, SyUser user);
    }
}