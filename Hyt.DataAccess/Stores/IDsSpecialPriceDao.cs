using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Stores
{
    /// <summary>
    /// 经销商产品特殊价格表
    /// </summary>
    /// <remarks>2015-12-7 杨浩 创建</remarks>
    public abstract class IDsSpecialPriceDao : DaoBase<IDsSpecialPriceDao>
    {
        /// <summary>
        /// 获得全部经销商产品价格列表
        /// </summary>
        /// <returns></returns>
        public abstract IList<DsSpecialPrice> GetAllSpecialPrices();
        /// <summary>
        /// 获得指定经销商的产品价格
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public abstract IList<DsSpecialPrice> GetSpecialPricesBySysNo(int sysNo);

        /// <summary>
        /// 获得指定经销商的产品价格
        /// </summary>
        /// <param name="sysNo">经销商编号</param>
        /// <param name="productSysNo">产品编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-3 杨浩 创建</remarks>
        public abstract DsSpecialPrice GetSpecialPricesBySysNo(int sysNo, int productSysNo);

    }
}
