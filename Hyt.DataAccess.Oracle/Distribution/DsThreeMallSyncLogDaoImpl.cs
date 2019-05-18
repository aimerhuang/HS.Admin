using Hyt.DataAccess.Base;
using Hyt.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 同步三方商城
    /// </summary>
    /// <remarks>2016-7-8 杨浩 创建</remarks>
    public class DsThreeMallSyncLogDaoImpl : IDsThreeMallSyncLogDao
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public override int Add(DsThreeMallSyncLog model)
        {
            var sysNo = Context.Insert("DsThreeMallSyncLog", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        public override int Update(DsThreeMallSyncLog model)
        {
            var rows = Context.Update("DsThreeMallSyncLog", model)
                                       .AutoMap(o => o.SysNo)
                                       .Where("SysNo",model.SysNo)
                                       .Execute();
            return rows;
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <param name="mallSysNo">商城编号</param>
        /// <param name="syncType">同步类型(订单:10 退货货单：20)</param>
        /// <remarks></remarks>
        public override DsThreeMallSyncLog GetThreeMallSyncLogInfo(int dealerSysNo, int mallTypeSysNo, int syncType)
        {
            return Context.Sql("select * from DsThreeMallSyncLog where mallSysNo=@mallSysNo and syncType=@syncType")
                .Parameter("mallSysNo", mallTypeSysNo)
                .Parameter("syncType", syncType)
                .QuerySingle<DsThreeMallSyncLog>();
        }
    }
}
