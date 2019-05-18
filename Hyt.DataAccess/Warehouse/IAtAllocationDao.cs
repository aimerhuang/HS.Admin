using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Warehouse
{
    public abstract class IAtAllocationDao : DaoBase<IAtAllocationDao>
    {
        /// <summary>
        /// 分页查询调拨单列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        public abstract Pager<CBAtAllocation> QueryAtAllocationPager(Pager<CBAtAllocation> pager);

        /// <summary>
        /// 获取调拨单实体
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public abstract AtAllocation GetAtAllocationEntity(int sysNo);

        /// <summary>
        /// 创建调拨单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public abstract AtAllocation CreateAtAllocation(AtAllocation model);

        /// <summary>
        /// 更新调拨单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-30 陈海裕 创建</remarks>
        public abstract int UpdateAtAllocation(AtAllocation model);

        /// <summary>
        /// 新增调拨单明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public abstract int AddAtAllocationItem(AtAllocationItem model);

        /// <summary>
        /// 获取库存调拨单商品列表（添加调拨商品用）
        /// </summary>
        /// <param name="atAllocationSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public abstract List<AtAllocationItem> GetAtAllocationProducts(int atAllocationSysNo);

        /// <summary>
        /// 删除调拨单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public abstract int DeleteAtAllocationItem(int sysNo);

        /// <summary>
        /// 分页查询调拨单明细列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public abstract Pager<AtAllocationItem> QueryAtAllocationItemPager(Pager<AtAllocationItem> pager);

        /// <summary>
        /// 更新调拨单明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2016-06-29 陈海裕 创建</remarks>
        public abstract int UpdateAtAllocationItem(AtAllocationItem model);

        /// <summary>
        /// 获取当前调拨库存信息
        /// </summary>
        /// <param name="outWareSysNo"></param>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract List<DBAtAllocationItem> GetByDBAtAllocationItem(int outWareSysNo, int SysNo);
        /// <summary>
        /// 获取调拨单信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract DBAtAllocation GetDBAtAllocationEntity(int SysNo);

        /// <summary>
        /// 检查调拨单产品是否有0的数量
        /// </summary>
        /// <param name="atAllocationSysNo">调拨单系统编号</param>
        /// <returns></returns>
        /// <remarks>2018-01-17 杨浩 创建</remarks>
        public abstract bool ExistAtAllocationProductQtyZero(int atAllocationSysNo);
    }
}
