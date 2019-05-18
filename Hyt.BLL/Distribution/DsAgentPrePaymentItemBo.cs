using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 代理商预存款来往明细业务类
    /// </summary>
    /// <remarks> 2016-04-18 刘伟豪 创建 </remarks>
    public class DsAgentPrePaymentItemBo : BOBase<DsAgentPrePaymentItemBo>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pager">分页实体</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public void Search(ref Pager<CBDsAgentPrePaymentItem> pager, ParaDsAgentPrePaymentItemFilter filter)
        {
            IDsAgentPrePaymentItemDao.Instance.Search(ref pager, filter);
        }
    }
}