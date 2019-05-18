using Hyt.DataAccess.Base;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.CRM
{
    /// <summary>
    /// 提现返利记录关联
    /// </summary>
    /// <remarks>2016-1-9 杨浩 创建</remarks>
    public abstract class ICrPredepositCashRebatesRecordAssociationDao : DaoBase<ICrPredepositCashRebatesRecordAssociationDao>
    {
        /// <summary>
        /// 更加提现订单号和客户系统编号获取实体
        /// </summary>
        /// <param name="crPredepositCashSysNo">提现订单编号</param>
        /// <param name="customerSysNo">客户系统编号</param>
        public abstract CrPredepositCashRebatesRecordAssociation GetModel(int crPredepositCashSysNo, int customerSysNo);
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        public abstract int Insert(CrPredepositCashRebatesRecordAssociation model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        public abstract int Update(CrPredepositCashRebatesRecordAssociation model);
    }
}
