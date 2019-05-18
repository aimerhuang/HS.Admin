using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Union;
using Hyt.Model;

namespace Hyt.BLL.Union
{
    /// <summary>
    /// CPS商品BO
    /// </summary>
    /// <remarks>2013-10-18 周唐炬 创建</remarks>
    public class UnCpsProductBo : BOBase<UnCpsProductBo>
    {
        /// <summary>
        /// 添加CPS商品
        /// </summary>
        /// <param name="model">CPS商品实体</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public int Create(UnCpsProduct model)
        {
            return UnCpsProductDao.Instance.Create(model);
        }

        /// <summary>
        /// 删除CPS商品
        /// </summary>
        /// <param name="sysNo">CPS商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public void Remove(int sysNo)
        {
            UnCpsProductDao.Instance.Remove(sysNo);
        }

        /// <summary>
        /// 获取联盟广告CPS商品列表
        /// </summary>
        /// <param name="advertisementSysNo">联盟广告系统编号</param>
        /// <returns>联盟广告CPS商品列表</returns>
        /// <remarks>2013-10-18 周唐炬 创建</remarks>
        public List<UnCpsProduct> GetList(int advertisementSysNo)
        {
            return UnCpsProductDao.Instance.GetList(advertisementSysNo);
        }
    }
}
