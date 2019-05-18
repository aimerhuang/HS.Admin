using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 文章类型管理接口抽象类
    /// </summary>
    /// <remarks>2013－06-17 杨晗 创建</remarks>
    public abstract class IFeArticleCategoryDao : DaoBase<IFeArticleCategoryDao>
    {
        /// <summary>
        /// 根据文章类型编号获取实体
        /// </summary>
        /// <param name="id">文章类型编号</param>
        /// <returns>文章类型实体</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract FeArticleCategory GetModel(int id);

        /// <summary>
        /// 判断文章分类标题是否重复
        /// </summary>
        /// <param name="title">文章分类标题</param>
        /// <param name="type">文章分类类型</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public abstract bool FeArticleCategoryVerify(string title, ForeStatus.文章分类类型 type);

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
        public abstract IList<FeArticleCategory> Seach(int pageIndex, int pageSize, ForeStatus.文章分类类型 type,
                                                       int searchStaus, string searchName, out int count);

        /// <summary>
        /// 根据文章类型获取所有文章分类
        /// </summary>
        /// <param name="type">文章分类类型</param>
        /// <returns>文章分类集合</returns>
        /// <remarks>2013－06-19 杨晗 创建</remarks>
        public abstract IList<FeArticleCategory> GetAll(ForeStatus.文章分类类型 type);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract int Insert(FeArticleCategory model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract int Update(FeArticleCategory model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章类型主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract bool Delete(int id);
    }
}
