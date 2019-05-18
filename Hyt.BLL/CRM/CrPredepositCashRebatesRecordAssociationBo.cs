using Hyt.DataAccess.CRM;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 提现返利记录关联
    /// </summary>
    /// <remarks>2016-1-9 杨浩 创建</remarks>
    public class CrPredepositCashRebatesRecordAssociationBo : BOBase<CrPredepositCashRebatesRecordAssociationBo>
    {
        /// <summary>
        /// 更加提现订单号和客户系统编号获取实体
        /// </summary>
        /// <param name="crPredepositCashSysNo">提现订单编号</param>
        /// <param name="customerSysNo">客户系统编号</param>
        public  CrPredepositCashRebatesRecordAssociation GetModel(int crPredepositCashSysNo, int customerSysNo)
        {
            return ICrPredepositCashRebatesRecordAssociationDao.Instance.GetModel(crPredepositCashSysNo, customerSysNo);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        public  int Insert(CrPredepositCashRebatesRecordAssociation model)
        {
            return ICrPredepositCashRebatesRecordAssociationDao.Instance.Insert(model);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        public int Update(CrPredepositCashRebatesRecordAssociation model)
        {
            return ICrPredepositCashRebatesRecordAssociationDao.Instance.Update(model);
        }
    }
}
