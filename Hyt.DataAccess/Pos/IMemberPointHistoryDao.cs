using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    /// <summary>
    /// 会员积分历史记录
    /// </summary>
    public abstract class IMemberPointHistoryDao : DaoBase<IMemberPointHistoryDao>
    {
        public abstract int Insert(MemberPointHistory pointHistory);

        public abstract void Update(MemberPointHistory pointHistory);

        public abstract List<MemberPointHistory> GetMemberPointHistory(string cardNumber);
    }
}
