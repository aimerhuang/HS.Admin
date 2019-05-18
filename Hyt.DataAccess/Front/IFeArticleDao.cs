using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Front
{
    /// <summary>
    /// 文章列表管理接口抽象类
    /// </summary>
    /// <remarks>2013－06-19 杨晗 创建</remarks>
    public abstract class IFeArticleDao : DaoBase<IFeArticleDao>
    {
        /// <summary>
        /// 根据文章详情编号获取实体
        /// </summary>
        /// <param name="id">文章详情编号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract FeArticle GetModel(int id);

        /// <summary>
        /// 判断文章标题是否重复
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public abstract bool FeArticleVerify(string title);

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
        public abstract IList<CBFeArticle> Seach(ParaArticleFilter filter, out int count);

        /// <summary>
        /// 根据文章分类系统号获取文章集合
        /// </summary>
        /// <param name="sysNo">文章分类系统号</param>
        /// <param name="recordNum">记录条数</param>
        /// <returns>文章集合</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        /// <remarks>2013-09-25 邵斌 修改：指定记录条数</remarks>
        public abstract IList<FeArticle> GetListByCategory(int sysNo, int? recordNum = null);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract int Insert(FeArticle model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract int Update(FeArticle model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public abstract bool Delete(int id);

        /// <summary>
        /// 根据文章分类系统号获取文章集合
        /// </summary>
        /// <param name="sysNo">文章分类系统号CBFeArticle</param>
        /// <returns>文章集合</returns>
        /// <remarks>2013－09-23 周瑜 创建</remarks>
        public abstract IList<CBFeArticle> GetArticleListByCategory(int sysNo);

        /// <summary>
        /// 根据分类编号获取特定状态的全部文章列表
        /// </summary>
        /// <param name="categroySysNo">分类编号</param>
        /// <param name="status">状态</param>
        /// <returns>文章列表</returns>
        /// <remarks>2013-09-28 黄波 创建</remarks>
        public abstract IList<CBFeArticle> GetArticle(int categroySysNo,ForeStatus.文章状态 status);
    }
}
