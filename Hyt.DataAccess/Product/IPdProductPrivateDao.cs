using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 运费模板 抽象类
    /// </summary>
    /// <remarks>
    /// 015-08-06 王耀发 创建
    /// </remarks>
    public abstract class IPdProductPrivateDao : Hyt.DataAccess.Base.DaoBase<IPdProductPrivateDao>
    {
        /// <summary>
        /// 获取运费模板列表
        /// </summary>
        /// <param name="sysNo">运费模板系统编号</param>
        /// <returns>运费模板列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract Pager<PdProductPrivateList> GetPdProductPrivateList(ParaProductPrivateFilter filter);
          /// <summary>
        /// 招商加盟申请列表（改）
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public abstract Pager<PdProductPrivateList> GetPdProductPrivateLists(ParaProductPrivateFilter filter);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract PdProductPrivateList GetEntity(int sysNo);
          /// <summary>
        /// 获取数据(改)
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract PdProductPrivateList GetEntitys(int sysNo);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(PdProductPrivate entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Update(PdProductPrivate entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract int Delete(int sysNo);
    }
}

