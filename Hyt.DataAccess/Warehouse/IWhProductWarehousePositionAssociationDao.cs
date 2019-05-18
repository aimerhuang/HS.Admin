using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;


namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 取商检数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public abstract class IWhProductWarehousePositionAssociationDao : DaoBase<IWhProductWarehousePositionAssociationDao>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract int Insert(WhProductWarehousePositionAssociation entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract void Update(WhProductWarehousePositionAssociation entity);

        /// <summary>
        /// 获取关联列表
        /// </summary>
        /// <param name="productStockSysNo">库存系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns></returns>
        public abstract IList<CBWhProductWarehousePositionAssociation> GetPositionAssociationDetail(int productStockSysNo, int warehouseSysNo);

                /// <summary>
        /// 保存库位关联列表信息
        /// </summary>
        /// <param name="sysNo">库存编号</param>
        /// <returns>库位关联列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract bool SetWarehousePositionAssociations(int sysNo, IList<WhProductWarehousePositionAssociation> listPositionAssociations);

        /// <summary>
        /// 获取仓库库位关联
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号集合</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-7-19 杨云奕 创建</remarks>
        public abstract IList<CBWhProductWarehousePositionAssociation> GetPositionAssociationDetail(List<int> ProSysNos, int? warehouseSysNo);

    }
}