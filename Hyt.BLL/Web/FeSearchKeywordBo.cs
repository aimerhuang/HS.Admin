using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Infrastructure.Caching;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 搜索热词
    /// </summary>
    /// <remarks>2014-1-8 黄波 创建</remarks>
    public class FeSearchKeywordBo : BOBase<FeSearchKeywordBo>
    {
        /// <summary>
        /// 获取搜索热词
        /// </summary>
        /// <returns>热词列表</returns>
        /// <remarks>2013-08-07 黄波 创建</remarks>
        public IList<FeSearchKeyword> GetSearchKeys()
        {
            return CacheManager.Get<IList<FeSearchKeyword>>(CacheKeys.Items.SearchKeys, () =>
            {
                return Hyt.DataAccess.Web.IFeSearchKeywordDao.Instance.GetSearchKeys();
            });
        }
    }
}
