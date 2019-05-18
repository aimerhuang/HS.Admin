using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.RMA
{
    /// <summary>
    ///取件单
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public abstract class ILgPickUpDao : DaoBase<ILgPickUpDao>
    {
        /// <summary>
        /// 添加取件单
        /// </summary>
        /// <param name="entity">取件单</param>
        /// <returns>取件单编号</returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public abstract int Insert(LgPickUp entity);

        /// <summary>
        /// 获取取件方式
        /// </summary>
        /// <param name="pickupTypeSysNo">取件方式编号</param>
        /// <returns>取件方式</returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public abstract LgPickupType GetPickupType(int pickupTypeSysNo);

        /// <summary>
        /// 获取入库单对应的取件单
        /// </summary>
        /// <param name="stockInSysNo">入库单号</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public abstract LgPickUp GetEntityByStockIn(int stockInSysNo);

        /// <summary>
        /// 更新取件单信息
        /// </summary>
        /// <param name="entity">取件单</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public abstract void Update(LgPickUp entity);

        /// <summary>
        /// 获取所有取件方式
        /// </summary>
        /// <returns>所有取件方式</returns>
        /// <remarks>2013-08-13 周唐炬 创建</remarks>
        public abstract List<LgPickupType> GetLgPickupTypeList();
    }
}
