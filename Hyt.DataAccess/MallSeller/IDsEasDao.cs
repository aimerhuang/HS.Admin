using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.MallSeller
{
    /// <summary>
    /// 分销商EAS关联
    /// </summary>
    /// <remarks>2013-10-10 黄志勇 创建</remarks>
    public abstract class IDsEasDao : DaoBase<IDsEasDao>
    {
        /// <summary>
        /// 查询分销商EAS关联
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <remarks>分销商EAS关联分页数据</remarks>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public abstract Pager<CBDsEasAssociation> Query(ParaDsEasFilter filter);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>返回新的编号</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public abstract int Insert(DsEasAssociation entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-25  黄志勇 创建</remarks>
        public abstract void Update(DsEasAssociation entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2013-10-11  黄志勇 创建</remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>分销商EAS关联</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public abstract DsEasAssociation GetEntity(int sysNo);

        /// <summary>
        /// 根据分销商商城编号获取
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <returns>分销商EAS关联</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public abstract DsEasAssociation Get(int dealerMallSysNo);

        /// <summary>
        /// 根据分销商商城编号获取
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="sellerNick">昵称</param>
        /// <returns>分销商EAS关联</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public abstract DsEasAssociation Get(int dealerMallSysNo, string sellerNick);

        /// <summary>
        /// 获取全部商城类型
        /// </summary>
        /// <returns>分销商城类型列表</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public abstract List<DsMallType> GetAllMallType();

        /// <summary>
        /// 获取全部商城
        /// </summary>
        /// <returns>分销商城列表</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public abstract List<DsDealerMall> GetAllMall();

        /// <summary>
        /// 获取新增商城
        /// </summary>
        /// <returns>分销商城列表</returns>
        /// <remarks>2013-10-10 黄志勇 创建</remarks>
        public abstract List<DsDealerMall> GetNewMall();

        /// <summary>
        /// 获取全部分销商EAS关联商城系统编号列表
        /// </summary>
        /// <returns>商城系统编号列表</returns>
        /// <remarks>2013-11-1 黄志勇 创建</remarks>
        public abstract List<int> GetAllDsEasAssociation();

    }
}
