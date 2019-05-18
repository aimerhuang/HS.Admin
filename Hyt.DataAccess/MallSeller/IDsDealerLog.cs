using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.MallSeller
{
    /// <summary>
    /// 分销商升舱错误日志
    /// </summary>
    /// <remarks>2014-03-31 唐文均 创建</remarks>
    public abstract class IDsDealerLog : DaoBase<IDsDealerLog>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2014-03-31 唐文均 创建</remarks>
        public abstract int Insert(DsDealerLog entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-03-31 唐文均 创建</remarks>
        public abstract void Update(DsDealerLog entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-03-31 唐文均 创建</remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2014-03-31 朱家宏 创建</remarks>
        public abstract Pager<DsDealerLog> Query(ParaDsDealerLogFilter filter);
        /// <summary>
        /// 检查订单号是否存在
        /// </summary>
        /// <param name="mallOrderId">商商城订单号</param>
        /// <param name="status">待解决(10),已解决(20)</param>
        /// <param name="mallSysNo">商城系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-11-06 杨浩 创建</remarks>
        public abstract bool ChekMallOrderId(string mallOrderId, int status, int mallSysNo);

        /// <summary>
        /// 取单条数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>model</returns>
        /// <remarks>2014-03-31 朱家宏 创建</remarks>
        public abstract DsDealerLog Get(int sysNo);
    }
}
