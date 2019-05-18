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
    public abstract class IWhInventoryAlarmDao : DaoBase<IWhInventoryAlarmDao>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract int Insert(WhInventoryAlarm entity);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 王耀发 创建</remarks>
        public abstract void Update(WhInventoryAlarm entity);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="ProductStockSysNo">库存编号</param>
        /// <returns></returns>
        /// <remarks>2016-06-15 王耀发 创建</remarks>
        public abstract WhInventoryAlarm GetAlarmByStockSysNo(int ProductStockSysNo);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        public abstract Pager<CBWhInventoryAlarm> Query(ParaWhInventoryAlarmFilter filter);
        /// <summary>
        /// 搜索出所有的报警商品
        /// </summary>
        /// <returns></returns>
        public abstract List<CBWhInventoryAlarm> SearAlarmProductStockList();
        /// <summary>
        /// 获取库存警报数据
        /// </summary>
        /// <returns></returns>
        public abstract int GetAlarmProductStockCount();

        public abstract int GetAlarmProductStockCount(IList<WhWarehouse> list);

        public abstract List<WhInventoryAlarm> GetAllAlarm();
    }
}