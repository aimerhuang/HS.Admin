using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.DataAccess.Front;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
namespace Hyt.BLL.Front
{
    /// <summary>
    /// 文章管理业务逻辑层
    /// </summary>
    /// <remarks>2013－06-17 杨晗 创建</remarks>
    public class FeArticleBo : BOBase<FeArticleBo>
    {
        /// <summary>
        /// 根据文章详情编号获取实体
        /// </summary>
        /// <param name="id">文章详情编号</param>
        /// <returns>文章实体</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public FeArticle GetModel(int id)
        {
            return IFeArticleDao.Instance.GetModel(id);
        }

        /// <summary>
        /// 判断文章标题是否重复
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public bool FeArticleVerify(string title)
        {
            return IFeArticleDao.Instance.FeArticleVerify(title);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="ids">类别系统号集合</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchName">文章标题名称</param>
        /// <returns>文章列表</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public PagedList<CBFeArticle> Seach(ParaArticleFilter filter)
        {
            filter.pageIndex = filter.pageIndex ?? 1;
            filter.searchStaus = filter.searchStaus ?? 0;
            var model = new PagedList<CBFeArticle>();
            filter.pageSize = model.PageSize;
            int count;
            var list = IFeArticleDao.Instance.Seach(filter, out count);
            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int)filter.pageIndex;
            return model;
        }
        /// <summary>
        /// 获取全部已通过审核的文章列表
        /// </summary>
        /// <param name="categroySysNo">分类编号</param>
        /// <returns>已审核的文章列表</returns>
        public IList<CBFeArticle> GetPassArticle(int categroySysNo)
        {
            return IFeArticleDao.Instance.GetArticle(categroySysNo, ForeStatus.文章状态.已审);
        }
        /// <summary>
        /// 根据文章分类系统号获取文章集合
        /// </summary>
        /// <param name="sysNo">文章分类系统号</param>
        /// <returns>文章集合</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public IList<FeArticle> GetListByCategory(int sysNo)
        {
            return IFeArticleDao.Instance.GetListByCategory(sysNo);
        }
        /// <summary>
        /// 根据文章分类系统号获取文章集合
        /// </summary>
        /// <param name="sysNo">文章分类系统号</param>
        /// <returns>文章集合</returns>
        /// <remarks>2013－09-23 周瑜 杨晗 创建</remarks>
        public IList<CBFeArticle> GetArticleListByCategory(int sysNo)
        {
            return IFeArticleDao.Instance.GetArticleListByCategory(sysNo);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public int Insert(FeArticle model)
        {
            int i = IFeArticleDao.Instance.Insert(model);
           
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "新增新闻/帮助文章"+model.Title, LogStatus.系统日志目标类型.新闻帮助管理, i, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return i;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public int Update(FeArticle model)
        {
            int u= IFeArticleDao.Instance.Update(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "新闻/帮助文章更改状态" + model.Title, LogStatus.系统日志目标类型.新闻帮助管理, u, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return u;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public bool Delete(int id)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "删除了系统号为"+id+"文章", LogStatus.系统日志目标类型.新闻帮助管理, id, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return IFeArticleDao.Instance.Delete(id);
        }
    }
}