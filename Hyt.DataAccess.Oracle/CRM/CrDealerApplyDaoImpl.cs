using Hyt.DataAccess.CRM;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 会员分销商申请
    /// </summary>
    /// <remarks> 2016-04-08 刘伟豪 创建</remarks>
    public class CrDealerApplyDaoImpl : ICrDealerApplyDao
    {
        /// <summary>
        /// 会员分销商申请查询
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询参数</param>
        public override void Seach(ref Pager<CBCrDealerApply> pager, ParaCrDealerApplyFilter condition)
        {
            using (var _context = Context.UseSharedConnection(true))
            {
                var sqlWhere = " 1=1 ";

                if (condition.SysNo > 0)
                    sqlWhere += " and Ap.SysNo=@SysNo ";

                if (condition.CustomerSysNo > 0)
                    sqlWhere += " and Ap.CustomerSysNo=@CustomerSysNo ";

                if (condition.Status != 0)
                    sqlWhere += " and Ap.Status=@Status ";

                if (condition.AppliedDateStartTime != null)
                    sqlWhere += " and Ap.AppliedDate >= @AppliedDateStartTime";

                if (condition.AppliedDateEndTime != null)
                    sqlWhere += " and Ap.AppliedDate <= @AppliedDateEndTime";

                if (condition.AuditedDateStartTime != null)
                    sqlWhere += " and Ap.AuditedDate >= @AuditedDateStartTime";

                if (condition.AuditedDateEndTime != null)
                    sqlWhere += " and  Ap.AuditedDate <= @AuditedDateEndTime";

                if (!string.IsNullOrWhiteSpace(condition.KeyWord))
                    sqlWhere += " and (Cr.Name like @KeyWord or Cr.MobilePhoneNumber like @KeyWord)";

                if (condition.LevelSysNo > 0)
                    sqlWhere += " and Cr.LevelSysNo=@LevelSysNo";

                if (!string.IsNullOrWhiteSpace(condition.Name))
                    sqlWhere += " and Cr.Name like @Name";

                if (!string.IsNullOrWhiteSpace(condition.MobilePhoneNumber))
                    sqlWhere += " and Cr.MobilePhoneNumber like @MobilePhoneNumber";

                if (condition.DealerSysNo > -1 && condition.DealerSysNo != null)
                {
                    sqlWhere += " and Cr.DealerSysNo = @DealerSysNo";
                }
                else
                {
                    if (!condition.CanSearchAll)
                    {
                        sqlWhere += " and Cr.DealerSysNo = @BindDealerSysNo";
                    }
                }

                if (condition.BindStatus != null && condition.BindStatus > -1)
                {
                    if (condition.BindStatus == 0)
                        sqlWhere += " and Ap.CustomerSysNo = 0 ";
                    if (condition.BindStatus > 0)
                        sqlWhere += " and Ap.CustomerSysNo > 0 ";
                }

                pager.Rows = _context.Select<CBCrDealerApply>(" Ap.*,Cr.Name,Cr.MobilePhoneNumber,Cr.LevelSysNo,Cr.LevelName,Cr.DealerSysNo,Cr.DealerName,cr.AreaName ")
                                     .From(@"CrDealerApply Ap" +
                                           @" Left Join (Select Cr.SysNo,Cr.Name,Cr.MobilePhoneNumber,Cr.LevelSysNo,Cl.LevelName,Ds.DealerName,Cr.DealerSysNo,(p.AreaName+' '+c.AreaName+' '+bsArea.AreaName) as AreaName
                                            From CrCustomer Cr Left Join CrCustomerLevel Cl On Cr.LevelSysNo=Cl.SysNo Left Join DsDealer Ds On Ds.SysNo=Cr.DealerSysNo
                                              left join bsArea on cr.AreaSysNo =bsArea.SysNo left join bsArea c on bsArea.ParentSysNo=c.SysNo left join bsArea p on p.SysNo=c.ParentSysNo  ) Cr " +
                                           " On Ap.CustomerSysNo=Cr.SysNo")
                                     .Where(sqlWhere)
                                     .Parameter("SysNo", condition.SysNo)
                                     .Parameter("CustomerSysNo", condition.CustomerSysNo)
                                     .Parameter("Status", condition.Status)
                                     .Parameter("AppliedDateStartTime", condition.AppliedDateStartTime)
                                     .Parameter("AppliedDateEndTime", condition.AppliedDateEndTime)
                                     .Parameter("AuditedDateStartTime", condition.AuditedDateStartTime)
                                     .Parameter("AuditedDateEndTime", condition.AuditedDateEndTime)
                                     .Parameter("KeyWord", "%" + condition.KeyWord + "%")
                                     .Parameter("LevelSysNo", condition.LevelSysNo)
                                     .Parameter("Name", "%" + condition.Name + "%")
                                     .Parameter("MobilePhoneNumber", "%" + condition.MobilePhoneNumber + "%")
                                     .Parameter("DealerSysNo", condition.DealerSysNo)
                                     .Parameter("BindDealerSysNo", condition.BindDealerSysNo)
                                     .OrderBy(" Ap.AuditedDate Desc, Ap.AppliedDate Desc, Ap.SysNo Desc ")
                                     .Paging(pager.CurrentPage, pager.PageSize)
                                     .QueryMany();

                pager.TotalRows = _context.Select<int>(" count(0) ")
                                          .From(@"CrDealerApply Ap" +
                                                " Left Join (Select Cr.SysNo,Cr.Name,Cr.MobilePhoneNumber,Cr.LevelSysNo,Cl.LevelName,Ds.DealerName,Cr.DealerSysNo From CrCustomer Cr Left Join CrCustomerLevel Cl On Cr.LevelSysNo=Cl.SysNo Left Join DsDealer Ds On Ds.SysNo=Cr.DealerSysNo) Cr " +
                                                " On Ap.CustomerSysNo=Cr.SysNo")
                                          .Where(sqlWhere)
                                          .Parameter("SysNo", condition.SysNo)
                                          .Parameter("CustomerSysNo", condition.CustomerSysNo)
                                          .Parameter("Status", condition.Status)
                                          .Parameter("AppliedDateStartTime", condition.AppliedDateStartTime)
                                          .Parameter("AppliedDateEndTime", condition.AppliedDateEndTime)
                                          .Parameter("AuditedDateStartTime", condition.AuditedDateStartTime)
                                          .Parameter("AuditedDateEndTime", condition.AuditedDateEndTime)
                                          .Parameter("KeyWord", "%" + condition.KeyWord + "%")
                                          .Parameter("LevelSysNo", condition.LevelSysNo)
                                          .Parameter("Name", "%" + condition.Name + "%")
                                          .Parameter("MobilePhoneNumber", "%" + condition.MobilePhoneNumber + "%")
                                          .Parameter("DealerSysNo", condition.DealerSysNo)
                                          .Parameter("BindDealerSysNo", condition.BindDealerSysNo)
                                          .QuerySingle();
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="model">会员分销商申请实体</param>
        /// <remarks> 2016-04-12 刘伟豪 创建 </remarks>
        public override int Insert(Hyt.Model.CrDealerApply model)
        {
            return Context.Insert("CrDealerApply", model)
                                      .AutoMap(x => x.SysNo)
                                      .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model">会员分销商申请实体</param>
        /// <remarks> 2016-04-12 刘伟豪 创建 </remarks>
        public override bool Update(CrDealerApply model)
        {
            int effect = Context.Update<CrDealerApply>("CrDealerApply", model)
                                      .AutoMap(x => x.SysNo)
                                      .Where("sysno", model.SysNo)
                                      .Execute();
            return effect > 0;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="sysno">系统编号</param>
        /// <remarks> 2016-04-12 刘伟豪 创建 </remarks>
        public override CrDealerApply GetModel(int sysno)
        {
            return Context.Sql(@"select * from CrDealerApply where SysNo = @0", sysno)
                          .QuerySingle<CrDealerApply>();
        }
    }
}