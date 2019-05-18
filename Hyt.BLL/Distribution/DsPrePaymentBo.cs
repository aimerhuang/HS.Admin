using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 分销商预存款主表DsPrePaymentBo
    /// </summary>
    /// <remarks>2013-09-11 杨浩 创建</remarks>
    public class DsPrePaymentBo : BOBase<DsPrePaymentBo>
    {
        /// <summary>
        /// 创建分销商预存款
        /// </summary>
        /// <param name="model">分销商预存款主表实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-09-11 周唐炬 创建</remarks>
        public int Create(DsPrePayment model)
        {
            var sysno = IDsPrePaymentDao.Instance.Insert(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建分销商预存款", LogStatus.系统日志目标类型.分销商预存款, sysno);
            return sysno;
        }

        /// <summary>
        /// 更新分销商预存款
        /// </summary>
        /// <param name="model">分销商预存款主表实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2016-1-7 杨浩 创建</remarks>
        public int Update(DsPrePayment model)
        {
            var sysno = IDsPrePaymentDao.Instance.Update(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "更新分销商预存款", LogStatus.系统日志目标类型.分销商预存款, sysno);
            return sysno;
        }

        /// <summary>
        /// 获取分销商预存款
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>返回实体</returns>
        /// <remarks>2013-09-11 周唐炬 创建</remarks>
        public DsPrePayment GetDsPrePayment(int dealerSysNo)
        {
            return IDsPrePaymentDao.Instance.GetEntityByDealerSysNo(dealerSysNo);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-10  朱成果 创建</remarks>
        public DsPrePayment GetEntity(int sysNo)
        {
            return IDsPrePaymentDao.Instance.GetEntity(sysNo);
        }
        /// <summary>
        /// 更新付款单数值
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-7 王耀发  创建</remarks>
        public void UpdatePaymentValue(int SysNo, decimal Value)
        {
            IDsPrePaymentDao.Instance.UpdatePaymentValue(SysNo, Value);
        }
        /// <summary>
        /// 更新付款单数值
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Value"></param>
        /// <remarks>2016-1-7 王耀发  创建</remarks>
        public void UpdatePaymentValueConfirm(int SysNo, decimal Value)
        {
            IDsPrePaymentDao.Instance.UpdatePaymentValueConfirm(SysNo, Value);
        }
    }
}
