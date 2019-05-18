using Hyt.DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Logistics
{
    /// <summary>
    /// 全局配置，第三方物流信息接口定义
    /// </summary>
    /// <remarks>2015-09-30 杨云奕 添加</remarks>
    public abstract class IThirdDeliveryConfigDao : DaoBase<IThirdDeliveryConfigDao>
    {
        public abstract Hyt.Model.ThirdDeliveryConfig GetThirdDeliveryConfigBySysNo(int sysNo);
    }
}
