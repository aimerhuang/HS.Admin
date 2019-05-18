using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Generated;
using Hyt.DataAccess.Atten;
using Hyt.DataAccess.Logistics;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 全局配置，第三方物流信息实例化
    /// </summary>
    /// <remarks>2015-09-30 杨云奕 添加</remarks>
    public class ThirdDeliveryConfigDaoImpl : IThirdDeliveryConfigDao
    {
        public override Hyt.Model.ThirdDeliveryConfig GetThirdDeliveryConfigBySysNo(int sysNo)
        {
            return Context.Sql("select * from ThirdDeliveryConfig where SysNo=@0", sysNo)
                          .QuerySingle<ThirdDeliveryConfig>();
        }
    }
}
