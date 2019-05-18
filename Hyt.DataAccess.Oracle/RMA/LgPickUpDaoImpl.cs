using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.RMA;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.RMA
{
    /// <summary>
    ///取件单
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public class LgPickUpDaoImpl : ILgPickUpDao
    {
        /// <summary>
        /// 添加取件单
        /// </summary>
        /// <param name="entity">取件单</param>
        /// <returns>取件单编号</returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public override int Insert(Model.LgPickUp entity)
        {
            var sysNO = Context.Insert("LgPickUp", entity)
                                      .AutoMap(o => o.SysNo, o => o.Items)
                                      .ExecuteReturnLastId<int>("SysNo");
            return sysNO;
        }

        /// <summary>
        /// 获取入库单对应的取件单
        /// </summary>
        /// <param name="stockInSysNo">入库单号</param>
        /// <returns>入库单对应的取件单</returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public override Model.LgPickUp GetEntityByStockIn(int stockInSysNo)
        {
            return Context.Sql("select * from LgPickUp where StockInSysNo=@StockInSysNo")
                 .Parameter("StockInSysNo", stockInSysNo).QuerySingle<LgPickUp>();
        }

        /// <summary>
        /// 获取取件方式
        /// </summary>
        /// <param name="pickupTypeSysNo">取件方式编号</param>
        /// <returns>取件方式</returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public override LgPickupType GetPickupType(int pickupTypeSysNo)
        {
            return Context.Sql("select * from LgPickupType where SysNo=@SysNo")
                .Parameter("SysNo", pickupTypeSysNo).QuerySingle<LgPickupType>();
        }

        /// <summary>
        /// 更新取件单信息
        /// </summary>
        /// <param name="entity">取件单</param>
        /// <returns></returns>
        /// <remarks>2013-07-12 朱成果 创建</remarks>
        public override void Update(Model.LgPickUp entity)
        {
            Context.Update("LgPickUp", entity)
               .AutoMap(o => o.SysNo, o => o.Items)
               .Where("SysNo", entity.SysNo).Execute();
        }

        /// <summary>
        /// 获取所有取件方式
        /// </summary>
        /// <returns>所有取件方式</returns>
        /// <remarks>2013-08-13 周唐炬 创建</remarks>
        public override List<LgPickupType> GetLgPickupTypeList()
        {
            return Context.Sql(@"SELECT * FROM LGPICKUPTYPE").QueryMany<LgPickupType>();
        }
    }
}
