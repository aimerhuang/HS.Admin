using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Promotion
{
    /// <summary>
    /// 仓库免运费
    /// </summary>
    /// <remarks>2016-04-20 王耀发 创建</remarks>
    public abstract class IWhouseFreightFreeDao : DaoBase<IWhouseFreightFreeDao>
    {
        /// <summary>
        /// 免运费信息
        /// </summary>
        /// <param name="filter">免运费信息</param>
        /// <returns>返回免运费信息</returns>
        /// <remarks>2016-04-20 王耀发 创建</remarks>
        public abstract Pager<CBWhouseFreightFree> GetWhouseFreightFreeList(ParaWhouseFreightFreeFilter filter);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2016-04-20  王耀发 创建</remarks>
        public abstract int Insert(WhouseFreightFree entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2016-04-20  王耀发 创建</remarks>
        public abstract int Update(WhouseFreightFree entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-04-20  王耀发 创建</remarks>
        public abstract WhouseFreightFree GetEntity(int SysNo);
    }
}
