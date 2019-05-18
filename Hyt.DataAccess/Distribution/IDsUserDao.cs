
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using System.Collections.Generic;
namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商用户
    /// </summary>
    /// <remarks>2014-06-05  朱成果 创建</remarks>
    public abstract class IDsUserDao : DaoBase<IDsUserDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public abstract int Insert(DsUser entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public abstract void Update(DsUser entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public abstract DsUser GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 获取分销商用户
        /// </summary>
        /// <param name="account">帐号</param>
        /// <returns>分销商用户实体</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public abstract DsUser GetEntity(string account);

        /// <summary>
        /// 根据分销商编号获取分销商用户列表
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>分销商用户列表</returns>
        /// <remarks>2014-06-05  朱成果 创建</remarks>
        public abstract List<DsUser> GetListByDealerSysNo(int dealerSysNo);


        public abstract DsUser GetListByDealerSysNo(int dsSysNo, string accout, string pass);

        public abstract List<DsUser> GetListByDealerSysNo();
    }
}
