using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.RMA
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>2013-09-23  朱成果 创建</remarks>
    public abstract class IRcReturnImageDao : DaoBase<IRcReturnImageDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-23  朱成果 创建</remarks>
        public abstract int Insert(RcReturnImage entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-23  朱成果 创建</remarks>
        public abstract void Update(RcReturnImage entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-23  朱成果 创建</remarks>
        public abstract RcReturnImage GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns> 
        /// <remarks>2013-09-23  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);


        /// <summary>
        /// 通过退换货编号获取数据
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <returns>list</returns>
        /// <remarks>2013-09-23  朱家宏 创建</remarks>
        public abstract IList<RcReturnImage> GetAll(int returnSysNo);
    }
}
