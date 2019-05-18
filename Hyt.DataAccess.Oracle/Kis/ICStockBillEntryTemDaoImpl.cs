using Hyt.DataAccess.Kis;
using Hyt.Model.Kis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Kis
{
    /// <summary>
    /// 明细表信息数据访问实现
    /// </summary>
    /// <remarks>2017-05-31 罗勤尧 创建</remarks>
    public class ICStockBillEntryTemDaoImpl : IICStockBillEntryTemDao
    {
        /// <summary>
        /// 插入明细表
        /// </summary>
        /// <param name="entity">明细表实体</param>
        /// <returns>影响的行数</returns>
        /// <remarks>2017-05-31 罗勤尧 创建</remarks>
        public override int InsertEntity(ICStockBillEntryTem entity)
        {
            //Context.Insert("icstockbillentry_tem", entity)
            //                 .Execute();
            //return entity;

            var id = ContextKis.Insert<ICStockBillEntryTem>("icstockbillentry_tem", entity)
           .AutoMap()
             .Execute();
            return id;
        }
        /// <summary>
        /// 插入明细表集合
        /// </summary>
        /// <param name="entity">明细表集合实体</param>
        /// <returns>影响的行数</returns>
        /// <remarks>2017-05-31 罗勤尧 创建</remarks>
        public override int InsertEntityList(List<ICStockBillEntryTem> entity)
        {
            int i = 0;
            foreach(ICStockBillEntryTem item in entity)
            {
               var e= InsertEntity(item);
                i++;
            }
            return i;
        }
    }
}
