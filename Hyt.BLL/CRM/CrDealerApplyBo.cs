using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.BLL.Web;
using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Pager;
using Hyt.DataAccess.LevelPoint;
using Hyt.Model.WorkflowStatus;
using Pisen.Framework.Service.Proxy;
using Pisen.Service.Share.SSO.Contract;
using Pisen.Service.Share.SSO.Contract.DataContract;
using Hyt.Model.SellBusiness;
using Hyt.Model.Parameter;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 会员分销商申请业务逻辑类
    /// </summary>
    /// <remarks>2016-04-08 刘伟豪 创建</remarks>
    public class CrDealerApplyBo : BOBase<CrDealerApplyBo>
    {
        /// <summary>
        /// 查询会员分销商申请
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-08 刘伟豪 创建</remarks>
        public void Seach(ref Pager<CBCrDealerApply> pager, ParaCrDealerApplyFilter filter)
        {
            ICrDealerApplyDao.Instance.Seach(ref pager, filter);
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">会员分销商申请实体</param>
        /// <remarks> 2016-04-12 刘伟豪 创建 </remarks>
        public int Insert(CrDealerApply model)
        {
            return ICrDealerApplyDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">会员分销商申请实体</param>
        /// <remarks> 2016-04-12 刘伟豪 创建 </remarks>
        public bool Update(CrDealerApply model)
        {
            return ICrDealerApplyDao.Instance.Update(model);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <remarks> 2016-04-12 刘伟豪 创建 </remarks>
        public CrDealerApply GetModel(int sysno)
        {
            return ICrDealerApplyDao.Instance.GetModel(sysno);
        }
    }
}