
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商仓库关系
    /// </summary>
    /// <remarks>2014-10-15 余勇 创建</remarks>
    public abstract class IDsDealerWharehouse : DaoBase<IDsDealerWharehouse>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回新的编号</returns>
         /// <remarks>2014-10-15 余勇 创建</remarks>
        public abstract int Insert(DsDealerWharehouse entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-10-15 余勇 创建</remarks>
        public abstract void Update(DsDealerWharehouse entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
         /// <remarks>2014-10-15 余勇 创建</remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 取单条数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>model</returns>
		/// <remarks>2014-10-15 余勇 创建</remarks>
        public abstract DsDealerWharehouse Get(int sysNo);

        /// <summary>
        /// 通过仓库编号取单条数据
        /// </summary>
        /// <param name="warehousSysNo">仓库编号</param>
        /// <returns>model</returns>
        /// <remarks>2014-10-15 余勇 创建</remarks>
        public abstract DsDealerWharehouse GetByWarehousSysNo(int warehousSysNo);

        /// <summary>
        /// 通过分销商编号获取数据
        /// </summary>
        /// <param name="UserSysNo"></param>
        public abstract DsDealerWharehouse GetByDsUserSysNo(int UserSysNo);

        /// <summary>
        /// 获取所有分销商数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2016-03-14 杨云奕 添加
        /// </remarks>
        public abstract System.Collections.Generic.List<CBDsDealerWharehouse> GetAllDealerWarehouse();
    }
}
