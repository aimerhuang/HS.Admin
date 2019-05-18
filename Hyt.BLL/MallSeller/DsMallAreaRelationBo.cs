using Hyt.Model;
using Hyt.DataAccess.MallSeller;

namespace Hyt.BLL.MallSeller
{
    /// <summary>
    /// 商城地区关联业务类
    /// </summary>
    /// <remarks></remarks>
    public class DsMallAreaRelationBo : BOBase<DsMallAreaRelationBo>
    {
        /// <summary>
        /// 查询商城地区关联
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>商城地区关联分页数据</returns>
        /// <remarks>2014-10-14 缪竞华 创建</remarks>
        public Model.Pager<Model.Transfer.CBDsMallAreaRelation> Query(Model.Parameter.ParaDsMallAreaRelationFilter filter)
        {
            return IDsMallAreaRelationDao.Instance.Query(filter);
        }

        /// <summary>
        /// 根据sysNo删除DsMallAreaAssociation表中的数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回result</returns>
        /// <remarks>2014-10-14 缪竞华 创建</remarks>
        public Result Delete(int sysNo)
        {
            var res = new Result();
            var r = IDsMallAreaRelationDao.Instance.Delete(sysNo);
            if (r > 0) res.Status = true;
            return res;
        }
    }
}
