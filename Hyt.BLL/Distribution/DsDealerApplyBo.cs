using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using Hyt.DataAccess.MallSeller;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Distribution;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Memory;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商信息维护业务层
    /// </summary>
    /// <remarks>
    /// 2016-04-16 王耀发 创建 
    /// </remarks>
    public class DsDealerApplyBo : BOBase<DsDealerApplyBo>
    {
        /// <summary>
        /// 查询分销商申请信息
        /// </summary>
        /// <param name="filter">查询参数实体</param>
        /// <returns>分销商申请信息列表</returns>
        /// <remarks> 
        /// 2016-04-16 王耀发 创建 
        public void GetDsDealerApplyList(ref Pager<CBDsDealerApply> pager, ParaDsDealerApplyFilter filter)
        {
            IDsDealerApplyDao.Instance.GetDsDealerApplyList(ref pager, filter);
        }

        /// <summary>
        /// 更新分销商申请状态
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="NsStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-4-6 王耀发 创建</remarks>
        public void UpdateStatus(int SysNo, int Status, int HandlerSysNo)
        {
            IDsDealerApplyDao.Instance.UpdateStatus(SysNo, Status, HandlerSysNo);
        }
        /// <summary>
        /// 查询分销商下有无领取活动商品
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="filter"></param>

        public void GetDealerReceiveProductList(ref Pager<CBDsReceiveProduct> pager, ParaReceiveProductFilter filter, string ContactKey)
        {
            IDsDealerApplyDao.Instance.GetDealerReceiveProductList(ref pager, filter, ContactKey);
        }
    }
}
