using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;


namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 新闻分类接口
    /// </summary>
    /// <remarks>2016-06-06 罗远康 创建</remarks>
    public abstract class IFeNewsCategoryDao : DaoBase<IFeNewsCategoryDao>
    {
        /// <summary>
        /// 添加新闻分类
        /// </summary>
        /// <param name="category">新闻分类实体</param>
        /// <param name="attributeGroupAso">新闻分类对应属性组</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract bool CreateCategory(FeNewsCategory category);

        /// <summary>
        /// 修改新闻分类
        /// </summary>
        /// <param name="category">新闻分类实体</param>
        /// <param name="attributeGroupAso">新闻分类对应属性组</param>
        /// <returns>返回操作是否成功 true:成功   false:不成功</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract bool EditCategory(FeNewsCategory category);

        /// 获取新的系统编号路由
        /// </summary>
        /// <param name="newParentSysNo">新的父级系统编号</param>
        /// <param name="sysNo">系统编号</param>
        /// <returns>新的系统编号路由</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract string GetNewSysNos(int newParentSysNo, int sysNo);

        /// <summary>
        /// 读取所以新闻分类包括无效分类
        /// </summary>
        /// <returns>返回新闻分类列表</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract IList<FeNewsCategory> GetAllCategory();

        /// <summary>
        /// 读取所以有效的新闻分类,可以读取指定的新闻分类类别
        /// </summary>
        /// <param name="sysNo">父节点新闻分类编号</param>
        /// <returns>返回新闻分类列表</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract IList<FeNewsCategory> GetCategoryList(int? sysNo);

        /// <summary>
        /// 获取单个新闻分类
        /// </summary>
        /// <param name="sysNo">新闻分类系统编号</param>
        /// <param name="isGetParent">是否获取父级新闻分类对象</param>
        /// <param name="context">共用数据库操作上下文</param>
        /// <returns>返回单个新闻分类</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract FeNewsCategory GetCategory(int sysNo, bool isGetParent = false, IDbContext context = null);

        /// <summary>
        /// 更新新闻分类状态
        /// </summary>
        /// <param name="sysNo">新闻分类系统编号</param>
        /// <param name="status">要变更的状态</param>
        /// <param name="adminSysNo">管理员账号，记录变更记录</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract bool ChangeStatus(int sysNo, NewsStatus.新闻分类状态 status, int adminSysNo);

        /// <summary>
        /// 保存分类信息
        /// </summary>
        /// <param name="updateCategory">分类实体对象</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract bool Update(FeNewsCategory updateCategory);

        /// <summary>
        /// 设置分类系统编号路由
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract bool SetCateorySysNosBySysNo(int sysNo);

        /// <summary>
        /// 更新子分类系统编码路由
        /// </summary>
        /// <param name="oldParentSysNos">原父级系统编码路由</param>
        /// <param name="newParentSysNos">新父级系统编码路由</param>
        /// <param name="oldParentSysNo">原父级系统编码</param>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract bool UpdateChildrenSysNos(string oldParentSysNos, string newParentSysNos, int oldParentSysNo);

        /// <summary>
        /// 将分类排在父分类的末尾显示
        /// </summary>
        /// <param name="category">新闻分类对象</param>
        /// <param name="context">供应数据库操作上线文</param>
        /// <returns></returns>
        /// <returns>返回： true 成功 false 失败</returns>
        /// <remarks>2016-06-06 罗远康 创建</remarks>
        public abstract bool SetCategoryToLastShow(FeNewsCategory category, IDbContext context = null);

    }
}
