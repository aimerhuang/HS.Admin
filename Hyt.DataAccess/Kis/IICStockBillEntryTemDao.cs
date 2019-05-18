using Hyt.DataAccess.Base;
using Hyt.Model.Kis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Kis
{
    /// <summary>
    /// IcStockBillTem明细表信息数据访问接口
    /// </summary>
    /// <remarks>2017-05-31 罗勤尧 创建</remarks>
    public abstract class IICStockBillEntryTemDao : DaoBase<IICStockBillEntryTemDao>
    {
        /// <summary>
        /// 插入明细表
        /// </summary>
        /// <param name="entity">明细表实体</param>
        /// <returns>影响的行数</returns>
        /// <remarks>2017-05-31 罗勤尧 创建</remarks>
        public abstract int InsertEntity(ICStockBillEntryTem entity);

        /// <summary>
        /// 集合插入明细表
        /// </summary>
        /// <param name="entity">明细表集合实体</param>
        /// <returns>影响的行数</returns>
        /// <remarks>2017-05-31 罗勤尧 创建</remarks>
        public abstract int InsertEntityList(List<ICStockBillEntryTem> entity);
    }
}
