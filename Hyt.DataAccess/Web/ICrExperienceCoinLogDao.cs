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
    /// 惠源币数据访问 抽象类
    /// </summary>
    /// <remarks>2013-08-12 苟治国 创建</remarks>
    public abstract class ICrExperienceCoinLogDao : DaoBase<ICrExperienceCoinLogDao>
    {
        /// <summary>
        /// 根据条件获取惠源币的列表
        /// </summary>
        /// <param name="pager">分页属性</param>
        /// <param name="exp">惠源币查询条件</param>
        /// <returns>惠源币列表</returns>
        /// <remarks>2013-08-21 苟治国 创建</remarks>
        public abstract Pager<Model.CrExperienceCoinLog> SeachPager(Pager<CrExperienceCoinLog> pager,ParaCrExperienceCoinLogFilter exp);
    }
}
