using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 分销商信息维护接口层
    /// </summary>
    /// <remarks>
    /// 2013-09-04 郑荣华 创建
    /// </remarks>
    public abstract class IDsDealerApplyDao : DaoBase<IDsDealerApplyDao>
    {
        /// <summary>
        /// 查询分销商申请信息
        /// </summary>
        /// <param name="filter">查询参数实体</param>
        /// <returns>分销商申请信息列表</returns>
        /// <remarks> 
        /// 2016-04-16 王耀发 创建 
        public abstract void GetDsDealerApplyList(ref Pager<CBDsDealerApply> pager, ParaDsDealerApplyFilter filter);

        /// <summary>
        /// 更新分销商申请状态
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="NsStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-4-6 王耀发 创建</remarks>
        public abstract void UpdateStatus(int SysNo, int Status, int HandlerSysNo);
        /// <summary>
        /// 查询分销商下有无领取活动商品
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="filter"></param>

        public abstract void GetDealerReceiveProductList(ref Pager<CBDsReceiveProduct> pager, ParaReceiveProductFilter filter, string ContactKey);

    }
}
