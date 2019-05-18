using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hyt.Model;
using Hyt.DataAccess.Front;
using Hyt.Infrastructure.Pager;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 友情链接业务逻辑层
    /// </summary>
    /// <remarks>2013－12 - 12 苟治国 创建</remarks>
    public class MkBlogrollBo : BOBase<MkBlogrollBo>
    {
        /// <summary>
        /// 根据条件获取友情链接列表
        /// </summary>
        /// <param name="id">索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <returns>友情链接列表</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public PagedList<Model.MkBlogroll> Seach(int id, int pageSize)
        {
            var pager = new Pager<MkBlogroll>()
            {
                CurrentPage = id,
                PageFilter = new MkBlogroll()
                {
                    Status = (int)MarketingStatus.友情链接管理状态.已审
                },
            };

            var list = new PagedList<MkBlogroll>();
            pager.PageSize = pageSize;
            pager = IMkBlogrollDao.Instance.Seach(pager);

            list = new PagedList<MkBlogroll>
            {
                PageSize = pageSize,
                Data = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                Style = PagedList.StyleEnum.Default
                
            };
            return list;
        }

        /// <summary>
        /// 插入友情链接
        /// </summary>
        /// <param name="model">友情链接明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013－12-09 苟治国 创建</remarks>
        public int Insert(Model.MkBlogroll model)
        {
            return IMkBlogrollDao.Instance.Insert(model);
        }
    }
}
