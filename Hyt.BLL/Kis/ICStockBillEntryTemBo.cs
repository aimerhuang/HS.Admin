using Hyt.DataAccess.Kis;
using Hyt.Model.Kis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Kis
{
    /// <summary>
    /// 明细表管理
    /// </summary>
    ///  <remarks>2017-05-31 罗勤尧 创建</remarks>
    public class ICStockBillEntryTemBo : BOBase<ICStockBillEntryTemBo>
    {
        /// <summary>
        /// 插入明细表
        /// </summary>
        /// <param name="entity">实体</param>
        ///<remarks>2017-05-31 罗勤尧 创建</remarks> 
        public int InsertEntity(ICStockBillEntryTem entity)
        {
            return IICStockBillEntryTemDao.Instance.InsertEntity(entity);
            //return en;
        }

        /// <summary>
        /// 插入明细表集合
        /// </summary>
        /// <param name="entity">明细表集合实体</param>
        /// <returns>影响的行数</returns>
        /// <remarks>2017-05-31 罗勤尧 创建</remarks>
        public  int InsertEntityList(List<ICStockBillEntryTem> entity)
        {
            return IICStockBillEntryTemDao.Instance.InsertEntityList(entity);
        }
    }
}
