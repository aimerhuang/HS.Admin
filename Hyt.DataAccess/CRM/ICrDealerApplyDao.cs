using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 会员分销商申请
    /// </summary>
    /// <remarks> 2016-04-08 刘伟豪 创建 </remarks>
    public abstract class ICrDealerApplyDao : DaoBase<ICrDealerApplyDao>
    {
        /// <summary>
        /// 查询会员分销商申请
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-08 刘伟豪 创建</remarks>
        public abstract void Seach(ref Pager<CBCrDealerApply> pager, ParaCrDealerApplyFilter filter);

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">会员分销商申请实体</param>
        /// <remarks> 2016-04-12 刘伟豪 创建 </remarks>
        public abstract int Insert(CrDealerApply model);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">会员分销商申请实体</param>
        /// <remarks> 2016-04-12 刘伟豪 创建 </remarks>
        public abstract bool Update(CrDealerApply model);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <remarks> 2016-04-12 刘伟豪 创建 </remarks>
        public abstract CrDealerApply GetModel(int sysno);
    }
}