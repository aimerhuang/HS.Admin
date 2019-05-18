using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Basic
{
    /// <summary>
    /// 库存信息
    /// </summary>
    /// <param name="filter">库存信息</param>
    /// <returns>返回库存信息</returns>
    /// <remarks>2015-08-27 王耀发 创建</remarks>
    public abstract class IOriginDao : Hyt.DataAccess.Base.DaoBase<IOriginDao>
    {
        /// <summary>
        /// 获取国家列表
        /// </summary>
        /// <param name="sysNo">国家系统编号</param>
        /// <returns>国家列表</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract Pager<Origin> GetOriginList(ParaOriginFilter filter);
        /// <summary>
        /// 获得国家列表
        /// </summary>
        /// <returns></returns>
        public abstract List<Origin> GetOrigin();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="PdProductSysNo"></param>
        /// <returns></returns>
        /// <remarkss>2015-08-06 王耀发 创建</remarks>
        public abstract Origin GetEntity(int SysNo);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(Origin entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Update(Origin entity);
        /// <summary>
        /// 删除国家
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract int Delete(int sysNo);
        /// <summary>
        /// 获取指定名称的国家信息
        /// </summary>
        /// <param name="name">国家名称</param>
        /// <returns>国家实体信息</returns>
        /// <remarks>2015-12-5 王耀发 创建</remarks>
        public abstract Origin GetEntityByName(string Origin_Name);
    }
}

