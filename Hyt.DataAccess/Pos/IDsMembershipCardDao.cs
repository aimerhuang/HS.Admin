using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    /// <summary>
    /// 会员卡
    /// </summary>
    public abstract class IDsMembershipCardDao : DaoBase<IDsMembershipCardDao>
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="cardMod"></param>
        /// <returns></returns>
        public abstract int Insert(DsMembershipCard cardMod);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cardMod"></param>
        public abstract void Update(DsMembershipCard cardMod);
        /// <summary>
        /// 获取会员卡信息
        /// </summary>
        /// <param name="cardSysNo"></param>
        /// <returns></returns>
        //public abstract CBDsMembershipCard GetDsMembershipCardByCardSysNo(string cardSysNo, string telePhone);

        public abstract List<DsMembershipCard> GetMembershipCardList(int dsSysNo);

        public abstract bool CheckMembershipCard(string cardNumber);

        public abstract void GetMembershipCardListByPager(ref Model.Pager<CBDsMembershipCard> pager);

        public abstract CBDsMembershipCard GetMembershipCardBySysNo(int SysNo);

        public abstract CBDsMembershipCard GetMembershipCardBySysNo(string Number);
    }
}
