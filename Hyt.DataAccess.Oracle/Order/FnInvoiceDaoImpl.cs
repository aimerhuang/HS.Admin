using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Order;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 发票相关
    /// </summary>
    /// <remarks>2013-06-21 朱成果 创建</remarks>
    public class FnInvoiceDaoImpl : IFnInvoiceDao
    {

        /// <summary>
        /// 根据订单号获取订单发票信息
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>根据订单号获取订单发票信息</returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public override Model.FnInvoice GetFnInvoiceByOrderID(int orderID)
        {
            var model = Context.Sql(@"select t.* from fninvoice t
inner join soorder p
on p.InvoiceSysNo=t.SysNo
where p.sysno=@sysno").Parameter("sysno", orderID).QuerySingle<Model.FnInvoice>();
            return model;
        }

        /// <summary>
        /// 根据系统编号获取发票信息
        /// </summary>
        /// <param name="sysNo">发票编号</param>
        /// <returns>系统编号获取发票信息</returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public override Model.FnInvoice GetFnInvoice(int sysNo)
        {
            return Context.Sql(@"select * from fninvoice where sysno=@sysno").Parameter("sysno", sysNo).QuerySingle<Model.FnInvoice>();
        }

        /// <summary>
        /// 插入发票信息
        /// </summary>
        /// <param name="entity">发票实体</param>
        /// <returns>发票编号</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public override int InsertEntity(Model.FnInvoice entity)
        {
            entity.SysNo = Context.Insert("FnInvoice", entity)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新发票信息
        /// </summary>
        /// <param name="entity">发票实体</param>
        /// <returns></returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public override void UpdateEntity(Model.FnInvoice entity)
        {
            Context.Update("FnInvoice", entity)
                 .AutoMap(x => x.SysNo)
                 .Where("SysNo", entity.SysNo)
                 .Execute();
        }
        /// <summary>
        /// 删除发票信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-13 王耀发 创建</remarks>
        public override int DeleteEntity(int sysNo)
        {
            return Context.Delete("FnInvoice").Where("SysNo", sysNo).Execute();
        }
        /// <summary>
        /// 获取发票类型列表
        /// </summary>
        /// <returns>发票类型列表</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public override IList<Model.FnInvoiceType> GetFnInvoiceTypeList()
        {
            return Context.Sql("select * from FnInvoiceType").QueryMany<Model.FnInvoiceType>();
        }

        /// <summary>
        /// 获取发票类型信息
        /// </summary>
        /// <param name="sysNo">发票类型编号</param>
        /// <returns>发票类型实体</returns>
        /// <remarks>2013-06-25 朱成果 创建</remarks>
        public override Model.FnInvoiceType GetFnInvoiceType(int sysNo)
        {
            return Context.Sql(@"select * from FnInvoiceType where sysno=@sysno").Parameter("sysno", sysNo).QuerySingle<Model.FnInvoiceType>();
        }

        /// <summary>
        /// 获取订单有效发票(已开票的发票)
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns>发票</returns>
        /// <remarks>2013-10-29 吴文强 创建</remarks>
        public override FnInvoice GetOrderValidInvoice(int orderSysNo)
        {
            const string sql = @"
                            select fi.* 
                            from WhStockOut wso
                             inner join FnInvoice fi on wso.InvoiceSysNo = fi.SysNo
                            where wso.ordersysno = @ordersysno
                              and fi.status = @status
                            ";

            return Context.Sql(sql)
                          .Parameter("ordersysno", orderSysNo)
                          .Parameter("status", (int)FinanceStatus.发票状态.已开票)
                          .QuerySingle<FnInvoice>();
        }
    }
}
