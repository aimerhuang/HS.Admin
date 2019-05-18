using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 取定制商品数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class PdProductStockInDaoImpl : IPdProductStockInDao
    {

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public override PdProductStockIn GetEntity(int sysNo)
        {

            return Context.Sql("select a.* from PdProductStockIn a where a.SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<PdProductStockIn>();
        }

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Insert(PdProductStockIn entity)
        {
            entity.SysNo = Context.Insert("PdProductStockIn", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Update(PdProductStockIn entity)
        {

            return Context.Update("PdProductStockIn", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
        public override bool UpdateStatus(int SysNo, WarehouseStatus.入库单状态 Status, int AuditorSysNo)
        {
            int result = Context.Update("PdProductStockIn")
                .Column("Status", (int)Status)
                .Column("AuditorSysNo", (int)AuditorSysNo)
                .Column("AuditDate", DateTime.Now)
                .Column("LastUpdateBy", AuditorSysNo)
                .Column("LastUpdateDate", DateTime.Now)
                .Where("SysNo", SysNo)
                .Execute();
            return result > 0;
        }

        /// <summary>
        /// 更新发送状态
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override bool UpdateSendStatus(int SysNo, int SendStatus)
        {

            int result = Context.Update("PdProductStockIn")
                .Column("SendStatus", (int)SendStatus)
                .Where("SysNo", SysNo)
                .Execute();
            return result > 0;
        }
        #endregion
    }
}
