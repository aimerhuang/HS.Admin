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
    /// 会员卡充值日志
    /// </summary>
    /// <remarks>2017-03-31 杨浩 创建</remarks>
    public class CrQianDaiVipCardRechargeLogImpl : ICrQianDaiVipCardRechargeLogDao
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCardRechargeLog CreateCrQianDaiVipCardRechargeLog(CrQianDaiVipCardRechargeLog model)
        {
            model.SysNo = Context.Insert<CrQianDaiVipCardRechargeLog>("CrQianDaiVipCardRechargeLog", model)
                                  .AutoMap(x => x.SysNo)
                                  .ExecuteReturnLastId<int>("SysNo");
            return model;
        }
        /// <summary>
        /// 获取充值日志
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCardRechargeLog GetModel(int sysNo)
        {
            return Context.Sql(string.Format("select * from CrQianDaiVipCardRechargeLog where sysNo={0}", sysNo))
                .QuerySingle<CrQianDaiVipCardRechargeLog>();
        }
        /// <summary>
        /// 获取充值日志
        /// </summary>
        /// <param name="rechargeNo ">充值流水号</param>
        /// <returns></returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCardRechargeLog GetQianDaiVipCardRechargeLogByRechargeNo(string rechargeNo)
        {
            return Context.Sql("select * from CrQianDaiVipCardRechargeLog where rechargeNo=@rechargeNo")
              .Parameter("rechargeNo", rechargeNo)
              .QuerySingle<CrQianDaiVipCardRechargeLog>();
        }

        /// <summary>
        /// 更新充值日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override int Update(CrQianDaiVipCardRechargeLog model)
        {
            int rowsAffected = Context.Update<CrQianDaiVipCardRechargeLog>("CrQianDaiVipCardRechargeLog", model)
                                     .AutoMap(x => x.SysNo, x => x.CreateDate, x => x.CreatedBy, x => x.CardId)
                                     .Where(x => x.SysNo)
                                     .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 根据条件获取充值日志列表
        /// </summary>
        /// <param name="pager">充值日志查询条件</param>
        /// <returns>充值日志列表</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override Pager<CrQianDaiVipCardRechargeLog> Seach(Pager<CrQianDaiVipCardRechargeLog> pager)
        {
            #region sql条件
            string sqlWhere = @" 1=1 ";

            if (pager.PageFilter.CardId > 0)
                sqlWhere += " and CardId=" + pager.PageFilter.CardId;

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CrQianDaiVipCardRechargeLog>("cc.*")
                                    .From(@"CrQianDaiVipCardRechargeLog cc")
                                    .Where(sqlWhere)

                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cc.sysNO desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                    .From(@"CrQianDaiVipCardRechargeLog cc")
                                    .Where(sqlWhere)
                                    .QuerySingle();
            }

            return pager;
        }
    }
}
