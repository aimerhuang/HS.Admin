using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Front;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 新闻数据访问 抽象类
    /// </summary>
    /// <remarks>2014-01-14 苟治国 创建</remarks>
    public class FeNewsDaoImpl : IFeNewsDao
    {
        /// <summary>
        /// 查看新闻
        /// </summary>
        /// <param name="sysNo">新闻编号</param>
        /// <returns>新闻</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override Model.FeNews GetModel(int sysNo)
        {
            return Context.Sql(@"select * from FeNews where SysNO = @0", sysNo).QuerySingle<FeNews>();
        }

        /// <summary>
        /// 判断重复数据
        /// </summary>
        /// <param name="model">分类实体信息</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override bool IsExists(FeNews model)
        {
            bool result = false;
            FeNews entity = Context.Select<FeNews>("*")
                .From("FeNews")
                .Where("Title= @Title and Sysno != @Sysno")
                .Parameter("Title", model.Title)
                .Parameter("Sysno", model.SysNo)
                .QuerySingle();

            if (entity != null && entity.SysNo > 0)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 根据条件获取新闻列表
        /// </summary>
        /// <param name="pager">新闻查询条件</param>
        /// <returns>新闻组列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override Pager<Model.CBFeNews> Seach(Pager<CBFeNews> pager)
        {
            #region sql条件
            string sqlWhere = @"(@Title is null or fn.Title like @Title1) and (@Status=-1 or fn.Status =@Status)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CBFeNews>("fn.*,(select username from syuser where sysno=fn.createdby) as createdbyname")
                              .From("FeNews fn")
                              .Where(sqlWhere)
                              .Parameter("Title", pager.PageFilter.Title)
                              .Parameter("Title1", "%" + pager.PageFilter.Title + "%")
                              .Parameter("Status", pager.PageFilter.Status)
                              .Paging(pager.CurrentPage, pager.PageSize).OrderBy("fn.DisplayOrder desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                              .From("FeNews fn")
                              .Where(sqlWhere)
                              .Parameter("Title", pager.PageFilter.Title)
                              .Parameter("Title1", "%" + pager.PageFilter.Title + "%")
                              .Parameter("Status", pager.PageFilter.Status)
                              .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 获取最新新闻
        /// </summary>
        /// <param name="rowNum">获取条数</param>
        /// <returns>新闻列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override IList<FeNews> GetNews(int rowNum)
        {
            #region sql条件
            string sql = string.Format("select * from FeNews fn where rownum<={0} and Status ={1} order by fn.SysNo asc", rowNum, (int)Hyt.Model.WorkflowStatus.ForeStatus.新闻状态.已审);
            #endregion

            var list = Context.Sql(sql).QueryMany<FeNews>();
            return list;
        }

        /// <summary>
        /// 获取热点新闻
        /// </summary>
        /// <param name="rowNum">获取条数</param>
        /// <returns>新闻列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override IList<FeNews> GetHots(int rowNum)
        {
            #region sql条件
            string sql = string.Format("select * from FeNews fn where rownum<={0} and Status ={1} order by fn.Views desc", rowNum, (int)Hyt.Model.WorkflowStatus.ForeStatus.新闻状态.已审);
            #endregion

            var list = Context.Sql(sql).QueryMany<FeNews>();
            return list;
        }

        /// <summary>
        /// 插入新闻
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override int Insert(Model.FeNews model)
        {
            var result = Context.Insert<FeNews>("FeNews", model)
                                .AutoMap(x => x.SysNo)
                                .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新新闻点击数
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2014－02-24 苟治国 创建</remarks>
        public override int UpdateViews(int sysNo)
        {
            string sql = string.Format("update FeNews set Views=Views+1 where sysNo={0}", sysNo);
            return Context.Sql(sql).Execute();
        }

        /// <summary>
        /// 更新新闻
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public override int Update(Model.FeNews model)
        {
            int rowsAffected = Context.Update<Model.FeNews>("FeNews", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
            return rowsAffected;
        }
    }
}
