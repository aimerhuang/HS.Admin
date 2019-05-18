using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 入库主表 抽象类
    /// </summary>
    /// <remarks>
    /// 2015-08-27 王耀发 创建
    /// </remarks>
    public abstract class IPdProductStockInDao : Hyt.DataAccess.Base.DaoBase<IPdProductStockInDao>
    {

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract PdProductStockIn GetEntity(int sysNo);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public abstract int Insert(PdProductStockIn entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract int Update(PdProductStockIn entity);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract bool UpdateStatus(int SysNo, WarehouseStatus.入库单状态 Status, int AuditorSysNo);

        /// <summary>
        /// 更新发送状态
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public abstract bool UpdateSendStatus(int SysNo, int SendStatus);

      
    }
}

