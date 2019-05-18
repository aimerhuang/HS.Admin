using Hyt.DataAccess.Base;
using Hyt.DataAccess.Finance;
using Hyt.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Finance
{
    /// <summary>
    /// 第三方对账
    /// </summary>
    /// <remarks>2014-8-21 朱成果 创建</remarks>
  public   class FnThirdpartyReconciliationDaoImpl : IFnThirdpartyReconciliationDao
    {
          /// <summary>
          /// 添加对账数据
          /// </summary>
          /// <param name="item">对账数据</param>
          /// <returns></returns>
          /// <remarks>2014-8-21 朱成果 创建</remarks>
        public override int Insert(FnThirdpartyReconciliation item)
        {
            string sql = @"
                            declare
                              v_cnt number;
                            begin
                              select count(1)  into v_cnt from  FnThirdpartyReconciliation where Source=@A and FnNo=@B;
                              if v_cnt=0 then
                                       insert into 
                                       FnThirdpartyReconciliation(Source,FnNo,OperationNo,TraderNo,ProductName,TradeDate,BuyerAccount,Amount,CheckDate,Remarks,Status,CreatedBy,CreatedDate,LastUpdateBy,LastUpdateDate) 
                                       values(@Source,@FnNo,@OperationNo,@TraderNo,@ProductName,@TradeDate,@BuyerAccount,@Amount,@CheckDate,@Remarks,@Status,@CreatedBy,@CreatedDate,@LastUpdateBy,@LastUpdateDate) 
                                       returning SysNo into @newId; 
                              end if;
                            end;
                          ";
            var cmd = Context.Sql(sql)
                    .Parameter("A", item.Source)
                    .Parameter("B", item.FnNo)
                    .Parameter("Source", item.Source)
                    .Parameter("FnNo", item.FnNo)
                    .Parameter("OperationNo", item.OperationNo)
                    .Parameter("TraderNo", item.TraderNo)
                    .Parameter("ProductName", item.ProductName)
                    .Parameter("TradeDate", item.TradeDate)
                    .Parameter("BuyerAccount", item.BuyerAccount)
                    .Parameter("Amount", item.Amount)
                    .Parameter("CheckDate", item.CheckDate)
                    .Parameter("Remarks", item.Remarks)
                    .Parameter("Status", item.Status)
                    .Parameter("CreatedBy", item.CreatedBy)
                    .Parameter("CreatedDate", item.CreatedDate)
                    .Parameter("LastUpdateBy", item.LastUpdateBy)
                    .Parameter("LastUpdateDate", item.LastUpdateDate)
                    .ParameterOut("newId", Base.DataTypes.Int32);
                var flg = cmd.Execute();//执行Sql
                    item.SysNo = cmd.ParameterValue<int>("newId");//接收返回值
                    return item.SysNo;
               
           
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-8-21 朱成果 创建</remarks>
        public override Pager<FnThirdpartyReconciliation> Query(Model.Parameter.ParaReconciliationFilter filter)
        {
            string selecttable = "FnThirdpartyReconciliation a";
            var paras = new ArrayList();
            var where = "1=1 ";
            int i = 0;
            if(filter.Source.HasValue)
            {
                where +=" and a.Source=@p0p"+i;
                paras.Add(filter.Source.Value);
                i++;
            }
            if(filter.Status.HasValue)
            {
                where += " and a.Status=@p0p" + i;
                paras.Add(filter.Status.Value);
                i++;
            }

            if(filter.BeginDate.HasValue)
            {
                where += " and a.TradeDate>=@p0p" + i;
                paras.Add(filter.BeginDate.Value);
                i++;
            }

            if (!string.IsNullOrEmpty(filter.FnNo))
            {
                where += " and a.FnNo=@p0p" + i;
                paras.Add(filter.FnNo);
                i++;
            }

            if (!string.IsNullOrEmpty(filter.TraderNo))
            {
                where += " and charindex(a.TraderNo,@p0p" + i + ")>0";
                paras.Add(filter.TraderNo);
                i++;
            }

            if (filter.EndDate.HasValue)
            {
                where += " and a.TradeDate<=@p0p" + i;
                paras.Add(filter.EndDate.Value);
                i++;
            }
            Pager<FnThirdpartyReconciliation> pager = new Pager<FnThirdpartyReconciliation>();
            pager.CurrentPage = filter.Id;
            pager.PageSize = filter.PageSize;
            pager.TotalRows = Context.Select<int>("count(0)").From(selecttable)
                .Where(where)
                .Parameters(paras)
                .QuerySingle();
            pager.Rows = Context.Select<FnThirdpartyReconciliation>("a.*")
                .From(selecttable)
                 .Where(where)
                .Parameters(paras)
                .OrderBy("a.TradeDate asc")
                .Paging(filter.Id, filter.PageSize)
                .QueryMany();
            return pager;

        }

        /// <summary>
        /// 加盟商对账(支付宝)
        /// </summary>
        /// <param name="item">对账数据</param>
        /// <remarks>2014-8-21 朱成果 创建</remarks
        public override void CheckAlipayReconciliation(FnThirdpartyReconciliation item)
        {
            var sql = @"
                    DECLARE
                         v_OrderTransactionSysNo  VARCHAR2(20);--事物编号
                         v_cnt number;
                    begin
                       select a.OrderTransactionSysNo into v_OrderTransactionSysNo
                       from DsOrder a
                       inner join DsDealerMall b
                       on a.dealermallsysno=b.sysno
                       where a.MallOrderId=@MallOrderId and a.Payment=@Payment and a.Status<>-10 and b.malltypesysno in (1,2,9);--淘宝，天猫，阿里巴巴
                       select count(1)  into v_cnt from  FnReceiptVoucher where IncomeAmount=ReceivedAmount and TransactionSysNo=v_OrderTransactionSysNo and Status<>-10;--是否存在
                       if v_cnt>0 then
                           update FnReceiptVoucher  set Status=@Status,ConfirmedDate=@TradeDate,LastUpdateDate=sysdate,Remark=@Remark where IncomeAmount=ReceivedAmount and TransactionSysNo=v_OrderTransactionSysNo and Status=10;
                           update FnThirdpartyReconciliation set Status=@Status1,CheckDate=sysdate,LastUpdateDate=sysdate where sysno=@SysNo;
                       end if;
                       EXCEPTION
                              WHEN no_data_found THEN
                                update FnThirdpartyReconciliation set Status=@Status2,LastUpdateDate=sysdate  where sysno=@SysNo1;--失败
                    end;
                    ";
            Context.Sql(sql)
                    .Parameter("MallOrderId", item.TraderNo)
                    .Parameter("Payment", item.Amount)
                    .Parameter("Status", (int)Model.WorkflowStatus.FinanceStatus.收款单状态.已确认)
                    .Parameter("TradeDate", item.TradeDate)
                    .Parameter("Remark", item.Remarks)
                    .Parameter("Status1", (int)Model.WorkflowStatus.FinanceStatus.第三方财务对账状态.已对账)
                    .Parameter("SysNo", item.SysNo)
                    .Parameter("Status2", (int)Model.WorkflowStatus.FinanceStatus.第三方财务对账状态.失败)
                    .Parameter("SysNo1", item.SysNo)
                    .Execute();
        }
    }
}
