using Hyt.DataAccess.Base;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Pos
{
    /// <summary>
    /// 会员等级
    /// </summary>
    public abstract class IDsMembershioLevelDao : DaoBase<IDsMembershioLevelDao>
    {
        // <summary>
        /// 添加数据
        /// </summary>
        /// <param name="cardMod"></param>
        /// <returns></returns>
        public abstract int Insert(DsMembershioLevel cardMod);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cardMod"></param>
        public abstract void Update(DsMembershioLevel cardMod);
        /// <summary>
        /// 获取会员卡信息
        /// </summary>
        /// <param name="cardSysNo"></param>
        /// <returns></returns>
        public abstract DsMembershioLevel GetDsMembershioLevelBySysNo(int SysNo);

        public abstract List<DsMembershioLevel> GetDsMembershipLevelList(int dsSysNo);
    }
}
