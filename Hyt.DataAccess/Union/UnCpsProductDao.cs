using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Union
{
    /// <summary>
    /// 联盟广告CPS商品Dao
    /// </summary>
    /// <remarks>2013-10-18 周唐炬 创建</remarks>
    public abstract class UnCpsProductDao : DaoBase<UnCpsProductDao>
    {
        /// <summary>
        /// 添加CPS商品
        /// </summary>
        /// <param name="model">CPS商品实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public abstract int Create(UnCpsProduct model);

        /// <summary>
        /// 删除CPS商品
        /// </summary>
        /// <param name="sysNo">CPS商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public abstract void Remove(int sysNo);

        /// <summary>
        /// 通过联盟广告删除CPS商品
        /// </summary>
        /// <param name="advertisementSysNo">联盟广告编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public abstract void RemoveByAdvertisementSysNo(int advertisementSysNo);

        /// <summary>
        /// 获取联盟广告CPS商品列表
        /// </summary>
        /// <param name="advertisementSysNo">联盟广告系统编号</param>
        /// <returns>联盟广告CPS商品列表</returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public abstract List<UnCpsProduct> GetList(int advertisementSysNo);
    }
}
