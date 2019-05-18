using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class MemberPointHistoryDaoImpl : IMemberPointHistoryDao
    {
        /// <summary>
        /// 添加积分历史记录
        /// </summary>
        /// <param name="pointHistory"></param>
        /// <returns></returns>
        public override int Insert(MemberPointHistory pointHistory)
        {
            string sql = "update DsMembershipCard set PointIntegral=PointIntegral+(" + pointHistory.mph_Point
                + ") where CardNumber='" + pointHistory.mph_CardNumber + "' and (DsSysNo='" + pointHistory.DsSysNo + "'  or " + pointHistory.DsSysNo + "=0 ) ";
            Context.Sql(sql).Execute();
            return Context.Insert("MemberPointHistory", pointHistory).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 更新计划历史记录
        /// </summary>
        /// <param name="pointHistory"></param>
        public override void Update(MemberPointHistory pointHistory)
        {
            Context.Update("MemberPointHistory", pointHistory).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        /// <summary>
        /// 会员积分明细
        /// </summary>
        /// <returns></returns>
        public override List<MemberPointHistory> GetMemberPointHistory(string cardNumber)
        {
            string sql = " select * from MemberPointHistory where mph_CardNumber='" + cardNumber + "' ";
            return Context.Sql(sql).QueryMany<MemberPointHistory>();
        }
    }
}
