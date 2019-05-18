using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Front;

namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 友情链接数据层实现类
    /// </summary>
    /// <remarks>2013－12-09 苟治国 创建</remarks>
    public class MkBlogrollDaoImpl : IMkBlogrollDao
    {
        /// <summary>
        /// 查看友情链接
        /// </summary>
        /// <param name="sysNo">友情链接编号</param>
        /// <returns>友情链接</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public override Model.MkBlogroll GetModel(int sysNo)
        {
            return Context.Sql(@"select * from MkBlogroll where SysNO = @0", sysNo).QuerySingle<Model.MkBlogroll>();
        }

        /// <summary>
        /// 判断友情连接标题是否重复
        /// </summary>
        /// <param name="key">友情连接标题</param>
        /// <param name="sysNo">系统编号</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public override bool Verify(string key, int sysNo)
        {
            string sql = @"(WebSiteName=@WebSiteName) and (@sysNo=0 or SysNo !=@sysNo)";
            int countBuilder = Context.Select<int>("count(1)")
                                     .From("MkBlogroll")
                                     .Where(sql)
                                     .Parameter("WebSiteName", key)
                                     .Parameter("sysNo", sysNo)
                                     .QuerySingle();
            return countBuilder > 0;
        }

        /// <summary>
        /// 根据条件获取友情链接列表
        /// </summary>
        /// <param name="pager">友情链接查询条件</param>
        /// <returns>友情链接列表</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public override Pager<Model.MkBlogroll> Seach(Pager<MkBlogroll> pager)
        {
            #region sql条件
            string sql = @"(@Status=-1 or Status =@Status) and (@WebSiteName is null or WebSiteName like @WebSiteName1)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<MkBlogroll>("mb.*")
                    .From("MkBlogroll mb")
                    .Where(sql)
                    .Parameter("Status", pager.PageFilter.Status)
                    .Parameter("WebSiteName", pager.PageFilter.WebSiteName)
                    .Parameter("WebSiteName1", "%" + pager.PageFilter.WebSiteName + "%")
                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("mb.DisplayOrder desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                    .From("MkBlogroll")
                    .Where(sql)
                    .Parameter("Status", pager.PageFilter.Status)
                    .Parameter("WebSiteName", pager.PageFilter.WebSiteName)
                    .Parameter("WebSiteName1", "%" + pager.PageFilter.WebSiteName + "%")
                    .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 插入友情链接
        /// </summary>
        /// <param name="model">友情链接明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public override int Insert(Model.MkBlogroll model)
        {
            if (model.AuditDate == DateTime.MinValue)
            {
                model.AuditDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (model.LastUpdateDate == DateTime.MinValue)
            {
                model.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var result = Context.Insert<MkBlogroll>("MkBlogroll", model)
                    .AutoMap(x => x.SysNo)
                    .ExecuteReturnLastId<int>("SysNo");
            return result;
        }

        /// <summary>
        /// 更新友情链接
        /// </summary>
        /// <param name="model">友情链接明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public override int Update(Model.MkBlogroll model)
        {
            int rowsAffected = Context.Update<Model.MkBlogroll>("MkBlogroll", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }
    }
}
