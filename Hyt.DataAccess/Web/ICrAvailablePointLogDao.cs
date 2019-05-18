using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Web
{
    /// <summary>
    /// 会员可用积分日志
    /// </summary>
    /// <remarks>2013-12-01 苟治国 创建</remarks>
    public abstract class ICrAvailablePointLogDao : DaoBase<ICrAvailablePointLogDao>
    {
        /// <summary>
        /// 会员可用积分日志
        /// </summary>
        /// <param name="pager">条件</param>
        /// <returns>可用积分日志列表</returns>
        /// <remarks>2013-12-01 苟治国 创建</remarks>
        public abstract Pager<Model.CrAvailablePointLog> GetPager(Pager<CrAvailablePointLog> pager,ParaCrAvailablePointLogFilter apl);

        public abstract List<CrAvailablePointLog> GetAll();

        public abstract void DeleteData(int p);
    }
}
