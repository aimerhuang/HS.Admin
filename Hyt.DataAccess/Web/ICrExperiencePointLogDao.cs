using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.DataAccess.Base;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 经验积分数据访问 抽象类
    /// </summary>
    /// <remarks>2013-11-1 苟治国 创建</remarks>
    public abstract class ICrExperiencePointLogDao : DaoBase<ICrExperiencePointLogDao>
    {
        /// <summary>
        /// 根据条件获取经验积分的列表
        /// </summary>
        /// <param name="pager">分页属性</param>
        /// <param name="exp">经验积分查询条件</param>
        /// <returns>经验积分列表</returns>
        /// <remarks>2013-11-1 苟治国 创建</remarks>
        public abstract Pager<Model.CrExperiencePointLog> SeachPager(Pager<CrExperiencePointLog> pager, ParaCrExperiencePointLogFilter exp);
    }
}
