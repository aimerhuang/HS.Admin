using System;
using System.Collections.Generic;
using Hyt.DataAccess.Front;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Front
{
    /// <summary>
    /// 搜索关键字数据层实现类
    /// </summary>
    /// <remarks>2013－06-27 杨晗 创建</remarks>
    public class FeSearchKeywordDaoImpl : IFeSearchKeywordDao
    {
        /// <summary>
        ///     根据搜索关键字系统号获取实体
        /// </summary>
        /// <param name="sysNo">搜索关键字系统号</param>
        /// <returns>搜索关键字实体</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public override FeSearchKeyword GetModel(int sysNo)
        {
            return
                Context.Sql(@"select * from fesearchkeyword where SysNO = @0", sysNo).QuerySingle<FeSearchKeyword>();
        }

        /// <summary>
        /// 判断搜索关键字是否重复
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public override bool FeSearchKeywordVerify(string keyword)
        {
            string sql = @"keyword=@keyword";
            int countBuilder = Context.Select<int>("count(1)")
                                     .From("fesearchkeyword")
                                     .Where(sql)
                                     .Parameter("keyword", keyword)
                                     .QuerySingle();
            return countBuilder > 0;
        }

        /// <summary>
        ///     搜索关键字分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="status">状态</param>
        /// <param name="hitsCount">点击次数</param>
        /// <param name="createdDateStart">创建时间</param>
        /// <param name="createdDateEnd">创建时间</param>
        /// <param name="searchName">文章标题名称</param>
        /// <returns>文章列表</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        /// <remarks>2016-03-22 罗远康 修改</remarks>
        public override IList<FeSearchKeyword> Seach(int pageIndex, int pageSize,
                                                     int? status, int? hitsCount,
                                                     DateTime? createdDateStart,
                                                     DateTime? createdDateEnd, string searchName = null)
        {
            #region sql条件

            string sql = @"(@keyword is null or keyword like @keyword1)  
                        and (@Status=-1 or Status =@Status) and ((@hitscount is null or @hitscount=0) or hitscount>=@hitscount)
                        and(@createdDateStart is null or createdDate>=@createdDateStart) and (@createdDateEnd is null or createdDate<=@createdDateEnd)";

            #endregion

            //if (createdDateStart == null || createdDateStart == DateTime.MinValue)
            //{
            //    createdDateStart = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            //}
            //if (createdDateEnd == null || createdDateEnd == DateTime.MinValue)
            //{
            //    createdDateEnd = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            //}
            status = status ?? -1;
            List<FeSearchKeyword> countBuilder = Context.Select<FeSearchKeyword>("fk.*")
                                                        .From("fesearchkeyword fk")
                                                        .Where(sql)
                                                        .Parameter("keyword", searchName)
                                                        .Parameter("keyword1", "%" + searchName + "%")
                                                        .Parameter("Status", (int)status)
                                                        .Parameter("hitscount", hitsCount)
                                                        .Parameter("createdDateStart", createdDateStart)
                                                        .Parameter("createdDateEnd", createdDateEnd)
                                                        .Paging(pageIndex, pageSize)
                                                        .OrderBy("fk.hitscount desc")
                                                        .QueryMany();
            return countBuilder;
        }

        /// <summary>
        ///     根据条件获取文章的总条数
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="hitsCount">点击次数</param>
        /// <param name="createdDateStart">创建时间</param>
        /// <param name="createdDateEnd">创建时间</param>
        /// <param name="searchName">文章标题名称</param>
        /// <returns>总数</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public override int GetCount(int? status, int? hitsCount, DateTime? createdDateStart,
                                     DateTime? createdDateEnd, string searchName = null)
        {
            #region sql条件

            string sql = @"(@keyword is null or keyword like @keyword1)  
                        and (@Status=-1 or Status =@Status) and ((@hitscount is null or @hitscount=0) or hitscount>=@hitscount)
                        and(@createdDateStart is null or createdDate>=@createdDateStart) and (@createdDateEnd is null or createdDate<=@createdDateEnd)";

            #endregion

            if (createdDateStart == null || createdDateStart == DateTime.MinValue)
            {
                createdDateStart = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            if (createdDateEnd == null || createdDateEnd == DateTime.MinValue)
            {
                createdDateEnd = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            status = status ?? -1;
            int countBuilder = Context.Select<int>("count(1)")
                                      .From("fesearchkeyword")
                                      .Where(sql)
                                      .Parameter("keyword", searchName)
                                      .Parameter("keyword1", "%" + searchName + "%")
                                      .Parameter("Status", (int)status)
                                      .Parameter("hitscount", hitsCount)
                                      .Parameter("createdDateStart", createdDateStart)
                                      .Parameter("createdDateEnd", createdDateEnd)
                                      .QuerySingle();
            return countBuilder;
        }

        /// <summary>
        ///     插入搜索关键字
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public override int Insert(FeSearchKeyword model)
        {
            return Context.Insert("fesearchkeyword", model)
                          .AutoMap(x => x.SysNo)
                          .ExecuteReturnLastId<int>("Sysno");
        }

        /// <summary>
        ///     更新搜索关键字
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public override int Update(FeSearchKeyword model)
        {
            return Context.Update("fesearchkeyword", model)
                          .AutoMap(x => x.SysNo)
                          .Where(x => x.SysNo)
                          .Execute();
        }

        /// <summary>
        ///     删除搜索关键字
        /// </summary>
        /// <param name="sysNo">搜索关键字系统号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        public override bool Delete(int sysNo)
        {
            int rowsAffected = Context.Delete("fesearchkeyword")
                                      .Where("Sysno", sysNo)
                                      .Execute();
            return rowsAffected > 0;
        }
    }
}