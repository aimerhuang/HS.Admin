using Hyt.DataAccess.Kis;
using Hyt.Model.Kis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Kis
{
    /// <summary>
    /// 主表信息数据访问实现
    /// </summary>
    /// <remarks>2017-05-31 罗勤尧 创建</remarks>
    public class IcStockBillTemDaoImpl : IIcStockBillTemDao
    {
        /// <summary>
        /// 插入主表
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>影响的行数</returns>
        /// <remarks>2017-05-31 罗勤尧 创建</remarks>
        public override int InsertEntity(IcStockBillTem entity)
        {
            //Context.Insert("icstockbill_tem", entity)
            //                .Execute();
            //return entity;

            var id = ContextKis.Insert<IcStockBillTem>("icstockbill_tem", entity)
               .AutoMap()
               .Execute();
            return id;
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name=""></param>
        /// <returns>影响行数</returns>
        /// <remarks>2017-05-31 罗勤尧  创建</remarks>
        public override int EXEC()
        {
            
                string Sql = string.Format("sp_stockbill");
                var result = ContextKis.StoredProcedure(Sql)
                    .Execute();
                return result;
            
        }
    }
}
