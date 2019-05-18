using System.Collections.Generic;
using Hyt.DataAccess.Union;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.BLL.Union
{
    /// <summary>
    /// 联盟网站
    /// </summary>
    /// <remarks>2013-10-14 吴文强 创建</remarks>
    public class UnUnionSiteBo : BOBase<UnUnionSiteBo>
    {
        /// <summary>
        /// 根据系统编号获取联盟网站
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <returns>联盟网站</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public UnUnionSite GetModel(int sysno)
        {
            return IUnUnionSiteDao.Instance.GetModel(sysno);
        }

        /// <summary>
        /// 创建联盟网站
        /// </summary>
        /// <param name="model">联盟网站实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public int Create(UnUnionSite model)
        {
            return IUnUnionSiteDao.Instance.Create(model);
        }

        /// <summary>
        /// 修改联盟网站
        /// </summary>
        /// <param name="model">联盟网站实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public int Update(UnUnionSite model)
        {
            return IUnUnionSiteDao.Instance.Update(model);
        }

        /// <summary>
        /// 获取所有联盟网站
        /// </summary>
        /// <param></param>
        /// <returns>所有联盟网站</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public List<UnUnionSite> GetAll()
        {
            return IUnUnionSiteDao.Instance.GetAll();
        }

        /// <summary>
        /// 通过条件获取联盟网站列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>联盟网站列表</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public PagedList<UnUnionSite> GetUnionSiteList(ParaUnUnionSiteFilter filter)
        {
            if (filter != null)
            {
                var model = new PagedList<UnUnionSite>();
                filter.PageSize = model.PageSize;
                var pager = IUnUnionSiteDao.Instance.GetUnionSiteList(filter);
                if (null != pager)
                {
                    model.TData = pager.Rows;
                    model.TotalItemCount = pager.TotalRows;
                    model.CurrentPageIndex = filter.CurrentPage;
                }
                return model;
            }
            return null;
        }

        /// <summary>
        /// 联盟网站名称验证
        /// </summary>
        /// <param name="siteName">联盟网站名称</param>
        /// <param name="sysNo">联盟网站系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        public bool UnionSiteVerify(string siteName, int? sysNo)
        {
            var count = IUnUnionSiteDao.Instance.UnionSiteVerify(siteName, sysNo);
            return count <= 0;
        }
    }
}
