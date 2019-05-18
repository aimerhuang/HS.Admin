using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Union
{
    /// <summary>
    /// 联盟广告
    /// </summary>
    /// <remarks>2013-10-14 吴文强 创建</remarks>
    public abstract class IUnAdvertisementDao : DaoBase<IUnAdvertisementDao>
    {
        /// <summary>
        /// 根据系统编号获取联盟广告
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>联盟广告</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public abstract UnAdvertisement GetModel(int sysNo);

        /// <summary>
        /// 根据来源Url和访问Url获取联盟广告
        /// </summary>
        /// <param name="urlReferrer">来源Url</param>
        /// <param name="accessUrl">访问Url</param>
        /// <returns>联盟广告</returns>
        /// <remarks>2013-10-15 吴文强 创建</remarks>
        public abstract UnAdvertisement GetModel(string urlReferrer, string accessUrl);

        /// <summary>
        /// 创建联盟广告
        /// </summary>
        /// <param name="model">联盟广告实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract int Create(UnAdvertisement model);

        /// <summary>
        /// 修改联盟广告
        /// </summary>
        /// <param name="model">联盟广告实体</param>
        /// <returns>受影响行</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract int Update(UnAdvertisement model);

        /// <summary>
        /// 根据商品系统编号获取Cps商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>Cps商品</returns>
        /// <remarks>2013-10-14 吴文强 创建</remarks>
        public abstract UnCpsProduct GetCpsProduct(int productSysNo);

        /// <summary>
        /// 通过条件获取联盟广告列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>联盟广告列表</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        public abstract Pager<UnAdvertisement> GetAdvertisementList(ParaUnAdvertisementFilter filter);
    }
}
