using Hyt.DataAccess.Base;
using Hyt.Model.Kis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Kis
{
    /// <summary>
    /// IcStockBillTem主表信息数据访问接口
    /// </summary>
    /// <remarks>2017-05-31 罗勤尧 创建</remarks>
    public abstract class IIcStockBillTemDao : DaoBase<IIcStockBillTemDao>
    {
        /// <summary>
        /// 插入主表
        /// </summary>
        /// <param name="entity">主表实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2017-05-31 罗勤尧  创建</remarks>
        public abstract int InsertEntity(IcStockBillTem entity);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name=""></param>
        /// <returns>影响行数</returns>
        /// <remarks>2017-05-31 罗勤尧  创建</remarks>
        public abstract int EXEC();
        
    }
}
