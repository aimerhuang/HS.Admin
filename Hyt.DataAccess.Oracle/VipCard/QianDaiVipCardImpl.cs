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
    /// 钱袋宝会员卡
    /// </summary>
    /// <remarks>2017-03-31 杨浩 创建</remarks>
    public class QianDaiVipCardImpl : IQianDaiVipCardDao
    {
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCard CreateVipCard(CrQianDaiVipCard model)
        {
            model.CreateDate = DateTime.Now;
            model.SysNo = Context.Insert<CrQianDaiVipCard>("CrQianDaiVipCard", model)
                                   .AutoMap(x => x.SysNo)
                                   .ExecuteReturnLastId<int>("SysNo");
            return model;
        }
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCard GetModel(int sysNo)
        {
            return Context.Sql(string.Format("select * from CrQianDaiVipCard where sysNo={0}",sysNo))
                .QuerySingle<CrQianDaiVipCard>();
        }
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="CustomerSysNo">客户系统编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCard GetVipCardByCustomerSysNo(int customerSysNo)
        {
            return Context.Sql(string.Format("select top 1 * from CrQianDaiVipCard where CustomerSysNo={0}", customerSysNo))
                .QuerySingle<CrQianDaiVipCard>();
        }
        /// <summary>
        /// 获取会员详细信息
        /// </summary>
        /// <param name="cardId">客钱袋宝会员卡编号</param>
        /// <returns>会员信息</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override CrQianDaiVipCard GetVipCardByCardId(int cardId)
        {
            return Context.Sql(string.Format("select top 1 * from CrQianDaiVipCard where cardId={0}", cardId))
              .QuerySingle<CrQianDaiVipCard>();
        }
        /// <summary>
        /// 更新会员
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override int Update(CrQianDaiVipCard model)
        {
            int rowsAffected = Context.Update<CrQianDaiVipCard>("CrQianDaiVipCard", model)
                                     .AutoMap(x => x.SysNo)
                                     .Where(x => x.SysNo)
                                     .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 根据条件获取会员列表
        /// </summary>
        /// <param name="pager">会员查询条件</param>
        /// <returns>会员列表</returns>
        /// <remarks>2017-03-31 杨浩 创建</remarks>
        public override Pager<CrQianDaiVipCard> Seach(Pager<CrQianDaiVipCard> pager)
        {
            #region sql条件
            string sqlWhere = @" 1=1 ";
            if (pager.PageFilter.CardId > 0)
                sqlWhere += " and  cardId=" + pager.PageFilter.CardId;
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CrQianDaiVipCard>("cc.*")
                                    .From(@"CrQianDaiVipCard cc")
                                    .Where(sqlWhere)
                                 
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cc.sysNO desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                    .From(@"CrQianDaiVipCard cc")
                                    .Where(sqlWhere)                                  
                                    .QuerySingle();
            }

            return pager;
        }
        /// <summary>
        /// 获取会员卡信息列表
        /// </summary>
        /// <param name="customerSysNos">客户系统编号多个逗号分隔</param>
        /// <returns></returns>
        /// <remarks>2017-04-01 杨浩 创建</remarks>
        public override IList<CrQianDaiVipCard> GetList(string customerSysNos)
        {
            return Context.Sql(string.Format("select * from CrQianDaiVipCard where CustomerSysNo in ({0})", customerSysNos))
             .QueryMany<CrQianDaiVipCard>();
        }
    }
}
