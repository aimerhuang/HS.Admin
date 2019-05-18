using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Finance;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Finance
{
    /// <summary>
    /// 收款明细
    /// </summary>
    /// <remarks>2013-07-08 朱成果 创建</remarks>
    public class FnReceiptVoucherItemDaoImpl : IFnReceiptVoucherItemDao
    {
        /// <summary>
        /// 插入收款明细
        /// </summary>
        /// <param name="entity">收款明细</param>
        /// <returns>收款明细编号</returns>
        /// <remarks>2013-07-08 朱成果 创建</remarks>
        public override int Insert(Model.FnReceiptVoucherItem entity)
        {
            if (entity.LastUpdateDate == DateTime.MinValue)
            {
                entity.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var sysNo = Context.Insert("FnReceiptVoucherItem", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }

        /// <summary>
        /// 获取收款明细列表
        /// </summary>
        /// <param name="receiptNo">收款单编号</param>
        /// <returns>收款明细列表</returns>
        /// <remarks>2013-07-17 朱成果 创建</remarks>
        public override List<FnReceiptVoucherItem> GetListByReceiptNo(int receiptNo)
        {
            return Context.Sql("select * from FnReceiptVoucherItem where Status=1 and ReceiptVoucherSysNo=@ReceiptVoucherSysNo")
                .Parameter("ReceiptVoucherSysNo", receiptNo).QueryMany<FnReceiptVoucherItem>();
        }

        /// <summary>
        /// 获取收款明细实体
        /// </summary>
        /// <param name="sysNo">收款明细编号</param>
        /// <returns>收款明细实体</returns>
        /// <remarks>2013-08-08 余勇 创建</remarks>
        public override FnReceiptVoucherItem Get(int sysNo)
        {
            return Context.Sql("select * from FnReceiptVoucherItem where SysNo=@0", sysNo)
                          .QuerySingle<FnReceiptVoucherItem>();
        }

        /// <summary>
        /// 获取收款明细列表
        /// </summary>
        /// <param name="receiptVoucherSysNo">收款单编号</param>
        /// <returns>收款明细列表</returns>
        /// <remarks>2013-7-22 余勇 创建 </remarks>
        public override List<CBFnReceiptVoucherItem> GetVoucherItems(int receiptVoucherSysNo)
        {
            return Context.Sql(@"select a.*,b.paymentname as PaymentTypeName from FnReceiptVoucherItem a left join BsPaymentType b
                                 on a.PaymentTypeSysNo=b.SysNo where a.Status=1 and a.ReceiptVoucherSysNo=@ReceiptVoucherSysNo")
            .Parameter("ReceiptVoucherSysNo", receiptVoucherSysNo).QueryMany<CBFnReceiptVoucherItem>();
        }

        /// <summary>
        /// 删除收款明细
        /// </summary>
        /// <param name="sysNo">收款明细编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Delete("FnReceiptVoucherItem").Where("SysNo", sysNo).Execute();
        }

        /// <summary>
        /// 作废收款明细
        /// </summary>
        /// <param name="sysNo">收款明细编号</param>
        /// <param name="status">状态1有效0无效</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-08 余勇 创建</remarks>
        public override int Invalid(int sysNo, int status)
        {
            return Context.Update("FnReceiptVoucherItem").Column("Status", status).Where("SysNo", sysNo).Execute();
        }

        /// <summary>
        /// 删除收款单明细
        /// </summary>
        /// <param name="fnReceiptVoucherSysNo">收款单系统编号</param>
        /// <returns>返回删除的条数</returns>
        /// <remarks>2013-07-26 黄伟 创建</remarks>
        public override int DeleteItemsBySysNo(int fnReceiptVoucherSysNo)
        {
            return
                Context.Sql(@"delete FnReceiptVoucherItem where receiptvouchersysno=@fnReceiptVoucherSysNo")
                       .Parameter("fnReceiptVoucherSysNo", fnReceiptVoucherSysNo)
                       .Execute();
        }

        /// <summary>
        /// 修改收款单明细
        /// </summary>
        /// <param name="item">收款单明细实体</param>
        /// <returns>void</returns>
        /// <remarks>2013-08-07 黄伟 创建</remarks>
        public override void Update(FnReceiptVoucherItem item)
        {
            Context.Update<FnReceiptVoucherItem>("FnReceiptVoucherItem", item)
                          .AutoMap(p => p.SysNo).Where("SysNo", item.SysNo).Execute();
        }
    }
}
