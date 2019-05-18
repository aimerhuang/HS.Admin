using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 返利记录信息
    /// </summary>
    /// <param name="filter">返利记录信息</param>
    /// <returns>返回返利记录信息</returns>
    /// <remarks>2015-09-15 王耀发 创建</remarks>
    public abstract class IDsDealerRebatesRecordDao : Hyt.DataAccess.Base.DaoBase<IDsDealerRebatesRecordDao>
    {
        /// <summary>
        /// 获取返利记录列表
        /// </summary>
        /// <param name="sysNo">返利记录系统编号</param>
        /// <returns>返利记录列表</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract Pager<CBDsDealerRebatesRecord> GetDsDealerRebatesRecordList(ParaDealerRebatesRecordFilter filter);
        /// <summary>
        /// 分销汇总明细
        /// </summary>
        /// <param name="filter"></param>
        /// <remarks>2016-07-15 周 创建</remarks>
        public abstract Pager<CBCCustomerRebatesRecord> GetDealerInfoSummaryList(ParaCBCCustomerRebatesRecordFilter filter);
  
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除记录</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 返利记录信息
        /// </summary>
        /// <param name="filter">返利记录信息</param>
        /// <returns>返回返利记录信息</returns>
        /// <remarks>2015-09-15 王耀发 创建</remarks>
        public abstract IList<CBDsDealerRebatesRecord> GetDsDealerRebatesRecordView(ParaDealerRebatesRecordFilter filter);
         /// <summary>
        /// 分销返利分页
        /// </summary>
        /// <param name="pager"></param>
        /// <remarks>2016-07-15 周 创建</remarks>
        public abstract void GeDirectOrdersList(ref Model.Pager<CBDsDealerRebatesRecord> pager);

         /// <summary>
        /// 分销团队
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pager"></param>
        /// <remarks>2016-07-15 周 创建</remarks>
        public abstract void GetMyDistTemsList(int? typeid, ref Model.Pager<CBCCrCustomerList> pager);
    }
}

