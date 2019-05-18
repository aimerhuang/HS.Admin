using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsMembershipCardDaoImpl : IDsMembershipCardDao
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="cardMod"></param>
        /// <returns></returns>
        public override int Insert(DsMembershipCard cardMod)
        {
            return Context.Insert("DsMembershipCard", cardMod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="cardMod"></param>
        public override void Update(DsMembershipCard cardMod)
        {
            Context.Update("DsMembershipCard", cardMod).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute(); ;
        }
        
        /// <summary>
        /// 会员卡列表
        /// </summary>
        /// <returns></returns>
        public override List<DsMembershipCard> GetMembershipCardList(int dsSysNo)
        {
            string sql = " select * from DsMembershipCard where DsSysNo='" + dsSysNo + "' or " + dsSysNo + " = 0 ";
            return Context.Sql(sql).QueryMany<DsMembershipCard>();
        }

        /// <summary>
        /// 通过会员编号检测会员信息是不是存在
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public override bool CheckMembershipCard(string cardNumber)
        {
            if(!string.IsNullOrEmpty(cardNumber))
            {
                string sql = " select * from DsMembershipCard  where CardNumber ='" + cardNumber + "' ";
                DsMembershipCard mod = Context.Sql(sql).QuerySingle<DsMembershipCard>();
                if ( mod == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pager"></param>
        public override void GetMembershipCardListByPager(ref Model.Pager<CBDsMembershipCard> pager)
        {
            #region sql条件
            string sqlWhere = @"( DsMembershipCard.DsSysNo=@DsSysNo or " + pager.PageFilter.DsSysNo + " = 0 ) ";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context
                    .Select<CBDsMembershipCard>("DsMembershipCard.*,DsDealer.DealerName as DealerName ")
                     .From(" DsMembershipCard inner join DsDealer on DsDealer.SysNo=DsMembershipCard.DsSysNo ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("DsMembershipCard.SysNo desc")
                           .QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                           .From(" DsMembershipCard inner join DsDealer on DsDealer.SysNo=DsMembershipCard.DsSysNo ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .QuerySingle();
            }
        }

        public override CBDsMembershipCard GetMembershipCardBySysNo(int SysNo)
        {
            string sql = " select DsMembershipCard.*,DsMembershioLevel.LevelName from DsMembershipCard inner join DsMembershioLevel on DsMembershipCard.UserLevel=DsMembershioLevel.SysNo ";
            sql += " where DsMembershipCard.SysNo='" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<CBDsMembershipCard>();
        }

        public override CBDsMembershipCard GetMembershipCardBySysNo(string Number)
        {
            string sql = " select DsMembershipCard.*,DsMembershioLevel.LevelName,DsDealer.DealerName from DsMembershipCard left join DsMembershioLevel on DsMembershipCard.UserLevel=DsMembershioLevel.SysNo ";
            sql += " left join DsDealer on  DsDealer.SysNo=DsMembershipCard.DsSysNo ";
            sql += " where DsMembershipCard.CardNumber='" + Number + "' ";
            return Context.Sql(sql).QuerySingle<CBDsMembershipCard>();
        }
    }

}
