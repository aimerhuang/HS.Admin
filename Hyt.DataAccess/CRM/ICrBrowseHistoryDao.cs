using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 商品浏览记录抽象类
    /// </summary>
    /// <remarks>
    /// 2013-09-10 郑荣华 创建
    /// </remarks>
    public abstract class ICrBrowseHistoryDao : DaoBase<ICrBrowseHistoryDao>
    {
        /// <summary>
        /// 新加商品浏览记录
        /// </summary>
        /// <param name="model">商品浏览记录实体</param>
        /// <returns>新加的系统编号</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public abstract int Create(CrBrowseHistory model);

        /// <summary>
        /// 更新商品浏览记录
        /// </summary>
        /// <param name="model">商品浏览记录实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public abstract int Update(CrBrowseHistory model);

        /// <summary>
        /// 获取商品浏览记录
        /// </summary>
        /// <param name="filter">商品浏览历史查询条件实体</param>
        /// <returns>商品浏览记录</returns>
        /// <remarks>
        /// 2013-09-10 郑荣华 创建
        /// </remarks>
        public abstract CrBrowseHistory GetCrBrowseHistory(ParaCrBrowseHistoryFilter filter);

    }
}
