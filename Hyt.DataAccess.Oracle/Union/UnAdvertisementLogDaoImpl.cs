using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Union;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.Union
{
    /// <summary>
    /// 联盟网站
    /// </summary>
    /// <remarks>2013-10-14 吴文强 创建</remarks>
    public class UnAdvertisementLogDaoImpl : IUnAdvertisementLogDao
    {
        /// <summary>
        /// 新增广告日志
        /// </summary>
        /// <param name="advertisementLog">广告日志</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public override void Insert(UnAdvertisementLog advertisementLog)
        {
            Context.Insert("UnAdvertisementLog", advertisementLog)
                   .AutoMap(x => x.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 更新广告日志
        /// </summary>
        /// <param name="advertisementLog">广告日志</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public override void Update(UnAdvertisementLog advertisementLog)
        {
            Context.Update<UnAdvertisementLog>("UnAdvertisementLog", advertisementLog)
                   .AutoMap(x => x.SysNo)
                   .Where(x => x.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 根据Cookie标识码获取广告日志集合
        /// </summary>
        /// <param name="cookieIdentity">Cookie标识码</param>
        /// <returns>Cookie标识码集合</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public override List<UnAdvertisementLog> GetListByCookieIdentity(string cookieIdentity)
        {
            return Context.Select<UnAdvertisementLog>("t.*")
                              .From("UnAdvertisementLog t")
                              .Where("t.CookieIdentity=@CookieIdentity")
                              .Parameter("CookieIdentity", cookieIdentity)
                              .QueryMany();
        }

        /// <summary>
        /// 查询联盟广告日志
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>查询数据</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override Pager<UnAdvertisementLog> GetList(Model.Parameter.ParaUnAdvertisementLogFilter filter)
        {
            const string sql = @"(SELECT A.*
                                          FROM UnAdvertisementLog A
                                          LEFT JOIN UnAdvertisement B
                                            ON B.SYSNO = A.ADVERTISEMENTSYSNO
                                          LEFT JOIN UnUnionSite C
                                            ON C.SYSNO = B.UNIONSITESYSNO
                                         WHERE (@0 IS NULL OR A.IsValid = @0)
                                           AND (@1 IS NULL OR A.AdvertisementType = @1)  
                                           AND (@2 IS NULL OR C.SYSNO=@2)                                       
                                           AND (@3 IS NULL OR A.AccessTime >= @3)
                                           AND (@4 IS NULL OR A.AccessTime <= @4)                                      
                                ) tb";

            var dataList = Context.Select<UnAdvertisementLog>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            if (filter.EndTime.HasValue)
            {
                var date = Convert.ToDateTime(filter.EndTime.Value);
                filter.EndTime = date.AddDays(1);
            }
            var paras = new object[]
                {
                    filter.IsValid,
                    filter.AdvertisementType,
                    filter.UnionSiteSysNo,
                    filter.StartTime,
                    filter.EndTime
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<UnAdvertisementLog>
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }

        /// <summary>
        /// 获取联盟广告日志实体
        /// </summary>
        /// <param name="sysNo">联盟广告日志系统编号</param>
        /// <returns>联盟广告日志</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override UnAdvertisementLog GetModel(int sysNo)
        {
            return Context.Sql("select * from UnAdvertisementLog where SysNo=@SysNo").Parameter("SysNo", sysNo).QuerySingle<UnAdvertisementLog>();
        }

        /// <summary>
        /// 根据订单号获取联盟广告日志
        /// </summary>
        /// <param name="validData">有效数据</param>
        /// <returns>联盟广告日志</returns>
        /// <remarks>2014-02-25 吴文强 创建</remarks>
        public override UnAdvertisementLog GetModelByValidData(string validData)
        {
            return Context.Sql("select * from   UnAdvertisementLog where charindex(ValidData,@ValidData)>0")
                .Parameter("ValidData", validData)
                .QuerySingle<UnAdvertisementLog>();
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>Excel数据</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override List<CBUnAdvertisementLog> GetExcelList(Model.Parameter.ParaUnAdvertisementLogFilter filter)
        {
            const string sql = @"(SELECT A.*,C.SITENAME,C.SITEURL
                                          FROM UnAdvertisementLog A
                                          LEFT JOIN UnAdvertisement B
                                            ON B.SYSNO = A.ADVERTISEMENTSYSNO
                                          LEFT JOIN UnUnionSite C
                                            ON C.SYSNO = B.UNIONSITESYSNO
                                         WHERE (@0 IS NULL OR A.IsValid = @0)
                                           AND (@1 IS NULL OR A.AdvertisementType = @1)
                                           AND (@2 IS NULL OR C.SYSNO=@2)
                                           AND (@3 IS NULL OR A.UpdateTime >= @3)
                                           AND (@4 IS NULL OR A.UpdateTime<=@4))
                                ) tb";

            var dataList = Context.Select<CBUnAdvertisementLog>("tb.*").From(sql);
            var paras = new object[]
                {
                    filter.IsValid,
                    filter.AdvertisementType,
                    filter.UnionSiteSysNo,
                    filter.StartTime,
                    filter.EndTime
                };
            dataList.Parameters(paras);
            return dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany();
        }
    }
}
