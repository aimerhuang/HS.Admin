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
    /// 取仓库库位数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public abstract class IWhWarehousePositionDao : DaoBase<IWhWarehousePositionDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract int Insert(WhWarehousePosition entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract void Update(WhWarehousePosition entity);

        /// <summary>
        /// 获取仓库库位
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract IList<WhWarehousePosition> GetWarehousePositions(int warehouseSysNo);

        /// <summary>
        /// 保存仓库库位列表信息
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract bool SetWarehousePositions(int sysNo, IList<WhWarehousePosition> listWarehousePositions);

        /// <summary>
        /// 获取出库商品库位列表
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="warehouseSysNo">商品编号</param>
        /// <returns>库位列表</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract IList<WhWarehousePosition> GetWPositionsByWsysNoAndProSysNo(int warehouseSysNo, int productSysNo);
    }
}