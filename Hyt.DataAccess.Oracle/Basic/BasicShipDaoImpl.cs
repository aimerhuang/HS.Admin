using System.Collections.Generic;
using Hyt.DataAccess.BaseInfo;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Basic
{
    /// <summary>
    /// 配送方式
    /// </summary>
    /// <remarks>2013-07-18 朱成果 创建 </remarks>
    public class BasicShipDaoImpl : IBasicShipDao
    {
        /// <summary>
        /// 获取全部配送方式
        /// </summary>
        /// <returns>配送方式列表</returns>
        /// <remarks>  2013-07-18 朱成果 创建 </remarks>
        public override IList<Model.LgDeliveryType> LoadAllDeliveryType()
        {
            return Context.Sql("select * from LgDeliveryType where Status=@Status")
                .Parameter("Status", (int)LogisticsStatus.配送方式状态.启用)
                .QueryMany<Model.LgDeliveryType>();
        }

        /// <summary>
        /// 获取配送方式信息
        /// </summary>
        /// <param name="sysNo">配送方式编号</param>
        /// <returns>配送方式信息</returns>
        /// <remarks>  2013-07-18 朱成果 创建 </remarks>
        public override LgDeliveryType GetEntity(int sysNo)
        {
           return Context.Sql("select * from LgDeliveryType where SysNo=@SysNo").Parameter("SysNo", sysNo).QuerySingle<LgDeliveryType>();
        }
    }
}
