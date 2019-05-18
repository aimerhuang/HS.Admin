using Hyt.DataAccess.VipCard;
using Hyt.Model;
using Hyt.Model.VipCard.QianDai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.VipCard
{
    /// <summary>
    /// 会员卡提现日志
    /// </summary>
    /// <remarks>2017-03-31 杨浩 创建</remarks>
    public class CrQianDaiVipCardWithdrawLogImpl : ICrQianDaiVipCardWithdrawLogDao
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCardWithdrawLog CreateCrQianDaiVipCardWithdrawLog(CrQianDaiVipCardWithdrawLog model)
        {
            model.SysNo = Context.Insert<CrQianDaiVipCardWithdrawLog>("CrQianDaiVipCardWithdrawLog", model)
                                  .AutoMap(x => x.SysNo)
                                  .ExecuteReturnLastId<int>("SysNo");
            return model;
        }
        /// <summary>
        /// 获取提现日志
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCardWithdrawLog GetModel(int sysNo)
        {
            return Context.Sql(string.Format("select * from CrQianDaiVipCardWithdrawLog where sysNo={0}", sysNo))
                .QuerySingle<CrQianDaiVipCardWithdrawLog>();
        }
        /// <summary>
        /// 获取充值日志
        /// </summary>
        /// <param name="rechargeNo ">充值流水号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCardWithdrawLog GetQianDaiVipCardWithdraweLogByWithdrawNo(string rechargeNo)
        {
            return Context.Sql("select * from CrQianDaiVipCardWithdrawLog where WithdrawNo=@rechargeNo")
              .Parameter("rechargeNo", rechargeNo)
              .QuerySingle<CrQianDaiVipCardWithdrawLog>();
        }

        /// <summary>
        /// 更新提现日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override int Update(CrQianDaiVipCardWithdrawLog model)
        {
            int rowsAffected = Context.Update<CrQianDaiVipCardWithdrawLog>("CrQianDaiVipCardWithdrawLog", model)
                                     .AutoMap(x => x.SysNo, x => x.CreateDate, x => x.CreatedBy, x => x.CardId)
                                     .Where(x => x.SysNo)
                                     .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 根据条件获取充值日志列表
        /// </summary>
        /// <param name="pager">提现日志查询条件</param>
        /// <returns>充值日志列表</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override Pager<CrQianDaiVipCardWithdrawLog> Seach(Pager<CrQianDaiVipCardWithdrawLog> pager)
        {
            #region sql条件
            string sqlWhere = @" 1=1 ";

            if (pager.PageFilter.CardId > 0)
                sqlWhere += " and CardId=" + pager.PageFilter.CardId;

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CrQianDaiVipCardWithdrawLog>("cc.*")
                                    .From(@"CrQianDaiVipCardWithdrawLog cc")
                                    .Where(sqlWhere)

                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cc.sysNO desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                    .From(@"CrQianDaiVipCardWithdrawLog cc")
                                    .Where(sqlWhere)
                                    .QuerySingle();
            }

            return pager;
        }
    }
}
