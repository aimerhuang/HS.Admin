using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 提现返利记录关联
    /// </summary>
    /// <remarks>2016-1-9 杨浩 创建</remarks>
    public class CrPredepositCashRebatesRecordAssociationImpl : ICrPredepositCashRebatesRecordAssociationDao
    {
        /// <summary>
        /// 更加提现订单号和客户系统编号获取实体
        /// </summary>
        /// <param name="crPredepositCashSysNo">提现订单编号</param>
        /// <param name="customerSysNo">客户系统编号</param>
        public override CrPredepositCashRebatesRecordAssociation GetModel(int crPredepositCashSysNo, int customerSysNo)
        {
            return Context.Sql(string.Format("select CustomerSysNo,CrCustomerRebatesRecordSysNos,CrPredepositCashSysNo  from CrPredepositCashRebatesRecordAssociation  where CustomerSysNo={0} and CrPredepositCashSysNo={1}", customerSysNo, crPredepositCashSysNo))
                      .QuerySingle<CrPredepositCashRebatesRecordAssociation>();
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="model">插入的数据模型</param>
        /// <returns>返回受影响行</returns>
        public override int Insert(CrPredepositCashRebatesRecordAssociation model)
        {
            return Context.Insert<CrPredepositCashRebatesRecordAssociation>("CrPredepositCashRebatesRecordAssociation", model)                        
                         .Execute();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">更新的数据模型</param>
        /// <returns>返回受影响行</returns>
        public override int Update(CrPredepositCashRebatesRecordAssociation model)
        {
            return Context.Sql(string.Format("Update CrPredepositCashRebatesRecordAssociation set CrCustomerRebatesRecordSysNos={0} where CustomerSysNo={1} and CrPredepositCashSysNo={2}",model.CrCustomerRebatesRecordSysNos,model.CustomerSysNo,model.CrPredepositCashSysNo))                       
                         .Execute();
        }
    }
}