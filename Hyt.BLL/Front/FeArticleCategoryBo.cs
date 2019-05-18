using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.DataAccess.Front;
using Hyt.DataAccess.Product;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Front
{
    /// <summary>
    /// 文章类型管理业务逻辑层
    /// </summary>
    /// <remarks>2013－06-17 杨晗 创建</remarks>
    public class FeArticleCategoryBo : BOBase<FeArticleCategoryBo>
    {
        /// <summary>
        /// 根据文章类型编号获取实体
        /// </summary>
        /// <param name="id">文章类型编号</param>
        /// <returns>文章类型实体</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public FeArticleCategory GetModel(int id)
        {
            return IFeArticleCategoryDao.Instance.GetModel(id);
        }

        /// <summary>
        /// 判断文章分类标题是否重复
        /// </summary>
        /// <param name="title">文章分类标题</param>
        /// <param name="type">文章分类类型</param>
        /// <returns>重复为true,否则为false</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        public bool FeArticleCategoryVerify(string title, ForeStatus.文章分类类型 type)
        {
            return IFeArticleCategoryDao.Instance.FeArticleCategoryVerify(title,type);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex">起始页</param>
        /// <param name="type">文章类型</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchName">分类名称</param>
        /// <returns>文章类型列表</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public PagedList<FeArticleCategory> Seach(int? pageIndex, ForeStatus.文章分类类型 type,
                                                  int? searchStaus, string searchName)
        {
            pageIndex = pageIndex ?? 1;
            searchStaus = searchStaus ?? 0;
            var model = new PagedList<FeArticleCategory>();
            int count;
            var list = IFeArticleCategoryDao.Instance.Seach((int) pageIndex, model.PageSize, type, (int) searchStaus,
                                                            searchName, out count);

            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int) pageIndex;
            return model;
        }

        /// <summary>
        /// 根据文章类型获取所有文章分类
        /// </summary>
        /// <param name="type">文章分类类型</param>
        /// <returns>文章分类集合</returns>
        /// <remarks>2013－06-19 杨晗 创建</remarks>
        public IList<FeArticleCategory> GetAll(ForeStatus.文章分类类型 type)
        {
            return IFeArticleCategoryDao.Instance.GetAll(type);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public int Insert(FeArticleCategory model)
        {
            string msg;
            int i = IFeArticleCategoryDao.Instance.Insert(model);
            if (model.Type == (int)ForeStatus.文章分类类型.帮助)
            {
                msg = "新增帮助分类" + model.Name;
            }
            else
            {
                msg = "新增文章分类" + model.Name;
            }
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                     msg, LogStatus.系统日志目标类型.新闻帮助管理, i, null, "", Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo
);
            return i;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public int Update(FeArticleCategory model)
        {
            string msg;
            int u= IFeArticleCategoryDao.Instance.Update(model);
            if (model.Type == (int)ForeStatus.文章分类类型.帮助)
            {
                msg = "帮助分类更改状态" + model.Name;
            }
            else
            {
                msg = "文章分类更改状态" + model.Name;
            }
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 msg, LogStatus.系统日志目标类型.新闻帮助管理, u, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return u;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章类型主键</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013－06-17 杨晗 创建</remarks>
        public bool Delete(int id)
        {
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "删除系统号为"+id+"的新闻/帮助分类", LogStatus.系统日志目标类型.新闻帮助管理, id, null, "",
                                 Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            return IFeArticleCategoryDao.Instance.Delete(id);
        }

        /// <summary>
        /// 获取所有文章类别(包含旗下文章)
        /// </summary>
        /// <param name="type">文章类型</param>
        /// <param name="recordNum">记录条数</param>
        /// <returns>所有文章类别(包含旗下文章)</returns>
        /// <remarks>2013-08-12 杨晗 创建</remarks>
        /// <remarks>2013-09-25 邵斌 修改：指定记录条数</remarks>
        private IList<CBFeArticleCategory> GetAllArticle(ForeStatus.文章分类类型 type, int? recordNum=null)
        {
            var list = GetAll(type);
            IList<CBFeArticleCategory> cbList = new List<CBFeArticleCategory>();
            foreach (var item in list)
            {
                var model = new CBFeArticleCategory
                    {
                        SysNo = item.SysNo,
                        Type = item.Type,
                        Name = item.Name,
                        Description = item.Description,
                        DisplayOrder = item.DisplayOrder,
                        Status = item.Status,
                        FeArticle = IFeArticleDao.Instance.GetListByCategory(item.SysNo, recordNum)
                    };
                cbList.Add(model);
            }
            return cbList;
        }

        /// <summary>
        /// 缓存方法获取帮助中心类别和旗下文章
        /// </summary>
        /// <returns>帮助中心类别和旗下文章</returns>
        /// <remarks>2013-08-12 杨晗 创建</remarks>
        public IList<CBFeArticleCategory> GetHelpArticle()
        {
            return CacheManager.Get<IList<CBFeArticleCategory>>(CacheKeys.Items.AllHelp, "", delegate()
                {
                    return GetAllArticle(ForeStatus.文章分类类型.帮助);
                });
        }

        /// <summary>
        /// 缓存方法获取新闻公告类别和旗下文章
        /// </summary>
        /// <param name="recordNum">记录条数</param>
        /// <returns>新闻公告类别和旗下文章</returns>
        /// <remarks>2013-08-12 杨晗 创建</remarks>
        /// <remarks>2013-09-25 邵斌 修改：指定记录条数</remarks>
        public IList<CBFeArticleCategory> GetArticle(int? recordNum=null)
        {
            return CacheManager.Get<IList<CBFeArticleCategory>>(CacheKeys.Items.WebBulletin, "", delegate()
            {
                return GetAllArticle(ForeStatus.文章分类类型.新闻, recordNum);
            });
        }
    }
}
