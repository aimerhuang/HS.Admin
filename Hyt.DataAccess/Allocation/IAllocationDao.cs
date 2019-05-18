using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Allocation
{
    /// <summary>
    /// 调拨单
    /// </summary>
    /// <remarks>2016-6-23 杨浩 创建</remarks>
    public abstract class IAllocationDao : DaoBase<IAllocationDao>
    {
        /// <summary>
        /// 添加调拨单
        /// </summary>
        /// <param name="model">调拨单实体对象</param>
        /// <returns></returns>
        /// <remarks>2016-6-23 杨浩 创建</remarks>
        public abstract int Add(AtAllocation model);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">调拨单实体对象</param>
        /// <returns></returns>
        /// <remarks>2016-6-23 杨浩 创建</remarks>
        public abstract int Update(AtAllocation model);
        /// 查询采购单
        /// </summary>
        /// <param name="para">查询参数</param>
        /// <returns></returns>
        public abstract Pager<CBAtAllocation> Query(ParaAtAllocationFilter para);
    }
}
