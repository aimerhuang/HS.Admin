using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 新闻接口抽象类
    /// </summary>
    /// <remarks>2013－01-14 苟治国 创建</remarks>
    public abstract class IFeNewsDao : DaoBase<IFeNewsDao>
    {
        /// <summary>
        /// 查看新闻
        /// </summary>
        /// <param name="sysNo">新闻编号</param>
        /// <returns>新闻</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract Model.FeNews GetModel(int sysNo);

        /// <summary>
        /// 判断重复数据
        /// </summary>
        /// <param name="model">分类实体信息</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract bool IsExists(FeNews model);

        /// <summary>
        /// 根据条件获取新闻列表
        /// </summary>
        /// <param name="pager">新闻查询条件</param>
        /// <returns>新闻组列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract Pager<Model.CBFeNews> Seach(Pager<CBFeNews> pager);

        /// <summary>
        /// 获取最新新闻
        /// </summary>
        /// <param name="rowNum">获取条数</param>
        /// <returns>新闻列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract IList<FeNews> GetNews(int rowNum);

        /// <summary>
        /// 获取热点新闻
        /// </summary>
        /// <param name="rowNum">获取条数</param>
        /// <returns>新闻列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract IList<FeNews> GetHots(int rowNum); 

        /// <summary>
        /// 插入新闻
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract int Insert(Model.FeNews model);

        /// <summary>
        /// 更新新闻点击数
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2014－02-24 苟治国 创建</remarks>
        public abstract int UpdateViews(int sysNo);

        /// <summary>
        /// 更新新闻
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public abstract int Update(Model.FeNews model);
    }
}
