using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Front;
using Lucene.Net.Index;

namespace Hyt.BLL.Front
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
        /// 判断重复数据
        /// </summary>
        /// <param name="model">分类实体信息</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public bool IsExists(FeNews model)
        {
            return IFeNewsDao.Instance.IsExists(model);
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
                Style = PagedList.StyleEnum.Default
            };
            return list;
        }

        /// <summary>
        /// 插入新闻
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public int Insert(Model.FeNews model)
        {
            return IFeNewsDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新新闻点击数
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2014－02-24 苟治国 创建</remarks>
        public int UpdateViews(int sysNo)
        {
            return IFeNewsDao.Instance.UpdateViews(sysNo);
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

        /// <summary>
        /// 更新新闻状态
        /// </summary>
        /// <param name="collocation">新闻实体</param>
        /// <param name="auditorSysNo">编号</param>
        /// <returns>返回受影响的行</returns>
        /// <remarks>2013－01-14 苟治国 创建</remarks>
        public int UpdateStatus(IList<Model.FeNews> collocation, int auditorSysNo)
        {
            int result = 0;
            try
            {
                for (int i = 0; i < collocation.Count; i++)
                {
                    int sysNo = Convert.ToInt32(collocation[i].SysNo);
                    int status = Convert.ToInt32(collocation[i].Status);

                    var model = this.GetModel(sysNo);
                    if (model.Status != (int)Hyt.Model.WorkflowStatus.ForeStatus.新闻状态.作废)
                    {
                        model.Status = status;
                        result = this.Update(model);
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
