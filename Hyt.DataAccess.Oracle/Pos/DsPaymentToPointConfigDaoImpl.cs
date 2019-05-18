using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsPaymentToPointConfigDaoImpl : IDsPaymentToPointConfigDao
    {
        /// <summary>
        /// 添加经销商等级数据
        /// </summary>
        /// <param name="cardMod"></param>
        /// <returns></returns>
        public override int Insert(DsPaymentToPointConfig cardMod)
        {
            return Context.Insert("DsPaymentToPointConfig", cardMod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="cardMod"></param>
        public override void Update(DsPaymentToPointConfig cardMod)
        {
            Context.Update("DsPaymentToPointConfig", cardMod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override DsPaymentToPointConfig GetDsPaymentToPointConfigBySysNo(int dsSysNo)
        {
            string sql = " select * from DsPaymentToPointConfig where DsSysNo='" + dsSysNo + "'  or " + dsSysNo + " = 0 ";
            return Context.Sql(sql).QuerySingle<DsPaymentToPointConfig>();
        }


    }
}
