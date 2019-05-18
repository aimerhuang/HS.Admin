using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.MallSeller
{
    public abstract class IDsProductRelationDao : DaoBase<IDsProductRelationDao>
    {
        /// <summary>
        /// 查询升舱商品关系维护关联
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <remarks>查询升舱商品关系维护关联分页数据</remarks>
        /// <remarks>2014-10-10 谭显锋 创建</remarks>
        public abstract Pager<CBDsProductRelation> Query(ParaDsProductRelationFilter filter);

        /// <summary>
        /// 根据sysNo删除DsProductAssociation表中的数据
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>受影响行数</returns>
        /// <remarks>2014-10-10 谭显锋 创建</remarks>
        public abstract int Delete(int sysNo);

    }
}
