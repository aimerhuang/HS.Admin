using System;
using System.Collections.Generic;
using System.ComponentModel;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    ///调货单 数据访问接口
    ///</summary>
    /// <remarks> 2016-04-01 朱成果 创建</remarks>
    public abstract class ITransferCargoDao : DaoBase<ITransferCargoDao>
    {
        #region 自动生成代码
        /// <summary>
        /// 插入(调货单)
        ///</summary>
        /// <param name="entity">调货单</param>
        /// <returns>新增记录编号</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public abstract int Insert(TransferCargo entity);

        /// <summary>
        /// 更新(调货单)
        /// </summary>
        /// <param name="entity">调货单</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public abstract int Update(TransferCargo entity);

        /// <summary>
        /// 获取(调货单)
        ///</summary>
        /// <param name="sysno">编号</param>
        /// <returns>调货单</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public abstract TransferCargo GetEntity(int sysno);

        /// <summary>
        /// 删除(调货单)
        ///</summary>
        /// <param name="sysno">编号</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public abstract int Delete(int sysno);
        #endregion

        /// <summary>
        /// 调货单分页列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="isHasAllWarehouse">是否拥有所有仓库的权限</param>
        /// <returns>分页列表</returns>
        /// <remarks>2016-04-05 杨浩 创建</remarks>
        public abstract Pager<CBTransferCargo> SearchTransferCargo(ParaTransferCargoFilter filter, int pageIndex,
            int pageSize,int userSysNo, bool isHasAllWarehouse);

     

        /// <summary>
        /// 根据出库单编号获取调货单
        /// </summary>
        /// <param name="stockOutSysno">出库单编号</param>
        /// <returns>调货单</returns>
        /// <remarks>2016-04-07 杨浩 创建</remarks>
        public abstract TransferCargo GetExistTransferCargoByStockOutSysno(int stockOutSysno);

        /// <summary>
        /// 根据订单号查询调货单
        /// </summary>
        /// <param name="orderSysno">订单号</param>
        /// <returns>调货单</returns>
        /// <remarks>2016-04-08 杨浩 创建</remarks>
        public abstract IList<CBTransferCargo> GetTransferCargoesByOrderSysno(int orderSysno);

    }

}
