using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.SellBusiness
{
    /// <summary>
    /// 会员提现信息
    /// </summary>
    /// <param name="filter">会员提现信息</param>
    /// <returns>返回会员提现信息</returns>
    /// <remarks>2015-09-15 王耀发 创建</remarks>
    public abstract class ICrPredepositCashDao : Hyt.DataAccess.Base.DaoBase<ICrPredepositCashDao>
    {
        /// <summary>
        /// 获取分销商列表
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商列表</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract Pager<CrPredepositCash> GetCrPredepositCashList(ParaCrPredepositCashFilter filter);
        /// <summary>
        /// 更新支付状态
        /// </summary>
        /// <param name="SysNo">会员提现单编号</param>
        /// <param name="PdcPayState">状态值</param>
        /// <returns></returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        public abstract void UpdatePdcPayState(int SysNo, int PdcPayState);
        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2016-1-6 王耀发 创建</remarks>
        public abstract int Insert(CrPredepositCash entity);
        /// <summary>
        /// 更新提现订单审核状态
        /// </summary>
        /// <param name="sysNo">提现订单系统编号</param>
        /// <param name="status">审核状态（0:未审核 1:已审核 -1 作废）</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        public abstract bool UpdateStatus(int sysNo, int status);
        /// <summary>
        /// 获取提现订单详情
        /// </summary>
        /// <param name="sysNo">提现订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        public abstract CrPredepositCash GetModel(int sysNo);
    }
}

