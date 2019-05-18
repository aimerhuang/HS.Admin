using Hyt.DataAccess.Balance;
using Hyt.Model;
using Hyt.Model.Generated;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hyt.DataAccess.Oracle.Balance
{
    /// <summary>
    /// 会员余额
    /// </summary>
    /// <remarks>2016-06-06 周 创建</remarks>
    public class CrRechargeDaolmpl : CrRechargeDao
    {
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int CreateCrRecharge(CrRecharge model)
        {
            return Context.Insert<CrRecharge>("CrRecharge", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 充值详情
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override CrRecharge GetCrRechargeEntity(int SysNo)
        {
            return Context.Sql("select * from CrRecharge where SysNo=@0", SysNo).QuerySingle<CrRecharge>();
        }
        /// <summary>
        /// 更新充值记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int UpdateCrRecharge(CrRecharge model)
        {
            return Context.Update<CrRecharge>("CrRecharge", model).AutoMap(x => x.SysNo).Where(x => x.SysNo).Execute();
        }
        /// <summary>
        /// 更新充值记录
        /// </summary>
        /// <param name="rSysNo"></param>
        /// <param name="State"></param>
        /// <param name="ReAmount"></param>
        /// <param name="OutTradeNo"></param>
        /// <returns></returns>
        public override int UpdateCrRecharge(int rSysNo, int State, decimal ReAmount, string OutTradeNo)
        {
            return Context.Sql("update CrRecharge set OutTradeNo=@1,ReAmount=@2,State=@3 where SysNo=@0", rSysNo, OutTradeNo, ReAmount, State).QuerySingle<int>();
        }


        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public override Pager<CrRecharge> GetCrRechargeList(Pager<CrRecharge> pager)
        {
            #region sql条件
            string sql = @"IsDelete = 0 and CustomerSysNo=" + pager.PageFilter.CustomerSysNo + "";
            //DateTime dt = new DateTime(0);

            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {

                pager.Rows = _context.Select<CrRecharge>("rc.*")
                              .From("CrRecharge rc")
                              .Where(sql)
                              .OrderBy(" ReAddTime  ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From("CrRecharge")
                              .Where(sql)
                              .QuerySingle();
            }
            return pager;
        }
        /// <summary>
        /// 创建余额记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int CreateCrAccountBalance(CrAccountBalance model)
        {
            return Context.Insert<CrAccountBalance>("CrAccountBalance", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public override CrAccountBalance GetCrABalanceEntity(int CustomerSysNo)
        {
            return Context.Sql("select * from CrAccountBalance where CustomerSysNo=@0", CustomerSysNo).QuerySingle<CrAccountBalance>();
        }
        /// <summary>
        /// 是否存在会员记录
        /// </summary>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public override int IsExistenceABalance(int CustomerSysNo)
        {
            return Context.Sql("select count(SysNo) from CrAccountBalance where CustomerSysNo=@0", CustomerSysNo).QuerySingle<int>();
        }
        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int UpdateABalance(CrAccountBalance model)
        {
            return Context.Update<CrAccountBalance>("CrAccountBalance", model).AutoMap(x => x.CustomerSysNo).Where(x => x.CustomerSysNo).Execute();
        }
        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="AvailableBalance"></param>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public override int UpdateAccountBalance(decimal AvailableBalance, int CustomerSysNo)
        {
            return Context.Sql("update CrAccountBalance set AvailableBalance=AvailableBalance+@0,TolBlance=TolBlance+@0 where CustomerSysNo=@1", AvailableBalance, CustomerSysNo).QuerySingle<int>();
        }
        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="AvailableBalance"></param>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public override int UpdateABalanceForPayOrder(decimal AvailableBalance, int CustomerSysNo)
        {
            return Context.Sql("update CrAccountBalance set AvailableBalance=AvailableBalance-@0 where CustomerSysNo=@1", AvailableBalance, CustomerSysNo).QuerySingle<int>();
        }
        /// <summary>
        /// 增加会员余额支付订单记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int CreateCrBalancePayOrderLog(CrBalancePayOrderLog model)
        {
            return Context.Insert<CrBalancePayOrderLog>("CrBalancePayOrderLog", model).AutoMap(x => x.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="pager"></param>
        public override void GetCrRechargeLog(ref Model.Pager<CrRecharge> pager)
        {
            #region sql条件

            string sqlWhere = @"(@CustomerSysNo=-1 or cp.CustomerSysNo=@CustomerSysNo) and (@State=-1 or cp.State=@State) and IsDelete=0";

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CrRecharge>("cp.*")
                                    .From("CrRecharge cp")
                                    .Where(sqlWhere)
                                    .Parameter("CustomerSysNo", pager.PageFilter.CustomerSysNo)
                                    .Parameter("State", pager.PageFilter.State)
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .OrderBy("cp.ReAddTime desc")
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrRecharge cp")
                                         .Where(sqlWhere)
                                         .Parameter("CustomerSysNo", pager.PageFilter.CustomerSysNo)
                                         .Parameter("State", pager.PageFilter.State)
                                         .QuerySingle();
            }
        }

        /// <summary>
        /// 是否存在会员保证金返利记录
        /// </summary>
        /// <param name="CustomerSysNo">用户ID</param>
        /// <param name="tradeNo">外部订单号</param>
        /// <returns>2016-7-6 罗远康 创建</returns>
        public override int IsDealerCrRecharge(int CustomerSysNo, string OutTradeNo)
        {
            return Context.Sql("SELECT COUNT(SysNo) FROM CrRecharge where CustomerSysNo=@0 and OutTradeNo=@1", CustomerSysNo, OutTradeNo).QuerySingle<int>();
        }

        /// <summary>
        /// 分页查询充值记录
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-13 刘伟豪 创建 </remarks>
        public override void Search(ref Pager<CBCrRecharge> pager, ParaCrRechargeFilter filter)
        {
            using (var _context = Context.UseSharedConnection(true))
            {
                var sqlWhere = " Re.State = 1 ";

                if (filter.CustomerSysNo > 0)
                    sqlWhere += " and Re.CustomerSysNo = @CustomerSysNo ";

                if (!string.IsNullOrWhiteSpace(filter.KeyWord))
                    sqlWhere += " and ( Cr.Name like @KeyWord or Cr.NickName like @KeyWord or Cr.MobilePhoneNumber like @KeyWord or Cr.Account like @KeyWord  ) ";

                if (filter.RePaymentId > 0)
                    sqlWhere += "and RePaymentId = @RePaymentId ";

                pager.Rows = _context.Select<CBCrRecharge>(" Re.*,Cr.Name,Cr.NickName,Cr.MobilePhoneNumber,Cr.Account,Bl.AvailableBalance ")
                                   .From(@" CrRecharge Re
                                            Left Join CrCustomer Cr On Re.CustomerSysNo = Cr.SysNo
                                            Left Join CrAccountBalance Bl On Re.CustomerSysNo = Bl.CustomerSysNo")
                                   .Where(sqlWhere)
                                   .Parameter("CustomerSysNo", filter.CustomerSysNo)
                                   .Parameter("KeyWord", "%" + filter.KeyWord + "%")
                                   .Parameter("RePaymentId", filter.RePaymentId)
                                   .OrderBy(" Re.ReAddTime Desc , Re.SysNo Desc ")
                                   .Paging(pager.CurrentPage, pager.PageSize)
                                   .QueryMany();

                pager.TotalRows = _context.Select<int>(" count(0) ")
                                   .From(@" CrRecharge Re
                                            Left Join CrCustomer Cr On Re.CustomerSysNo = Cr.SysNo
                                            Left Join CrAccountBalance Bl On Re.CustomerSysNo = Bl.CustomerSysNo")
                                   .Where(sqlWhere)
                                   .Parameter("CustomerSysNo", filter.CustomerSysNo)
                                   .Parameter("KeyWord", "%" + filter.KeyWord + "%")
                                   .Parameter("RePaymentId", filter.RePaymentId)
                                   .QuerySingle();
            }
        }
    }
}