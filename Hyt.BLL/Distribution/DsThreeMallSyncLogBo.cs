using Hyt.DataAccess.Distribution;
using Hyt.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 同步三方商城
    /// </summary>
    /// <remarks>2016-7-8 杨浩 创建</remarks>
    public class DsThreeMallSyncLogBo : BOBase<DsThreeMallSyncLogBo>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">实体</param>
        public int Add(DsThreeMallSyncLog model)
        {
           return IDsThreeMallSyncLogDao.Instance.Add(model);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        public int Update(DsThreeMallSyncLog model)
        {
            return IDsThreeMallSyncLogDao.Instance.Update(model);
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <param name="mallTypeSysNo">经销商商城类型编号</param>
        /// <param name="syncType">同步类型</param>
        /// <remarks></remarks>
        public DsThreeMallSyncLog GetThreeMallSyncLogInfo(int dealerSysNo, int mallSysNo, int syncType)
        {
            return IDsThreeMallSyncLogDao.Instance.GetThreeMallSyncLogInfo(dealerSysNo,mallSysNo,syncType);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="mallSysNo">商城编号</param>
        /// <param name="syncType">同步类型(订单:10 退货货单：20)</param>
        public DsThreeMallSyncLog GetThreeMallSyncLogInfo(int mallSysNo,int syncType)
        {
            return IDsThreeMallSyncLogDao.Instance.GetThreeMallSyncLogInfo(0,mallSysNo,syncType);
        }
    }
}
