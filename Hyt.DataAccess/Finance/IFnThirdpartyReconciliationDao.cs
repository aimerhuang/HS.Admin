using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Finance
{
    /// <summary>
    /// 第三方对账
    /// </summary>
    /// <remarks>2014-8-21 朱成果 创建</remarks>
    public abstract class IFnThirdpartyReconciliationDao : DaoBase<IFnThirdpartyReconciliationDao>
    {

        /// <summary>
        /// 添加对账数据
        /// </summary>
        /// <param name="item">对账数据</param>
        /// <returns></returns>
        /// <remarks>2014-8-21 朱成果 创建</remarks>
        public abstract int Insert(FnThirdpartyReconciliation item);


        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="filter">筛选</param>
        /// <returns></returns>
        /// <remarks>2014-8-21 朱成果 创建</remarks>
        public abstract Pager<FnThirdpartyReconciliation> Query(ParaReconciliationFilter filter);


       /// <summary>
       /// 加盟商对账（支付宝)
       /// </summary>
       /// <param name="item">对账数据</param>
        /// <remarks>2014-8-21 朱成果 创建</remarks>
        public abstract void CheckAlipayReconciliation(FnThirdpartyReconciliation item);
    }
}
