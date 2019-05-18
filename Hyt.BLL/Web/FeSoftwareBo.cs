using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.DataAccess.Front;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 软件分类信息业务逻辑层
    /// </summary>
    /// <remarks>2014-01-20 苟治国 创建</remarks>
    public class FeSoftwareBo : BOBase<FeSoftwareBo>
    {
        /// <summary>
        /// 获取指定编号的软件分类信息
        /// </summary>
        /// <param name="sysNo">软件分类编号</param>
        /// <returns>软件分类实体信息</returns>
        /// <remarks>2014-01-20 苟治国 创建</remarks>
        public FeSoftware GetEntity(int sysNo)
        {
            return IFeSoftwareDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 获取软件分类列表
        /// </summary>
        /// <param name="id">索引</param>
        /// <param name="cid">软件分类编号</param>
        /// <param name="key">标题</param>
        /// <returns>软件分类列表</returns>
        /// <remarks>2014-01-20 苟治国 创建</remarks>
        public PagedList<FeSoftware> GetList(int id, int cid, string key=null)
        {

            var pager = new Pager<FeSoftware>()
            {
                CurrentPage = id,
                PageFilter = new FeSoftware()
                {
                    SoftCategorySysNo = cid,
                    Title = key,
                    Status = (int)Hyt.Model.WorkflowStatus.ForeStatus.软件下载状态.已审
                },
            };

            var list = new PagedList<FeSoftware>();
            pager.PageSize = list.PageSize;
            pager = IFeSoftwareDao.Instance.GetList(pager);

            list = new PagedList<FeSoftware>
            {
                //PageSize = 2,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                Style = PagedList.StyleEnum.Default
            };
            return list;
        }
    }
}
