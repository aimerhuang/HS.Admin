using System;
using System.Collections.Generic;
using System.ComponentModel;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    ///调货配置 数据访问接口
    ///</summary>
    /// <remarks> 2016-04-01 朱成果 创建</remarks>
    public abstract class ITransferCargoConfigDao : DaoBase<ITransferCargoConfigDao>
    {
        #region 自动生成代码

        /// <summary>
        /// 插入(调货配置)
        ///</summary>
        /// <param name="entity">调货配置</param>
        /// <returns>新增记录编号</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public abstract int Insert(TransferCargoConfig entity);

        /// <summary>
        /// 更新(调货配置)
        /// </summary>
        /// <param name="entity">调货配置</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public abstract int Update(TransferCargoConfig entity);

        /// <summary>
        /// 获取(调货配置)
        ///</summary>
        /// <param name="sysno">编号</param>
        /// <returns>调货配置</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public abstract TransferCargoConfig GetEntity(int sysno);

        /// <summary>
        /// 删除(调货配置)
        ///</summary>
        /// <param name="sysno">编号</param>
        /// <returns>影响的行</returns>
        /// <remarks> 2016-04-01 朱成果 创建</remarks>
        public abstract int Delete(int sysno);

        #endregion

        /// <summary>
        /// 根据“申请调货仓库编号”获取“配货仓库编号”
        ///</summary>
        /// <param name="ApplyWarehouseSysNo">申请调货仓库编号</param>
        /// <returns>配货仓库编号</returns>
        /// <remarks> 2016-04-05 王江 创建</remarks>
        public abstract int GetDeliveryWarehouseSysNoByApplyWarehouseSysNo(int ApplyWarehouseSysNo);

        /// <summary>
        /// 根据 配货仓库编号 获取 申请调货仓库列表
        ///</summary>
        /// <param name="deliveryWarehouseSysNo">配货仓库编号</param>
        /// <returns>申请调货仓库列表</returns>
        /// <remarks> 2016-04-06 王江 创建</remarks>
        public abstract IList<CBTransferCargoConfig> GetApplyWarehouseListByDeliveryWarehouseSysNo(int deliveryWarehouseSysNo);

        /// <summary>
        /// 获取所有已存在的申请调货仓库
        ///</summary>
        /// <returns>已存在的申请调货仓库结果集</returns>
        /// <remarks> 2016-04-19 王江 创建</remarks>
        public abstract IList<int> GetAllApplyWarehouseSysNo();

        /// <summary>
        /// 根据 配货仓库编号 申请调货仓库编号 获取单条记录
        /// </summary>
        /// <param name="deliveryWarehouseSysNo">配货仓库编号</param>
        /// <param name="applyWarehouseSysNo">申请调货仓库编号</param>
        /// <returns>调货配置</returns>
        /// <remarks> 2016-04-06 王江 创建</remarks>
        public abstract TransferCargoConfig QuerySingle(int deliveryWarehouseSysNo, int applyWarehouseSysNo);

        /// <summary>
        /// 界面分页列表
        /// </summary>
        /// <param name="filter">筛选字段</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-10-9 王江 创建</remarks>
        public abstract Pager<CBTransferCargoConfig> GetTransferCargoConfigList(Model.Parameter.ParaTransferCargoConfigFilter filter);

        /// <summary>
        /// 查询可用的调货配置表
        ///</summary>
        /// <param name="applyWarehouseSysNo">申请调货仓库编号</param>
        /// <returns>调货配置表</returns>
        /// <remarks> 2016-04-05 朱成果 创建</remarks>
        public abstract TransferCargoConfig GetEntityByApplyWarehouseSysNo(int applyWarehouseSysNo);
       
    }

}
