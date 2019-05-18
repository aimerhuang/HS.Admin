using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Lucene.Net.Index;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 新闻管理业务逻辑层
    /// </summary>
    /// <remarks>2014－01-15 苟治国 创建</remarks>
    public class FeNewsBo : BOBase<FeNewsBo>
    {
        /// <summary>
        /// 查看新闻
        /// </summary>
        /// <param name="sysNo">新闻编号</param>
        /// <returns>新闻</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public Model.FeNews GetModel(int sysNo)
        {
            return IFeNewsDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据条件获取新闻列表
        /// </summary>
        /// <param name="id">索引</param>
        /// <param name="status">状态</param>
        /// <param name="title">标题</param>
        /// <returns>新闻组列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public PagedList<Model.CBFeNews> Seach(int id, int? status, string title = null)
        {
            if (status == null){
                status = -1;
            }else{
                status = status ?? -1;
            }

            var pager = new Pager<CBFeNews>()
            {
                CurrentPage = id,
                PageFilter = new CBFeNews()
                {
                    Title = title,
                    Status = (int)status
                },
            };

            var list = new PagedList<CBFeNews>();
            pager.PageSize = list.PageSize;
            pager = IFeNewsDao.Instance.Seach(pager);

            list = new PagedList<CBFeNews>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                Style = PagedList.StyleEnum.WebDefault
            };
            return list;
        }

        /// <summary>
        /// 获取最新新闻
        /// </summary>
        /// <param name="rownum">获取条数</param>
        /// <returns>新闻列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public IList<FeNews> GetNews(int rownum)
        {
            return IFeNewsDao.Instance.GetNews(rownum);
        }

        /// <summary>
        /// 获取热点新闻
        /// </summary>
        /// <param name="rownum">获取条数</param>
        /// <returns>新闻列表</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public IList<FeNews> GetHots(int rownum)
        {
            return IFeNewsDao.Instance.GetHots(rownum);
        }

        /// <summary>
        /// 更新新闻
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public int Update(Model.FeNews model)
        {
            return IFeNewsDao.Instance.Update(model);
        }
    }
}
