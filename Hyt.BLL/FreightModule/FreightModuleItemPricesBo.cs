using Hyt.DataAccess.FreightModule;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.FreightModule
{

    /// <summary>
    /// 运费模板项价格
    /// </summary>
    /// <remarks>2015-11-21 杨浩 创建</remarks>
    public class FreightModuleItemPricesBo : BOBase<FreightModuleItemPricesBo>
    {
        /// <summary>
        /// 添加运费模板项价格
        /// </summary>
        /// <param name="lgFreightModuleItemPrices">运费模板项价格实体类</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public int AddFreightModuleItemPrices(LgFreightModuleItemPrices lgFreightModuleItemPrices)
        {
            return IFreightModuleItemPricesDao.Instance.AddFreightModuleItemPrices(lgFreightModuleItemPrices);
        }
        /// <summary>
        /// 更新运费模板项价格
        /// </summary>
        /// <param name="lgFreightModuleItemPrices">运费模板项价格实体类</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public int UpdateFreightModuleItemPrices(LgFreightModuleItemPrices lgFreightModuleItemPrices)
        {
            return IFreightModuleItemPricesDao.Instance.UpdateFreightModuleItemPrices(lgFreightModuleItemPrices);
        }
        /// <summary>
        /// 删除运费模板项价格
        /// </summary>
        /// <param name="sysNos">运费模板项价格编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public  int DeleteFreightModuleItemPricesBySysNos(string sysNos)
        {
            return IFreightModuleItemPricesDao.Instance.DeleteFreightModuleItemPricesBySysNos(sysNos);
        }

        /// <summary>
        /// 获取运费模板项价格
        /// </summary>
        /// <param name="freightModuleDetailsSysNo">运费模板详情编号</param>
        /// <param name="sysNo">运费模板项价格编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public  IList<LgFreightModuleItemPrices> GetFreightModuleItemPricesList(int freightModuleDetailsSysNo, int sysNo)
        {
            return IFreightModuleItemPricesDao.Instance.GetFreightModuleItemPricesList(freightModuleDetailsSysNo, sysNo);

        }
    }
}
