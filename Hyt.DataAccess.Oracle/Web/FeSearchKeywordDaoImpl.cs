using System;
using System.Collections.Generic;
using Hyt.DataAccess.Web;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 搜索关键字
    /// </summary>
    /// <remarks>2013-08-07 黄波 创建</remarks>
    public class FeSearchKeywordDaoImpl : IFeSearchKeywordDao
    {
        /// <summary>
        /// 获取搜索热词
        /// </summary>
        /// <returns>热词列表</returns>
        /// <remarks>2013-08-07 黄波 创建</remarks>
        public override IList<FeSearchKeyword> GetSearchKeys()
        {
            string sql = @"
                                    select * from FeSearchKeyword t where t.status=@status order by t.HitsCount desc
                                  ";

            return Context.Sql(sql)
                .Parameter("status", (int)ForeStatus.搜索关键字状态.前台显示)
                .QueryMany<FeSearchKeyword>();
        }
    }
}
