using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Front;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 文章详情数据层实现类
    /// </summary>
    /// <remarks>2013－06-17 杨晗 创建</remarks>
    public class FeArticleDaoImpl : IFeArticleDao
    {
        /// <summary>
        /// 根据文章详情编号获取实体
        /// </summary>
        /// <param name="id">文章详情编号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override FeArticle GetModel(int id)
        {
            return
                Context.Sql(@"select * from fearticle where SysNO = @0", id).QuerySingle<FeArticle>();
        }

        /// <summary>
        /// 判断文章标题是否重复
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public override bool FeArticleVerify(string title)
        {
            string sql = @"title=@title";
            int countBuilder = Context.Select<int>("count(1)")
                                     .From("fearticle")
                                     .Where(sql)
                                     .Parameter("title", title)
                                     .QuerySingle();
            return countBuilder > 0;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="ids">类别系统号集合</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchName">文章标题名称</param>
        /// <param name="count">总条数</param>
        /// <returns>文章列表</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override IList<CBFeArticle> Seach(ParaArticleFilter filter, out int count)
        {
            #region sql条件
            var sqlWhere = "1=1";

            //判断是否绑定所有分销商
            if (!filter.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (filter.IsBindDealer)
                {
                    sqlWhere += " and d.SysNo = @DealerSysNo";
                }
                else
                {
                    sqlWhere += " and d.CreatedBy = @DealerCreatedBy";
                }
            }
            if (filter.SelectedDealerSysNo != -1)
            {
                sqlWhere += " and d.SysNo = " + filter.SelectedDealerSysNo;
            }
            string sql = @"(@title is null or ft.title like @title1) and (ft.categorysysno in {0})  
                        and (@Status=0 or ft.Status =@Status1) and " + sqlWhere ;

            #endregion

            if (!filter.ids.Any())
                filter.ids.Add(0);
            sql = string.Format(sql, "(" + string.Join(",", filter.ids) + ")");
            count = Context.Select<int>("count(1)")
                           .From("fearticle ft left join DsDealer d on ft.DealerSysNo = d.SysNo")
                           .Where(sql)
                           .Parameter("title", filter.searchName)
                           .Parameter("title1", "%" + filter.searchName + "%")
                           .Parameter("Status", filter.searchStaus)
                           .Parameter("Status1", filter.searchStaus)
                           .Parameter("DealerSysNo", filter.DealerSysNo)
                           .Parameter("DealerCreatedBy", filter.DealerCreatedBy)
                           .QuerySingle();
                           //.OrderBy("ft.lastupdatedate desc").QuerySingle();

            var countBuilder =
                Context.Select<CBFeArticle>(
                    "ft.*, (select username from syuser where sysno=ft.createdby) as createdbyname,(select username from syuser where sysno=ft.lastupdateby) as lastupdatebyname,d.DealerName")
                       .From("fearticle ft left join DsDealer d on ft.DealerSysNo = d.SysNo")
                       .Where(sql)
                       .Parameter("title", filter.searchName)
                       .Parameter("title1", "%" + filter.searchName + "%")
                       .Parameter("Status", filter.searchStaus)
                       .Parameter("Status1", filter.searchStaus)
                       .Parameter("DealerSysNo", filter.DealerSysNo)
                       .Parameter("DealerCreatedBy", filter.DealerCreatedBy)
                       .Paging((int)filter.pageIndex, filter.pageSize).OrderBy("ft.lastupdatedate desc").QueryMany();
            return countBuilder;
        }

        /// <summary>
        /// 根据文章分类系统号获取文章集合
        /// </summary>
        /// <param name="sysNo">文章分类系统号</param>
        /// <param name="recordNum">记录条数</param>
        /// <returns>文章集合</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        /// <remarks>2013-09-25 邵斌 修改：指定记录条数</remarks>
        public override IList<FeArticle> GetListByCategory(int sysNo, int? recordNum)
        {
            #region sql条件

            string sql = @"(@categorysysno=0 or categorysysno=@categorysysno) and Status=@Status";

            #endregion

            IList<FeArticle> countBuilder;

            if (recordNum.HasValue)
            {
                countBuilder = Context.Select<FeArticle>("ft.*")
                                          .From("fearticle ft")
                                          .Where(sql)
                                          .Parameter("categorysysno", sysNo)
                                          //.Parameter("categorysysno", sysNo)
                                          .Parameter("Status", (int)ForeStatus.文章状态.已审)
                                          .OrderBy("lastupdatedate desc")
                                          .Paging(1, recordNum.Value)
                                          .QueryMany();
            }
            else
            {
                countBuilder = Context.Select<FeArticle>("ft.*")
                                          .From("fearticle ft")
                                          .Where(sql)
                                          .Parameter("categorysysno", sysNo)
                                         // .Parameter("categorysysno", sysNo)
                                          .Parameter("Status", (int)ForeStatus.文章状态.已审)
                                          .OrderBy("lastupdatedate desc")
                                          .QueryMany();
            }

            return countBuilder;
        }

        /// <summary>
        /// 根据文章分类系统号获取文章集合
        /// </summary>
        /// <param name="sysNo">文章分类系统号CBFeArticle</param>
        /// <returns>文章集合</returns>
        /// <remarks>2013－09-23 周瑜 创建</remarks>
        public override IList<CBFeArticle> GetArticleListByCategory(int sysNo)
        {

            var countBuilder = Context.Select<CBFeArticle>("a.*,b.username as CreatedByName ")
                                      .From("fearticle a inner join syuser b on a.createdby = b.sysno")
                                      .Where("(@categorysysno=0 or a.categorysysno=@categorysysno)")
                                      //.Parameter("categorysysno", sysNo)
                                      .Parameter("categorysysno", sysNo)
                                      .OrderBy("a.lastupdatedate desc").QueryMany();
            return countBuilder;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override int Insert(FeArticle model)
        {
            int id = Context.Insert<FeArticle>("fearticle", model)
                            .AutoMap(x => x.SysNo)
                            .ExecuteReturnLastId<int>("Sysno");
            return id;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override int Update(FeArticle model)
        {
            int rowsAffected = Context.Update<FeArticle>("fearticle", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override bool Delete(int id)
        {
            int rowsAffected = Context.Delete("fearticle")
                                      .Where("Sysno", id)
                                      .Execute();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 根据分类编号获取特定状态的全部文章列表
        /// </summary>
        /// <param name="categroySysNo">分类编号</param>
        /// <param name="status">状态</param>
        /// <returns>文章列表</returns>
        /// <remarks>2013-09-28 黄波 创建</remarks>
        /// <remarks>2013-10-28 何方 去除  a inner join syuser b on a.createdby = b.sysno,避免用户被删除后不能显示文章 </remarks>
        public override IList<CBFeArticle> GetArticle(int categroySysNo, ForeStatus.文章状态 status)
        {
            var countBuilder = Context.Select<CBFeArticle>("a.*")
                                      .From("fearticle a")
                                      .Where("a.categorysysno=@categorysysno and a.status=@status")
                                      .Parameter("categorysysno", categroySysNo)
                                      .Parameter("status", (int)status)
                                      .OrderBy("a.lastupdatedate desc").QueryMany();
            return countBuilder;
        }
    }
}
