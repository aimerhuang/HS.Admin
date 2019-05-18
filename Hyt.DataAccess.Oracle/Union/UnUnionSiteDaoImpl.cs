using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Union;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Union
{
    /// <summary>
    /// 联盟网站
    /// </summary>
    /// <remarks>2013-10-14 吴文强 创建</remarks>
    public class UnUnionSiteDaoImpl : IUnUnionSiteDao
    {
        /// <summary>
        /// 根据系统编号获取联盟网站
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>联盟网站</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public override UnUnionSite GetModel(int sysNo)
        {
            string sql = @"
                            select * from UnUnionSite a 
                            where a.sysno= @sysno
                        ";
            var model = Context.Sql(sql)
                                     .Parameter("sysno", sysNo)
                                     .QuerySingle<UnUnionSite>();
            return model;
        }

        /// <summary>
        /// 创建联盟网站
        /// </summary>
        /// <param name="model">联盟网站实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override int Create(UnUnionSite model)
        {
            return Context.Insert<UnUnionSite>("UnUnionSite", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改联盟网站
        /// </summary>
        /// <param name="model">联盟网站实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override int Update(UnUnionSite model)
        {
            return Context.Update<UnUnionSite>("UnUnionSite", model).AutoMap(x => x.SysNo).Where(x => x.SysNo).Execute();
        }

        /// <summary>
        /// 获取所有联盟网站
        /// </summary>
        /// <returns>所有联盟网站</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override List<UnUnionSite> GetAll()
        {
            return Context.Sql(@"select * from UnUnionSite").QueryMany<UnUnionSite>();
        }

        /// <summary>
        /// 通过条件获取联盟网站列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>联盟网站列表</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override Pager<UnUnionSite> GetUnionSiteList(ParaUnUnionSiteFilter filter)
        {
            //            const string sql = @"(SELECT A.*
            //                                          FROM UnUnionSite A                                        
            //                                         WHERE (@p0p0 IS NULL OR A.STATUS = @p0p0)                                          
            //                                           AND (@p0p1 IS NULL OR REGEXP_charindex(A.SiteName, @p0p1, 1, 1, 1, 'i') > 0)
            //                                ) tb";

            const string sql = @"(SELECT A.*
                                          FROM UnUnionSite A                                        
                                         WHERE (@0 IS NULL OR A.STATUS = @0)                                          
                                           AND (@1 IS NULL OR CHARINDEX(','+A.SiteName+',',','+CAST(@1 as VARCHAR(255))+',') > 0)
                                ) tb";

            var dataList = Context.Select<UnUnionSite>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var paras = new object[]
                {
                    filter.Status,
                    filter.SiteName
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<UnUnionSite>
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }

        /// <summary>
        /// 联盟网站名称验证
        /// </summary>
        /// <param name="siteName">联盟网站名称</param>
        /// <param name="sysNo">联盟网站系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public override int UnionSiteVerify(string siteName, int? sysNo)
        {
            const string sql = "SELECT COUNT(1) FROM UnUnionSite WHERE  SiteName=@0 AND (@1 IS NULL OR SYSNO<>@1) ";
            var paras = new object[]
                {
                    siteName,
                    sysNo
                };
            return Context.Sql(sql).Parameters(paras).QuerySingle<int>();
        }
    }
}
