using System.Linq;
using Hyt.DataAccess.Union;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Union
{
    /// <summary>
    /// 联盟广告
    /// </summary>
    /// <remarks>2013-10-14 吴文强 创建</remarks>
    public class UnAdvertisementBo : BOBase<UnAdvertisementBo>
    {
        /// <summary>
        /// 根据系统编号获取联盟广告
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>联盟广告</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public UnAdvertisement GetModel(int sysNo)
        {
            return IUnAdvertisementDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据来源Url和访问Url获取联盟广告
        /// </summary>
        /// <param name="urlReferrer">来源Url</param>
        /// <param name="accessUrl">访问Url</param>
        /// <returns>联盟广告</returns>
        /// <remarks>2013-10-15 吴文强 创建</remarks>
        public UnAdvertisement GetModel(string urlReferrer, string accessUrl)
        {
            return IUnAdvertisementDao.Instance.GetModel(urlReferrer, accessUrl);
        }

        /// <summary>
        /// 创建联盟广告
        /// </summary>
        /// <param name="model">联盟广告实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public int Create(UnAdvertisement model)
        {
            var id = IUnAdvertisementDao.Instance.Create(model);
            if (model.Products != null && model.Products.Count > 0)
            {
                var status = UnionStatus.CPS商品状态.启用.GetHashCode();
                foreach (var item in model.Products.Select(p => new UnCpsProduct()
                    {
                        AdvertisementSysNo = id,
                        ProductSysNo = p,
                        Status = status
                    }))
                {
                    UnCpsProductBo.Instance.Create(item);
                }
            }
            return id;
        }

        /// <summary>
        /// 修改联盟广告
        /// </summary>
        /// <param name="model">联盟广告实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public int Update(UnAdvertisement model)
        {
            var entiy = IUnAdvertisementDao.Instance.GetModel(model.SysNo);
            if (entiy == null) throw new HytException("未找到该联盟广告，请刷新页面重试！");
            entiy = model;
            UnCpsProductDao.Instance.RemoveByAdvertisementSysNo(model.SysNo);
            if (model.ItemList != null && model.ItemList.Any())
            {
                foreach (var item in model.ItemList)
                {
                    item.AdvertisementSysNo = model.SysNo;
                    UnCpsProductBo.Instance.Create(item);
                }
            }
            return IUnAdvertisementDao.Instance.Update(entiy);
        }

        /// <summary>
        /// 根据商品系统编号获取Cps商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>Cps商品</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public UnCpsProduct GetCpsProduct(int productSysNo)
        {
            return IUnAdvertisementDao.Instance.GetCpsProduct(productSysNo);
        }

        /// <summary>
        /// 通过条件获取联盟广告列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>联盟广告列表</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public PagedList<UnAdvertisement> GetAdvertisementList(ParaUnAdvertisementFilter filter)
        {
            if (filter != null)
            {
                var model = new PagedList<UnAdvertisement>();
                filter.PageSize = model.PageSize;
                var pager = IUnAdvertisementDao.Instance.GetAdvertisementList(filter);
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
    }
}
