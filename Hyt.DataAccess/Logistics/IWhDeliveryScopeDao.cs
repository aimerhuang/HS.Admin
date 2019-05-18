
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Collections.Generic;
namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 仓库当日达配送范围
    /// </summary>
    /// <remarks>2014-10-09  朱成果 创建</remarks>
    public abstract class IWhDeliveryScopeDao : DaoBase<IWhDeliveryScopeDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public abstract int Insert(WhDeliveryScope entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public abstract void Update(WhDeliveryScope entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public abstract WhDeliveryScope GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 根据仓库编号删除数据
        /// </summary>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public abstract void DeleteByWarehouseSysNo(int warehouseSysNo);

        /// <summary>
        /// 获取仓库配送区域列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public abstract List<CBWhDeliveryScope> GetList();

        /// <summary>
        /// 根据条件筛选仓库来设置配送范围
        /// </summary>
        /// <param name="cityNo">所在城市编号</param>
        /// <param name="warehouseType">仓库类型</param>
        /// <param name="isSelfSupport">是否自营</param>
        /// <param name="deliveryTypeSysNo">配送方式</param>
        /// <returns></returns>
        /// <remarks>2014-10-09  朱成果 创建</remarks>
        public abstract List<WhWarehouse> GetWhWarehouseForDeliveryScope(int cityNo, int? warehouseType, int? isSelfSupport, int? deliveryTypeSysNo);
    }
}
