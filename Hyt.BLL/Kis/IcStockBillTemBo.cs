using Hyt.DataAccess.Kis;
using Hyt.Model.Kis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Kis
{
    /// <summary>
    /// 主表管理
    /// </summary>
    ///  <remarks>2017-05-31 罗勤尧 创建</remarks>
    public class IcStockBillTemBo : BOBase<IcStockBillTemBo>
    {
        /// <summary>
        /// 插入主表
        /// </summary>
        /// <param name="entity">实体</param>
        ///<remarks>2017-05-31 罗勤尧 创建</remarks> 
        public int InsertEntity(IcStockBillTem entity)
        {
            var en = IIcStockBillTemDao.Instance.InsertEntity(entity);
            return en;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name=""></param>
        /// <returns>影响行数</returns>
        /// <remarks>2017-05-31 罗勤尧  创建</remarks>
        public int EXEC()
        {
            var en = IIcStockBillTemDao.Instance.EXEC();
            return en;
        }
    }
}
