using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.Front;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 文章类型管理数据层实现类
    /// </summary>
    /// <remarks>2013－06-17 杨晗 创建</remarks>from fearticle
    public class FeArticleCategoryDaoImpl : IFeArticleCategoryDao
    {
        /// <summary>
        /// 根据文章类型编号获取实体
        /// </summary>
        /// <param name="sysNo">文章类型系统编号</param>
        /// <returns>文章类型实体</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override FeArticleCategory GetModel(int sysNo)
        {
            return Context.Sql(@"select * from fearticlecategory where SysNO = @0", sysNo)
                          .QuerySingle<FeArticleCategory>();
        }

        /// <summary>
        /// 判断文章分类标题是否重复
        /// </summary>
        /// <param name="title">文章分类标题</param>
        /// <param name="type">文章分类类型</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public override bool FeArticleCategoryVerify(string title, ForeStatus.文章分类类型 type)
        {
            string sql = @"Name=@title and type=@type and status<>@status";
            int countBuilder = Context.Select<int>("count(1)")
                                     .From("fearticlecategory")
                                     .Where(sql)
                                     .Parameter("title", title)
                                     .Parameter("type", type)
                                     .Parameter("status", ForeStatus.文章分类状态.作废)
                                     .QuerySingle();
            return countBuilder > 0;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="type">文章类型</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchName">分类名称</param>
        /// <param name="count">抛出总条数</param>
        /// <returns>文章类型列表</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override IList<FeArticleCategory> Seach(int pageIndex, int pageSize, ForeStatus.文章分类类型 type,
                                                       int searchStaus, string searchName, out int count)
        {
            #region sql条件

            string sql = @"(@Name is null or Name like @Name1)
                       and (@Status=0 or Status =@Status1) 
                       and (@Type=0 or Type=@Type1)";

            #endregion
            using (var context = Context.UseSharedConnection(true))
            {
                count = context.Select<int>("count(1)")
                               .From("fearticlecategory ft")
                               .Where(sql)
                               .Parameter("Name", searchName)
                               .Parameter("Name1", "%" + searchName + "%")
                               .Parameter("Status", searchStaus)
                               .Parameter("Status1", searchStaus)
                               .Parameter("Type", (int)type)
                               .Parameter("Type1", (int)type)
                               .QuerySingle();

                var countBuilder = context.Select<FeArticleCategory>("ft.*")
                                          .From("fearticlecategory ft")
                                          .Where(sql)
                                          .Parameter("Name", searchName)
                                          .Parameter("Name1", "%" + searchName + "%")
                                          .Parameter("Status", searchStaus)
                                          .Parameter("Status1", searchStaus)
                                          .Parameter("Type", (int)type)
                                          .Parameter("Type1", (int)type)
                                          .Paging(pageIndex, pageSize).OrderBy("DISPLAYORDER desc").QueryMany();
                return countBuilder;
            }
        }

        /// <summary>
        /// 根据文章类型获取所有文章分类
        /// </summary>
        /// <param name="type">文章分类类型</param>
        /// <returns>文章分类集合</returns>
        /// <remarks>2013－06-19 杨晗 创建</remarks>
        public override IList<FeArticleCategory> GetAll(ForeStatus.文章分类类型 type)
        {
            #region sql条件
            string sql = @"Type=@Type and Status=@Status";
            #endregion

            var countBuilder = Context.Select<FeArticleCategory>("ft.*")
                                      .From("fearticlecategory ft")
                                      .Where(sql)
                                      .Parameter("Type", (int)type)
                                      .Parameter("Status", (int)ForeStatus.文章分类状态.已审)
                                      .OrderBy("DISPLAYORDER").QueryMany();
            return countBuilder;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override int Insert(FeArticleCategory model)
        {
            int id = Context.Insert<FeArticleCategory>("fearticlecategory", model)
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
        public override int Update(FeArticleCategory model)
        {
            int rowsAffected = Context.Update<FeArticleCategory>("fearticlecategory", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where(x => x.SysNo)
                                      .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章类型主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public override bool Delete(int id)
        {
            int rowsAffected = Context.Delete("fearticlecategory")
                                      .Where("Sysno", id)
                                      .Execute();
            return rowsAffected > 0;
        }
    }
}
