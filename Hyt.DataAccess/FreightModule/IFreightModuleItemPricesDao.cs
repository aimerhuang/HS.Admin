using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.FreightModule
{
    /// <summary>
    /// 运费模板项价格
    /// </summary>
    /// <remarks>2015-11-21 杨浩 创建</remarks>
    public abstract class IFreightModuleItemPricesDao : DaoBase<IFreightModuleItemPricesDao>
    {
        /// <summary>
        /// 添加运费模板项价格
        /// </summary>
        /// <param name="lgFreightModuleItemPrices">运费模板项价格实体类</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public abstract int AddFreightModuleItemPrices(LgFreightModuleItemPrices lgFreightModuleItemPrices);
        /// <summary>
        /// 更新运费模板项价格
        /// </summary>
        /// <param name="lgFreightModuleItemPrices">运费模板项价格实体类</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public abstract int UpdateFreightModuleItemPrices(LgFreightModuleItemPrices lgFreightModuleItemPrices);
        /// <summary>
        /// 删除运费模板项价格
        /// </summary>
        /// <param name="sysNos">运费模板项价格编号</param>
        /// <returns></returns>
        /// <remarks>2015-11-21 杨浩 创建</remarks>
        public abstract int DeleteFreightModuleItemPricesBySysNos(string sysNos);
        /// <summary>
        /// 获取运费模板项价格
        /// </summary>
        /// <param name="freightModuleDetailsSysNo">运费模板详情编号</param>
        /// <param name="sysNo">运费模板项价格编号</param>
        /// <returns></returns>
        public abstract IList<LgFreightModuleItemPrices> GetFreightModuleItemPricesList(int freightModuleDetailsSysNo, int sysNo);

    }
}
