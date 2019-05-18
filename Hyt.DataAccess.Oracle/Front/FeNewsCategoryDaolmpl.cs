using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Front;
using Hyt.Model;
using Hyt.Util;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Front
{

    /// <summary>
    /// 新闻分类维护管理
    /// </summary>
    /// <remarks>2016-06-06 罗远康 创建</remarks>
    public class FeNewsCategoryDaolmpl : IFeNewsCategoryDao
    {
        /// <summary>
        /// 添加新闻分类
        /// </summary>
        /// <param name="category">新闻分类实体</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override bool CreateCategory(FeNewsCategory category)
        {
            //接收新添加新闻分类的系统编号
            int result = 0;

            //启动事务
            using (var _context = Context.UseSharedConnection(true))
            {
                //添加新分类
                result = _context.Insert<FeNewsCategory>("FeNewsCategory", category)
                                    .AutoMap(c => c.SysNo, c => c.ParentCategory)
                                    .ExecuteReturnLastId<int>("SysNo");   //记录返还系统编号
                category.SysNo = result;

                //通过系统编号来判断是否添加成功
                if (result > 0)
                {
                    //设置分类路由
                    SetCateorySysNosBySysNo(category.SysNo);
                }
                else 
                {
                    return false;
                }
                //设置新添加的新闻分类到他父级分类末尾显示
                SetCategoryToLastShow(category, _context);

            }
            return true;
        }
        /// <summary>
        /// 修改新闻分类
        /// </summary>
        /// <param name="category">新闻分类实体</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override bool EditCategory(FeNewsCategory category)
        {
            //接收新添加新闻分类的系统编号
            int result = 0;
            using (var _context = Context.UseSharedConnection(true))
            {
                result = _context.Sql(@"UPDATE FeNewsCategory SET 
                                            CategoryImage = @CategoryImage
                                            ,CategoryName = @CategoryName
                                            ,DealerSysNo= @DealerSysNo
                                            ,ParentSysNo= @ParentSysNo
                                            ,Remarks= @Remarks
                                            ,SeoDescription=@SeoDescription
                                            ,SeoKeyword=@SeoKeyword
                                            ,SeoTitle=@SeoTitle
                                            ,LastUpdateBy=@LastUpdateBy
                                            ,LastUpdateDate=@LastUpdateDate 
                                            ,SysNos=@SysNos
                                            where SysNo = @SysNo")
                                    .Parameter("CategoryImage", category.CategoryImage)
                                    .Parameter("CategoryName", category.CategoryName)
                                    .Parameter("DealerSysNo", category.DealerSysNo)
                                    .Parameter("ParentSysNo", category.ParentSysNo)
                                    .Parameter("Remarks", category.Remarks)
                                    .Parameter("SeoDescription", category.SeoDescription)
                                    .Parameter("SeoKeyword", category.SeoKeyword)
                                    .Parameter("SeoTitle", category.SeoTitle)
                                    .Parameter("LastUpdateBy", category.LastUpdateBy)
                                    .Parameter("LastUpdateDate", category.LastUpdateDate)
                                    .Parameter("SysNos", category.SysNos)
                                    .Parameter("SysNo", category.SysNo)
                                    .Execute();
            }

            return result > 0;
        }
        /// <summary>
        /// 获取新的系统编号路由
        /// </summary>
        /// <param name="newParentSysNo">新的父级系统编号</param>
        /// <param name="sysNo">系统编号</param>
        /// <returns>新的系统编号路由</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override string GetNewSysNos(int newParentSysNo, int sysNo)
        {
            string newSysNos = "";
            if (newParentSysNo != 0)
                newSysNos = string.Format("{0}{1},", GetCategory(newParentSysNo).SysNos, sysNo);
            else
                newSysNos = string.Format(",{0},", sysNo);
            return newSysNos;
        }
        /// <summary>
        /// 读取所以新闻分类包括无效分类
        /// </summary>
        /// <returns>返回新闻分类列表</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override IList<FeNewsCategory> GetAllCategory()
        {
            //读取新闻分类
            var categoryList = Context.Select<FeNewsCategory>("*")
                                      .From("FeNewsCategory")
                                      .OrderBy("DisplayOrder ASC")
                                      .QueryMany();
            //返回结果
            return categoryList;
        }

        /// <summary>
        /// 读取所以有效的新闻分类,可以读取指定的新闻分类类别
        /// </summary>
        /// <param name="sysNo">父节点新闻分类编号</param>
        /// <returns>返回新闻分类列表</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override IList<FeNewsCategory> GetCategoryList(int? sysNo)
        {
            //读取列表，但只读取有效的分类

            var categoryList = Context.Select<FeNewsCategory>("*")
                                        .From("FeNewsCategory")
                                        .Where("(@SysNo is null or SysNo =@SysNo) and Status=@Status")
                                        .OrderBy("displayorder asc")
                                        .Parameter("SysNo", sysNo)
                                        .Parameter("Status", (int)NewsStatus.新闻分类状态.有效)
                                        .QueryMany();

            //返回结果
            return categoryList;
        }

        /// <summary>
        /// 获取单个新闻分类
        /// </summary>
        /// <param name="sysNo">新闻分类系统编号</param>
        /// <param name="isGetParent">是否获取父级新闻分类对象</param>
        /// <param name="context">共用数据库操作上下文</param>
        /// <returns>返回单个新闻分类</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override FeNewsCategory GetCategory(int sysNo, bool isGetParent = false, IDbContext context = null)
        {
            FeNewsCategory category;
            context = context ?? Context.UseSharedConnection(true);
            using (context)
            {

                category = context.Select<FeNewsCategory>("*")
                              .From("FeNewsCategory")
                              .Where("SysNo = @SysNo")
                              .Parameter("SysNo", sysNo)
                              .QuerySingle();

                //由于新闻分类时无限极分类所以使用递归来查找上级分类
                //获取分类的父级分类对象
                if (isGetParent && category != null && category.ParentSysNo != 0)
                {
                    category.ParentCategory = GetCategory(category.ParentSysNo, isGetParent, context);
                }
            }

            return category;
        }

        /// <summary>
        /// 更新新闻分类状态
        /// </summary>
        /// <param name="sysNo">新闻分类系统编号</param>
        /// <param name="status">要变更的状态</param>
        /// <param name="adminSysNo">变更人</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override bool ChangeStatus(int sysNo, NewsStatus.新闻分类状态 status, int adminSysNo)
        {
            var result = (Context.Sql(
                 "UPDATE FeNewsCategory SET Status=@Status,LastUpdateBy=@UserSysNo,LastUpdateDate=@UpdateDate WHERE sysnos  LIKE '%," + sysNo + ",%' ")
                     .Parameter("Status", status)
                     .Parameter("UserSysNo", adminSysNo)
                     .Parameter("UpdateDate", DateTime.Now)
                     .Execute() > 0);

            return result;
        }

        /// <summary>
        /// 保存分类信息
        /// </summary>
        /// <param name="updateCategory">分类实体对象</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override bool Update(FeNewsCategory updateCategory)
        {
            return Context.Sql(@"UPDATE FeNewsCategory SET 
                                            CategoryName = @CategoryName
                                            ,ParentSysNo=@ParentSysNo
                                            ,SeoDescription=@SeoDescription
                                            ,SeoKeyword=@SeoKeyword
                                            ,SeoTitle=@SeoTitle
                                            ,LastUpdateBy=@LastUpdateBy
                                            ,LastUpdateDate=@LastUpdateDate 
                                            ,DisplayOrder=@DisplayOrder 
                                            where SysNo =@SysNo")
                            .Parameter("CategoryName", updateCategory.CategoryName)
                            .Parameter("ParentSysNo", updateCategory.ParentSysNo)
                            .Parameter("SeoDescription", updateCategory.SeoDescription)
                            .Parameter("SeoKeyword", updateCategory.SeoKeyword)
                            .Parameter("SeoTitle", updateCategory.SeoTitle)
                            .Parameter("LastUpdateBy", updateCategory.LastUpdateBy)
                            .Parameter("LastUpdateDate", updateCategory.LastUpdateDate)
                            .Parameter("DisplayOrder", updateCategory.DisplayOrder)
                            .Parameter("SysNo", updateCategory.SysNo)
                            .Execute() > 0;

        }

        /// <summary>
        /// 设置分类系统编号路由
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override bool SetCateorySysNosBySysNo(int sysNo)
        {
            return Context.Sql("UPDATE FeNewsCategory SET SysNos=ISNULL(SysNos,',')+CONVERT(nvarchar(200),sysNo)+',' where sysNo=@sysNo")
                .Parameter("sysNo", sysNo).Execute() > 0;
        }

        /// <summary>
        /// 更新子分类系统编码路由
        /// </summary>
        /// <param name="oldParentSysNos">原父级系统编码路由</param>
        /// <param name="newParentSysNos">新父级系统编码路由</param>
        /// <param name="oldParentSysNo">原父级系统编码</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override bool UpdateChildrenSysNos(string oldParentSysNos, string newParentSysNos, int oldParentSysNo)
        {
            return Context.Sql("UPDATE FeNewsCategory SET SysNos =replace(SysNos,@oldParentSysNos,@newParentSysNos) WHERE SysNos LIKE '%," + oldParentSysNo + ",%' AND sysNo<>@oldParentSysNo")
              .Parameter("oldParentSysNos", oldParentSysNos)
              .Parameter("newParentSysNos", newParentSysNos)
              .Parameter("oldParentSysNo", oldParentSysNo)
              .Execute() > 0;
        }
        /// <summary>
        /// 将分类排在父分类的末尾显示
        /// </summary>
        /// <param name="category">新闻分类对象</param>
        /// <param name="context">供应数据库操作上线文</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public override bool SetCategoryToLastShow(FeNewsCategory category, IDbContext context = null)
        {
            context = context ?? Context;

            return
                context.Sql(
                    @"UPDATE FeNewsCategory SET displayorder = (select COUNT(sysno)+1 FROM FeNewsCategory WHERE parentsysno=@parentSysNo and sysno<>@sysno) where sysno=@sysno")
                       .Parameter("parentSysNo", category.ParentSysNo)
                       .Parameter("sysno", category.SysNo)
                       .Execute() > 0;

        }
    }
}
