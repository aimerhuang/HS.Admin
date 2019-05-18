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
    /// 等级积分日志
    /// </summary>
    /// <remarks>2013-08-22 苟治国 添加</remarks>
    public abstract class ICrLevelPointLogDao : DaoBase<ICrLevelPointLogDao>
    {
        /// <summary>
        /// 根据条件获取等级积分日志列表
        /// </summary>
        /// <param name="pager">分页属性</param>
        /// <param name="type">tab类型</param>
        /// <returns>等级积分日志列表</returns>
        /// <remarks>2013-08-22 苟治国 创建</remarks>
        public abstract Pager<Model.CrLevelPointLog> SeachPager(Pager<CrLevelPointLog> pager, int type);
    }
}
