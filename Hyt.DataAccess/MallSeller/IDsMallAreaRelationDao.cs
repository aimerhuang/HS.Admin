using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;


namespace Hyt.DataAccess.MallSeller
{
    /// <summary>
    /// 商城地区关联
    /// </summary>
    /// <remarks>2014-10-14 缪竞华 创建</remarks>
    public abstract class IDsMallAreaRelationDao : DaoBase<IDsMallAreaRelationDao>
    {
        /// <summary>
        /// 查询商城地区关联
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>商城地区关联分页数据</returns>
        /// <remarks>2014-10-14 缪竞华 创建</remarks>
        public abstract Pager<CBDsMallAreaRelation> Query(ParaDsMallAreaRelationFilter filter);

        /// <summary>
        /// 根据sysNo删除DsMallAreaAssociation表中的数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-10-14 缪竞华 创建</remarks>        
        public abstract int Delete(int sysNo);
    }
}
