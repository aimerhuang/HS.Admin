using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Union;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Union
{
    /// <summary>
    /// 联盟广告
    /// </summary>
    /// <remarks>2013-10-14 吴文强 创建</remarks>
    public class UnAdvertisementDaoImpl : IUnAdvertisementDao
    {
        /// <summary>
        /// 根据系统编号获取联盟广告
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>联盟广告</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public override UnAdvertisement GetModel(int sysNo)
        {
            string sql = @"
                            select * from UnAdvertisement a 
                            where a.sysno= @sysno
                        ";
            var model = Context.Sql(sql)
                                     .Parameter("sysno", sysNo)
                                     .QuerySingle<UnAdvertisement>();
            return model;
        }

        /// <summary>
        /// 根据来源Url和访问Url获取联盟广告
        /// </summary>
        /// <param name="urlReferrer">来源Url</param>
        /// <param name="accessUrl">访问Url</param>
        /// <returns>联盟广告</returns>
        /// <remarks>2013-10-15 吴文强 创建</remarks>
        public override UnAdvertisement GetModel(string urlReferrer, string accessUrl)
        {
            string sql = @"
                            select adv.* from UnUnionSite us
                            inner join UnAdvertisement adv on us.sysno = adv.unionsitesysno
                            where charindex(lower(@urlReferrer),lower(us.siteurl))>0
                              and charindex(lower(@accessUrl),lower(adv.accessurl))>0
                        ";
            var model = Context.Sql(sql)
                                     .Parameter("urlReferrer", urlReferrer)
                                     .Parameter("accessUrl", accessUrl)
                                     .QuerySingle<UnAdvertisement>();
            return model;
        }

        /// <summary>
        /// 创建联盟广告
        /// </summary>
        /// <param name="model">联盟广告实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override int Create(UnAdvertisement model)
        {
            return Context.Insert<UnAdvertisement>("UnAdvertisement", model)
                .AutoMap(x => x.SysNo, x => x.Products, x => x.ItemList)
                .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 修改联盟广告
        /// </summary>
        /// <param name="model">联盟广告实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override int Update(UnAdvertisement model)
        {
            return Context.Update<UnAdvertisement>("UnAdvertisement", model)
                .AutoMap(x => x.SysNo, x => x.Products, x => x.ItemList)
                .Where(x => x.SysNo)
                .Execute();
        }

        /// <summary>
        /// 根据商品系统编号获取Cps商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>Cps商品</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public override UnCpsProduct GetCpsProduct(int productSysNo)
        {
            string sql = @"
                            select * from UnCpsProduct a 
                            where a.productSysNo= @productSysNo
                        ";
            var model = Context.Sql(sql)
                                     .Parameter("productSysNo", productSysNo)
                                     .QuerySingle<UnCpsProduct>();
            return model;
        }

        /// <summary>
        /// 通过条件获取联盟广告列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>联盟广告列表</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public override Pager<UnAdvertisement> GetAdvertisementList(Model.Parameter.ParaUnAdvertisementFilter filter)
        {
            const string sql = @"(SELECT A.*
                                          FROM UnAdvertisement A
                                         WHERE 1=1
                                                AND (@0 IS NULL OR A.STATUS = @0)
                                                AND (@1 IS NULL OR A.UnionSiteSysNo = @1)
                                                AND (@2 IS NULL OR A.AdvertisementType = @2)
                                ) tb";

            var dataList = Context.Select<UnAdvertisement>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var paras = new object[]
                {
                    filter.Status,
                    filter.UnionSiteSysNo,
                    filter.AdvertisementType,
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);
            var pager = new Pager<UnAdvertisement>
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }
    }
}
